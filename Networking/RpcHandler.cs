using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Security.Cryptography;
using Google.Protobuf;
using Axlebolt.RpcSupport.Protobuf;
using Axlebolt.Bolt.Protobuf;
using StandoffServer.Services;
using StandoffServer.Models;
using ServerInventoryItemDefinition = Axlebolt.Bolt.Protobuf.InventoryItemDefinition;

namespace StandoffServer.Networking
{
    public class RpcHandler
    {
        private readonly MongoService _mongo;
        private readonly ConcurrentDictionary<string, UserDoc> _activeSessions = new ConcurrentDictionary<string, UserDoc>();
        private readonly ConcurrentDictionary<string, NetworkStream> _clientStreams = new ConcurrentDictionary<string, NetworkStream>();
        private readonly ConcurrentDictionary<string, int> _inventoryItemCounters = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentDictionary<string, string> _clientVersions = new ConcurrentDictionary<string, string>(); // ticket -> version

        private readonly ConcurrentDictionary<int, List<OpenRequest>> _saleRequestsByDefinitionId = new ConcurrentDictionary<int, List<OpenRequest>>();
        private readonly ConcurrentDictionary<string, OpenRequest> _saleRequestsById = new ConcurrentDictionary<string, OpenRequest>();

        private readonly ConcurrentDictionary<int, List<OpenRequest>> _purchaseRequestsByDefinitionId = new ConcurrentDictionary<int, List<OpenRequest>>();
        private readonly ConcurrentDictionary<string, OpenRequest> _purchaseRequestsById = new ConcurrentDictionary<string, OpenRequest>();

        private readonly ConcurrentDictionary<string, HashSet<string>> _topicSubscribers = new ConcurrentDictionary<string, HashSet<string>>();

        private readonly ConcurrentDictionary<string, ServerLobby> _lobbiesById = new ConcurrentDictionary<string, ServerLobby>();
        private readonly ConcurrentDictionary<string, HashSet<string>> _lobbyMembersByLobbyId = new ConcurrentDictionary<string, HashSet<string>>();

        private readonly ConcurrentDictionary<string, HashSet<string>> _lobbySpectatorsByLobbyId = new ConcurrentDictionary<string, HashSet<string>>();
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, LobbyPlayerType>> _lobbyPlayerTypesByLobbyId = new ConcurrentDictionary<string, ConcurrentDictionary<string, LobbyPlayerType>>();
        private readonly ConcurrentDictionary<string, string> _playerLobbyByPlayerId = new ConcurrentDictionary<string, string>();
        private readonly ConcurrentDictionary<string, List<ServerLobbyInvite>> _lobbyInvitesByPlayerId = new ConcurrentDictionary<string, List<ServerLobbyInvite>>();

        private readonly ConcurrentDictionary<string, long> _sessionStartTime = new ConcurrentDictionary<string, long>();

        public RpcHandler(MongoService mongo)
        {
            _mongo = mongo;
            _ = InitializeMarketplaceAsync();
        }

        private async Task InitializeMarketplaceAsync()
        {
            try
            {
                var sales = await _mongo.GetAllMarketplaceSalesAsync();
                foreach (var s in sales)
                {
                    var req = new OpenRequest
                    {
                        Id = s.Id,
                        ItemDefinitionId = s.ItemDefinitionId,
                        Price = s.Price,
                        Quantity = s.Quantity,
                        CreateDate = s.CreateDate,
                        Type = MarketRequestType.SaleRequest
                    };
                    
                    req.Creator = new Player { Id = s.SellerUserId.ToString(), Name = "Seller" };

                    foreach (var kv in s.Properties)
                    {
                        req.Properties[kv.Key] = new InventoryItemProperty
                        {
                            Type = (PropertyType)kv.Value.Type,
                            IntValue = kv.Value.IntValue,
                            FloatValue = kv.Value.FloatValue,
                            StringValue = kv.Value.StringValue,
                            BooleanValue = kv.Value.BoolValue
                        };
                    }
                    AddSaleRequest(req);
                }
                Console.WriteLine($"[Market] Loaded {sales.Count} active sales from database.");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Market] Initialization error: {ex.Message}");
            }
        }

        private void PublishPlayerRequestOpened(string ticket, OpenRequest req)
        {
            if (string.IsNullOrEmpty(ticket)) return;
            if (!_clientStreams.TryGetValue(ticket, out var stream)) return;

            var ev = new OnPlayerRequestOpenedEvent { Request = req };
            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
            {
                ListenerName = "MarketplaceRemoteEventListener",
                EventName = "onPlayerRequestOpened"
            };
            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = ev.ToByteString() });
            
            try {
                NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt });
            } catch {}
        }

        private void PublishPlayerRequestClosed(string ticket, ClosedRequest req, InventoryItem item = null)
        {
            if (string.IsNullOrEmpty(ticket)) return;
            if (!_clientStreams.TryGetValue(ticket, out var stream)) return;

            var ev = new OnPlayerRequestClosedEvent { Request = req, Item = item };
            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
            {
                ListenerName = "MarketplaceRemoteEventListener",
                EventName = "onPlayerRequestClosed"
            };
            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = ev.ToByteString() });
            
            try {
                NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt });
            } catch {}
        }

        private void RemoveSaleRequest(OpenRequest req)
        {
            if (req == null) return;
            _saleRequestsById.TryRemove(req.Id, out _);

            if (!_saleRequestsByDefinitionId.TryGetValue(req.ItemDefinitionId, out var list))
            {
                return;
            }

            lock (list)
            {
                var idx = list.FindIndex(x => x != null && x.Id == req.Id);
                if (idx >= 0)
                {
                    list.RemoveAt(idx);
                }
            }
        }

        private void RemovePurchaseRequest(OpenRequest req)
        {
            if (req == null) return;
            _purchaseRequestsById.TryRemove(req.Id, out _);

            if (!_purchaseRequestsByDefinitionId.TryGetValue(req.ItemDefinitionId, out var list))
            {
                return;
            }

            lock (list)
            {
                var idx = list.FindIndex(x => x != null && x.Id == req.Id);
                if (idx >= 0)
                {
                    list.RemoveAt(idx);
                }
            }
        }

        private void SubscribeTopic(string ticket, string topic)
        {
            if (string.IsNullOrWhiteSpace(ticket) || string.IsNullOrWhiteSpace(topic)) return;
            var set = _topicSubscribers.GetOrAdd(topic, _ => new HashSet<string>());
            lock (set)
            {
                set.Add(ticket);
            }
        }

        private void UnsubscribeTopic(string ticket, string topic)
        {
            if (string.IsNullOrWhiteSpace(ticket) || string.IsNullOrWhiteSpace(topic)) return;
            if (!_topicSubscribers.TryGetValue(topic, out var set)) return;
            lock (set)
            {
                set.Remove(ticket);
                if (set.Count == 0)
                {
                    _topicSubscribers.TryRemove(topic, out _);
                }
            }
        }

        private void PublishEvent(string topic, Axlebolt.RpcSupport.Protobuf.EventResponse evt)
        {
            if (evt == null || string.IsNullOrWhiteSpace(topic)) return;

            if (!_topicSubscribers.TryGetValue(topic, out var subs))
            {
                return;
            }

            string[] tickets;
            lock (subs)
            {
                tickets = subs.ToArray();
            }

            foreach (var t in tickets)
            {
                if (string.IsNullOrWhiteSpace(t)) continue;
                if (!_clientStreams.TryGetValue(t, out var stream)) continue;
                if (stream == null) continue;

                try
                {
                    var msg = new Axlebolt.RpcSupport.Protobuf.ResponseMessage
                    {
                        EventResponse = evt
                    };
                    NetworkPacket.WritePacket(stream, msg);
                }
                catch
                {
                    // ignore broken streams
                }
            }
        }

        private void PublishTradeOpened(OpenRequest req)
        {
            var ev = new Axlebolt.Bolt.Protobuf.OnTradeRequestOpenedEvent { Request = req };
            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
            {
                ListenerName = "MarketplaceRemoteEventListener",
                EventName = "onTradeRequestOpened"
            };
            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = ev.ToByteString() });
            PublishEvent(TradeTopic(req.ItemDefinitionId), evt);
        }

        private void PublishTradeClosed(ClosedRequest req)
        {
            var ev = new Axlebolt.Bolt.Protobuf.OnTradeRequestClosedEvent { Request = req };
            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
            {
                ListenerName = "MarketplaceRemoteEventListener",
                EventName = "onTradeRequestClosed"
            };
            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = ev.ToByteString() });
            PublishEvent(TradeTopic(req.ItemDefinitionId), evt);
        }

        private void PublishTradeUpdated(int itemDefinitionId)
        {
            var (saleCount, minPrice) = GetSaleSummary(itemDefinitionId);
            var (purchaseCount, maxPrice) = GetPurchaseSummary(itemDefinitionId);
            var trade = new Trade
            {
                Id = itemDefinitionId,
                SalesCount = saleCount,
                PurchasesCount = purchaseCount,
                SalesPrice = saleCount > 0 ? minPrice : 0f,
                PurchasesPrice = purchaseCount > 0 ? maxPrice : 0f
            };

            var ev = new Axlebolt.Bolt.Protobuf.OnTradeUpdatedEvent { Trade = trade };
            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
            {
                ListenerName = "MarketplaceRemoteEventListener",
                EventName = "onTradeUpdated"
            };
            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = ev.ToByteString() });
            PublishEvent(TradeTopic(itemDefinitionId), evt);
        }

        private static long NowMs() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        private static string TradeTopic(int itemDefinitionId) => "marketplace_trade_" + itemDefinitionId;

        private Player ToProtoPlayer(UserDoc user)
        {
            if (user == null)
            {
                return new Player { Id = "0", Uid = "0", Name = "Guest", AvatarId = "0" };
            }

            var t = FindTicketByPlayerId(user.Id.ToString());
            var online = !string.IsNullOrWhiteSpace(t);

            // Используем CustomId если задан, иначе числовой Id
            string displayId = !string.IsNullOrWhiteSpace(user.CustomId)
                ? user.CustomId
                : user.Id.ToString();

            // AvatarId: возвращаем userId только если аватарка реально установлена
            // Если AvatarId == 0 — аватарки нет, возвращаем "0"
            string avatarIdStr;
            if (user.AvatarId > 0 && (user.AvatarId == (int)user.Id || user.AvatarId > 10000))
                avatarIdStr = user.Id.ToString();
            else
                avatarIdStr = "0";

            var proto = new Player
            {
                Id = user.Id.ToString(),
                Uid = displayId,
                Name = user.Name ?? "Player",
                AvatarId = avatarIdStr,
                RegistrationDate = user.RegistrationDate,
                TimeInGame = (int)user.TimeInGame,
                PlayerStatus = new PlayerStatus
                {
                    PlayerId = user.Id.ToString(),
                    OnlineStatus = online ? OnlineStatus.StateOnline : OnlineStatus.StateOffline
                }
            };

            return proto;
        }

        private void AddSaleRequest(OpenRequest req)
        {
            if (req == null) return;

            _saleRequestsById[req.Id] = req;
            var list = _saleRequestsByDefinitionId.GetOrAdd(req.ItemDefinitionId, _ => new List<OpenRequest>());
            lock (list)
            {
                list.Add(req);
                // Keep cheapest first for UI and fast min-price.
                list.Sort((a, b) => a.Price.CompareTo(b.Price));
            }
        }

        private void AddPurchaseRequest(OpenRequest req)
        {
            if (req == null) return;

            _purchaseRequestsById[req.Id] = req;
            var list = _purchaseRequestsByDefinitionId.GetOrAdd(req.ItemDefinitionId, _ => new List<OpenRequest>());
            lock (list)
            {
                list.Add(req);
                // Keep highest bid first for fast max-price.
                list.Sort((a, b) => b.Price.CompareTo(a.Price));
            }
        }

        private (int count, float minPrice) GetSaleSummary(int itemDefinitionId)
        {
            if (!_saleRequestsByDefinitionId.TryGetValue(itemDefinitionId, out var list))
            {
                return (0, 0f);
            }

            lock (list)
            {
                if (list.Count == 0)
                {
                    return (0, 0f);
                }

                int total = 0;
                foreach (var r in list)
                {
                    if (r != null && r.Quantity > 0) total += r.Quantity;
                }

                var min = list.FirstOrDefault(r => r != null && r.Quantity > 0);
                return (total, min?.Price ?? 0f);
            }
        }

        private (int count, float maxPrice) GetPurchaseSummary(int itemDefinitionId)
        {
            if (!_purchaseRequestsByDefinitionId.TryGetValue(itemDefinitionId, out var list))
            {
                return (0, 0f);
            }

            lock (list)
            {
                if (list.Count == 0)
                {
                    return (0, 0f);
                }

                int total = 0;
                foreach (var r in list)
                {
                    if (r != null && r.Quantity > 0) total += r.Quantity;
                }

                var max = list.FirstOrDefault(r => r != null && r.Quantity > 0);
                return (total, max?.Price ?? 0f);
            }
        }

        public void RegisterClient(string ticket, NetworkStream stream)
        {
            // If this ticket already has an active stream — kick the old session
            if (_clientStreams.TryGetValue(ticket, out var oldStream) && oldStream != null && !ReferenceEquals(oldStream, stream))
            {
                Console.WriteLine($"[Session] Duplicate login detected for ticket '{ticket}' — closing old session");
                try { oldStream.Close(); } catch { }
            }

            _clientStreams[ticket] = stream;
            _sessionStartTime[ticket] = NowMs();

            // Best-effort: publish online status to friends.
            _ = Task.Run(async () =>
            {
                try
                {
                    if (_activeSessions.TryGetValue(ticket, out var u) && u != null)
                    {
                        await PublishPlayerStatusToFriendsAsync(u.Id, OnlineStatus.StateOnline);
                    }
                }
                catch { }
            });
        }

        public void UnregisterClient(string ticket)
        {
            _clientStreams.TryRemove(ticket, out _);

            if (_sessionStartTime.TryRemove(ticket, out var startMs) && _activeSessions.TryGetValue(ticket, out var user))
            {
                var durationSec = (int)((NowMs() - startMs) / 1000);
                if (durationSec > 0)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            user.TimeInGame += durationSec;
                            await _mongo.UpdateUserAsync(user);

                            var currentStats = await _mongo.GetStatsAsync(user.Id) ?? new PlayerStatsDoc { UserId = user.Id };
                            var timeEntry = currentStats.Stats.FirstOrDefault(x => x.Name == "time_in_game");
                            if (timeEntry == null)
                                currentStats.Stats.Add(new StatEntry { Name = "time_in_game", IntValue = durationSec, Type = (int)StatDefType.Int });
                            else
                                timeEntry.IntValue += durationSec;

                            await _mongo.UpdateStatsAsync(currentStats);
                            Console.WriteLine($"[Session] User {user.Id} disconnected. Added {durationSec}s playtime.");
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Session] Error saving background playtime: {ex.Message}");
                        }
                    });
                }
            }

            // Best-effort cleanup: remove ticket from all topics.
            foreach (var kv in _topicSubscribers)
            {
                lock (kv.Value)
                {
                    kv.Value.Remove(ticket);
                }
            }

            // Best-effort: publish offline status to friends.
            _ = Task.Run(async () =>
            {
                try
                {
                    if (_activeSessions.TryGetValue(ticket, out var u) && u != null)
                    {
                        await PublishPlayerStatusToFriendsAsync(u.Id, OnlineStatus.StateOffline);
                    }
                }
                catch { }
            });
        }

        private async Task PublishPlayerStatusToFriendsAsync(long userId, OnlineStatus status)
        {
            try
            {
                var links = await _mongo.GetFriendLinksAsync(userId, new[] { (int)RelationshipStatus.Friend }, 0, 5000);
                foreach (var l in links)
                {
                    if (l == null) continue;
                    var targetTicket = FindTicketByPlayerId(l.FriendUserId.ToString());
                    if (string.IsNullOrWhiteSpace(targetTicket)) continue;
                    if (!_clientStreams.TryGetValue(targetTicket, out var stream) || stream == null) continue;

                    var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
                    {
                        ListenerName = "FriendsRemoteEventListener",
                        EventName = "onPlayerStatusChanged"
                    };
                    evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = new Axlebolt.RpcSupport.Protobuf.String { Value = userId.ToString() }.ToByteString() });
                    evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = new PlayerStatus { PlayerId = userId.ToString(), OnlineStatus = status }.ToByteString() });
                    try { NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt }); } catch { }
                }
            }
            catch { }
        }

        private int NextInventoryItemId(string ticket, InventoryDoc inv = null)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                ticket = "guest";
            }

            // If we know the inventory, seed the counter from actual maxId to guarantee uniqueness.
            var maxInInv = inv?.Items?.Count > 0 ? inv.Items.Max(x => x.Id) : 9;
            if (_inventoryItemCounters.TryGetValue(ticket, out var current) && current < maxInInv)
            {
                _inventoryItemCounters[ticket] = maxInInv;
            }

            // AddOrUpdate returns the stored value. For new keys we want max+1, not a fixed 10.
            return _inventoryItemCounters.AddOrUpdate(ticket, maxInInv + 1, (_, cur) => cur + 1);
        }

        private async Task<InventoryDoc> GetOrCreateInventoryDocAsync(UserDoc user)
        {
            // Safety guard: never create/wipe inventory for guest or unresolved users
            if (user == null || user.Id <= 0)
            {
                Console.WriteLine($"[Inventory] GetOrCreateInventoryDocAsync called with invalid user (Id={user?.Id}, Uid={user?.Uid}) — returning empty doc without saving");
                return new InventoryDoc { UserId = 0 };
            }

            InventoryDoc inv = null;
            try
            {
                inv = await _mongo.GetInventoryAsync(user.Id);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Inventory] GetInventoryAsync threw for user {user.Id}: {ex.Message}");
                inv = null;
            }

            if (inv != null)
            {
                // Remove starter case (301) if it's still in inventory
                var starterCase = inv.Items.FirstOrDefault(x => x.ItemDefinitionId == 301 && x.Id == 1);
                if (starterCase != null)
                {
                    inv.Items.Remove(starterCase);
                    await _mongo.UpsertInventoryAsync(inv);
                }
                EnsureInventoryCounterInitialized(user.Uid, inv);
                return inv;
            }

            // inv == null means either no record exists OR deserialization failed.
            // Check if a raw document exists to avoid creating a blank inventory over existing data.
            bool docExists = await _mongo.InventoryExistsAsync(user.Id);
            if (docExists)
            {
                Console.WriteLine($"[Inventory] WARNING: inventory doc exists for user {user.Id} but failed to deserialize — returning empty shell to avoid data loss");
                // Return a shell with correct UserId so operations don't create a new blank record
                var shell = new InventoryDoc { UserId = user.Id };
                EnsureInventoryCounterInitialized(user.Uid, shell);
                return shell;
            }

            // No record at all — create fresh inventory
            inv = new InventoryDoc
            {
                UserId = user.Id,
                Currencies = new List<InventoryCurrencyEntry>
                {
                    new InventoryCurrencyEntry { CurrencyId = 101, Value = 1000 },
                    new InventoryCurrencyEntry { CurrencyId = 102, Value = 1000 }
                },
                Items = new List<InventoryItemEntry>()
            };

            await _mongo.UpsertInventoryAsync(inv);
            EnsureInventoryCounterInitialized(user.Uid, inv);
            return inv;
        }

        private void EnsureInventoryCounterInitialized(string ticket, InventoryDoc inv)
        {
            if (string.IsNullOrWhiteSpace(ticket)) ticket = "guest";
            if (_inventoryItemCounters.ContainsKey(ticket)) return;

            var maxId = inv?.Items?.Count > 0 ? inv.Items.Max(x => x.Id) : 10;
            if (maxId < 10) maxId = 10;
            _inventoryItemCounters[ticket] = maxId;
        }

        private async Task<PlayerInventory> LoadOrCreateInventoryAsync(UserDoc user)
        {
            try 
            {
                // Guard: never load/create inventory for unresolved users
                if (user == null || user.Id <= 0)
                {
                    Console.WriteLine($"[Inventory] LoadOrCreateInventoryAsync: invalid user (Id={user?.Id}) — returning empty inventory");
                    return new PlayerInventory();
                }

                var invDoc = await GetOrCreateInventoryDocAsync(user);

                // Auto-repair: client builds a dictionary keyed by InventoryItem.Id and crashes on duplicates.
                if (invDoc?.Items != null && invDoc.Items.Count > 1)
                {
                    var seen = new HashSet<int>();
                    var maxId = invDoc.Items.Count > 0 ? invDoc.Items.Max(x => x.Id) : 10;
                    var mutated = false;

                    foreach (var it in invDoc.Items)
                    {
                        if (it == null) continue;
                        if (seen.Add(it.Id)) continue;
                        maxId += 1;
                        it.Id = maxId;
                        mutated = true;
                    }

                    if (mutated)
                    {
                        await _mongo.UpsertInventoryAsync(invDoc);
                        EnsureInventoryCounterInitialized(user.Uid, invDoc);
                    }
                }

                var inv = new PlayerInventory();
                if (invDoc != null)
                {
                    foreach (var c in invDoc.Currencies)
                    {
                        inv.Currencies.Add(new CurrencyAmount { CurrencyId = c.CurrencyId, Value = c.Value });
                    }

                    foreach (var item in invDoc.Items)
                    {
                        try
                        {
                            var pb = new InventoryItem
                            {
                                Id = item.Id,
                                ItemDefinitionId = item.ItemDefinitionId,
                                Quantity = item.Quantity,
                                Flags = item.Flags,
                                Date = item.Date
                            };

                            if (item.Properties != null)
                            {
                                foreach (var kv in item.Properties)
                                {
                                    if (kv.Value == null) continue;
                                    pb.Properties[kv.Key] = new InventoryItemProperty
                                    {
                                        Type        = (PropertyType)kv.Value.Type,
                                        IntValue    = kv.Value.IntValue,
                                        FloatValue  = kv.Value.FloatValue,
                                        StringValue = kv.Value.StringValue,
                                        BooleanValue = kv.Value.BoolValue
                                    };
                                }
                            }
                            inv.InventoryItems.Add(pb);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Inventory] Skipped item {item?.Id} (def={item?.ItemDefinitionId}) due to error: {ex.Message}");
                        }
                    }
                }

                return inv;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Inventory] LoadOrCreateInventoryAsync error: {ex.Message}");
                return new PlayerInventory();
            }
        }

        private async Task<UserDoc> GetCurrentUserAsync(string ticket)
        {
            if (!string.IsNullOrEmpty(ticket) && _activeSessions.TryGetValue(ticket, out var user) && user != null)
            {
                return user;
            }

            if (!string.IsNullOrEmpty(ticket))
            {
                // Try DB lookup by UID (exact match)
                var dbUser = await _mongo.GetUserByUidAsync(ticket);
                if (dbUser != null)
                {
                    _activeSessions[ticket] = dbUser;
                    Console.WriteLine($"[Session] Restored session for user {dbUser.Name} (ID: {dbUser.Id}) by UID match");
                    return dbUser;
                }

                // Fallback: ticket might be a numeric user ID string
                if (long.TryParse(ticket, out var numericId) && numericId > 0)
                {
                    var byId = await _mongo.GetUserByIdAsync(numericId);
                    if (byId != null)
                    {
                        _activeSessions[ticket] = byId;
                        Console.WriteLine($"[Session] Restored session for user {byId.Name} (ID: {byId.Id}) by numeric ticket");
                        return byId;
                    }
                }

                // Last resort: search by partial UID match — DISABLED (security risk)
                // Partial matching could allow one user to accidentally access another's account

                Console.WriteLine($"[Session] WARNING: ticket not found — returning null (no access)");
            }

            return null; // caller must handle null — never return a shared guest object
        }

        public async Task<RpcResponse> HandleRequestAsync(RpcRequest request, string currentTicket)
        {
            Console.WriteLine($"[RPC] Request: {request.ServiceName}.{request.MethodName} (ID: {request.Id})");

            try
            {
                switch (request.ServiceName)
                {
                    case "TestAuthRemoteService":
                        return await HandleAuthAsync(request);
                    case "GoogleAuthRemoteService":
                    case "FacebookAuthRemoteService":
                    case "GameCenterAuthRemoteService":
                        return await HandleExternalAuthAsync(request, prefix: request.ServiceName.Replace("RemoteService", "").ToLowerInvariant());
                    case "HandshakeRemoteService":
                        return await HandleHandshakeAsync(request, currentTicket);
                    case "PlayerRemoteService":
                        return await HandlePlayerAsync(request, currentTicket);
                    case "PlayerStatsRemoteService":
                        return await HandlePlayerStatsAsync(request, currentTicket);
                    case "GameSettingsRemoteService":
                        return await HandleGameSettingsAsync(request, currentTicket);
                    case "GameSettingsPlayerRemoteService":
                    case "GameSettingsGameServerRemoteService":
                        return await HandleGameSettingsPassthroughAsync(request, currentTicket);
                    case "VersionRemoteService":
                        return await HandleVersionAsync(request);
                    case "GameEventRemoteService":
                        return await HandleGameEventAsync(request, currentTicket);
                    case "BoltRemoteService":
                        return await HandleBoltAsync(request, currentTicket);
                    case "FriendsRemoteService":
                        return await HandleFriendsAsync(request, currentTicket);
                    case "ClanRemoteService":
                        return await HandleClansAsync(request, currentTicket);
                    case "InventoryRemoteService":
                        return await HandleInventoryAsync(request, currentTicket);
                    case "MarketplaceRemoteService":
                        return await HandleMarketplaceAsync(request, currentTicket);
                    case "SettingsRemoteService":
                        return await HandleSettingsAsync(request, currentTicket);
                    case "ChatRemoteService":
                        return await HandleChatAsync(request, currentTicket);
                    case "AvatarRemoteService":
                        return await HandleAvatarAsync(request, currentTicket);
                    case "MatchmakingRemoteService":
                        return await HandleMatchmakingAsync(request, currentTicket);
                    case "StorageRemoteService":
                        return await HandleStorageAsync(request, currentTicket);
                    case "AnalyticsRemoteService":
                        return await HandleAnalyticsAsync(request, currentTicket);
                    case "AdsRemoteService":
                        return await HandleAdsAsync(request, currentTicket);
                    case "HackDetectionRemoteService":
                        return await HandleHackDetectionAsync(request, currentTicket);
                    case "GroupRemoteService":
                        return await HandleGroupAsync(request, currentTicket);
                    case "AccountLinkRemoteService":
                        return await HandleAccountLinkAsync(request, currentTicket);
                    case "GoogleInAppRemoteService":
                    case "AmazonInAppRemoteService":
                    case "AppStoreInAppRemoteService":
                        return await HandleInAppAsync(request, currentTicket);
                    case "GameServerRemoteService":
                    case "GameServerPlayerRemoteService":
                    case "GameServerStatsRemoteService":
                        return await HandleGameServerAsync(request, currentTicket);
                    default:
                        Console.WriteLine($"[RPC] Service {request.ServiceName} not explicitly handled. Using fallback.");
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[RPC] Error handling {request.ServiceName}.{request.MethodName}: {ex.Message}");
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private static List<ServerInventoryItemDefinition> _cachedJsonDefinitions;

        private static List<ServerInventoryItemDefinition> LoadInventoryDefinitionsFromJson()
        {
            if (_cachedJsonDefinitions != null)
            {
                return _cachedJsonDefinitions;
            }

            try
            {
                var baseDir = AppContext.BaseDirectory;
                var path = Path.Combine(baseDir, "StandRise.inventory_item_definition.json");
                if (!File.Exists(path))
                {
                    // Try parent directories if not in base dir (for development)
                    var devPath = Path.Combine(baseDir, "..", "..", "..", "StandRise.inventory_item_definition.json");
                    if (File.Exists(devPath)) path = devPath;
                }

                if (!File.Exists(path))
                {
                    Console.WriteLine($"[Inventory] JSON definitions file not found: {path}");
                    _cachedJsonDefinitions = new List<ServerInventoryItemDefinition>();
                    return _cachedJsonDefinitions;
                }

                var json = File.ReadAllText(path);

                using var doc = JsonDocument.Parse(json);
                var list = new List<ServerInventoryItemDefinition>();

                foreach (var el in doc.RootElement.EnumerateArray())
                {
                    if (!el.TryGetProperty("key", out var keyProp) || keyProp.ValueKind != JsonValueKind.Number)
                    {
                        continue;
                    }

                    int id = keyProp.GetInt32();
                    string name = el.TryGetProperty("displayName", out var nameProp) && nameProp.ValueKind == JsonValueKind.String
                        ? nameProp.GetString()
                        : $"Item {id}";

                    // Filter out disabled items to reduce packet size and prevent client confusion
                    if (el.TryGetProperty("enabled", out var enabledProp) && enabledProp.ValueKind == JsonValueKind.False)
                    {
                        continue; // Skip disabled items
                    }

                    var def = new ServerInventoryItemDefinition
                    {
                        Id = id,
                        DisplayName = name
                    };
                    // Check if this is a StatTrack item (ID >= 1,000,000)
                    if (id >= 1000000)
                    {
                        def.Properties["stattrack"] = "true";
                    }

                    // Marketplace/trades tab uses CanBeTraded filter to build the catalog.
                    // If this is false for all items, marketplace becomes empty and filters find nothing.
                    if (el.TryGetProperty("canBeTraded", out var canBeTradedProp) && canBeTradedProp.ValueKind == JsonValueKind.True)
                    {
                        def.CanBeTraded = true;
                    }
                    else
                    {
                        def.CanBeTraded = false;
                    }

                    // properties: collection, value, rarity, etc.
                    if (el.TryGetProperty("properties", out var props) && props.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in props.EnumerateObject())
                        {
                            string propName = prop.Name.ToLower(); // normalized keys
                            string propValue = prop.Value.ValueKind switch
                            {
                                JsonValueKind.String => prop.Value.GetString(),
                                JsonValueKind.Number => prop.Value.ToString(),
                                JsonValueKind.True => "true",
                                JsonValueKind.False => "false",
                                _ => null
                            };

                            if (propName != null && propValue != null)
                            {
                                def.Properties[propName] = propValue;
                            }
                        }
                    }

                    // buyPrice array
                    if (el.TryGetProperty("buyPrice", out var buyPriceEl) && buyPriceEl.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var bp in buyPriceEl.EnumerateArray())
                        {
                            if (bp.ValueKind != JsonValueKind.Object)
                                continue;

                            int currencyId = bp.TryGetProperty("currencyId", out var cid) && cid.ValueKind == JsonValueKind.Number
                                ? cid.GetInt32()
                                : 101;
                            float value = bp.TryGetProperty("value", out var val) && val.ValueKind == JsonValueKind.Number
                                ? (float)val.GetDouble()
                                : 0f;

                            def.BuyPrice.Add(new CurrencyAmount
                            {
                                CurrencyId = currencyId,
                                Value = value
                            });
                        }
                    }

                    list.Add(def);
                }

                // Verify and FORCE mandatory items for the Shop UI
                var mandatoryItems = new List<(int id, string name, string collection, int currency, float price)>
                {
                    (301, "Origin Case", "origin", 102, 100),
                    (302, "Furious Case", "furious", 102, 100),
                    (303, "Rival Case", "rival", 102, 100),
                    (304, "Fable Case", "fable", 102, 100),
                    (401, "Origin Box", "origin", 101, 100),
                    (402, "Furious Box", "furious", 101, 100),
                    (403, "Rival Box", "rival", 101, 100),
                    (404, "Fable Box", "fable", 101, 100),
                    (701, "Halloween Sticker Pack", "halloween_2019", 102, 5000)
                };

                foreach (var item in mandatoryItems)
                {
                    if (!list.Any(d => d.Id == item.id))
                    {
                        Console.WriteLine($"[Inventory] Mandatory Item ID {item.id} was missing! Injecting fallback.");
                        var def = new ServerInventoryItemDefinition { Id = item.id, DisplayName = item.name };
                        def.Properties["collection"] = item.collection;
                        def.BuyPrice.Add(new CurrencyAmount { CurrencyId = item.currency, Value = item.price });
                        list.Add(def);
                    }
                }

                Console.WriteLine($"[Inventory] Loaded {list.Count} item definitions. (IDs: {string.Join(", ", list.Select(x => x.Id).OrderBy(x => x).Take(15))}...)");
                _cachedJsonDefinitions = list;
                return _cachedJsonDefinitions;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Inventory] Failed to load JSON definitions: {ex}");
                _cachedJsonDefinitions = new List<ServerInventoryItemDefinition>();
                return _cachedJsonDefinitions;
            }
        }

        private HashSet<int> TryReadClientInventoryIds()
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "Scripts", "Axlebolt", "Standoff", "Main", "Inventory", "InventoryId.cs"));
                if (!File.Exists(path))
                {
                    return null;
                }

                var text = File.ReadAllText(path);
                var matches = Regex.Matches(text, @"=\s*(\d+)\s*,");
                var ids = new HashSet<int>();
                foreach (Match m in matches)
                {
                    if (int.TryParse(m.Groups[1].Value, out var id) && id > 0)
                    {
                        ids.Add(id);
                    }
                }

                return ids.Count > 0 ? ids : null;
            }
            catch
            {
                return null;
            }
        }

        private static bool IsEnabledDefinition(ServerInventoryItemDefinition def)
        {
            return !def.Properties.TryGetValue("enabled", out var v) || !string.Equals(v, "false", StringComparison.OrdinalIgnoreCase);
        }

        private static float GetDefaultMarketPrice(ServerInventoryItemDefinition def)
        {
            // If the definition has buyPrice in coins (101), use it as a hint; otherwise use value-based fallback.
            try
            {
                var coinPrice = def.BuyPrice?.FirstOrDefault(x => x.CurrencyId == 101);
                if (coinPrice != null && coinPrice.Value > 0)
                {
                    return coinPrice.Value;
                }

                if (def.Properties != null && def.Properties.TryGetValue("value", out var valueStr) && int.TryParse(valueStr, out var v))
                {
                    return v switch
                    {
                        1 => 5f,
                        2 => 10f,
                        3 => 25f,
                        4 => 50f,
                        5 => 100f,
                        6 => 200f,
                        _ => 25f
                    };
                }
            }
            catch
            {
                // ignore
            }

            return 25f;
        }

        private void NormalizeStats(Stats stats, UserDoc user = null)
        {
            int ClampInt(int v)
            {
                if (v < 0) return 0;
                // Avoid insane UI values if DB contains garbage
                if (v > 10_000_000) return 0;
                return v;
            }

            int GetInt(string name)
            {
                var s = stats.Stat.FirstOrDefault(x => x.Name == name);
                return s == null ? 0 : ClampInt(s.IntValue);
            }

            void SetInt(string name, int value)
            {
                var s = stats.Stat.FirstOrDefault(x => x.Name == name);
                if (s == null)
                {
                    stats.Stat.Add(new PlayerStat { Name = name, IntValue = ClampInt(value), Type = StatDefType.Int });
                }
                else
                {
                    // For global stats like shots/hits, we want to ensure we only increase, never decrease
                    if (name == "shots" || name == "hits" || name == "kills" || name == "deaths")
                    {
                        if (ClampInt(value) > s.IntValue)
                            s.IntValue = ClampInt(value);
                    }
                    else
                    {
                        s.IntValue = ClampInt(value);
                    }
                    s.Type = StatDefType.Int;
                }
            }

            void SetFloat(string name, float value)
            {
                var s = stats.Stat.FirstOrDefault(x => x.Name == name);
                if (s == null)
                {
                    stats.Stat.Add(new PlayerStat { Name = name, FloatValue = value, Type = StatDefType.Float });
                }
                else
                {
                    s.FloatValue = value;
                    s.Type = StatDefType.Float;
                }
            }

            void NormalizeMode(string modePrefix)
            {
                var shots = GetInt(modePrefix + "shots");
                var hits = GetInt(modePrefix + "hits");
                if (hits > shots) hits = shots;
                SetInt(modePrefix + "shots", shots);
                SetInt(modePrefix + "hits", hits);

                var kills = GetInt(modePrefix + "kills");
                var headshots = GetInt(modePrefix + "headshots");
                if (headshots > kills) headshots = kills;
                SetInt(modePrefix + "kills", kills);
                SetInt(modePrefix + "headshots", headshots);

                SetInt(modePrefix + "deaths", GetInt(modePrefix + "deaths"));
                SetInt(modePrefix + "assists", GetInt(modePrefix + "assists"));
                SetInt(modePrefix + "damage", GetInt(modePrefix + "damage"));
                SetInt(modePrefix + "games_played", GetInt(modePrefix + "games_played"));
            }

            // Global stats
            var totalKills = GetInt("kills");
            var totalDeaths = GetInt("deaths");
            var totalAssists = GetInt("assists");
            var totalHeadshots = GetInt("headshots");
            var totalShots = GetInt("shots");
            var totalHits = GetInt("hits");

            // Normalize supported game modes and accumulate global
            string[] modes = { "deathmatch_", "defuse_" };
            foreach (var m in modes)
            {
                NormalizeMode(m);
                // We no longer overwrite totalShots/totalHits from modes, 
                // because shots are often stored directly in "shots" or "hits" 
                // or in specific mode prefixes. We want to ensure we don't LOSE them.
                var modeShots = GetInt(m + "shots");
                var modeHits = GetInt(m + "hits");
                
                // If mode-specific stats exist but global is lower, sync them
                if (modeShots > totalShots) totalShots = modeShots;
                if (modeHits > totalHits) totalHits = modeHits;
            }

            SetInt("shots", totalShots);
            SetInt("hits", totalHits);

            // Playtime: Custom logic for 0.1h = 10 min, 0.6h = 1 hour (as requested)
            // This means 1 hour is 60 units of 1 minute each, but displayed as 0.6.
            // 1 minute = 0.01 units in this display logic.
            // 60 minutes = 0.60 units = 1 hour in UI? 
            // Actually, if the user wants 0.6 to be 1 hour, then 1 unit in UI = 100 minutes?
            // Let's stick to: 0.1 = 10 min, 1.0 = 100 min. 1 hour = 60 min = 0.6.
            var timeInSeconds = GetInt("time_in_game");
            if (timeInSeconds == 0 && user != null) timeInSeconds = (int)user.TimeInGame;
            
            // 1 minute = 0.01 in UI
            // 10 minutes = 0.10 in UI
            // 60 minutes = 0.60 in UI
            float timeInCustomHours = (timeInSeconds / 60) * 0.01f;
            SetFloat("time_in_game_hours", timeInCustomHours);
            SetInt("time_in_game", timeInSeconds);

            // Level bounds
            var levelId = GetInt("level_id");
            if (levelId < 0) levelId = 0;
            if (levelId > 99) levelId = 99;
            SetInt("level_id", levelId);
            SetInt("level", levelId);

            var levelXp = GetInt("level_xp");
            if (levelXp < 0) levelXp = 0;
            // 1000 XP per level
            if (levelXp >= 1000)
            {
                // If XP is more than 1000 but level is still low, it means we need to trigger LevelFromXp
                // However, NormalizeStats is just for UI/View, so we just clamp for display
                levelXp %= 1000;
            }
            SetInt("level_xp", levelXp);
        }

        private async Task<RpcResponse> HandleGameEventAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "getCurrentGameEvents":
                    // Minimal current event + passes, required for BattlePass init.
                    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var ev = new CurrentGameEvent
                    {
                        Id = "season_1",
                        Code = "season_1",
                        DateSince = now - 86400,
                        DateUntil = now + 86400 * 365,
                        DurationDays = 365,
                        CurrentDay = 1,
                        Points = 0
                    };

                    var emptyReward = new RewardInfo();
                    var level1Reward = new RewardInfo();
                    level1Reward.Currencies.Add(new CurrencyAmount { CurrencyId = 101, Value = 1 });

                    GamePassLevel[] levels = new[]
                    {
                        new GamePassLevel { Level = 0, MinPoints = 0, Reward = emptyReward },
                        new GamePassLevel { Level = 1, MinPoints = 100, Reward = level1Reward }
                    };

                    var freePass = new GamePass
                    {
                        Id = "free",
                        Code = "free",
                        KeyItemDefinitionId = 0,
                        CurrentLevel = 0
                    };
                    freePass.Levels.Add(levels);

                    var goldPass = new GamePass
                    {
                        Id = "gold",
                        Code = "gold",
                        KeyItemDefinitionId = 602,
                        CurrentLevel = 0
                    };
                    goldPass.Levels.Add(levels);

                    ev.GamePasses.Add(freePass);
                    ev.GamePasses.Add(goldPass);

                    var resp = new GetCurrentGameEventsResponse();
                    resp.GameEvents.Add(ev);
                    return CreateSuccessResponse(request.Id, resp);

                case "getCurrentChallenges":
                    // Client can work with an empty list, but must not throw.
                    return CreateSuccessResponse(request.Id, new GetCurrentChallengesResponse());

                case "setChallengeProgress":
                    // Return a minimal response that doesn't break inventory ops.
                    return CreateSuccessResponse(request.Id, new ProgressChallengeResponse
                    {
                        Completed = false,
                        ChallengePoints = 0,
                        EventPoints = 0
                    });

                case "progressGameEvent":
                    return CreateSuccessResponse(request.Id, new ProgressGameEventResponse());

                case "saveChallenge":
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleVersionAsync(RpcRequest request)
        {
            if (request.MethodName == "checkVersion")
            {
                var srvSettings = await _mongo.GetSettingsAsync();

                string clientVersion = null;
                try
                {
                    if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                        clientVersion = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                }
                catch { }

                Console.WriteLine($"[Version] checkVersion: versionEnabled={srvSettings.VersionEnabled} required={srvSettings.GameVersion} client={clientVersion}");

                // Регистрируем версию как отдельный документ в коллекции "version"
                if (!string.IsNullOrWhiteSpace(clientVersion))
                    await _mongo.RegisterVersionAsync(clientVersion);

                // Проверяем разрешена ли эта версия (enabled=false → blocked)
                bool blocked = false;
                if (!string.IsNullOrWhiteSpace(clientVersion))
                    blocked = !await _mongo.IsVersionAllowedAsync(clientVersion);

                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = blocked });
            }

            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = false });
        }

        private async Task<RpcResponse> HandleGameSettingsAsync(RpcRequest request, string currentTkt = null)
        {
            if (request.MethodName == "getGameSettings")
            {
                var srvSettings = await _mongo.GetSettingsAsync();

                // Определяем версию текущего клиента из сессии
                string clientVersion = null;
                if (!string.IsNullOrEmpty(currentTkt))
                    _clientVersions.TryGetValue(currentTkt, out clientVersion);

                // Если версия не известна — берём из global как fallback
                if (string.IsNullOrWhiteSpace(clientVersion))
                    clientVersion = srvSettings.GameVersion;

                // Проверяем разрешена ли версия клиента
                bool versionBlocked = !await _mongo.IsVersionAllowedAsync(clientVersion);

                var settings = new List<GameSetting>
                {
                    new GameSetting
                    {
                        Key = "regions",
                        Value = "{\"Servers\":[{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"test\",\"DisplayName\":\"Test\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true},{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"testfra01\",\"DisplayName\":\"Test Fra01\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true},{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"testfra02\",\"DisplayName\":\"Test Fra02\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true},{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"testfra03\",\"DisplayName\":\"Test Fra03\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true},{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"testfra04\",\"DisplayName\":\"Test Fra04\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true},{\"Dns\":\"YOUR_SERVER_IP\",\"Location\":\"testfra05\",\"DisplayName\":\"Test Fra05\",\"Ip\":\"YOUR_SERVER_IP\",\"Online\":true,\"Enabled\":true}]}"
                    },
                    new GameSetting { Key = "ranked_client_matchmaking_config", Value = "{\"SearchRange\":[0,20,40,60,100,150,200],\"CalibrationMathces\":10,\"PlayerTTL\":100000,\"BanDurations\":[900,3600,43200,86400],\"SearchRoomCreateInterval\":4,\"SearchRoomSizeStep\":2,\"SearchStepDelay\":1000}" },
                    new GameSetting { Key = "virtual_space_packages", Value = "" },
                    new GameSetting { Key = "game_event_configs", Value = "[{\"Id\":\"season_1\",\"StartTime\":0,\"EndTime\":2000000000,\"Type\":1}]" },
                    new GameSetting
                    {
                        Key = "version_enabled",
                        Value = versionBlocked ? "true" : "false"
                    },
                    new GameSetting
                    {
                        Key = "required_version",
                        Value = clientVersion ?? ""
                    }
                };

                return CreateSuccessResponseArray(request.Id, settings);
            }

            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
        }

        private async Task SendEventAsync(string ticket, string listenerName, string eventName, IMessage eventData)
        {
            if (_clientStreams.TryGetValue(ticket, out var stream))
            {
                var ev = new EventResponse
                {
                    ListenerName = listenerName,
                    EventName = eventName,
                    Params = { new BinaryValue { One = eventData.ToByteString() } }
                };

                var responseMessage = new ResponseMessage
                {
                    EventResponse = ev
                };

                NetworkPacket.WritePacket(stream, responseMessage);
            }
        }

        private async Task<RpcResponse> HandleAuthAsync(RpcRequest request)
        {
            if (request.MethodName == "auth" || request.MethodName == "protoAuth")
            {
                string token = null;
                try
                {
                    Console.WriteLine($"[RPC] Auth request: {request.MethodName}, Params: {request.Params.Count}");
                    for (int i = 0; i < request.Params.Count; i++)
                    {
                        var p = request.Params[i];
                        Console.WriteLine($"  Param[{i}]: Length={p.One.Length}, IsEmpty={p.One.IsEmpty}");
                    }

                    // For "auth" method: params[3] is the token string
                    if (request.MethodName == "auth" && request.Params.Count > 3 && request.Params[3].One != null && !request.Params[3].One.IsEmpty)
                    {
                        var tokenBytes = request.Params[3].One;
                        try 
                        {
                            token = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(tokenBytes).Value;
                            Console.WriteLine($"  Parsed token (proto string): '{token}'");
                        }
                        catch 
                        {
                            token = tokenBytes.ToStringUtf8();
                            Console.WriteLine($"  Parsed token (raw utf8): '{token}'");
                        }
                    }
                    
                    // If client does not provide any token — reject, never fall back to shared "guest"
                    if (string.IsNullOrEmpty(token))
                    {
                        Console.WriteLine("[Auth] No token provided — rejecting auth request");
                        return new RpcResponse
                        {
                            Id = request.Id,
                            Exception = new Axlebolt.RpcSupport.Protobuf.Exception { Code = 401 }
                        };
                    }

                    UserDoc user = null;
                    user = await _mongo.GetUserByUidAsync(token);
                    if (user != null)
                    {
                        Console.WriteLine($"[Auth] Found existing user by token: {user.Name} (UID: {user.Uid})");
                    }
                    else
                    {
                        Console.WriteLine($"[Auth] User NOT found in DB for token/UID: '{token}'");
                    }

                    if (user == null)
                    {
                        // Create a new user if not found
                        user = new UserDoc
                        {
                            Uid = token,
                            Name = "Player",
                            RegistrationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                            AvatarId = 0,
                            IsBanned = false,
                            CustomId = null
                        };
                        await _mongo.CreateUserAsync(user);
                        
                        // If we didn't have a token, use the generated ID as the UID
                        if (string.IsNullOrEmpty(user.Uid))
                        {
                            user.Uid = user.Id.ToString();
                        }
                        user.Name = $"Player_{user.Id}";
                        await _mongo.UpdateUserAsync(user);
                        
                        token = user.Uid;
                        Console.WriteLine($"[Auth] Created NEW user: {user.Name} (ID: {user.Id}, UID: {user.Uid})");
                    }
                    else
                    {
                        token = user.Uid;
                        Console.WriteLine($"[Auth] Logged in existing user: {user.Name} (UID: {user.Uid})");
                    }

                    // Проверяем бан
                    if (user.IsBanned)
                    {
                        Console.WriteLine($"[Auth] User {user.Name} (ID: {user.Id}) is BANNED.");
                        return new RpcResponse
                        {
                            Id = request.Id,
                            Exception = new Axlebolt.RpcSupport.Protobuf.Exception
                            {
                                Code = 9999,
                                Params = { { "reason", "banned" }, { "banCode", "1" }, { "uid", user.Uid ?? "" } }
                            }
                        };
                    }

                    // Проверяем версию игры
                    var srvSettings = await _mongo.GetSettingsAsync();
                    string clientVersion = null;
                    try
                    {
                        if (request.Params.Count > 2 && request.Params[2].One != null && !request.Params[2].One.IsEmpty)
                            clientVersion = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[2].One).Value;
                    }
                    catch { }

                    // Регистрируем версию клиента как отдельный документ в коллекции "version"
                    if (!string.IsNullOrWhiteSpace(clientVersion))
                        await _mongo.RegisterVersionAsync(clientVersion);

                    // Проверяем разрешена ли эта версия (enabled=false → диалог обновления)
                    bool versionAllowed = await _mongo.IsVersionAllowedAsync(clientVersion);
                    if (!versionAllowed)
                    {
                        Console.WriteLine($"[Auth] *** VERSION DISABLED *** user={user.Name} clientVersion={clientVersion}");
                        var blockResponse = new RpcResponse
                        {
                            Id = request.Id,
                            Exception = new Axlebolt.RpcSupport.Protobuf.Exception { Code = 2000 }
                        };
                        blockResponse.Exception.Params["version"] = clientVersion ?? "";
                        return blockResponse;
                    }

                    Console.WriteLine($"[Auth] *** ACCESS GRANTED *** user={user.Name} clientVersion={clientVersion} versionEnabled={srvSettings.VersionEnabled}");
                    _activeSessions[token] = user;
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = token });
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"[Auth] Critical error in HandleAuthAsync: {ex}");
                    string fallback = token ?? "guest_" + Guid.NewGuid().ToString("N");
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = fallback });
                }
            }
            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
        }

        private async Task<RpcResponse> HandleExternalAuthAsync(RpcRequest request, string prefix)
        {
            // These auth services are used by the stock client. We don't validate providers here:
            // we just derive a stable UID from the payload so the same device logs into the same user.
            if (request.MethodName != "auth" && request.MethodName != "protoAuth" && request.MethodName != "protoAuthSecured")
            {
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = $"{prefix}_guest" });
            }

            try
            {
                string token = null;
                if (request.MethodName == "auth" && request.Params.Count > 3 && request.Params[3].One != null && !request.Params[3].One.IsEmpty)
                {
                    try { token = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[3].One).Value; } catch { token = request.Params[3].One.ToStringUtf8(); }
                }
                else if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                {
                    var bytes = request.Params[0].One.ToByteArray();
                    var hash = SHA256.HashData(bytes);
                    token = $"{prefix}_{Convert.ToHexString(hash).Substring(0, 16).ToLowerInvariant()}";
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    token = $"{prefix}_guest";
                }

                // Reuse the same user creation/login logic as TestAuthRemoteService.
                var authRequest = new RpcRequest
                {
                    Id = request.Id,
                    ServiceName = "TestAuthRemoteService",
                    MethodName = "auth"
                };
                authRequest.Params.Add(new BinaryValue());
                authRequest.Params.Add(new BinaryValue());
                authRequest.Params.Add(new BinaryValue());
                authRequest.Params.Add(new BinaryValue { One = new Axlebolt.RpcSupport.Protobuf.String { Value = token }.ToByteString() });
                return await HandleAuthAsync(authRequest);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Auth] External auth error ({request.ServiceName}.{request.MethodName}): {ex.Message}");
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = $"{prefix}_guest" });
            }
        }

        private async Task<RpcResponse> HandleChatAsync(RpcRequest request, string ticket)
        {
            var user = await GetCurrentUserAsync(ticket);
            if (user == null || user.Id <= 0)
            {
                // Without a real userId we can't persist chat; keep client stable.
                switch (request.MethodName)
                {
                    case "getFriendMsgs":
                    case "getFriendMsgsByPage":
                    case "getFriendMsgsByOffset":
                    case "getGroupMsgs":
                        return CreateSuccessResponseArray(request.Id, Array.Empty<UserMessage>());
                    case "getChatUsers":
                        return CreateSuccessResponseArray(request.Id, Array.Empty<ChatUser>());
                    case "getUnreadChatUsersCount":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = 0 });
                    default:
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                }
            }

            static string ChatId(long a, long b)
            {
                var min = Math.Min(a, b);
                var max = Math.Max(a, b);
                return $"u_{min}_{max}";
            }

            switch (request.MethodName)
            {
                case "getGroupMsgs":
                case "getFriendMsgs":
                case "getFriendMsgsByPage":
                case "getFriendMsgsByOffset":
                    {
                        // We support only friend chats here; group msgs are stubbed empty.
                        if (request.MethodName == "getGroupMsgs")
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<UserMessage>());
                        }

                        string friendIdStr = null;
                        try { friendIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        if (!long.TryParse(friendIdStr, out var friendId) || friendId <= 0)
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<UserMessage>());
                        }

                        int skip = 0;
                        int limit = 50;

                        try
                        {
                            if (request.MethodName == "getFriendMsgs")
                            {
                                // params: friendId, page, size
                                skip = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value * Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                                limit = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            }
                            else if (request.MethodName == "getFriendMsgsByPage")
                            {
                                // params: friendId, Page {page, pageSize}
                                var page = Page.Parser.ParseFrom(request.Params[1].One);
                                skip = page.Page_ * page.Size;
                                limit = page.Size;
                            }
                            else if (request.MethodName == "getFriendMsgsByOffset")
                            {
                                // params: friendId, Offset {offset, length}
                                var off = Offset.Parser.ParseFrom(request.Params[1].One);
                                skip = off.Offset_;
                                limit = off.Length;
                            }
                        }
                        catch
                        {
                            // keep defaults
                        }

                        if (limit <= 0) limit = 50;
                        if (limit > 200) limit = 200;
                        if (skip < 0) skip = 0;

                        var chatId = ChatId(user.Id, friendId);
                        var docs = await _mongo.GetChatMessagesAsync(chatId, skip, limit);
                        // DB is sorted desc; client expects chronological in UI lists usually.
                        docs.Reverse();

                        var msgs = new List<UserMessage>(docs.Count);
                        foreach (var d in docs)
                        {
                            msgs.Add(new UserMessage
                            {
                                SenderId = d.SenderUserId.ToString(),
                                Message = d.Message ?? "",
                                Timestamp = d.TimestampMs,
                                IsRead = d.IsRead
                            });
                        }
                        return CreateSuccessResponseArray(request.Id, msgs);
                    }

                case "getChatUsers":
                    {
                        var summary = await _mongo.GetChatUsersSummaryAsync(user.Id, limit: 100);
                        var list = new List<ChatUser>(summary.Count);
                        foreach (var item in summary)
                        {
                            var last = item.lastMsg;
                            var otherId = last.SenderUserId == user.Id ? last.ReceiverUserId : last.SenderUserId;
                            var otherUser = await _mongo.GetUserByIdAsync(otherId);
                            list.Add(new ChatUser
                            {
                                Player = new PlayerFriend
                                {
                                    Player = ToProtoPlayer(otherUser ?? new UserDoc { Id = otherId, Name = "Player_" + otherId }),
                                    RelationshipStatus = RelationshipStatus.None
                                },
                                Message = last.Message ?? "",
                                Timestamp = last.TimestampMs,
                                UnreadMsgsCount = item.unread
                            });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "getUnreadChatUsersCount":
                    {
                        var count = await _mongo.CountUnreadChatsAsync(user.Id);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = count });
                    }

                case "sendFriendMsg":
                    {
                        try
                        {
                            var friendIdStr = request.Params.Count > 0 ? Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value : null;
                            var msg = request.Params.Count > 1 ? Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value : "";

                            if (long.TryParse(friendIdStr, out var friendId) && friendId > 0)
                            {
                                var now = NowMs();
                                var chatId = ChatId(user.Id, friendId);
                                await _mongo.InsertChatMessageAsync(new ChatMessageDoc
                                {
                                    ChatId = chatId,
                                    SenderUserId = user.Id,
                                    ReceiverUserId = friendId,
                                    Message = msg ?? "",
                                    TimestampMs = now,
                                    IsRead = false
                                });

                                // Push onMsgFromFriend to recipient (if online)
                                var targetTicket = FindTicketByPlayerId(friendId.ToString());
                                if (!string.IsNullOrWhiteSpace(targetTicket) && _clientStreams.TryGetValue(targetTicket, out var stream) && stream != null)
                                {
                                    var userMessage = new UserMessage
                                    {
                                        SenderId = user.Id.ToString(),
                                        Message = msg ?? "",
                                        Timestamp = now,
                                        IsRead = false
                                    };
                                    var evtRecipient = new Axlebolt.RpcSupport.Protobuf.EventResponse
                                    {
                                        ListenerName = "MessagesRemoteEventListener",
                                        EventName = "onMsgFromFriend"
                                    };
                                    evtRecipient.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = userMessage.ToByteString() });
                                    NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evtRecipient });
                                }

                                // Push onChatUpdated back to SENDER so chat list refreshes immediately
                                var senderTicket = FindTicketByPlayerId(user.Id.ToString());
                                if (!string.IsNullOrWhiteSpace(senderTicket) && _clientStreams.TryGetValue(senderTicket, out var senderStream) && senderStream != null)
                                {
                                    var sentMsg = new UserMessage
                                    {
                                        SenderId = user.Id.ToString(),
                                        Message = msg ?? "",
                                        Timestamp = now,
                                        IsRead = true
                                    };
                                    var evtSender = new Axlebolt.RpcSupport.Protobuf.EventResponse
                                    {
                                        ListenerName = "MessagesRemoteEventListener",
                                        EventName = "onChatUpdated"
                                    };
                                    evtSender.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = sentMsg.ToByteString() });
                                    NetworkPacket.WritePacket(senderStream, new ResponseMessage { EventResponse = evtSender });
                                }
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "sendGlobalChatMessage":
                    {
                        try
                        {
                            var topic = request.Params.Count > 0 ? Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value : "default";
                            var msg = request.Params.Count > 1 ? Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value : "";

                            var now = NowMs();
                            await _mongo.InsertGlobalChatMessageAsync(new GlobalChatMessageDoc
                            {
                                Topic = topic ?? "default",
                                SenderUserId = user.Id,
                                Message = msg ?? "",
                                TimestampMs = now
                            });

                            var global = new GlobalChatUserMessage
                            {
                                Sender = ToProtoPlayer(user),
                                Message = msg ?? "",
                                Timestamp = now
                            };

                            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
                            {
                                ListenerName = "GlobalChatRemoteEventListener",
                                EventName = "onIncomingGlobalChatMessage"
                            };
                            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = global.ToByteString() });
                            PublishEvent("global_chat_" + (topic ?? "default"), evt);
                        }
                        catch
                        {
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "getGlobalChatMessages":
                    {
                        try
                        {
                            var topic = request.Params.Count > 0 ? Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value : "default";
                            int page = 0, size = 50;
                            try
                            {
                                page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                                size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            }
                            catch { }
                            if (size <= 0) size = 50;
                            if (size > 100) size = 100;
                            if (page < 0) page = 0;

                            var docs = await _mongo.GetGlobalChatMessagesAsync(topic ?? "default", page * size, size);
                            docs.Reverse();
                            var list = new List<GlobalChatUserMessage>(docs.Count);
                            foreach (var d in docs)
                            {
                                var sender = await _mongo.GetUserByIdAsync(d.SenderUserId);
                                list.Add(new GlobalChatUserMessage
                                {
                                    Sender = ToProtoPlayer(sender ?? new UserDoc { Id = d.SenderUserId, Name = "Player_" + d.SenderUserId }),
                                    Message = d.Message ?? "",
                                    Timestamp = d.TimestampMs
                                });
                            }
                            return CreateSuccessResponseArray(request.Id, list);
                        }
                        catch
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<GlobalChatUserMessage>());
                        }
                    }

                case "sendGroupMsg":
                case "readFriendMsgs":
                    {
                        try
                        {
                            var friendIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(friendIdStr, out var friendId) && friendId > 0)
                            {
                                await _mongo.MarkChatAsReadAsync(ChatId(user.Id, friendId), user.Id);
                            }
                        }
                        catch
                        {
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "readGroupMsgs":
                case "deleteFriendMsgs":
                case "deleteGroupMsgs":
                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleAvatarAsync(RpcRequest request, string ticket)
        {
            if (request.MethodName != "getAvatars")
            {
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }

            try
            {
                // Param[0] is string[] avatarIds (RpcSupport.StringArray encoded as BinaryValue.One)
                var ids = new List<string>();
                if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                {
                    try
                    {
                        var arr = Axlebolt.RpcSupport.Protobuf.StringArray.Parser.ParseFrom(request.Params[0].One);
                        ids.AddRange(arr.Value);
                    }
                    catch
                    {
                        // ignore parse issues; return empty
                    }
                }

                var list = new List<AvatarBinary>();
                foreach (var id in ids.Distinct())
                {
                    Console.WriteLine($"[Avatar] getAvatars: looking for avatar_{id}");
                    var file = await _mongo.GetStorageFileAsync(0, "avatar_" + id);

                    // Fallback: если не нашли по id, попробуем найти по числовому userId
                    if ((file == null || file.Bytes == null) && long.TryParse(id, out var numId) && numId > 0)
                    {
                        // Ищем среди всех файлов с именем avatar_<numId>
                        file = await _mongo.GetStorageFileAsync(numId, "avatar_" + numId);
                        if (file == null || file.Bytes == null)
                            file = await _mongo.GetStorageFileAsync(0, "avatar_" + numId);
                    }

                    if (file != null && file.Bytes != null)
                    {
                        Console.WriteLine($"[Avatar] getAvatars: found avatar_{id}, size={file.Bytes.Length}");
                        list.Add(new AvatarBinary { Id = id ?? "", Avatar = ByteString.CopyFrom(file.Bytes) });
                    }
                    else
                    {
                        Console.WriteLine($"[Avatar] getAvatars: NOT found avatar_{id}");
                        list.Add(new AvatarBinary { Id = id ?? "", Avatar = ByteString.Empty });
                    }
                }

                return CreateSuccessResponseArray(request.Id, list);
            }
            catch
            {
                return CreateSuccessResponseArray(request.Id, Array.Empty<AvatarBinary>());
            }
        }

        private async Task<RpcResponse> HandleStorageAsync(RpcRequest request, string ticket)
        {
            var user = await GetCurrentUserAsync(ticket);
            if (user == null || user.Id <= 0)
            {
                // Keep client stable even without persistence.
                switch (request.MethodName)
                {
                    case "readAllFiles":
                        return CreateSuccessResponseArray(request.Id, Array.Empty<Storage>());
                    case "readFile":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.ByteArray { Value = ByteString.Empty });
                    default:
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                }
            }

            switch (request.MethodName)
            {
                case "writeFile":
                    try
                    {
                        var filename = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                        var bytes = Axlebolt.RpcSupport.Protobuf.ByteArray.Parser.ParseFrom(request.Params[1].One).Value.ToByteArray();
                        if (!string.IsNullOrWhiteSpace(filename))
                        {
                            await _mongo.UpsertStorageFileAsync(new StorageFileDoc
                            {
                                UserId = user.Id,
                                Filename = filename,
                                Bytes = bytes ?? Array.Empty<byte>(),
                                UpdatedAtMs = NowMs()
                            });
                        }
                    }
                    catch
                    {
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "deleteFile":
                    try
                    {
                        var filename = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                        if (!string.IsNullOrWhiteSpace(filename))
                        {
                            await _mongo.DeleteStorageFileAsync(user.Id, filename);
                        }
                    }
                    catch
                    {
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "readAllFiles":
                    {
                        var list = new List<Storage>();
                        var docs = await _mongo.GetAllStorageFilesAsync(user.Id);
                        foreach (var d in docs)
                        {
                            list.Add(new Storage { Filename = d.Filename ?? "", File = ByteString.CopyFrom(d.Bytes ?? Array.Empty<byte>()) });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "readFile":
                    try
                    {
                        var filename = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                        var doc = await _mongo.GetStorageFileAsync(user.Id, filename);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.ByteArray { Value = ByteString.CopyFrom(doc?.Bytes ?? Array.Empty<byte>()) });
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.ByteArray { Value = ByteString.Empty });
                    }

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private Task<RpcResponse> HandleAnalyticsAsync(RpcRequest request, string ticket)
        {
            // We accept analytics events but do not store them.
            return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }));
        }

        private Task<RpcResponse> HandleAdsAsync(RpcRequest request, string ticket)
        {
            // giveAdReward: no-op
            return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }));
        }

        private Task<RpcResponse> HandleHackDetectionAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "systemTime":
                    return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = NowMs() }));
                case "hackDetection":
                default:
                    return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }));
            }
        }

        private Task<RpcResponse> HandleGroupAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "createGroup":
                case "joinGroup":
                    return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.Bolt.Protobuf.Group { Id = Guid.NewGuid().ToString("N") }));
                case "leaveGroup":
                default:
                    return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }));
            }
        }

        private async Task<RpcResponse> HandleAccountLinkAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "createLinkTicket":
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = "link_" + Guid.NewGuid().ToString("N") });
                case "getPlayerByTicket":
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        return CreateSuccessResponse(request.Id, ToProtoPlayer(user));
                    }
                case "linkAccount":
                case "unlinkAccount":
                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleInAppAsync(RpcRequest request, string ticket)
        {
            if (request.MethodName != "buyInApp")
            {
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }

            var user = await GetCurrentUserAsync(ticket);
            var inv = await LoadOrCreateInventoryAsync(user);
            return CreateSuccessResponse(request.Id, inv);
        }

        private async Task<RpcResponse> HandleGameServerAsync(RpcRequest request, string ticket)
        {
            // Minimal support for offline / private server flows.
            switch (request.ServiceName)
            {
                case "GameServerStatsRemoteService":
                    switch (request.MethodName)
                    {
                        case "getStats":
                            {
                                var user = await GetCurrentUserAsync(ticket);
                                var statsDoc = await _mongo.GetStatsAsync(user.Id);
                                var stats = new Stats();
                                if (statsDoc != null)
                                {
                                    foreach (var s in statsDoc.Stats)
                                        stats.Stat.Add(new PlayerStat { Name = s.Name, IntValue = (int)s.IntValue, FloatValue = s.FloatValue, Type = (StatDefType)s.Type });
                                }
                                AddDefaultStats(stats);
                                NormalizeStats(stats, user);
                                return CreateSuccessResponse(request.Id, stats);
                            }
                        case "getPlayersStats":
                            return CreateSuccessResponseArray(request.Id, Array.Empty<PlayerStats>());
                        case "getAvatars":
                            {
                                try
                                {
                                    var ids = Axlebolt.RpcSupport.Protobuf.StringArray.Parser.ParseFrom(request.Params[0].One).Value;
                                    var result = new List<AvatarBinary>();
                                    foreach (var id in ids)
                                    {
                                        if (long.TryParse(id, out var pid))
                                        {
                                            var file = await _mongo.GetStorageFileAsync(pid, "avatar.jpg");
                                            if (file != null && file.Bytes != null)
                                            {
                                                result.Add(new AvatarBinary { Id = id, Avatar = ByteString.CopyFrom(file.Bytes) });
                                            }
                                        }
                                    }
                                    return CreateSuccessResponseArray(request.Id, result);
                                }
                                catch (System.Exception ex)
                                {
                                    Console.WriteLine($"[Friends] getAvatars error: {ex.Message}");
                                    return CreateSuccessResponseArray(request.Id, Array.Empty<AvatarBinary>());
                                }
                            }
                        case "storeStats":
                            return await HandlePlayerStatsAsync(request, ticket);
                        default:
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "GameServerPlayerRemoteService":
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "GameServerRemoteService":
                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private Task<RpcResponse> HandleGameSettingsPassthroughAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "getGameSettings":
                    return Task.FromResult(CreateSuccessResponseArray(request.Id, Array.Empty<GameSetting>()));
                case "updateGameSettings":
                case "deleteGameSettings":
                default:
                    return Task.FromResult(CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }));
            }
        }

        private async Task<RpcResponse> HandleMatchmakingAsync(RpcRequest request, string ticket)
        {
            async Task<PlayerFriend> PlayerFriendByIdAsync(string playerId)
            {
                if (long.TryParse(playerId, out var pid) && pid > 0)
                {
                    var u = await _mongo.GetUserByIdAsync(pid);
                    return new PlayerFriend { Player = ToProtoPlayer(u ?? new UserDoc { Id = pid, Name = "Player_" + pid, AvatarId = 0, RegistrationDate = 0, TimeInGame = 0 }), RelationshipStatus = RelationshipStatus.None };
                }
                return new PlayerFriend { Player = new Player { Id = playerId ?? "0", Uid = playerId ?? "0", Name = "Player_" + (playerId ?? "0"), AvatarId = "0" }, RelationshipStatus = RelationshipStatus.None };
            }

            void PublishToPlayer(string playerId, string eventName, params IMessage[] args)
            {
                var t = FindTicketByPlayerId(playerId);
                if (string.IsNullOrWhiteSpace(t)) return;
                if (!_clientStreams.TryGetValue(t, out var stream) || stream == null) return;

                var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
                {
                    ListenerName = "MatchmakingRemoteEventListener",
                    EventName = eventName
                };
                foreach (var a in args)
                {
                    evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = a.ToByteString() });
                }
                try { NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt }); } catch { }
            }

            void PublishToLobby(string lobbyId, string eventName, params IMessage[] args)
            {
                if (string.IsNullOrWhiteSpace(lobbyId)) return;

                var memberIds = new HashSet<string>();
                if (_lobbyMembersByLobbyId.TryGetValue(lobbyId, out var members))
                {
                    lock (members) foreach (var m in members) memberIds.Add(m);
                }
                if (_lobbySpectatorsByLobbyId.TryGetValue(lobbyId, out var spectators))
                {
                    lock (spectators) foreach (var s in spectators) memberIds.Add(s);
                }

                foreach (var pid in memberIds)
                {
                    PublishToPlayer(pid, eventName, args);
                }
            }

            Lobby EnsureLobby(string lobbyId)
            {
                if (string.IsNullOrWhiteSpace(lobbyId))
                {
                    lobbyId = Guid.NewGuid().ToString("N");
                }

                var lobby = _lobbiesById.GetOrAdd(lobbyId, id => new StandoffServer.Models.ServerLobby { Id = id, Name = "Lobby", MaxMembers = 10, Joinable = true, LobbyType = LobbyType.Private });
                _lobbyMembersByLobbyId.GetOrAdd(lobby.Id, _ => new HashSet<string>());
                _lobbySpectatorsByLobbyId.GetOrAdd(lobby.Id, _ => new HashSet<string>());
                _lobbyPlayerTypesByLobbyId.GetOrAdd(lobby.Id, _ => new ConcurrentDictionary<string, LobbyPlayerType>());
                return (Lobby)lobby;
            }

            async Task AddMember(string lobbyId, LobbyPlayerType playerType)
            {
                var user = await GetCurrentUserAsync(ticket);
                var pid = user?.Id.ToString() ?? "0";
                if (pid == "0") return;

                var set = _lobbyMembersByLobbyId.GetOrAdd(lobbyId, _ => new HashSet<string>());
                var spec = _lobbySpectatorsByLobbyId.GetOrAdd(lobbyId, _ => new HashSet<string>());
                var types = _lobbyPlayerTypesByLobbyId.GetOrAdd(lobbyId, _ => new ConcurrentDictionary<string, LobbyPlayerType>());

                lock (set) { set.Add(pid); }
                lock (spec) { spec.Remove(pid); }

                if (playerType == LobbyPlayerType.Spectator)
                {
                    lock (set) { set.Remove(pid); }
                    lock (spec) { spec.Add(pid); }
                }

                types[pid] = playerType;
                _playerLobbyByPlayerId[pid] = lobbyId;
            }

            async Task RemoveFromLobbyAsync(string playerId)
            {
                if (string.IsNullOrWhiteSpace(playerId)) return;
                if (!_playerLobbyByPlayerId.TryRemove(playerId, out var lobbyId) || string.IsNullOrWhiteSpace(lobbyId)) return;

                if (_lobbyMembersByLobbyId.TryGetValue(lobbyId, out var members))
                {
                    lock (members) members.Remove(playerId);
                }
                if (_lobbySpectatorsByLobbyId.TryGetValue(lobbyId, out var spectators))
                {
                    lock (spectators) spectators.Remove(playerId);
                }
                if (_lobbyPlayerTypesByLobbyId.TryGetValue(lobbyId, out var types))
                {
                    types.TryRemove(playerId, out _);
                }

                PublishToLobby(lobbyId, "onPlayerLeftLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = playerId });
            }

            async Task<Lobby> SnapshotLobbyAsync(string lobbyId)
            {
                var baseLobby = EnsureLobby(lobbyId);
                var snap = new Lobby
                {
                    Id = baseLobby.Id,
                    OwnerPlayerId = baseLobby.OwnerPlayerId ?? "",
                    Name = baseLobby.Name ?? "",
                    LobbyType = baseLobby.LobbyType,
                    Joinable = baseLobby.Joinable,
                    MaxMembers = baseLobby.MaxMembers,
                    MaxSpectators = baseLobby.MaxSpectators,
                    GameServer = baseLobby.GameServer,
                    PhotonGame = baseLobby.PhotonGame
                };

                foreach (var kv in baseLobby.Data)
                {
                    snap.Data[kv.Key] = kv.Value;
                }

                if (_lobbyMembersByLobbyId.TryGetValue(lobbyId, out var members))
                {
                    string[] ids;
                    lock (members) ids = members.ToArray();
                    bool ownerInMembers = false;
                    foreach (var id in ids)
                    {
                        if (id == baseLobby.OwnerPlayerId) ownerInMembers = true;
                        snap.Members.Add(await PlayerFriendByIdAsync(id));
                    }

                    // CRITICAL FIX: The client (BoltLobby.cs:30) throws if OwnerPlayerId is NOT in Members.
                    if (!ownerInMembers && !string.IsNullOrWhiteSpace(baseLobby.OwnerPlayerId))
                    {
                        snap.Members.Add(await PlayerFriendByIdAsync(baseLobby.OwnerPlayerId));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(baseLobby.OwnerPlayerId))
                {
                    // If no members list exists yet, at least add the owner.
                    snap.Members.Add(await PlayerFriendByIdAsync(baseLobby.OwnerPlayerId));
                }

                if (_lobbySpectatorsByLobbyId.TryGetValue(lobbyId, out var spectators))
                {
                    string[] ids;
                    lock (spectators) ids = spectators.ToArray();
                    foreach (var id in ids)
                    {
                        snap.Spectators.Add(await PlayerFriendByIdAsync(id));
                    }
                }

                return snap;
            }

            switch (request.MethodName)
            {
                case "createLobby":
                case "createLobbyWithSpectators":
                    {
                        var lobby = EnsureLobby(Guid.NewGuid().ToString("N"));
                        try
                        {
                            if (request.Params.Count > 0) lobby.Name = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? lobby.Name;
                            if (request.Params.Count > 1) lobby.LobbyType = (LobbyType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[1].One).Value;
                            if (request.Params.Count > 2) lobby.MaxMembers = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            if (request.MethodName == "createLobbyWithSpectators" && request.Params.Count > 3) lobby.MaxSpectators = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[3].One).Value;
                        }
                        catch { }

                        var u = await GetCurrentUserAsync(ticket);
                        lobby.OwnerPlayerId = u.Id.ToString();
                        lobby.Joinable = true;
                        await AddMember(lobby.Id, LobbyPlayerType.Member);
                        return CreateSuccessResponse(request.Id, await SnapshotLobbyAsync(lobby.Id));
                    }

                case "joinLobby":
                case "joinLobbyAs":
                    {
                        var lobbyId = "";
                        var playerType = LobbyPlayerType.Member;
                        try { lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        var lobby = EnsureLobby(lobbyId);
                        if (request.MethodName == "joinLobbyAs")
                        {
                            try { playerType = (LobbyPlayerType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[1].One).Value; } catch { }
                        }

                        if (!lobby.Joinable)
                        {
                            // Allow join if invited.
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            var invited = false;
                            if (pid != "0" && _lobbyInvitesByPlayerId.TryGetValue(pid, out var inv))
                            {
                                lock (inv) invited = inv.Any(i => i != null && i.LobbyId == lobby.Id);
                            }
                            if (!invited)
                            {
                                return CreateSuccessResponse(request.Id, await SnapshotLobbyAsync(lobby.Id));
                            }
                        }

                        await AddMember(lobby.Id, playerType);
                        var u2 = await GetCurrentUserAsync(ticket);
                        var pf = await PlayerFriendByIdAsync(u2.Id.ToString());
                        if (playerType == LobbyPlayerType.Spectator)
                        {
                            PublishToLobby(lobby.Id, "onNewSpectatorJoinedLobby", pf);
                        }
                        else
                        {
                            PublishToLobby(lobby.Id, "onNewPlayerJoinedLobby", pf);
                        }

                        return CreateSuccessResponse(request.Id, await SnapshotLobbyAsync(lobby.Id));
                    }

                case "getLobby":
                    {
                        var lobbyId = "";
                        try { lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        return CreateSuccessResponse(request.Id, await SnapshotLobbyAsync(lobbyId));
                    }

                case "leaveLobby":
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        await RemoveFromLobbyAsync(user?.Id.ToString() ?? "0");
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "getLobbyMembers":
                    {
                        var lobbyId = "";
                        try { lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        if (!_lobbyMembersByLobbyId.TryGetValue(lobbyId, out var set))
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<Player>());
                        }

                        string[] ids;
                        lock (set) { ids = set.ToArray(); }

                        var players = new List<Player>();
                        foreach (var id in ids)
                        {
                            if (long.TryParse(id, out var pid) && pid > 0)
                            {
                                var u = await _mongo.GetUserByIdAsync(pid);
                                players.Add(ToProtoPlayer(u ?? new UserDoc { Id = pid, Name = "Player_" + pid, AvatarId = 0, RegistrationDate = 0, TimeInGame = 0 }));
                            }
                        }
                        return CreateSuccessResponseArray(request.Id, players);
                    }

                case "requestLobbyList":
                    return CreateSuccessResponseArray(request.Id, _lobbiesById.Values.ToArray());

                case "requestInternetServerList":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<GameServer>());

                case "getGameServerDetails":
                case "getLobbyGameServer":
                    return CreateSuccessResponse(request.Id, new GameServerDetails());

                case "getGameServerPlayers":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<Player>());

                case "getInvitesToLobby":
                    {
                        var u = await GetCurrentUserAsync(ticket);
                        var pid = u?.Id.ToString() ?? "0";
                        if (pid == "0") return CreateSuccessResponseArray(request.Id, Array.Empty<LobbyInvite>());
                        if (!_lobbyInvitesByPlayerId.TryGetValue(pid, out var inv)) return CreateSuccessResponseArray(request.Id, Array.Empty<LobbyInvite>());
                        lock (inv)
                        {
                            return CreateSuccessResponseArray(request.Id, inv.ToArray());
                        }
                    }

                case "getLobbyOwner":
                    {
                        var lobbyId = "";
                        try { lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        var lobby = EnsureLobby(lobbyId);
                        if (long.TryParse(lobby.OwnerPlayerId, out var pid) && pid > 0)
                        {
                            var u = await _mongo.GetUserByIdAsync(pid);
                            return CreateSuccessResponse(request.Id, ToProtoPlayer(u ?? new UserDoc { Id = pid, Name = "Player_" + pid, AvatarId = 0, RegistrationDate = 0, TimeInGame = 0 }));
                        }
                        return CreateSuccessResponse(request.Id, new Player());
                    }

                case "getLobbyPhotonGame":
                    {
                        var lobbyId = "";
                        try { lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value; } catch { }
                        var lobby = EnsureLobby(lobbyId);
                        return CreateSuccessResponse(request.Id, lobby.PhotonGame ?? new PhotonGame());
                    }

                case "setLobbyPhotonGame":
                    {
                        try
                        {
                            var pg = PhotonGame.Parser.ParseFrom(request.Params[0].One);
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.PhotonGame = pg;
                                PublishToLobby(lobbyId, "onLobbyPhotonGameChanged", pg);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyData":
                    {
                        try
                        {
                            var data = Dictionary.Parser.ParseFrom(request.Params[0].One);
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                foreach (var kv in data.Content)
                                {
                                    lobby.Data[kv.Key] = kv.Value;
                                }
                                PublishToLobby(lobbyId, "onLobbyDataChanged", data);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "deleteLobbyData":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                foreach (var b in request.Params[0].Array)
                                {
                                    var key = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(b).Value ?? "";
                                    if (!string.IsNullOrWhiteSpace(key)) lobby.Data.Remove(key);
                                }
                                var d = new Dictionary();
                                foreach (var kv in lobby.Data) d.Content[kv.Key] = kv.Value;
                                PublishToLobby(lobbyId, "onLobbyDataChanged", d);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyOwner":
                    {
                        try
                        {
                            var newOwnerId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.OwnerPlayerId = newOwnerId;
                                PublishToLobby(lobbyId, "onLobbyOwnerChanged", new Axlebolt.RpcSupport.Protobuf.String { Value = newOwnerId });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyType":
                    {
                        try
                        {
                            var t = (LobbyType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[0].One).Value;
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.LobbyType = t;
                                PublishToLobby(lobbyId, "onLobbyTypeChanged", new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)t });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyMaxMembers":
                    {
                        try
                        {
                            var mm = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.MaxMembers = mm;
                                PublishToLobby(lobbyId, "onLobbyMaxMembersChanged", new Axlebolt.RpcSupport.Protobuf.Integer { Value = mm });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyMaxSpectators":
                    {
                        try
                        {
                            var ms = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.MaxSpectators = ms;
                                PublishToLobby(lobbyId, "onLobbyMaxSpectatorsChanged", new Axlebolt.RpcSupport.Protobuf.Integer { Value = ms });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyJoinable":
                    {
                        try
                        {
                            var joinable = Axlebolt.RpcSupport.Protobuf.Boolean.Parser.ParseFrom(request.Params[0].One).Value;
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.Joinable = joinable;
                                PublishToLobby(lobbyId, "onLobbyJoinableChanged", new Axlebolt.RpcSupport.Protobuf.Boolean { Value = joinable });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyName":
                    {
                        try
                        {
                            var name = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                var lobby = EnsureLobby(lobbyId);
                                lobby.Name = name;
                                PublishToLobby(lobbyId, "onLobbyNameChanged", new Axlebolt.RpcSupport.Protobuf.String { Value = name });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "setLobbyGameServer":
                case "invitePlayerToLobby":
                case "invitePlayerToLobbyAs":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var senderId = u?.Id.ToString() ?? "0";
                            if (senderId == "0") return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            if (!_playerLobbyByPlayerId.TryGetValue(senderId, out var lobbyId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                            var invitedId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var type = LobbyPlayerType.Member;
                            if (request.MethodName == "invitePlayerToLobbyAs")
                            {
                                type = (LobbyPlayerType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[1].One).Value;
                            }

                            var invite = new StandoffServer.Models.ServerLobbyInvite
                            {
                                LobbyId = lobbyId,
                                InviteCreator = await PlayerFriendByIdAsync(senderId),
                                Date = NowMs(),
                                PlayerType = type
                            };

                            var list = _lobbyInvitesByPlayerId.GetOrAdd(invitedId, _ => new List<StandoffServer.Models.ServerLobbyInvite>());
                            lock (list)
                            {
                                list.RemoveAll(x => x != null && x.LobbyId == lobbyId);
                                list.Add(invite);
                            }

                            var invitedPf = await PlayerFriendByIdAsync(invitedId);
                            if (type == LobbyPlayerType.Spectator)
                            {
                                PublishToPlayer(invitedId, "onReceivedSpectatorInviteToLobby", await PlayerFriendByIdAsync(senderId), new Axlebolt.RpcSupport.Protobuf.String { Value = lobbyId });
                                PublishToLobby(lobbyId, "onNewSpectatorInvitedToLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = senderId }, invitedPf);
                            }
                            else
                            {
                                PublishToPlayer(invitedId, "onReceivedInviteToLobby", await PlayerFriendByIdAsync(senderId), new Axlebolt.RpcSupport.Protobuf.String { Value = lobbyId });
                                PublishToLobby(lobbyId, "onNewPlayerInvitedToLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = senderId }, invitedPf);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "revokePlayerInvitationToLobby":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var senderId = u?.Id.ToString() ?? "0";
                            if (senderId == "0") return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            if (!_playerLobbyByPlayerId.TryGetValue(senderId, out var lobbyId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                            var revokedId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            if (_lobbyInvitesByPlayerId.TryGetValue(revokedId, out var inv))
                            {
                                lock (inv) inv.RemoveAll(x => x != null && x.LobbyId == lobbyId);
                            }
                            PublishToLobby(lobbyId, "onRevokeInviteToLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = senderId }, new Axlebolt.RpcSupport.Protobuf.String { Value = revokedId });
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "refuseInvitationToLobby":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var invitedId = u?.Id.ToString() ?? "0";
                            var lobbyId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            if (invitedId != "0" && _lobbyInvitesByPlayerId.TryGetValue(invitedId, out var inv))
                            {
                                StandoffServer.Models.ServerLobbyInvite removed = null;
                                lock (inv)
                                {
                                    removed = inv.FirstOrDefault(x => x != null && x.LobbyId == lobbyId);
                                    inv.RemoveAll(x => x != null && x.LobbyId == lobbyId);
                                }
                                var senderId = removed?.InviteCreator?.Player?.Id ?? "";
                                if (!string.IsNullOrWhiteSpace(senderId))
                                {
                                    PublishToLobby(lobbyId, "onRefuseInviteToLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = senderId }, new Axlebolt.RpcSupport.Protobuf.String { Value = invitedId });
                                }
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "kickPlayerFromLobby":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var kickInitiatorId = u?.Id.ToString() ?? "0";
                            var kickedId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            if (kickInitiatorId != "0" && _playerLobbyByPlayerId.TryGetValue(kickInitiatorId, out var lobbyId))
                            {
                                await RemoveFromLobbyAsync(kickedId);
                                PublishToLobby(lobbyId, "onPlayerKickedFromLobby", new Axlebolt.RpcSupport.Protobuf.String { Value = kickInitiatorId }, new Axlebolt.RpcSupport.Protobuf.String { Value = kickedId });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "sendLobbyChatMsg":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            var msg = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                PublishToLobby(lobbyId, "onLobbyChatMessage",
                                    new Axlebolt.RpcSupport.Protobuf.String { Value = pid },
                                    new Axlebolt.RpcSupport.Protobuf.String { Value = msg });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "changeLobbyPlayerType":
                    {
                        try
                        {
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            var t = (LobbyPlayerType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[0].One).Value;
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                await AddMember(lobbyId, t);
                                PublishToLobby(lobbyId, "onLobbyPlayerTypeChanged",
                                    new Axlebolt.RpcSupport.Protobuf.String { Value = pid },
                                    new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)t });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "changeLobbyOtherPlayerType":
                    {
                        try
                        {
                            var otherId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var t = (LobbyPlayerType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[1].One).Value;
                            var u = await GetCurrentUserAsync(ticket);
                            var pid = u?.Id.ToString() ?? "0";
                            if (pid != "0" && _playerLobbyByPlayerId.TryGetValue(pid, out var lobbyId))
                            {
                                // Update stored type if target is in lobby.
                                if (_lobbyPlayerTypesByLobbyId.TryGetValue(lobbyId, out var types)) types[otherId] = t;
                                PublishToLobby(lobbyId, "onLobbyPlayerTypeChanged",
                                    new Axlebolt.RpcSupport.Protobuf.String { Value = otherId },
                                    new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)t });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private string FindTicketByPlayerId(string playerId)
        {
            if (string.IsNullOrWhiteSpace(playerId)) return null;

            foreach (var kv in _activeSessions)
            {
                try
                {
                    if (kv.Value != null && kv.Value.Id.ToString() == playerId)
                    {
                        return kv.Key;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        private async Task<RpcResponse> HandleHandshakeAsync(RpcRequest request, string ticket)
        {
            if (request.MethodName == "protoHandshake" || request.MethodName == "handshake")
            {
                if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                {
                    try
                    {
                        var handshake = Handshake.Parser.ParseFrom(request.Params[0].One);
                        var rawTicket = handshake.Ticket;

                        // Parse "ticket@version" format
                        string clientTicket = rawTicket;
                        string clientVersion = null;
                        if (!string.IsNullOrWhiteSpace(rawTicket) && rawTicket.Contains("@"))
                        {
                            var parts = rawTicket.Split('@');
                            clientTicket = parts[0];
                            clientVersion = parts.Length > 1 ? parts[1] : null;
                        }

                        // Register version as separate document in gameVersions collection
                        if (!string.IsNullOrWhiteSpace(clientVersion))
                        {
                            await _mongo.RegisterVersionAsync(clientVersion);
                            Console.WriteLine($"[Handshake] Client version: {clientVersion}");
                            // Store version for this session
                            _clientVersions[clientTicket] = clientVersion;

                            // Block if version is disabled
                            bool versionAllowed = await _mongo.IsVersionAllowedAsync(clientVersion);
                            if (!versionAllowed)
                            {
                                Console.WriteLine($"[Handshake] *** VERSION BLOCKED *** version={clientVersion}");
                                return new RpcResponse
                                {
                                    Id = request.Id,
                                    Exception = new Axlebolt.RpcSupport.Protobuf.Exception
                                    {
                                        Code = 2000
                                    }
                                };
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(clientTicket))
                        {
                            // Only process tokens in our expected format to prevent account spoofing
                            if (!clientTicket.StartsWith("dev_") && !clientTicket.StartsWith("google_") &&
                                !clientTicket.StartsWith("fb_") && !clientTicket.StartsWith("gc_"))
                            {
                                Console.WriteLine($"[Handshake] Rejected unknown token format: '{clientTicket.Substring(0, Math.Min(10, clientTicket.Length))}...'");
                            }
                            else if (!_activeSessions.TryGetValue(clientTicket, out var user) || user == null)
                            {
                                user = await _mongo.GetUserByUidAsync(clientTicket);
                                if (user == null)
                                {
                                    user = new UserDoc
                                    {
                                        Uid = clientTicket,
                                        Name = "Player",
                                        RegistrationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                                        AvatarId = 0
                                    };
                                    await _mongo.CreateUserAsync(user);
                                    user.Name = $"Player_{user.Id}";
                                    await _mongo.UpdateUserAsync(user);
                                    Console.WriteLine($"[Handshake] Auto-created user: {user.Name} (ID: {user.Id})");
                                }
                                else
                                {
                                    Console.WriteLine($"[Handshake] Found existing user: {user.Name} (ID: {user.Id})");
                                }
                                _activeSessions[clientTicket] = user;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"[Handshake] Error processing ticket: {ex.Message}");
                    }
                }

                var response = new ServerHandshake
                {
                    ApiKey = "standoff2",
                    GameId = "so2",
                    Version = "0.13.0"
                };
                return CreateSuccessResponse(request.Id, response);
            }
            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
        }

        private async Task<RpcResponse> HandlePlayerAsync(RpcRequest request, string ticket)
        {
            // Always ensure we return something to prevent Code 500
            try
            {
                if (string.IsNullOrEmpty(ticket) || !_activeSessions.TryGetValue(ticket, out var user))
                {
                    // Fallback user for debugging if session is lost
                    user = new UserDoc { Name = "Guest", Uid = "guest", RegistrationDate = 0 };
                }

                switch (request.MethodName)
                {
                    case "getCurrentPlayer":
                    case "getPlayer":
                        return CreateSuccessResponse(request.Id, ToProtoPlayer(user));

                    case "setPlayerAvatar":
                        try
                        {
                            // Param[0] is byte[] (RpcSupport.ByteArray)
                            if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                            {
                                var avatarBytes = request.Params[0].One.ToByteArray();
                                await _mongo.UpsertStorageFileAsync(new StorageFileDoc
                                {
                                    UserId = 0, // 0 means global/system accessible by ID
                                    Filename = "avatar_" + user.Id,
                                    Bytes = avatarBytes,
                                    UpdatedAtMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                                });

                                user.AvatarId = (int)user.Id; // Use player ID as the avatar ID
                                await _mongo.UpdateUserAsync(user);
                                _activeSessions[ticket] = user;
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = user.Id.ToString() });
                            }
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = false });
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Avatar] Error setting avatar for user {user.Id}: {ex.Message}");
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = false });
                        }

                    case "setPlayerName":
                        try
                        {
                            var newName = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (!string.IsNullOrWhiteSpace(newName))
                            {
                                user.Name = newName;
                                await _mongo.UpdateUserAsync(user);
                                _activeSessions[ticket] = user;
                            }
                        }
                        catch
                        {
                            // ignore
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                    case "setPlayerFirebaseToken":
                    case "setAwayStatus":
                    case "setOnlineStatus":
                    case "banMe":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                    default:
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                }
            }
            catch
            {
                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandlePlayerStatsAsync(RpcRequest request, string ticket)
        {
            UserDoc user = null;
            if (!string.IsNullOrEmpty(ticket))
            {
                _activeSessions.TryGetValue(ticket, out user);
                if (user == null)
                {
                    // Session not in memory — try DB lookup (e.g. after server restart)
                    user = await _mongo.GetUserByUidAsync(ticket);
                    if (user != null)
                        _activeSessions[ticket] = user;
                }
            }

            if (user == null)
            {
                var guestStats = new Stats();
                AddDefaultStats(guestStats);
                NormalizeStats(guestStats, null);
                return CreateSuccessResponse(request.Id, guestStats);
            }

            switch (request.MethodName)
            {
                case "resetAllStats":
                    {
                        var empty = new PlayerStatsDoc { UserId = user.Id, Stats = new List<StatEntry>() };
                        await _mongo.UpdateStatsAsync(empty);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "getPlayerStats":
                    {
                        // Param[0] is playerId string.
                        long playerId = user.Id;
                        try
                        {
                            var pidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(pidStr, out var parsed) && parsed > 0) playerId = parsed;
                        }
                        catch { }

                        var otherStatsDoc = await _mongo.GetStatsAsync(playerId);
                        var otherUser = await _mongo.GetUserByIdAsync(playerId);
                        var otherStats = new Stats();
                        if (otherStatsDoc?.Stats?.Count > 0)
                        {
                            foreach (var s in otherStatsDoc.Stats)
                            {
                                otherStats.Stat.Add(new PlayerStat
                                {
                                    Name = s.Name,
                                    IntValue = (int)s.IntValue,
                                    FloatValue = s.FloatValue,
                                    Type = (StatDefType)s.Type
                                });
                            }
                        }
                        AddDefaultStats(otherStats);
                        NormalizeStats(otherStats, otherUser);
                        return CreateSuccessResponse(request.Id, otherStats);
                    }

                case "getStats":
                    var statsDoc = await _mongo.GetStatsAsync(user.Id);
                    var stats = new Stats();
                    if (statsDoc != null && statsDoc.Stats.Count > 0)
                    {
                        foreach (var s in statsDoc.Stats)
                        {
                            stats.Stat.Add(new PlayerStat 
                            { 
                                Name = s.Name, 
                                IntValue = (int)s.IntValue, 
                                FloatValue = s.FloatValue,
                                Type = (StatDefType)s.Type
                            });
                        }
                    }
                    else
                    {
                        AddDefaultStats(stats);
                    }
                    AddDefaultStats(stats);
                    NormalizeStats(stats, user);
                    return CreateSuccessResponse(request.Id, stats);

                case "storeStats":
                case "storeGameServerStats":
                    {
                        var currentStats = await _mongo.GetStatsAsync(user.Id) ?? new PlayerStatsDoc { UserId = user.Id };
                        var updatedStats = new List<PlayerStat>();

                        try
                        {
                            if (request.Params.Count > 0)
                            {
                                if (request.Params[0].Array != null && request.Params[0].Array.Count > 0)
                                {
                                    foreach (var b in request.Params[0].Array)
                                    {
                                        var s = StorePlayerStat.Parser.ParseFrom(b);
                                        // Accumulate instead of overwrite for certain stats
                                        if (s.Name.EndsWith("_shots") || s.Name.EndsWith("_hits") || 
                                            s.Name.EndsWith("_kills") || s.Name.EndsWith("_deaths") || 
                                            s.Name.EndsWith("_assists") || s.Name.EndsWith("_headshots") ||
                                            s.Name.EndsWith("_games_played") || s.Name.EndsWith("_damage") ||
                                            s.Name == "level_xp" || s.Name == "experience" ||
                                            s.Name == "time_in_game" || s.Name == "total_time")
                                        {
                                            var existing = currentStats.Stats.FirstOrDefault(x => x.Name == s.Name);
                                            if (existing != null)
                                            {
                                                existing.IntValue += s.StoreInt;
                                                existing.FloatValue += s.StoreFloat;
                                            }
                                            else
                                            {
                                                currentStats.Stats.Add(new StatEntry { Name = s.Name, IntValue = s.StoreInt, FloatValue = s.StoreFloat, Type = (int)StatDefType.Int });
                                            }

                                            if (s.Name == "time_in_game" || s.Name == "total_time")
                                            {
                                                user.TimeInGame += s.StoreInt;
                                                await _mongo.UpdateUserAsync(user);
                                            }
                                        }
                                        else
                                        {
                                            UpsertStatFromStore(currentStats, s);
                                        }
                                        
                                        updatedStats.Add(new PlayerStat { Name = s.Name, IntValue = s.StoreInt, FloatValue = s.StoreFloat, Type = StatDefType.Int });
                                    }
                                }
                                else if (request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                                {
                                    var legacy = Axlebolt.Bolt.Protobuf.PlayerStats.Parser.ParseFrom(request.Params[0].One);
                                    foreach (var s in legacy.Stats)
                                    {
                                        var st = new StorePlayerStat { Name = s.Name, StoreInt = s.StoreInt, StoreFloat = s.StoreFloat };
                                        
                                        if (st.Name.EndsWith("_shots") || st.Name.EndsWith("_hits") || 
                                            st.Name.EndsWith("_kills") || st.Name.EndsWith("_deaths") || 
                                            st.Name.EndsWith("_assists") || st.Name.EndsWith("_headshots") ||
                                            st.Name.EndsWith("_games_played") || st.Name.EndsWith("_damage") ||
                                            st.Name == "level_xp" || st.Name == "experience" ||
                                            st.Name == "time_in_game" || st.Name == "total_time")
                                        {
                                            var existing = currentStats.Stats.FirstOrDefault(x => x.Name == st.Name);
                                            if (existing != null)
                                            {
                                                existing.IntValue += st.StoreInt;
                                                existing.FloatValue += st.StoreFloat;
                                            }
                                            else
                                            {
                                                currentStats.Stats.Add(new StatEntry { Name = st.Name, IntValue = st.StoreInt, FloatValue = st.StoreFloat, Type = (int)StatDefType.Int });
                                            }

                                            if (st.Name == "time_in_game" || st.Name == "total_time")
                                            {
                                                user.TimeInGame += st.StoreInt;
                                                await _mongo.UpdateUserAsync(user);
                                            }
                                        }
                                        else
                                        {
                                            UpsertStatFromStore(currentStats, st);
                                        }

                                        updatedStats.Add(new PlayerStat { Name = st.Name, IntValue = st.StoreInt, FloatValue = st.StoreFloat, Type = StatDefType.Int });
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        // Sync global stats from mode-specific stats only if mode stats were actually updated this call
                        void SyncGlobal(string globalName, string[] modeNames)
                        {
                            // Only sync if at least one of the mode stats was updated in this call
                            bool anyModeUpdated = updatedStats.Any(x => modeNames.Contains(x.Name));
                            if (!anyModeUpdated) return;

                            long total = 0;
                            foreach(var mn in modeNames) {
                                var entry = currentStats.Stats.FirstOrDefault(x => x.Name == mn);
                                if (entry != null) total += entry.IntValue;
                            }

                            var gEntry = currentStats.Stats.FirstOrDefault(x => x.Name == globalName);
                            if (gEntry == null) currentStats.Stats.Add(new StatEntry { Name = globalName, IntValue = total, Type = (int)StatDefType.Int });
                            else if (total > gEntry.IntValue) gEntry.IntValue = total; // never decrease global stats
                        }

                        // Special handling for Solo/Warmup: if client sends stats without mode prefix, we map them to deathmatch by default
                        var baseStats = new[] { "shots", "hits", "kills", "deaths", "assists", "headshots", "damage" };
                        foreach (var bs in baseStats)
                        {
                            var soloStat = updatedStats.FirstOrDefault(x => x.Name == bs);
                            if (soloStat != null)
                            {
                                var dmStatName = "deathmatch_" + bs;
                                var dmEntry = currentStats.Stats.FirstOrDefault(x => x.Name == dmStatName);
                                if (dmEntry != null) dmEntry.IntValue += soloStat.IntValue;
                                else currentStats.Stats.Add(new StatEntry { Name = dmStatName, IntValue = soloStat.IntValue, Type = (int)StatDefType.Int });
                            }
                        }

                        SyncGlobal("kills", new[] { "deathmatch_kills", "defuse_kills" });
                        SyncGlobal("deaths", new[] { "deathmatch_deaths", "defuse_deaths" });
                        SyncGlobal("assists", new[] { "deathmatch_assists", "defuse_assists" });
                        SyncGlobal("headshots", new[] { "deathmatch_headshots", "defuse_headshots" });
                        SyncGlobal("shots", new[] { "deathmatch_shots", "defuse_shots" });
                        SyncGlobal("hits", new[] { "deathmatch_hits", "defuse_hits" });
                        SyncGlobal("damage", new[] { "deathmatch_damage", "defuse_damage" });
                        
                        // Increment games_played if any kills/deaths were recorded in this store call
                        if (updatedStats.Any(x => x.Name.Contains("kills") || x.Name.Contains("deaths")))
                        {
                            var gp = currentStats.Stats.FirstOrDefault(x => x.Name == "matches_played");
                            if (gp == null) currentStats.Stats.Add(new StatEntry { Name = "matches_played", IntValue = 1, Type = (int)StatDefType.Int });
                            else gp.IntValue += 1;
                        }

                        // Compute level from accumulated XP (level_xp stat)
                        // XP thresholds: 1000 per level
                        // Level 0 in Proto = Level 1 in UI
                        static int XpForLevel(int lvl) => lvl <= 0 ? 0 : lvl * 1000;
                        static int LevelFromXp(int xp)
                        {
                            int lv = 0;
                            while (lv < 100 && XpForLevel(lv + 1) <= xp) lv++;
                            return lv;
                        }

                        var xpEntry = currentStats.Stats.FirstOrDefault(x => x.Name == "level_xp");
                        if (xpEntry != null)
                        {
                            int newLevel = LevelFromXp((int)xpEntry.IntValue);
                            var lvlEntry = currentStats.Stats.FirstOrDefault(x => x.Name == "level_id");
                            if (lvlEntry == null)
                            {
                                currentStats.Stats.Add(new StatEntry { Name = "level_id", IntValue = newLevel, Type = (int)StatDefType.Int });
                                currentStats.Stats.Add(new StatEntry { Name = "level", IntValue = newLevel, Type = (int)StatDefType.Int });
                            }
                            else
                            {
                                int oldLevel = (int)lvlEntry.IntValue;
                                lvlEntry.IntValue = newLevel;
                                var lvlAlias = currentStats.Stats.FirstOrDefault(x => x.Name == "level");
                                if (lvlAlias != null) lvlAlias.IntValue = newLevel;
                                else currentStats.Stats.Add(new StatEntry { Name = "level", IntValue = newLevel, Type = (int)StatDefType.Int });

                                // Reward on level up
                                if (newLevel > oldLevel)
                                {
                                    var inv = await GetOrCreateInventoryDocAsync(user);
                                    // Give Silver (101)
                                    var silver = inv.Currencies.FirstOrDefault(c => c.CurrencyId == 101);
                                    int rewardAmount = 100;
                                    if (silver != null) silver.Value += rewardAmount;
                                    else inv.Currencies.Add(new InventoryCurrencyEntry { CurrencyId = 101, Value = rewardAmount });
                                    
                                    await _mongo.UpsertInventoryAsync(inv);
                                    Console.WriteLine($"[Stats] User {user.Id} leveled up to {newLevel + 1} (internal {newLevel}). Reward: {rewardAmount} Silver.");
                                }
                            }
                        }

                        await _mongo.UpdateStatsAsync(currentStats);

                        // Build full stats proto to push so client profile updates immediately
                        var fullStats = new Stats();
                        foreach (var s in currentStats.Stats)
                            fullStats.Stat.Add(new PlayerStat { Name = s.Name, IntValue = (int)s.IntValue, FloatValue = s.FloatValue, Type = (StatDefType)s.Type });
                        AddDefaultStats(fullStats);
                        NormalizeStats(fullStats, user);
                        await SendEventAsync(ticket, "PlayerStatsRemoteService", "onStatsUpdated", fullStats);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                case "getGlobalStats":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<PlayerStat>());

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private static void UpsertStatFromStore(PlayerStatsDoc currentStats, StorePlayerStat s)
        {
            if (currentStats == null || s == null) return;
            if (string.IsNullOrWhiteSpace(s.Name)) return;

            var existing = currentStats.Stats.FirstOrDefault(x => x.Name == s.Name);
            if (existing != null)
            {
                existing.IntValue = s.StoreInt;
                existing.FloatValue = s.StoreFloat;
                existing.Type = (int)StatDefType.Int;
            }
            else
            {
                currentStats.Stats.Add(new StatEntry
                {
                    Name = s.Name,
                    IntValue = s.StoreInt,
                    FloatValue = s.StoreFloat,
                    Type = (int)StatDefType.Int
                });
            }
        }

        private void AddDefaultStats(Stats stats)
        {
            var defaultStats = new Dictionary<string, (int val, StatDefType type)>
            {
                { "level", (0, StatDefType.Int) },
                { "level_id", (0, StatDefType.Int) },
                { "level_xp", (0, StatDefType.Int) },
                { "experience", (0, StatDefType.Int) },
                { "shots", (0, StatDefType.Int) },
                { "hits", (0, StatDefType.Int) },
                // Per-game-mode stats used by PlayerStatsExtension (supported modes: DeathMatch/Defuse)
                { "deathmatch_kills", (0, StatDefType.Int) },
                { "deathmatch_deaths", (0, StatDefType.Int) },
                { "deathmatch_assists", (0, StatDefType.Int) },
                { "deathmatch_shots", (0, StatDefType.Int) },
                { "deathmatch_hits", (0, StatDefType.Int) },
                { "deathmatch_headshots", (0, StatDefType.Int) },
                { "deathmatch_damage", (0, StatDefType.Int) },
                { "deathmatch_games_played", (0, StatDefType.Int) },
                { "defuse_kills", (0, StatDefType.Int) },
                { "defuse_deaths", (0, StatDefType.Int) },
                { "defuse_assists", (0, StatDefType.Int) },
                { "defuse_shots", (0, StatDefType.Int) },
                { "defuse_hits", (0, StatDefType.Int) },
                { "defuse_headshots", (0, StatDefType.Int) },
                { "defuse_damage", (0, StatDefType.Int) },
                { "defuse_games_played", (0, StatDefType.Int) },
                { "ranked_rank", (0, StatDefType.Int) },
                { "ranked_current_mmr", (1000, StatDefType.Int) },
                { "ranked_best_mmr", (1000, StatDefType.Int) },
                { "ranked_wins", (0, StatDefType.Int) },
                { "ranked_matches", (0, StatDefType.Int) },
                { "ranked_ban_code", (0, StatDefType.Int) },
                { "ranked_ban_expiration", (0, StatDefType.Int) },
                { "ranked_ban_duration", (0, StatDefType.Int) },
                { "ranked_calibration_match_count", (0, StatDefType.Int) },
                { "ranked_last_activity_time1", (0, StatDefType.Int) },
                { "ranked_last_activity_time2", (0, StatDefType.Int) },
                { "ranked_last_match_status", (0, StatDefType.Int) },
                { "ranked_last_match_mmr", (1000, StatDefType.Int) },
                { "kills", (0, StatDefType.Int) },
                { "deaths", (0, StatDefType.Int) },
                { "headshots", (0, StatDefType.Int) },
                { "assists", (0, StatDefType.Int) },
                { "mvp", (0, StatDefType.Int) },
                { "matches_played", (0, StatDefType.Int) },
                { "matches_won", (0, StatDefType.Int) },
                { "grenade_kills", (0, StatDefType.Int) },
                { "knife_kills", (0, StatDefType.Int) }
            };

            // Weapon stats used by PlayerStatsExtension.WrapModeGun (supported game modes: DeathMatch/Defuse)
            // Format: gun_<mode>_<weapon>_<stat>
            string[] modes = { "deathmatch", "defuse" };
            string[] gunStats = { "kills", "shots", "hits", "headshots", "damage" };
            string[] weapons =
            {
                "g22", "usp", "p350", "deagle", "tec9", "fiveseven", "ump45", "mp7", "p90", "akr", "akr12", "m4", "m16",
                "famas", "fnfal", "awm", "m40", "m110", "sm1014", "knife", "knifebayonet", "knifekarambit", "jkommando",
                "knifebutterfly", "flipknife", "grenadehe", "grenadesmoke", "grenadeflash", "bomb", "defuser", "defusekit",
                "vest", "vestandhelmet", "santagrenadesnowball", "grenadesnowball", "candycane", "snowm40", "snowpistol"
            };

            foreach (var mode in modes)
            {
                foreach (var weapon in weapons)
                {
                    foreach (var statName in gunStats)
                    {
                        var key = $"gun_{mode}_{weapon}_{statName}";
                        if (!defaultStats.ContainsKey(key))
                        {
                            defaultStats[key] = (0, StatDefType.Int);
                        }
                    }
                }
            }

            foreach (var ds in defaultStats)
            {
                if (!stats.Stat.Any(s => s.Name == ds.Key))
                {
                    stats.Stat.Add(new PlayerStat 
                    { 
                        Name = ds.Key, 
                        IntValue = ds.Value.val, 
                        Type = ds.Value.type 
                    });
                }
            }
        }

        private async Task<RpcResponse> HandleFriendsAsync(RpcRequest request, string ticket)
        {
            var user = await GetCurrentUserAsync(ticket);
            long.TryParse(user?.Id.ToString() ?? "0", out var userId);
            if (user == null || userId <= 0)
            {
                // Keep client stable.
                switch (request.MethodName)
                {
                    case "getPlayerFriends":
                    case "searchPlayers":
                        return CreateSuccessResponseArray(request.Id, Array.Empty<PlayerFriend>());
                    case "getPlayerFriendsIds":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.StringArray());
                    case "getPlayerFriendsCount":
                    case "getPlayersCount":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = 0 });
                    case "getOnlineStatus":
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = 0 });
                    case "getPlayerById":
                        return CreateSuccessResponse(request.Id, new Axlebolt.Bolt.Protobuf.Player());
                    case "getPlayerFriendById":
                        return CreateSuccessResponse(request.Id, new PlayerFriend());
                    default:
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                }
            }

            async Task<Axlebolt.Bolt.Protobuf.Player> PlayerByIdAsync(long id)
            {
                var u = await _mongo.GetUserByIdAsync(id);
                return ToProtoPlayer(u ?? new UserDoc { Id = id, Name = "Player_" + id, AvatarId = 0, RegistrationDate = 0, TimeInGame = 0 });
            }

            void PublishFriendsEvent(long targetUserId, string eventName, params IMessage[] args)
            {
                var targetTicket = FindTicketByPlayerId(targetUserId.ToString());
                if (string.IsNullOrWhiteSpace(targetTicket)) return;
                if (!_clientStreams.TryGetValue(targetTicket, out var stream) || stream == null) return;

                var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
                {
                    ListenerName = "FriendsRemoteEventListener",
                    EventName = eventName
                };
                foreach (var a in args)
                {
                    evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = a.ToByteString() });
                }

                try { NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt }); } catch { }
            }

            switch (request.MethodName)
            {
                case "getPlayerFriends":
                    {
                        // params: RelationshipStatus[] statuses, page, size
                        var statuses = new List<int>();
                        int page = 0, size = 50;
                        try
                        {
                            if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                            {
                                // enum array encoded as RpcSupport.BinaryValue.Array of ByteString, each element is RpcSupport.Enum
                                foreach (var b in request.Params[0].Array)
                                {
                                    var e = Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(b);
                                    statuses.Add(e.Value);
                                }
                            }
                            if (request.Params.Count > 1) page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                            if (request.Params.Count > 2) size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                        }
                        catch
                        {
                        }

                        if (size <= 0) size = 50;
                        if (size > 200) size = 200;
                        if (page < 0) page = 0;

                        var links = await _mongo.GetFriendLinksAsync(userId, statuses.ToArray(), page * size, size);
                        var list = new List<PlayerFriend>(links.Count);
                        foreach (var l in links)
                        {
                            var p = await PlayerByIdAsync(l.FriendUserId);
                            list.Add(new PlayerFriend
                            {
                                Player = p,
                                RelationshipStatus = (RelationshipStatus)l.Status
                            });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "searchPlayers":
                    {
                        // Search Mongo users by id or name.
                        string value = "";
                        int page = 0, size = 20;
                        try
                        {
                            value = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                            size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                        }
                        catch { }

                        if (size <= 0) size = 20;
                        if (size > 50) size = 50;
                        if (page < 0) page = 0;

                        var docs = await _mongo.SearchUsersAsync(value, page * size, size);
                        var list = new List<PlayerFriend>(docs.Count);
                        foreach (var d in docs)
                        {
                            if (d == null) continue;
                            var link = await _mongo.GetFriendLinkAsync(userId, d.Id);
                            var rs = link != null ? (RelationshipStatus)link.Status : RelationshipStatus.None;
                            list.Add(new PlayerFriend { Player = ToProtoPlayer(d), RelationshipStatus = rs });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "getPlayerFriendsIds":
                    {
                        // params: RelationshipStatus[] statuses
                        var statuses = new List<int>();
                        try
                        {
                            if (request.Params.Count > 0 && request.Params[0].Array != null)
                            {
                                foreach (var b in request.Params[0].Array)
                                {
                                    var e = Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(b);
                                    statuses.Add(e.Value);
                                }
                            }
                        }
                        catch { }

                        var links = await _mongo.GetFriendLinksAsync(userId, statuses.ToArray(), 0, 5000);
                        var arr = new Axlebolt.RpcSupport.Protobuf.StringArray();
                        foreach (var l in links)
                        {
                            arr.Value.Add(l.FriendUserId.ToString());
                        }
                        return CreateSuccessResponse(request.Id, arr);
                    }

                case "getPlayerFriendsCount":
                    {
                        var statuses = new List<int>();
                        try
                        {
                            if (request.Params.Count > 0 && request.Params[0].Array != null)
                            {
                                foreach (var b in request.Params[0].Array)
                                {
                                    var e = Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(b);
                                    statuses.Add(e.Value);
                                }
                            }
                        }
                        catch { }
                        var count = await _mongo.CountFriendLinksAsync(userId, statuses.ToArray());
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = count });
                    }

                case "getPlayersCount":
                    {
                        try
                        {
                            var value = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var count = await _mongo.CountUsersAsync(value);
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = count });
                        }
                        catch
                        {
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = 0 });
                        }
                    }

                case "getOnlineStatus":
                    {
                        try
                        {
                            var pidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            var online = !string.IsNullOrWhiteSpace(FindTicketByPlayerId(pidStr));
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum
                            {
                                Value = online ? (int)OnlineStatus.StateOnline : (int)OnlineStatus.StateOffline
                            });
                        }
                        catch
                        {
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)OnlineStatus.StateOffline });
                        }
                    }

                case "getPlayerById":
                    {
                        try
                        {
                            var playerIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(playerIdStr, out var pid) && pid > 0)
                            {
                                return CreateSuccessResponse(request.Id, await PlayerByIdAsync(pid));
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.Bolt.Protobuf.Player());
                    }

                case "getPlayerFriendById":
                    {
                        try
                        {
                            var playerIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(playerIdStr, out var pid) && pid > 0)
                            {
                                var link = await _mongo.GetFriendLinkAsync(userId, pid);
                                var status = link != null ? (RelationshipStatus)link.Status : RelationshipStatus.None;
                                return CreateSuccessResponse(request.Id, new PlayerFriend { Player = await PlayerByIdAsync(pid), RelationshipStatus = status });
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new PlayerFriend());
                    }

                case "getAvatars":
                    {
                        try
                        {
                            var ids = Axlebolt.RpcSupport.Protobuf.StringArray.Parser.ParseFrom(request.Params[0].One).Value;
                            var result = new List<AvatarBinary>();
                            foreach (var id in ids)
                            {
                                if (long.TryParse(id, out var pid))
                                {
                                    var file = await _mongo.GetStorageFileAsync(pid, "avatar.jpg");
                                    if (file != null && file.Bytes != null)
                                    {
                                        result.Add(new AvatarBinary { Id = id, Avatar = ByteString.CopyFrom(file.Bytes) });
                                    }
                                }
                            }
                            return CreateSuccessResponseArray(request.Id, result);
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Friends] getAvatars error: {ex.Message}");
                            return CreateSuccessResponseArray(request.Id, Array.Empty<AvatarBinary>());
                        }
                    }

                case "sendFriendRequest":
                    {
                        try
                        {
                            var fidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(fidStr, out var fid) && fid > 0 && fid != userId)
                            {
                                var existing = await _mongo.GetFriendLinkAsync(userId, fid);
                                var currentStatus = existing != null ? (RelationshipStatus)existing.Status : RelationshipStatus.None;

                                if (currentStatus == RelationshipStatus.Friend)
                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.Friend });

                                if (currentStatus == RelationshipStatus.RequestRecipient)
                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.RequestRecipient });

                                var now = NowMs();
                                if (currentStatus == RelationshipStatus.RequestInitiator)
                                {
                                    // They sent us a request — auto-accept
                                    await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = userId, FriendUserId = fid, Status = (int)RelationshipStatus.Friend, UpdatedAtMs = now });
                                    await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = fid, FriendUserId = userId, Status = (int)RelationshipStatus.Friend, UpdatedAtMs = now });
                                    PublishFriendsEvent(userId, "onFriendAdded", new PlayerFriend { Player = await PlayerByIdAsync(fid), RelationshipStatus = RelationshipStatus.Friend });
                                    PublishFriendsEvent(fid, "onFriendAdded", new PlayerFriend { Player = await PlayerByIdAsync(userId), RelationshipStatus = RelationshipStatus.Friend });
                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.Friend });
                                }

                                // Send new request:
                                // Sender (userId) -> RequestRecipient (outgoing, shows "Revoke")
                                // Receiver (fid)  -> RequestInitiator (incoming, shows "Accept/Decline")
                                await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = userId, FriendUserId = fid, Status = (int)RelationshipStatus.RequestRecipient, UpdatedAtMs = now });
                                await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = fid, FriendUserId = userId, Status = (int)RelationshipStatus.RequestInitiator, UpdatedAtMs = now });
                                PublishFriendsEvent(fid, "onNewFriendshipRequest", new PlayerFriend { Player = await PlayerByIdAsync(userId), RelationshipStatus = RelationshipStatus.RequestInitiator });
                                Console.WriteLine($"[Friends] {userId} sent request to {fid}");
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.RequestRecipient });
                            }
                        }
                        catch (System.Exception ex) { Console.WriteLine($"[Friends] sendFriendRequest error: {ex.Message}"); }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.None });
                    }

                case "acceptFriendRequest":
                    {
                        try
                        {
                            var fidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(fidStr, out var fid) && fid > 0)
                            {
                                var existing = await _mongo.GetFriendLinkAsync(userId, fid);
                                // RequestInitiator = they sent us a request (incoming) -> we accept
                                if (existing != null && existing.Status == (int)RelationshipStatus.RequestInitiator)
                                {
                                    var now = NowMs();
                                    await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = userId, FriendUserId = fid, Status = (int)RelationshipStatus.Friend, UpdatedAtMs = now });
                                    await _mongo.UpsertFriendLinkAsync(new FriendLinkDoc { UserId = fid, FriendUserId = userId, Status = (int)RelationshipStatus.Friend, UpdatedAtMs = now });
                                    
                                    var me = await PlayerByIdAsync(userId);
                                    var friend = await PlayerByIdAsync(fid);
                                    
                                    PublishFriendsEvent(userId, "onFriendAdded", new PlayerFriend { Player = friend, RelationshipStatus = RelationshipStatus.Friend });
                                    PublishFriendsEvent(fid, "onFriendAdded", new PlayerFriend { Player = me, RelationshipStatus = RelationshipStatus.Friend });
                                    // Remove the pending request from both sides' request lists
                                    PublishFriendsEvent(userId, "onRevokeFriendshipRequest", new Axlebolt.RpcSupport.Protobuf.String { Value = fid.ToString() });
                                    PublishFriendsEvent(fid, "onRevokeFriendshipRequest", new Axlebolt.RpcSupport.Protobuf.String { Value = userId.ToString() });
                                    
                                    Console.WriteLine($"[Friends] {userId} accepted request from {fid} (Real-time sync)");
                                }
                                else if (existing == null || existing.Status != (int)RelationshipStatus.Friend)
                                {
                                    Console.WriteLine($"[Friends] {userId} tried to accept invalid request from {fid}");
                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.None });
                                }
                            }
                        }
                        catch (System.Exception ex) { Console.WriteLine($"[Friends] acceptFriendRequest error: {ex.Message}"); }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.Friend });
                    }

                case "removeFriend":
                    {
                        try
                        {
                            var fidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(fidStr, out var fid) && fid > 0)
                            {
                                await _mongo.DeleteFriendLinkAsync(userId, fid);
                                await _mongo.DeleteFriendLinkAsync(fid, userId);
                                PublishFriendsEvent(userId, "onFriendRemoved", new Axlebolt.RpcSupport.Protobuf.String { Value = fid.ToString() });
                                PublishFriendsEvent(fid, "onFriendRemoved", new Axlebolt.RpcSupport.Protobuf.String { Value = userId.ToString() });
                                Console.WriteLine($"[Friends] {userId} removed friend {fid}");
                            }
                        }
                        catch (System.Exception ex) { Console.WriteLine($"[Friends] removeFriend error: {ex.Message}"); }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.None });
                    }

                case "ignoreFriendRequest":
                case "revokeFriendRequest":
                    {
                        try
                        {
                            var fidStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (long.TryParse(fidStr, out var fid) && fid > 0)
                            {
                                var existing = await _mongo.GetFriendLinkAsync(userId, fid);
                                if (existing != null)
                                {
                                    var wasInitiator = existing.Status == (int)RelationshipStatus.RequestInitiator;
                                    var wasRecipient = existing.Status == (int)RelationshipStatus.RequestRecipient;
                                    await _mongo.DeleteFriendLinkAsync(userId, fid);
                                    await _mongo.DeleteFriendLinkAsync(fid, userId);
                                    if (wasInitiator)
                                    {
                                        // We received request and declined it — notify sender and update our own list
                                        PublishFriendsEvent(fid, "onFriendRequestDeclined", new Axlebolt.RpcSupport.Protobuf.String { Value = userId.ToString() });
                                        PublishFriendsEvent(userId, "onRevokeFriendshipRequest", new Axlebolt.RpcSupport.Protobuf.String { Value = fid.ToString() });
                                        Console.WriteLine($"[Friends] {userId} declined request from {fid}");
                                    }
                                    else if (wasRecipient)
                                    {
                                        // We sent request and cancelled it — notify receiver and update our own list
                                        PublishFriendsEvent(fid, "onRevokeFriendshipRequest", new Axlebolt.RpcSupport.Protobuf.String { Value = userId.ToString() });
                                        PublishFriendsEvent(userId, "onRevokeFriendshipRequest", new Axlebolt.RpcSupport.Protobuf.String { Value = fid.ToString() });
                                        Console.WriteLine($"[Friends] {userId} cancelled request to {fid}");
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex) { Console.WriteLine($"[Friends] revokeFriendRequest error: {ex.Message}"); }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Enum { Value = (int)RelationshipStatus.None });
                    }

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleClansAsync(RpcRequest request, string ticket)
        {
            var user = await GetCurrentUserAsync(ticket);
            var userId = user?.Id ?? 0;

            async Task<Clan> ClanToProtoAsync(ClanDoc c)
            {
                if (c == null) return new Clan();
                var clan = new Clan
                {
                    Id = c.Id ?? "",
                    Tag = c.Tag ?? "",
                    Name = c.Name ?? "",
                    AvatarId = c.AvatarId ?? "",
                    ClanLevel = c.Level,
                    ClanType = (ClanType)c.ClanType,
                    CreateDate = c.CreatedAtMs
                };

                return clan;
            }

            async Task<string> CurrentClanIdAsync()
            {
                if (userId <= 0) return null;
                return await _mongo.GetPlayerClanIdAsync(userId);
            }

            async Task<ClanRequest> ToProtoClanRequestAsync(ClanRequestDoc d)
            {
                if (d == null) return new ClanRequest();
                var clanDoc = await _mongo.GetClanByIdAsync(d.ClanId);
                var clan = await ClanToProtoAsync(clanDoc);
                var sender = ToProtoPlayer(await _mongo.GetUserByIdAsync(d.SenderUserId));
                var invited = d.InvitedUserId.HasValue ? ToProtoPlayer(await _mongo.GetUserByIdAsync(d.InvitedUserId.Value)) : null;
                return new ClanRequest
                {
                    Id = d.Id ?? "",
                    Clan = clan,
                    RequestSender = sender,
                    CreateDate = d.CreateDateMs,
                    RequestType = (RequestType)d.RequestType,
                    InvitedPlayer = invited
                };
            }

            void PublishClanMessageEvent(string clanId, ClanUserMessage msg)
            {
                if (string.IsNullOrWhiteSpace(clanId) || msg == null) return;
                // push to all online members
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var members = await _mongo.GetClanMembersAsync(clanId);
                        foreach (var m in members)
                        {
                            var targetTicket = FindTicketByPlayerId(m.UserId.ToString());
                            if (string.IsNullOrWhiteSpace(targetTicket)) continue;
                            if (!_clientStreams.TryGetValue(targetTicket, out var stream) || stream == null) continue;

                            var evt = new Axlebolt.RpcSupport.Protobuf.EventResponse
                            {
                                ListenerName = "ClanMessagesRemoteEventListener",
                                EventName = "onIncomingClanChatMessage"
                            };
                            evt.Params.Add(new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = msg.ToByteString() });
                            try { NetworkPacket.WritePacket(stream, new ResponseMessage { EventResponse = evt }); } catch { }
                        }
                    }
                    catch { }
                });
            }

            switch (request.MethodName)
            {
                case "getRoles":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<ClanMemberRole>());

                case "getPlayerClan":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (string.IsNullOrWhiteSpace(clanId))
                        {
                            return CreateSuccessResponse(request.Id, new Clan());
                        }
                        var c = await _mongo.GetClanByIdAsync(clanId);
                        return CreateSuccessResponse(request.Id, await ClanToProtoAsync(c));
                    }

                case "getClan":
                    {
                        try
                        {
                            var clanId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            var c = await _mongo.GetClanByIdAsync(clanId);
                            return CreateSuccessResponse(request.Id, await ClanToProtoAsync(c));
                        }
                        catch
                        {
                            return CreateSuccessResponse(request.Id, new Clan());
                        }
                    }

                case "findClan":
                    {
                        string clanName = "";
                        string tag = "";
                        int page = 0, size = 20;
                        try
                        {
                            clanName = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? "";
                            tag = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value ?? "";
                            page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[3].One).Value;
                        }
                        catch { }
                        if (size <= 0) size = 20;
                        if (size > 50) size = 50;
                        if (page < 0) page = 0;
                        var query = string.Join(" ", new[] { clanName, tag }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        var clans = await _mongo.FindClansAsync(query, page * size, size);
                        var list = new List<Clan>();
                        foreach (var c in clans)
                        {
                            list.Add(await ClanToProtoAsync(c));
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "getLevels":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<ClanLevel>());

                case "getAllClanMembers":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (string.IsNullOrWhiteSpace(clanId))
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<ClanMember>());
                        }
                        var members = await _mongo.GetClanMembersAsync(clanId);
                        var list = new List<ClanMember>();
                        foreach (var m in members)
                        {
                            list.Add(new ClanMember
                            {
                                Id = $"{m.ClanId}_{m.UserId}",
                                ClanId = m.ClanId ?? "",
                                Player = ToProtoPlayer(await _mongo.GetUserByIdAsync(m.UserId)),
                                Role = new ClanMemberRole { Id = "role_" + m.Role, Name = "Role", Level = m.Role },
                                CreateDate = m.JoinedAtMs
                            });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "getClanMsgs":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (string.IsNullOrWhiteSpace(clanId))
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<ClanUserMessage>());
                        }

                        int page = 0, size = 50;
                        try
                        {
                            page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                            size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        }
                        catch { }
                        if (size <= 0) size = 50;
                        if (size > 200) size = 200;
                        if (page < 0) page = 0;

                        var docs = await _mongo.GetClanMessagesAsync(clanId, page * size, size);
                        docs.Reverse();
                        var list = new List<ClanUserMessage>(docs.Count);
                        foreach (var d in docs)
                        {
                            list.Add(new ClanUserMessage
                            {
                                SenderId = d.SenderUserId.ToString(),
                                Message = d.Message ?? "",
                                Timestamp = d.TimestampMs
                            });
                        }
                        return CreateSuccessResponseArray(request.Id, list);
                    }

                case "getPlayerOpenRequests":
                case "getClanOpenRequests":
                    {
                        try
                        {
                            var requestType = RequestType.NoneType;
                            int page = 0, size = 50;
                            try
                            {
                                requestType = (RequestType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[0].One).Value;
                                page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                                size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            }
                            catch { }
                            if (page < 0) page = 0;
                            if (size <= 0) size = 50;
                            if (size > 200) size = 200;

                            List<ClanRequestDoc> docs;
                            if (request.MethodName == "getClanOpenRequests")
                            {
                                var clanId = await CurrentClanIdAsync();
                                if (string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponseArray(request.Id, Array.Empty<ClanOpenRequest>());
                                docs = await _mongo.GetOpenClanRequestsForClanAsync(clanId);
                            }
                            else
                            {
                                docs = await _mongo.GetOpenClanRequestsForPlayerAsync(userId);
                            }

                            if (requestType != RequestType.NoneType)
                            {
                                docs = docs.Where(d => d.RequestType == (int)requestType).ToList();
                            }
                            docs = docs.Skip(page * size).Take(size).ToList();

                            var list = new List<ClanOpenRequest>();
                            foreach (var d in docs)
                            {
                                list.Add(new ClanOpenRequest { ClanRequest = await ToProtoClanRequestAsync(d) });
                            }
                            return CreateSuccessResponseArray(request.Id, list);
                        }
                        catch
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<ClanOpenRequest>());
                        }
                    }

                case "getPlayerClosedRequests":
                case "getClanClosedRequests":
                    {
                        try
                        {
                            var requestType = RequestType.NoneType;
                            int page = 0, size = 50;
                            try
                            {
                                requestType = (RequestType)Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[0].One).Value;
                                page = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                                size = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                            }
                            catch { }
                            if (page < 0) page = 0;
                            if (size <= 0) size = 50;
                            if (size > 200) size = 200;

                            List<ClanRequestDoc> docs;
                            if (request.MethodName == "getClanClosedRequests")
                            {
                                var clanId = await CurrentClanIdAsync();
                                if (string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponseArray(request.Id, Array.Empty<ClanClosedRequest>());
                                docs = await _mongo.GetClosedClanRequestsForClanAsync(clanId);
                            }
                            else
                            {
                                docs = await _mongo.GetClosedClanRequestsForPlayerAsync(userId);
                            }

                            if (requestType != RequestType.NoneType)
                            {
                                docs = docs.Where(d => d.RequestType == (int)requestType).ToList();
                            }
                            docs = docs.Skip(page * size).Take(size).ToList();

                            var list = new List<ClanClosedRequest>();
                            foreach (var d in docs)
                            {
                                list.Add(new ClanClosedRequest
                                {
                                    ClanRequest = await ToProtoClanRequestAsync(d),
                                    CloseDate = d.CloseDateMs ?? 0,
                                    ClosingReason = (ClanClosingReason)(d.ClosingReason ?? 0)
                                });
                            }
                            return CreateSuccessResponseArray(request.Id, list);
                        }
                        catch
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<ClanClosedRequest>());
                        }
                    }

                case "getUnreadClanMessagesCount":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (string.IsNullOrWhiteSpace(clanId))
                        {
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = 0 });
                        }
                        var count = await _mongo.GetUnreadClanMessagesCountAsync(clanId, userId);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = count });
                    }

                case "setClanAvatar":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            if (!string.IsNullOrWhiteSpace(clanId))
                            {
                                var c = await _mongo.GetClanByIdAsync(clanId);
                                if (c != null)
                                {
                                    c.AvatarId = Guid.NewGuid().ToString("N");
                                    await _mongo.UpsertClanAsync(c);
                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = c.AvatarId });
                                }
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                    }

                // void methods
                case "requestToJoinClan":
                    {
                        try
                        {
                            // Param[0] is clanId
                            var clanId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            var id = "cr_" + Guid.NewGuid().ToString("N");
                            await _mongo.UpsertClanRequestAsync(new ClanRequestDoc
                            {
                                Id = id,
                                ClanId = clanId,
                                SenderUserId = userId,
                                InvitedUserId = null,
                                RequestType = (int)RequestType.JoinRequest,
                                CreateDateMs = NowMs(),
                                Closed = false
                            });
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "inviteToClan":
                    {
                        try
                        {
                            // Signature in Assets: inviteToClan(string playerId)
                            var clanId = await CurrentClanIdAsync();
                            var invitedStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (string.IsNullOrWhiteSpace(clanId) || !long.TryParse(invitedStr, out var invitedId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            var id = "cr_" + Guid.NewGuid().ToString("N");
                            await _mongo.UpsertClanRequestAsync(new ClanRequestDoc
                            {
                                Id = id,
                                ClanId = clanId,
                                SenderUserId = userId,
                                InvitedUserId = invitedId,
                                RequestType = (int)RequestType.InviteRequest,
                                CreateDateMs = NowMs(),
                                Closed = false
                            });
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "changeClanType":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            if (string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            var type = Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[0].One).Value;
                            var c = await _mongo.GetClanByIdAsync(clanId);
                            if (c != null)
                            {
                                c.ClanType = type;
                                await _mongo.UpsertClanAsync(c);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "cancelRequest":
                    {
                        try
                        {
                            var reqId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            var d = await _mongo.GetClanRequestAsync(reqId);
                            if (d != null && !d.Closed)
                            {
                                d.Closed = true;
                                d.CloseDateMs = NowMs();
                                d.ClosingReason = (int)ClanClosingReason.CancelRequest;
                                await _mongo.UpsertClanRequestAsync(d);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "assignRoleToMember":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            var memberIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            var newRole = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value;
                            if (!long.TryParse(memberIdStr, out var memberId) || string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                            // Accept join request if exists for this member
                            var open = (await _mongo.GetOpenClanRequestsForClanAsync(clanId))
                                .FirstOrDefault(r => r.RequestType == (int)RequestType.JoinRequest && r.SenderUserId == memberId);
                            if (open != null)
                            {
                                open.Closed = true;
                                open.CloseDateMs = NowMs();
                                open.ClosingReason = (int)ClanClosingReason.AcceptRequest;
                                await _mongo.UpsertClanRequestAsync(open);
                            }

                            // Store member (role mapping simplified)
                            await _mongo.SetClanMemberAsync(new ClanMemberDoc { ClanId = clanId, UserId = memberId, Role = 10, JoinedAtMs = NowMs() });
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "sendMsgToClan":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (string.IsNullOrWhiteSpace(clanId))
                        {
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                        }

                        string msg = "";
                        try { msg = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? ""; } catch { }
                        var now = NowMs();
                        await _mongo.InsertClanMessageAsync(new ClanMessageDoc
                        {
                            ClanId = clanId,
                            SenderUserId = userId,
                            Message = msg ?? "",
                            TimestampMs = now
                        });

                        var pb = new ClanUserMessage
                        {
                            SenderId = userId.ToString(),
                            Message = msg ?? "",
                            Timestamp = now
                        };
                        PublishClanMessageEvent(clanId, pb);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "kickMember":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            var memberIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (!long.TryParse(memberIdStr, out var memberId) || string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            await _mongo.RemoveClanMemberAsync(clanId, memberId);
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "changeClanName":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            if (string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            var newName = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            var c = await _mongo.GetClanByIdAsync(clanId);
                            if (c != null)
                            {
                                if (!string.IsNullOrWhiteSpace(newName)) c.Name = newName;
                                await _mongo.UpsertClanAsync(c);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "assignLeaderRole":
                    {
                        try
                        {
                            var clanId = await CurrentClanIdAsync();
                            var memberIdStr = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            if (!long.TryParse(memberIdStr, out var memberId) || string.IsNullOrWhiteSpace(clanId)) return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            await _mongo.SetClanMemberAsync(new ClanMemberDoc { ClanId = clanId, UserId = memberId, Role = 100, JoinedAtMs = NowMs() });
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "declineRequest":
                    {
                        try
                        {
                            var reqId = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            var d = await _mongo.GetClanRequestAsync(reqId);
                            if (d != null && !d.Closed)
                            {
                                d.Closed = true;
                                d.CloseDateMs = NowMs();
                                d.ClosingReason = (int)ClanClosingReason.DeclineRequest;
                                await _mongo.UpsertClanRequestAsync(d);
                            }
                        }
                        catch { }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "leaveClan":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (!string.IsNullOrWhiteSpace(clanId))
                        {
                            await _mongo.RemoveClanMemberAsync(clanId, userId);
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "readClanMsgs":
                    {
                        var clanId = await CurrentClanIdAsync();
                        if (!string.IsNullOrWhiteSpace(clanId))
                        {
                            await _mongo.SetClanReadStateAsync(clanId, userId, NowMs());
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }
                case "deleteClanMsgs":
                case "upgradeClan":
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "createClan":
                    {
                        // Signature in Assets: createClan(string clanName, string clanTag, ClanType clanType)
                        string name = "Clan";
                        string tag = "TAG";
                        int clanType = (int)ClanType.Closed;
                        try
                        {
                            if (request.Params.Count > 0) name = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value ?? name;
                            if (request.Params.Count > 1) tag = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value ?? tag;
                            if (request.Params.Count > 2) clanType = Axlebolt.RpcSupport.Protobuf.Enum.Parser.ParseFrom(request.Params[2].One).Value;
                        }
                        catch { }

                        var clanId = "clan_" + Guid.NewGuid().ToString("N");
                        var now = NowMs();
                        var clanDoc = new ClanDoc
                        {
                            Id = clanId,
                            Tag = tag,
                            Name = name,
                            AvatarId = "",
                            CreatedAtMs = now,
                            JoinType = (int)JoinClanType.NoneJoinType,
                            Level = 1,
                            ClanType = clanType
                        };
                        await _mongo.UpsertClanAsync(clanDoc);
                        await _mongo.SetClanMemberAsync(new ClanMemberDoc
                        {
                            ClanId = clanId,
                            UserId = userId,
                            Role = 100,
                            JoinedAtMs = now
                        });
                        await _mongo.SetClanReadStateAsync(clanId, userId, now);
                        return CreateSuccessResponse(request.Id, await ClanToProtoAsync(clanDoc));
                    }

                case "getAvatars":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<AvatarBinary>());

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleInventoryAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "getInventoryItemDefinitions":
                    var jsonDefs = LoadInventoryDefinitionsFromJson();
                    
                    // Force add 301 and others if not present in JSON
                    var forcedIds = new HashSet<int>(jsonDefs.Select(d => d.Id));
                    if (!forcedIds.Contains(301)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 301, DisplayName = "Origin Case", BuyPrice = { new CurrencyAmount { CurrencyId = 102, Value = 100 } } });
                    if (!forcedIds.Contains(302)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 302, DisplayName = "Furious Case", BuyPrice = { new CurrencyAmount { CurrencyId = 102, Value = 100 } } });
                    if (!forcedIds.Contains(303)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 303, DisplayName = "Rival Case", BuyPrice = { new CurrencyAmount { CurrencyId = 102, Value = 100 } } });
                    if (!forcedIds.Contains(304)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 304, DisplayName = "Fable Case", BuyPrice = { new CurrencyAmount { CurrencyId = 102, Value = 100 } } });
                    if (!forcedIds.Contains(401)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 401, DisplayName = "Origin Box", BuyPrice = { new CurrencyAmount { CurrencyId = 101, Value = 100 } } });
                    if (!forcedIds.Contains(402)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 402, DisplayName = "Furious Box", BuyPrice = { new CurrencyAmount { CurrencyId = 101, Value = 100 } } });
                    if (!forcedIds.Contains(403)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 403, DisplayName = "Rival Box", BuyPrice = { new CurrencyAmount { CurrencyId = 101, Value = 100 } } });
                    if (!forcedIds.Contains(404)) jsonDefs.Add(new ServerInventoryItemDefinition { Id = 404, DisplayName = "Fable Box", BuyPrice = { new CurrencyAmount { CurrencyId = 101, Value = 100 } } });

                    if (jsonDefs.Count > 0)
                    {
                        return CreateSuccessResponseArray(request.Id, jsonDefs);
                    }

                    // Fallback: old hardcoded minimal set if JSON not available
                    var definitions = new List<ServerInventoryItemDefinition>();

                    CurrencyAmount Coins(float value) => new CurrencyAmount { CurrencyId = 101, Value = value };
                    CurrencyAmount Gold(float value) => new CurrencyAmount { CurrencyId = 102, Value = value };

                    ServerInventoryItemDefinition Case(int id, string name, string collection, float price, bool gold)
                    {
                        var def = new ServerInventoryItemDefinition { Id = id, DisplayName = name };
                        def.Properties.Add("collection", collection);
                        if (gold) def.BuyPrice.Add(Gold(price)); else def.BuyPrice.Add(Coins(price));
                        return def;
                    }

                    definitions.Add(Case(301, "Origin Case", "origin", 100, gold: true));
                    definitions.Add(Case(401, "Origin Box", "origin", 100, gold: false));
                    definitions.Add(Case(302, "Furious Case", "furious", 100, gold: true));
                    definitions.Add(Case(402, "Furious Box", "furious", 100, gold: false));
                    definitions.Add(Case(303, "Rival Case", "rival", 100, gold: true));
                    definitions.Add(Case(403, "Rival Box", "rival", 100, gold: false));
                    definitions.Add(Case(304, "Fable Case", "fable", 100, gold: true));
                    definitions.Add(Case(404, "Fable Box", "fable", 100, gold: false));

                    // Add Halloween Pack (701) to fallback
                    var halloweenPack = new ServerInventoryItemDefinition { Id = 701, DisplayName = "Halloween Sticker Pack" };
                    halloweenPack.Properties["collection"] = "halloween_2019";
                    halloweenPack.BuyPrice.Add(new CurrencyAmount { CurrencyId = 102, Value = 5000 });
                    definitions.Add(halloweenPack);

                    return CreateSuccessResponseArray(request.Id, definitions);

                case "getPlayerInventory":
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        if (user.Id <= 0)
                        {
                            Console.WriteLine($"[Inventory] getPlayerInventory: session lost for ticket='{ticket}', returning empty");
                            return CreateSuccessResponse(request.Id, new PlayerInventory());
                        }
                        Console.WriteLine($"[Inventory] getPlayerInventory: loading for user {user.Name} (Id={user.Id}, Uid={user.Uid})");
                        var inventory = await LoadOrCreateInventoryAsync(user);
                        Console.WriteLine($"[Inventory] getPlayerInventory: returning {inventory.InventoryItems.Count} items, {inventory.Currencies.Count} currencies for user {user.Id}");
                        return CreateSuccessResponse(request.Id, inventory);
                    }

                case "getInventoryItemPropertyDefinitions":
                    // Generate property definitions dynamically from all loaded definitions
                    var allDefs = LoadInventoryDefinitionsFromJson();
                    var propDefs = allDefs
                        .Select(d => new InventoryItemPropertyDefinitions { ItemDefinitionId = d.Id })
                        .ToList();
                    // Ensure mandatory IDs are always present
                    var mandatoryPropIds = new[] { 301, 302, 303, 304, 401, 402, 403, 404, 701, 602, 105, 108 };
                    foreach (var mid in mandatoryPropIds)
                    {
                        if (!propDefs.Any(p => p.ItemDefinitionId == mid))
                            propDefs.Add(new InventoryItemPropertyDefinitions { ItemDefinitionId = mid });
                    }
                    return CreateSuccessResponseArray(request.Id, propDefs);

                case "setInventoryItemFlags":
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await _mongo.GetInventoryAsync(user.Id);
                        if (invDoc != null && request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                        {
                            var args = ItemFlags.Parser.ParseFrom(request.Params[0].One);
                            foreach (var kv in args.Flags)
                            {
                                var item = invDoc.Items.FirstOrDefault(x => x.Id == kv.Key);
                                if (item != null)
                                {
                                    item.Flags = kv.Value;
                                }
                            }
                            await _mongo.UpsertInventoryAsync(invDoc);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"[Inventory] setInventoryItemFlags persist error: {ex.Message}");
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "setInventoryItemsProperties":
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await _mongo.GetInventoryAsync(user.Id);
                        if (invDoc != null && request.Params.Count > 0)
                        {
                            var p = request.Params[0];
                            IEnumerable<ByteString> payloads = p.Array.Count > 0 ? p.Array : (!p.One.IsEmpty ? new[] { p.One } : Array.Empty<ByteString>());
                            foreach (var bs in payloads)
                            {
                                if (bs == null || bs.IsEmpty) continue;
                                var propsMsg = InventoryItemProperties.Parser.ParseFrom(bs);
                                var propsItem = invDoc.Items.FirstOrDefault(x => x.Id == propsMsg.Id);
                                if (propsItem == null) continue;
                                foreach (var kv in propsMsg.Properties)
                                {
                                    propsItem.Properties[kv.Key] = new InventoryItemPropertyEntry
                                    {
                                        Type = (int)kv.Value.Type,
                                        IntValue = kv.Value.IntValue,
                                        FloatValue = kv.Value.FloatValue,
                                        StringValue = kv.Value.StringValue,
                                        BoolValue = kv.Value.BooleanValue
                                    };
                                }
                            }
                            await _mongo.UpsertInventoryAsync(invDoc);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"[Inventory] setInventoryItemsProperties persist error: {ex.Message}");
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "buyInventoryItem":
                    try
                    {
                        int definitionId = 0;
                        int quantity = 0;
                        int currencyId = 0;

                        if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                            definitionId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        if (request.Params.Count > 1 && request.Params[1].One != null && !request.Params[1].One.IsEmpty)
                            quantity = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        if (request.Params.Count > 2 && request.Params[2].One != null && !request.Params[2].One.IsEmpty)
                            currencyId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;

                        if (quantity <= 0) quantity = 1;

                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        var buyDefs = LoadInventoryDefinitionsFromJson();
                        var buyItemDef = buyDefs.FirstOrDefault(d => d.Id == definitionId);
                        if (buyItemDef != null && buyItemDef.BuyPrice != null)
                        {
                            foreach (var price in buyItemDef.BuyPrice)
                            {
                                var cur = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == price.CurrencyId);
                                if (cur != null) { cur.Value -= (price.Value * quantity); if (cur.Value < 0) cur.Value = 0; }
                            }
                        }

                        var items = new List<InventoryItem>();
                        var nowBuy = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                        for (int i = 0; i < quantity; i++)
                        {
                            var newItem = new InventoryItem { Id = NextInventoryItemId(ticket, invDoc), ItemDefinitionId = definitionId, Quantity = 1, Flags = 0, Date = nowBuy };
                            items.Add(newItem);
                            invDoc.Items.Add(new InventoryItemEntry { Id = newItem.Id, ItemDefinitionId = newItem.ItemDefinitionId, Quantity = 1, Flags = 0, Date = nowBuy });
                        }

                        await _mongo.UpsertInventoryAsync(invDoc);
                        return CreateSuccessResponseArray(request.Id, items);
                    }
                    catch
                    {
                        return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());
                    }

                case "exchangeInventoryItems":
                    // Used to open cases/gifts/craft.
                    // Return ExchangeResult with at least one skin item.
                    try
                    {
                        var recipe = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                        var result = new ExchangeResult();
                        var nowEx = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        // Consume input items (e.g. case item) so that open result persists across relog.
                        // inventoryItemIds is sent as int[] => BinaryValue.Array of RpcSupport.Protobuf.Integer.
                        if (request.Params.Count > 2)
                        {
                            var idsParam = request.Params[2];
                            var consumedIds = new List<int>();

                            if (idsParam.Array != null && idsParam.Array.Count > 0)
                            {
                                foreach (var bs in idsParam.Array)
                                {
                                    if (bs == null || bs.IsEmpty) continue;
                                    try
                                    {
                                        consumedIds.Add(Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(bs).Value);
                                    }
                                    catch
                                    {
                                        // ignore malformed entries
                                    }
                                }
                            }
                            else if (idsParam.One != null && !idsParam.One.IsEmpty)
                            {
                                // Some serializers might send IntegerArray in One.
                                try
                                {
                                    var arr = Axlebolt.RpcSupport.Protobuf.IntegerArray.Parser.ParseFrom(idsParam.One);
                                    consumedIds.AddRange(arr.Value);
                                }
                                catch
                                {
                                    // ignore
                                }
                            }

                            foreach (var consumedId in consumedIds.Distinct())
                            {
                                var entry = invDoc.Items.FirstOrDefault(x => x.Id == consumedId);
                                if (entry == null) continue;

                                entry.Quantity -= 1;
                                if (entry.Quantity <= 0)
                                {
                                    invDoc.Items.Remove(entry);
                                }
                            }
                        }

                        // Always consume the case from inventory by defId (prevents re-appearing on relog).
                        {
                            var mFallback = Regex.Match(recipe ?? string.Empty, @"RECIPE_V2_(\d+)");
                            if (mFallback.Success && int.TryParse(mFallback.Groups[1].Value, out int fallbackDefId))
                            {
                                var fallbackEntry = invDoc.Items.FirstOrDefault(x => x.ItemDefinitionId == fallbackDefId);
                                if (fallbackEntry != null)
                                {
                                    fallbackEntry.Quantity -= 1;
                                    if (fallbackEntry.Quantity <= 0)
                                        invDoc.Items.Remove(fallbackEntry);
                                }
                            }
                        }


                        // Recipe format from client: "RECIPE_V2_<caseDefId>".
                        // We'll determine case collection from JSON definitions and then randomly pick a skin from that collection.
                        int caseDefId = 0;
                        var m = Regex.Match(recipe ?? string.Empty, @"RECIPE_V2_(\d+)");
                        if (m.Success)
                        {
                            int.TryParse(m.Groups[1].Value, out caseDefId);
                        }

                        var defs = LoadInventoryDefinitionsFromJson();
                        var caseDef = caseDefId > 0 ? defs.FirstOrDefault(d => d.Id == caseDefId) : null;
                        string collection = null;
                        caseDef?.Properties.TryGetValue("collection", out collection);

                        var knownIds = TryReadClientInventoryIds();

                        // Helper: pick a weighted skin drop (Arcana 5%, Legendary 20%, Rare 75%)
                        
                        InventoryItem PickWeightedSkin()
                        {
                            IEnumerable<ServerInventoryItemDefinition> skinBase = defs
                                .Where(d => IsEnabledDefinition(d))
                                .Where(d => d.Id >= 2000 && d.Id <= 299000)
                                .Where(d => d.Id != 11001)
                                .Where(d => knownIds == null || knownIds.Contains(d.Id));

                            int GetRarity(ServerInventoryItemDefinition d) {
                                if (d.Properties.TryGetValue("value", out var v) && v != null && int.TryParse(v.ToString(), out int r)) return r;
                                return 0;
                            }

                            var arcana    = skinBase.Where(d => GetRarity(d) == 6).ToList(); //  5%
                            var legendary = skinBase.Where(d => GetRarity(d) == 5).ToList(); // 20%
                            var rare      = skinBase.Where(d => GetRarity(d) == 4).ToList(); // 75%

                            int roll = Random.Shared.Next(100);
                            List<ServerInventoryItemDefinition> pool;
                            if      (roll < 5  && arcana.Count > 0)    pool = arcana;
                            else if (roll < 25 && legendary.Count > 0) pool = legendary;
                            else if (rare.Count > 0)                    pool = rare;
                            else                                        pool = skinBase.ToList();

                            int defId = pool.Count > 0 ? pool[Random.Shared.Next(pool.Count)].Id : 11002;
                            return new InventoryItem { Id = NextInventoryItemId(ticket), ItemDefinitionId = defId, Quantity = 1, Flags = 0, Date = nowEx };
                        }

                        // Helper: add silver (currencyId=101) to result + persist
                        
                        void AddSilver(int amount)
                        {
                            result.Currencies.Add(new CurrencyAmount { CurrencyId = 101, Value = amount });
                            var cur = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == 101);
                            if (cur != null) cur.Value += amount;
                            else invDoc.Currencies.Add(new InventoryCurrencyEntry { CurrencyId = 101, Value = amount });
                        }

                        // Helper: add gold (currencyId=102) to result + persist
                        
                        void AddGold(int amount)
                        {
                            result.Currencies.Add(new CurrencyAmount { CurrencyId = 102, Value = amount });
                            var cur = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == 102);
                            if (cur != null) cur.Value += amount;
                            else invDoc.Currencies.Add(new InventoryCurrencyEntry { CurrencyId = 102, Value = amount });
                        }

                        // Helper: add a weighted skin to result + persist
                        
                        void AddSkinDrop()
                        {
                            var skin = PickWeightedSkin();
                            result.InventoryItems.Add(skin);
                            invDoc.Items.Add(new InventoryItemEntry { Id = skin.Id, ItemDefinitionId = skin.ItemDefinitionId, Quantity = 1, Flags = 0, Date = nowEx });
                        }

                        // Helper: добавить случайный стикер (id 1101-1199)
                        void AddStickerDrop()
                        {
                            var stickers = defs
                                .Where(d => d.Id >= 1101 && d.Id <= 1199 && IsEnabledDefinition(d))
                                .ToList();
                            if (stickers.Count == 0) return;
                            var sticker = stickers[Random.Shared.Next(stickers.Count)];
                            var item = new InventoryItem { Id = NextInventoryItemId(ticket), ItemDefinitionId = sticker.Id, Quantity = 1, Flags = 0, Date = nowEx };
                            result.InventoryItems.Add(item);
                            invDoc.Items.Add(new InventoryItemEntry { Id = item.Id, ItemDefinitionId = item.ItemDefinitionId, Quantity = 1, Flags = 0, Date = nowEx });
                        }

                        // POST-GAME DROP RECIPES
                        
                        if (recipe == "EXCHANGE_GOLD")
                        {
                            // Gold -> Silver exchange: 10 gold = 100 silver
                            var goldCur = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == 102);
                            if (goldCur != null && goldCur.Value >= 10)
                            {
                                goldCur.Value -= 10;
                                var silverCur = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == 101);
                                if (silverCur != null) silverCur.Value += 100;
                                else invDoc.Currencies.Add(new InventoryCurrencyEntry { CurrencyId = 101, Value = 100 });
                                result.Currencies.Add(new CurrencyAmount { CurrencyId = 101, Value = 100 });
                                result.Currencies.Add(new CurrencyAmount { CurrencyId = 102, Value = -10 });
                            }
                            await _mongo.UpsertInventoryAsync(invDoc);
                            return CreateSuccessResponse(request.Id, result);
                        }

                        if (recipe == "RECIPE_DROP_IN_GAME" || recipe == "RECIPE_DROP_IN_GAME_PRO")
                        {
                            // Silver always drops
                            AddSilver(Random.Shared.Next(50, 201));

                            int dropRoll = Random.Shared.Next(100);
                            if (dropRoll < 50)
                            {
                                // 50% -> gold 20-250
                                AddGold(Random.Shared.Next(20, 251));
                            }
                            else if (dropRoll < 75)
                            {
                                // 25% -> skin (Arcana 5%, Legendary 20%, Rare 75%)
                                AddSkinDrop();
                            }
                            // remaining 25% -> silver only (already added above)

                            // 20% шанс стикера после матча
                            if (Random.Shared.Next(100) < 20)
                                AddStickerDrop();

                            await _mongo.UpsertInventoryAsync(invDoc);
                            return CreateSuccessResponse(request.Id, result);
                        }

                        
                        if (recipe == "RECIPE_DROP_ON_LVL")
                        {
                            // Level up -> silver + skin
                            AddSilver(Random.Shared.Next(30, 101));
                            AddSkinDrop();
                            await _mongo.UpsertInventoryAsync(invDoc);
                            return CreateSuccessResponse(request.Id, result);
                        }

                        
                        if (recipe == "RECIPE_DROP_ON_BONUS")
                        {
                            // Bonus -> silver + 50% chance gold
                            AddSilver(Random.Shared.Next(20, 81));
                            if (Random.Shared.Next(2) == 0)
                                AddGold(Random.Shared.Next(10, 51));
                            await _mongo.UpsertInventoryAsync(invDoc);
                            return CreateSuccessResponse(request.Id, result);
                        }

                        
                        if (recipe != null && recipe.StartsWith("RECIPE_GOOD_GAME_"))
                        {
                            // Top place -> silver + gold + 25% skin
                            AddSilver(Random.Shared.Next(50, 151));
                            AddGold(Random.Shared.Next(30, 101));
                            if (Random.Shared.Next(4) == 0)
                                AddSkinDrop();
                            await _mongo.UpsertInventoryAsync(invDoc);
                            return CreateSuccessResponse(request.Id, result);
                        }
                        // END POST-GAME DROP RECIPES

                        // STICKER PACK: RECIPE_V2_701 (or any pack in 700-799 range)
                        {
                            var mPack = Regex.Match(recipe ?? string.Empty, @"RECIPE_V2_(\d+)");
                            if (mPack.Success && int.TryParse(mPack.Groups[1].Value, out int packDefId) && packDefId >= 700 && packDefId <= 799)
                            {
                                // Find enabled stickers from the pack's collection
                                var packDef = defs.FirstOrDefault(d => d.Id == packDefId);
                                string packCollection = null;
                                packDef?.Properties.TryGetValue("collection", out packCollection);

                                var stickerPool = defs
                                    .Where(d => d.Id >= 1101 && d.Id <= 1199 && IsEnabledDefinition(d))
                                    .ToList();

                                // Filter by collection if pack has one
                                if (!string.IsNullOrWhiteSpace(packCollection))
                                {
                                    var collectionStickers = stickerPool
                                        .Where(d => d.Properties.TryGetValue("collection", out var c) && string.Equals(c, packCollection, StringComparison.OrdinalIgnoreCase))
                                        .ToList();
                                    if (collectionStickers.Count > 0)
                                        stickerPool = collectionStickers;
                                }

                                if (stickerPool.Count == 0)
                                {
                                    Console.WriteLine($"[Inventory] No stickers found for pack {packDefId}, collection={packCollection}");
                                    await _mongo.UpsertInventoryAsync(invDoc);
                                    return CreateSuccessResponse(request.Id, result);
                                }

                                // Give 3 random stickers from the pack
                                int stickerCount = 3;
                                for (int si = 0; si < stickerCount; si++)
                                {
                                    var picked = stickerPool[Random.Shared.Next(stickerPool.Count)];
                                    var stickerItem = new InventoryItem
                                    {
                                        Id = NextInventoryItemId(ticket, invDoc),
                                        ItemDefinitionId = picked.Id,
                                        Quantity = 1,
                                        Flags = 0,
                                        Date = nowEx
                                    };
                                    result.InventoryItems.Add(stickerItem);
                                    invDoc.Items.Add(new InventoryItemEntry
                                    {
                                        Id = stickerItem.Id,
                                        ItemDefinitionId = stickerItem.ItemDefinitionId,
                                        Quantity = 1,
                                        Flags = 0,
                                        Date = nowEx
                                    });
                                    Console.WriteLine($"[Inventory] Sticker pack {packDefId}: gave sticker {picked.Id} ({picked.DisplayName}) to user {user.Id}");
                                }

                                await _mongo.UpsertInventoryAsync(invDoc);
                                return CreateSuccessResponse(request.Id, result);
                            }
                        }

                        IEnumerable<ServerInventoryItemDefinition> candidates = defs
                            .Where(d => IsEnabledDefinition(d))
                            .Where(d => d.Id >= 2000 && d.Id <= 299000)
                            .Where(d => knownIds == null || knownIds.Contains(d.Id));

                        if (!string.IsNullOrWhiteSpace(collection))
                        {
                            candidates = candidates.Where(d => d.Properties.TryGetValue("collection", out var c) && string.Equals(c, collection, StringComparison.OrdinalIgnoreCase));
                        }

                        // User reported that 11001 shouldn't always (or at all) drop.
                        candidates = candidates.Where(d => d.Id != 11001);

                        // Filter by rarity: cases should not drop Common (value 1) or Uncommon (value 2) skins.
                        // Boxes drop value 1-2, Cases drop value 3-6.
                        if (recipe != null && (recipe.Contains("CASE") || recipe.Contains("BOX") || recipe.Contains("STICKER_PACK") || recipe.StartsWith("RECIPE_V2_")))
                        {
                            candidates = candidates.Where(d => {
                                if (d.Properties.TryGetValue("value", out var vObj) && vObj != null) {
                                    int val = 0;
                                    string sVal = vObj.ToString();
                                    int.TryParse(sVal, out val);
                                    return val >= 3;
                                }
                                return false;
                            });
                        }

                        var candidateList = candidates.ToList();

                        // Weighted rarity selection for cases:
                        // Arcana (6): 2%, Legendary (5): 8%, Epic (5 alias): 8%, Rare (4): 35%, Uncommon (3): 55%
                        // But since we already filtered val >= 3, split into tiers:
                        // value 6 = 2%, value 5 = 8%, value 4 = 35%, value 3 = 55%
                        int GetVal(ServerInventoryItemDefinition d) {
                            if (d.Properties.TryGetValue("value", out var v) && v != null && int.TryParse(v.ToString(), out int r)) return r;
                            return 3;
                        }

                        var tier6 = candidateList.Where(d => GetVal(d) == 6).ToList();
                        var tier5 = candidateList.Where(d => GetVal(d) == 5).ToList();
                        var tier4 = candidateList.Where(d => GetVal(d) == 4).ToList();
                        var tier3 = candidateList.Where(d => GetVal(d) == 3).ToList();

                        int rarityRoll = Random.Shared.Next(100);
                        List<ServerInventoryItemDefinition> rarityPool;
                        if      (rarityRoll < 2  && tier6.Count > 0) rarityPool = tier6;
                        else if (rarityRoll < 10 && tier5.Count > 0) rarityPool = tier5;
                        else if (rarityRoll < 45 && tier4.Count > 0) rarityPool = tier4;
                        else if (tier3.Count > 0)                     rarityPool = tier3;
                        else                                          rarityPool = candidateList;

                        var droppedDefId = rarityPool.Count > 0 ? rarityPool[Random.Shared.Next(rarityPool.Count)].Id : 11002;

                        // 35% шанс StatTrack версии скина (id + 1000000)
                        if (Random.Shared.Next(100) < 35)
                        {
                            int statTrackId = droppedDefId + 1000000;
                            if (defs.Any(d => d.Id == statTrackId))
                                droppedDefId = statTrackId;
                        }

                        var dropped = new InventoryItem
                        {
                            Id = NextInventoryItemId(ticket),
                            ItemDefinitionId = droppedDefId,
                            Quantity = 1,
                            Flags = 0,
                            Date = nowEx
                        };

                        result.InventoryItems.Add(dropped);

                        invDoc.Items.Add(new InventoryItemEntry
                        {
                            Id = dropped.Id,
                            ItemDefinitionId = dropped.ItemDefinitionId,
                            Quantity = dropped.Quantity,
                            Flags = dropped.Flags,
                            Date = dropped.Date
                        });
                        await _mongo.UpsertInventoryAsync(invDoc);

                        return CreateSuccessResponse(request.Id, result);
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new ExchangeResult());
                    }

                case "getOtherPlayerItems":
                    // Friend inventory badges/skins queries. Return empty array by default.
                    return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());

                case "sellInventoryItem":
                    // Return: InventoryItem
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        int itemId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        int qty = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        int currencyId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                        if (qty <= 0) qty = 1;

                        var entry = invDoc.Items.FirstOrDefault(x => x.Id == itemId);
                        if (entry == null) return CreateSuccessResponse(request.Id, new InventoryItem());

                        entry.Quantity -= qty;
                        if (entry.Quantity <= 0)
                        {
                            invDoc.Items.Remove(entry);
                        }

                        // Credit currency (simple): +25 per item unless definition has "sellPrice"
                        var credit = 25f * qty;
                        var cur = invDoc.Currencies.FirstOrDefault(x => x.CurrencyId == currencyId);
                        if (cur == null)
                        {
                            invDoc.Currencies.Add(new InventoryCurrencyEntry { CurrencyId = currencyId, Value = credit });
                        }
                        else
                        {
                            cur.Value += credit;
                        }

                        await _mongo.UpsertInventoryAsync(invDoc);

                        var pb = new InventoryItem
                        {
                            Id = itemId,
                            ItemDefinitionId = entry.ItemDefinitionId,
                            Quantity = Math.Max(0, entry.Quantity),
                            Flags = entry.Flags,
                            Date = entry.Date
                        };
                        return CreateSuccessResponse(request.Id, pb);
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new InventoryItem());
                    }

                case "consumeInventoryItem":
                    // Return: InventoryItem
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        int itemId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        int qty = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        if (qty <= 0) qty = 1;

                        var entry = invDoc.Items.FirstOrDefault(x => x.Id == itemId);
                        if (entry == null) return CreateSuccessResponse(request.Id, new InventoryItem());

                        entry.Quantity -= qty;
                        if (entry.Quantity <= 0)
                        {
                            invDoc.Items.Remove(entry);
                        }
                        await _mongo.UpsertInventoryAsync(invDoc);

                        return CreateSuccessResponse(request.Id, new InventoryItem
                        {
                            Id = itemId,
                            ItemDefinitionId = entry.ItemDefinitionId,
                            Quantity = Math.Max(0, entry.Quantity),
                            Flags = entry.Flags,
                            Date = entry.Date
                        });
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new InventoryItem());
                    }

                case "transferInventoryItems":
                    // Return: InventoryItem[]
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        int fromId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        int toId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        int qty = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[2].One).Value;
                        if (qty <= 0) qty = 1;

                        var from = invDoc.Items.FirstOrDefault(x => x.Id == fromId);
                        var to = invDoc.Items.FirstOrDefault(x => x.Id == toId);
                        if (from == null || to == null) return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());
                        if (from.ItemDefinitionId != to.ItemDefinitionId) return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());

                        if (qty > from.Quantity) qty = from.Quantity;
                        from.Quantity -= qty;
                        to.Quantity += qty;
                        if (from.Quantity <= 0) invDoc.Items.Remove(from);

                        await _mongo.UpsertInventoryAsync(invDoc);

                        var res = new List<InventoryItem>
                        {
                            new InventoryItem { Id = to.Id, ItemDefinitionId = to.ItemDefinitionId, Quantity = to.Quantity, Flags = to.Flags, Date = to.Date }
                        };
                        if (from.Quantity > 0)
                        {
                            res.Add(new InventoryItem { Id = from.Id, ItemDefinitionId = from.ItemDefinitionId, Quantity = from.Quantity, Flags = from.Flags, Date = from.Date });
                        }
                        return CreateSuccessResponseArray(request.Id, res);
                    }
                    catch
                    {
                        return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());
                    }

                case "tradeInventoryItems":
                    // Return: InventoryItem[] (client uses it as confirmation)
                    // Minimal: no real trading; just echo local items as still existing.
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var inv = await LoadOrCreateInventoryAsync(user);
                        return CreateSuccessResponseArray(request.Id, inv.InventoryItems);
                    }
                    catch
                    {
                        return CreateSuccessResponseArray(request.Id, Array.Empty<InventoryItem>());
                    }

                case "applyInventoryItem":
                    // Return: InventoryItem (applied item updated)
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        int consumedItemId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        int appliedItemId  = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                        var propName       = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[2].One).Value ?? "";
                        bool isRemovable   = Axlebolt.RpcSupport.Protobuf.Boolean.Parser.ParseFrom(request.Params[3].One).Value;

                        // Session lost — can't safely modify inventory, return skin unchanged
                        if (user.Id <= 0)
                        {
                            Console.WriteLine($"[Inventory] applyInventoryItem: session lost for ticket={ticket}, returning item {appliedItemId} unchanged");
                            return CreateSuccessResponse(request.Id, new InventoryItem { Id = appliedItemId });
                        }

                        var consumed = invDoc.Items.FirstOrDefault(x => x.Id == consumedItemId);
                        var applied  = invDoc.Items.FirstOrDefault(x => x.Id == appliedItemId);

                        // If the target skin doesn't exist, return it as-is so client doesn't lose it
                        if (applied == null)
                        {
                            Console.WriteLine($"[Inventory] applyInventoryItem: target item {appliedItemId} not found for user {user.Id}");
                            return CreateSuccessResponse(request.Id, new InventoryItem { Id = appliedItemId });
                        }

                        // Consume the sticker (quantity -1, remove if 0)
                        if (consumed != null)
                        {
                            consumed.Quantity -= 1;
                            if (consumed.Quantity <= 0)
                                invDoc.Items.Remove(consumed);
                        }

                        if (applied.Properties == null)
                            applied.Properties = new Dictionary<string, InventoryItemPropertyEntry>();

                        if (!string.IsNullOrWhiteSpace(propName))
                        {
                            // Store the sticker's ItemDefinitionId so the client can render it correctly.
                            // propName is e.g. "sticker_0", "sticker_1", etc.
                            int stickerDefId = consumed?.ItemDefinitionId ?? 0;
                            applied.Properties[propName] = new InventoryItemPropertyEntry
                            {
                                Type      = (int)PropertyType.Int,
                                IntValue  = stickerDefId,
                                BoolValue = isRemovable
                            };
                            Console.WriteLine($"[Inventory] Applied sticker defId={stickerDefId} to item {appliedItemId} slot '{propName}' for user {user.Id}");
                        }

                        await _mongo.UpsertInventoryAsync(invDoc);

                        // Build and return the full updated InventoryItem so the client cache stays correct
                        var pb = new InventoryItem
                        {
                            Id               = applied.Id,
                            ItemDefinitionId = applied.ItemDefinitionId,
                            Quantity         = applied.Quantity,
                            Flags            = applied.Flags,
                            Date             = applied.Date
                        };
                        foreach (var kv in applied.Properties)
                        {
                            pb.Properties[kv.Key] = new InventoryItemProperty
                            {
                                Type         = (PropertyType)kv.Value.Type,
                                IntValue     = kv.Value.IntValue,
                                FloatValue   = kv.Value.FloatValue,
                                StringValue  = kv.Value.StringValue,
                                BooleanValue = kv.Value.BoolValue
                            };
                        }
                        return CreateSuccessResponse(request.Id, pb);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine($"[Inventory] applyInventoryItem error: {ex.Message}");
                        // Return a non-empty item so the client doesn't wipe the skin from local cache
                        try
                        {
                            int fallbackId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[1].One).Value;
                            return CreateSuccessResponse(request.Id, new InventoryItem { Id = fallbackId });
                        }
                        catch
                        {
                            return CreateSuccessResponse(request.Id, new InventoryItem());
                        }
                    }

                case "removeInventoryItemProperty":
                    // Return: InventoryItem
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var invDoc = await GetOrCreateInventoryDocAsync(user);

                        int itemId = Axlebolt.RpcSupport.Protobuf.Integer.Parser.ParseFrom(request.Params[0].One).Value;
                        var propName = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[1].One).Value ?? "";

                        var entry = invDoc.Items.FirstOrDefault(x => x.Id == itemId);
                        if (entry == null) return CreateSuccessResponse(request.Id, new InventoryItem());
                        if (entry.Properties != null && !string.IsNullOrWhiteSpace(propName))
                        {
                            entry.Properties.Remove(propName);
                        }

                        await _mongo.UpsertInventoryAsync(invDoc);

                        var pb = new InventoryItem
                        {
                            Id = entry.Id,
                            ItemDefinitionId = entry.ItemDefinitionId,
                            Quantity = entry.Quantity,
                            Flags = entry.Flags,
                            Date = entry.Date
                        };
                        if (entry.Properties != null)
                        {
                            foreach (var kv in entry.Properties)
                            {
                                pb.Properties[kv.Key] = new InventoryItemProperty
                                {
                                    Type = (PropertyType)kv.Value.Type,
                                    IntValue = kv.Value.IntValue,
                                    FloatValue = kv.Value.FloatValue,
                                    StringValue = kv.Value.StringValue,
                                    BooleanValue = kv.Value.BoolValue
                                };
                            }
                        }
                        return CreateSuccessResponse(request.Id, pb);
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new InventoryItem());
                    }

                case "activateCoupon":
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        if (user == null)
                            return CreateErrorResponse(request.Id, "Not authorized");

                        string couponId = "";
                        if (request.Params.Count > 0 && !request.Params[0].One.IsEmpty)
                            couponId = ActivateCouponRequest.Parser.ParseFrom(request.Params[0].One).CouponId;

                        if (string.IsNullOrWhiteSpace(couponId))
                            return CreateErrorResponse(request.Id, "Promo code not found");

                        var promo = await _mongo.GetPromocodeAsync(couponId.Trim().ToUpper());
                        if (promo == null)
                            return CreateErrorResponse(request.Id, "Promo code not found");

                        // Проверяем срок действия
                        if (promo.ExpiresAt > 0 && NowMs() > promo.ExpiresAt)
                            return CreateErrorResponse(request.Id, "Promo code not found");

                        // Проверяем уже активировал ли этот игрок
                        if (promo.ActivatedBy.Contains(user.Id))
                            return CreateErrorResponse(request.Id, "Promo code has already acivated");

                        // Проверяем лимит активаций
                        if (promo.MaxActivations > 0 && promo.ActivatedBy.Count >= promo.MaxActivations)
                            return CreateErrorResponse(request.Id, "Promo code not found");

                        var invDoc = await GetOrCreateInventoryDocAsync(user);
                        var couponResponse = new ActivateCouponResponse();

                        if (promo.ItemDefinitionId > 0)
                        {
                            // Промокод на скин
                            int qty = promo.ItemQuantity > 0 ? promo.ItemQuantity : 1;
                            for (int q = 0; q < qty; q++)
                            {
                                var newItem = new InventoryItemEntry
                                {
                                    Id = NextInventoryItemId(ticket, invDoc),
                                    ItemDefinitionId = promo.ItemDefinitionId,
                                    Quantity = 1,
                                    Flags = 0,
                                    Date = NowMs()
                                };
                                invDoc.Items.Add(newItem);
                                couponResponse.InventoryItems.Add(new InventoryItem
                                {
                                    Id = newItem.Id,
                                    ItemDefinitionId = newItem.ItemDefinitionId,
                                    Quantity = newItem.Quantity,
                                    Date = newItem.Date
                                });
                            }
                        }
                        else
                        {
                            // Промокод на голду (currencyId 102)
                            var goldEntry = invDoc.Currencies.FirstOrDefault(c => c.CurrencyId == 102);
                            if (goldEntry == null)
                            {
                                goldEntry = new InventoryCurrencyEntry { CurrencyId = 102, Value = 0 };
                                invDoc.Currencies.Add(goldEntry);
                            }
                            goldEntry.Value += promo.GoldAmount;
                            couponResponse.Currencies.Add(new CurrencyAmount { CurrencyId = 102, Value = promo.GoldAmount });
                        }

                        await _mongo.UpsertInventoryAsync(invDoc);
                        await _mongo.AddActivationToPromocodeAsync(promo.Code, user.Id);
                        return CreateSuccessResponse(request.Id, couponResponse);
                    }

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private IEnumerable<int> TryReadClientInventorySkinIds()
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "Scripts", "Axlebolt", "Standoff", "Main", "Inventory", "InventoryId.cs"));
                if (!File.Exists(path))
                {
                    return Array.Empty<int>();
                }

                var text = File.ReadAllText(path);

                // Matches: Name = 11001,
                var matches = Regex.Matches(text, @"=\s*(\d+)\s*,");
                var ids = new HashSet<int>();
                foreach (Match m in matches)
                {
                    if (!int.TryParse(m.Groups[1].Value, out var id))
                    {
                        continue;
                    }

                    // Heuristic: skins are the big numeric IDs (e.g. 11001, 12002, ...)
                    // StatTrack skins also live in the 1,0xx,xxx range.
                    if (id >= 10_000)
                    {
                        ids.Add(id);
                    }
                }

                return ids.OrderBy(x => x).ToArray();
            }
            catch
            {
                return Array.Empty<int>();
            }
        }

        private async Task<RpcResponse> HandleMarketplaceAsync(RpcRequest request, string ticket)
        {
            switch (request.MethodName)
            {
                case "getMarketplaceSettings":
                    {
                        var ms = await _mongo.GetSettingsAsync();
                        return CreateSuccessResponse(request.Id, new MarketplaceSettings
                        {
                            Enabled = ms.MarketplaceEnabled,
                            CurrencyId = 102,
                            CommissionPercent = 0.20f,
                            MinCommission = 0.01f
                        });
                    }

                case "getPlayerProcessingRequest":
                    return CreateSuccessResponseArray(request.Id, Array.Empty<ProcessingRequest>());

                case "getPlayerOpenRequests":
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var myId = user.Id.ToString();
                        var mySales = _saleRequestsById.Values.Where(r => r?.Creator?.Id == myId);
                        var myPurchases = _purchaseRequestsById.Values.Where(r => r?.Creator?.Id == myId);
                        return CreateSuccessResponseArray(request.Id, mySales.Concat(myPurchases).ToList());
                    }
                    catch
                    {
                        return CreateSuccessResponseArray(request.Id, Array.Empty<OpenRequest>());
                    }

                case "getClosedRequestsCount":
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var history = await _mongo.GetMarketplaceHistoryAsync(user.Id);
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = history.Count });
                    }
                    catch
                    {
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = 0 });
                    }

                case "getClosedRequests":
                    try
                    {
                        var user = await GetCurrentUserAsync(ticket);
                        var history = await _mongo.GetMarketplaceHistoryAsync(user.Id);
                        var closedList = history.Select(h => {
                            var cr = new ClosedRequest
                            {
                                Id = h.Id,
                                OriginId = h.OriginId,
                                ItemDefinitionId = h.ItemDefinitionId,
                                Price = h.Price,
                                CreateDate = h.CreateDate,
                                CloseDate = h.CloseDate,
                                Quantity = h.Quantity,
                                Reason = (ClosingReason)h.Reason,
                                Type = MarketRequestType.SaleRequest,
                                Creator = new Player { Id = h.CreatorUserId.ToString(), Name = "Seller" }
                            };
                            if (h.PartnerUserId.HasValue)
                            {
                                cr.Partner = new Player { Id = h.PartnerUserId.Value.ToString(), Name = "Buyer" };
                            }
                            foreach (var kv in h.Properties)
                            {
                                cr.Properties[kv.Key] = new InventoryItemProperty
                                {
                                    Type = (PropertyType)kv.Value.Type,
                                    IntValue = kv.Value.IntValue,
                                    FloatValue = kv.Value.FloatValue,
                                    StringValue = kv.Value.StringValue,
                                    BooleanValue = kv.Value.BoolValue
                                };
                            }
                            return cr;
                        }).OrderByDescending(x => x.CloseDate).ToList();
                        return CreateSuccessResponseArray(request.Id, closedList);
                    }
                    catch
                    {
                        return CreateSuccessResponseArray(request.Id, Array.Empty<ClosedRequest>());
                    }

                case "getPlayerClosedRequests":
                    {
                        try
                        {
                            var user = await GetCurrentUserAsync(ticket);
                            var args = request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty
                                ? GetClosedRequestsArgs.Parser.ParseFrom(request.Params[0].One)
                                : new GetClosedRequestsArgs { Page = 0, Size = 50 };

                            var page = args.Page;
                            var size = args.Size;
                            if (page < 0) page = 0;
                            if (size <= 0) size = 50;
                            if (size > 200) size = 200;

                            var history = await _mongo.GetMarketplaceHistoryAsync(user.Id);
                            IEnumerable<MarketplaceHistoryDoc> filtered = history;
                            if (args.Type != MarketRequestType.NoneType)
                            {
                                // We currently persist only SaleRequest closings.
                                if (args.Type != MarketRequestType.SaleRequest)
                                {
                                    filtered = Array.Empty<MarketplaceHistoryDoc>();
                                }
                            }
                            if (args.Reason != ClosingReason.NoneReason)
                            {
                                filtered = filtered.Where(h => h.Reason == (int)args.Reason);
                            }

                            var slice = filtered
                                .OrderByDescending(h => h.CloseDate)
                                .Skip(page * size)
                                .Take(size)
                                .ToList();

                            var closedList = slice.Select(h =>
                            {
                                var cr = new ClosedRequest
                                {
                                    Id = h.Id,
                                    OriginId = h.OriginId,
                                    ItemDefinitionId = h.ItemDefinitionId,
                                    Price = h.Price,
                                    CreateDate = h.CreateDate,
                                    CloseDate = h.CloseDate,
                                    Quantity = h.Quantity,
                                    Reason = (ClosingReason)h.Reason,
                                    Type = MarketRequestType.SaleRequest,
                                    Creator = new Player { Id = h.CreatorUserId.ToString(), Name = "Seller" }
                                };
                                if (h.PartnerUserId.HasValue)
                                {
                                    cr.Partner = new Player { Id = h.PartnerUserId.Value.ToString(), Name = "Buyer" };
                                }
                                foreach (var kv in h.Properties)
                                {
                                    cr.Properties[kv.Key] = new InventoryItemProperty
                                    {
                                        Type = (PropertyType)kv.Value.Type,
                                        IntValue = kv.Value.IntValue,
                                        FloatValue = kv.Value.FloatValue,
                                        StringValue = kv.Value.StringValue,
                                        BooleanValue = kv.Value.BoolValue
                                    };
                                }
                                return cr;
                            }).ToList();

                            return CreateSuccessResponseArray(request.Id, closedList);
                        }
                        catch
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<ClosedRequest>());
                        }
                    }

                case "getPlayerClosedRequestsCount":
                    {
                        try
                        {
                            var user = await GetCurrentUserAsync(ticket);
                            var args = request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty
                                ? GetClosedRequestsCountArgs.Parser.ParseFrom(request.Params[0].One)
                                : new GetClosedRequestsCountArgs();

                            var history = await _mongo.GetMarketplaceHistoryAsync(user.Id);
                            IEnumerable<MarketplaceHistoryDoc> filtered = history;
                            if (args.Type != MarketRequestType.NoneType)
                            {
                                if (args.Type != MarketRequestType.SaleRequest)
                                {
                                    filtered = Array.Empty<MarketplaceHistoryDoc>();
                                }
                            }
                            if (args.Reason != ClosingReason.NoneReason)
                            {
                                filtered = filtered.Where(h => h.Reason == (int)args.Reason);
                            }

                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = filtered.Count() });
                        }
                        catch
                        {
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Integer { Value = 0 });
                        }
                    }

                case "getTrades":
                    {
                        if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<Trade>());
                        }

                        var args = GetTradesArgs.Parser.ParseFrom(request.Params[0].One);
                        var requestedIds = args?.ItemDefinitionIds?.ToArray() ?? Array.Empty<int>();

                        var trades = new List<Trade>(requestedIds.Length);
                        foreach (var id in requestedIds)
                        {
                            var (saleCount, minPrice) = GetSaleSummary(id);
                            // If there are no real sale lots yet, show 0 so UI displays "нет в продаже".
                            var p = saleCount > 0 ? minPrice : 0f;
                            trades.Add(new Trade
                            {
                                Id = id,
                                SalesCount = saleCount,
                                PurchasesCount = 0,
                                SalesPrice = p,
                                PurchasesPrice = 0f
                            });
                        }

                        return CreateSuccessResponseArray(request.Id, trades);
                    }

                case "getTrade":
                    {
                        if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                        {
                            return CreateSuccessResponse(request.Id, new Trade());
                        }

                        var args = GetTradeArgs.Parser.ParseFrom(request.Params[0].One);
                        var id = args?.Id ?? 0;
                        var (saleCount, minPrice) = GetSaleSummary(id);
                        var (purchaseCount, maxPrice) = GetPurchaseSummary(id);
                        return CreateSuccessResponse(request.Id, new Trade
                        {
                            Id = id,
                            SalesCount = saleCount,
                            PurchasesCount = purchaseCount,
                            SalesPrice = saleCount > 0 ? minPrice : 0f,
                            PurchasesPrice = purchaseCount > 0 ? maxPrice : 0f
                        });
                    }

                case "getTradeOpenSaleRequests":
                    {
                        if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<OpenRequest>());
                        }

                        var args = GetTradeOpenSaleRequestsArgs.Parser.ParseFrom(request.Params[0].One);
                        var id = args?.Id ?? 0;
                        var page = args?.Page ?? 0;
                        var size = args?.Size ?? 10;
                        if (page < 0) page = 0;
                        if (size <= 0) size = 10;

                        if (!_saleRequestsByDefinitionId.TryGetValue(id, out var list))
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<OpenRequest>());
                        }

                        OpenRequest[] pageItems;
                        lock (list)
                        {
                            pageItems = list
                                .Where(r => r != null && r.Quantity > 0)
                                .Skip(page * size)
                                .Take(size)
                                .ToArray();
                        }

                        return CreateSuccessResponseArray(request.Id, pageItems);
                    }

                case "getTradeOpenPurchaseRequests":
                    {
                        if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<OpenRequest>());
                        }

                        var args = GetTradeOpenPurchaseRequestsArgs.Parser.ParseFrom(request.Params[0].One);
                        var id = args?.Id ?? 0;
                        var page = args?.Page ?? 0;
                        var size = args?.Size ?? 10;
                        if (page < 0) page = 0;
                        if (size <= 0) size = 10;

                        if (!_purchaseRequestsByDefinitionId.TryGetValue(id, out var list))
                        {
                            return CreateSuccessResponseArray(request.Id, Array.Empty<OpenRequest>());
                        }

                        OpenRequest[] pageItems;
                        lock (list)
                        {
                            pageItems = list
                                .Where(r => r != null && r.Quantity > 0)
                                .Skip(page * size)
                                .Take(size)
                                .ToArray();
                        }

                        return CreateSuccessResponseArray(request.Id, pageItems);
                    }

                case "createSaleRequest":
                    {
                        // Params: CreateSaleRequestArgs
                        try
                        {
                            if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                            {
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                            }

                            var args = CreateSaleRequestArgs.Parser.ParseFrom(request.Params[0].One);
                            var inventoryItemId = args.ItemId;
                            var price = args.Price;

                            var user = await GetCurrentUserAsync(ticket);
                            var invDoc = await _mongo.GetInventoryAsync(user.Id);
                            var item = invDoc?.Items.FirstOrDefault(x => x.Id == inventoryItemId);

                            if (invDoc != null && item != null)
                            {
                                invDoc.Items.Remove(item);
                                await _mongo.UpsertInventoryAsync(invDoc);

                                var reqId = Guid.NewGuid().ToString("N");
                                var openReq = new OpenRequest
                                {
                                    Id = reqId,
                                    Creator = ToProtoPlayer(user),
                                    ItemDefinitionId = item.ItemDefinitionId,
                                    Price = price,
                                    CreateDate = NowMs(),
                                    Type = MarketRequestType.SaleRequest,
                                    Quantity = item.Quantity <= 0 ? 1 : item.Quantity
                                };

                                if (item.Properties != null)
                                {
                                    foreach (var kv in item.Properties)
                                    {
                                        openReq.Properties[kv.Key] = new InventoryItemProperty
                                        {
                                            Type = (PropertyType)kv.Value.Type,
                                            IntValue = kv.Value.IntValue,
                                            FloatValue = kv.Value.FloatValue,
                                            StringValue = kv.Value.StringValue,
                                            BooleanValue = kv.Value.BoolValue
                                        };
                                    }
                                }

                                AddSaleRequest(openReq);

                                // Save to MongoDB
                                var saleDoc = new MarketplaceSaleDoc
                                {
                                    Id = reqId,
                                    SellerUserId = user.Id,
                                    ItemDefinitionId = openReq.ItemDefinitionId,
                                    Price = openReq.Price,
                                    Quantity = openReq.Quantity,
                                    CreateDate = openReq.CreateDate
                                };
                                foreach (var kv in item.Properties)
                                {
                                    saleDoc.Properties[kv.Key] = new InventoryItemPropertyEntry
                                    {
                                        Type = kv.Value.Type,
                                        IntValue = kv.Value.IntValue,
                                        FloatValue = kv.Value.FloatValue,
                                        StringValue = kv.Value.StringValue,
                                        BoolValue = kv.Value.BoolValue
                                    };
                                }
                                await _mongo.InsertMarketplaceSaleAsync(saleDoc);

                                Console.WriteLine($"[Market] User {user.Name} listed itemDef {item.ItemDefinitionId} (invId={inventoryItemId}) for {price}");

                                PublishTradeOpened(openReq);
                                PublishPlayerRequestOpened(ticket, openReq);
                                PublishTradeUpdated(openReq.ItemDefinitionId);
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = reqId });
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Market] createSaleRequest error: {ex.Message}");
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                    }

                case "createPurchaseRequestBySale":
                    {
                        // Params: CreatePurchaseBySaleArgs
                        try
                        {
                            if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                            {
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                            }

                            var args = CreatePurchaseBySaleArgs.Parser.ParseFrom(request.Params[0].One);
                            var saleId = args?.SaleId;
                            if (string.IsNullOrWhiteSpace(saleId) || !_saleRequestsById.TryGetValue(saleId, out var saleReq) || saleReq == null)
                            {
                                Console.WriteLine("[Market] Purchase failed: saleId not found");
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                            }

                            Console.WriteLine("[Market] Purchase request: saleId=" + saleId + " price=" + saleReq.Price);

                            // Remove from market first to avoid double-buy.
                            RemoveSaleRequest(saleReq);
                            await _mongo.DeleteMarketplaceSaleAsync(saleId);

                            var buyer = await GetCurrentUserAsync(ticket);
                            var buyerInv = await GetOrCreateInventoryDocAsync(buyer);

                            // Check if buyer has enough balance (Gold = 102)
                            var buyerCurrency = buyerInv.Currencies.FirstOrDefault(c => c.CurrencyId == 102);
                            Console.WriteLine("[Market] Buyer balance before: " + (buyerCurrency != null ? buyerCurrency.Value.ToString() : "0"));
                            if (buyerCurrency == null || buyerCurrency.Value < saleReq.Price)
                            {
                                // Return lot back to market if failed
                                AddSaleRequest(saleReq);
                                await _mongo.InsertMarketplaceSaleAsync(new MarketplaceSaleDoc
                                {
                                    Id = saleId,
                                    SellerUserId = long.Parse(saleReq.Creator.Id),
                                    ItemDefinitionId = saleReq.ItemDefinitionId,
                                    Price = saleReq.Price,
                                    Quantity = saleReq.Quantity,
                                    CreateDate = saleReq.CreateDate
                                });
                                Console.WriteLine("[Market] Purchase failed: not enough gold");
                                return CreateErrorResponse(request.Id, "Not enough gold");
                            }

                            // Deduct gold from buyer
                            buyerCurrency.Value -= saleReq.Price;

                            // Add item to buyer inventory
                            var newItemId = NextInventoryItemId(ticket, buyerInv);
                            var nowSec = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            var newEntry = new InventoryItemEntry
                            {
                                Id = newItemId,
                                ItemDefinitionId = saleReq.ItemDefinitionId,
                                Quantity = saleReq.Quantity <= 0 ? 1 : saleReq.Quantity,
                                Flags = 0,
                                Date = nowSec
                            };
                            foreach (var kv in saleReq.Properties)
                                newEntry.Properties[kv.Key] = new InventoryItemPropertyEntry
                                {
                                    Type = (int)kv.Value.Type,
                                    IntValue = kv.Value.IntValue,
                                    FloatValue = kv.Value.FloatValue,
                                    StringValue = kv.Value.StringValue,
                                    BoolValue = kv.Value.BooleanValue
                                };
                            buyerInv.Items.Add(newEntry);
                            await _mongo.UpsertInventoryAsync(buyerInv);
                            Console.WriteLine("[Market] Buyer " + buyer.Id + " gold after deduction: " + buyerCurrency.Value);

                            // Pay seller (with 20% commission)
                            if (long.TryParse(saleReq.Creator?.Id, out var sellerUserId))
                            {
                                var sellerInv = await _mongo.GetInventoryAsync(sellerUserId) ?? new InventoryDoc { UserId = sellerUserId };
                                var sellerCurrency = sellerInv.Currencies.FirstOrDefault(c => c.CurrencyId == 102);
                                if (sellerCurrency == null)
                                {
                                    sellerCurrency = new InventoryCurrencyEntry { CurrencyId = 102, Value = 0 };
                                    sellerInv.Currencies.Add(sellerCurrency);
                                }
                                float commission = saleReq.Price * 0.20f;
                                if (commission < 0.01f && saleReq.Price > 0) commission = 0.01f;
                                float sellerProceeds = saleReq.Price - commission;
                                sellerCurrency.Value += (float)Math.Round(sellerProceeds, 2);
                                await _mongo.UpsertInventoryAsync(sellerInv);
                                Console.WriteLine("[Market] Seller " + sellerUserId + " received " + sellerProceeds + " gold");
                            }

                            var closeId = Guid.NewGuid().ToString("N");
                            var closeDate = NowMs();

                            // Notify SELLER: Type=SaleRequest -> client does AddAmount (gold received)
                            var closedForSeller = new ClosedRequest
                            {
                                Id = closeId,
                                OriginId = saleReq.Id,
                                Creator = saleReq.Creator,
                                ItemDefinitionId = saleReq.ItemDefinitionId,
                                Price = saleReq.Price,
                                CreateDate = saleReq.CreateDate,
                                CloseDate = closeDate,
                                Type = MarketRequestType.SaleRequest,
                                Partner = ToProtoPlayer(buyer),
                                PartnerRequestId = string.Empty,
                                Reason = ClosingReason.SuccessTransaction,
                                Quantity = saleReq.Quantity
                            };
                            foreach (var kv in saleReq.Properties) closedForSeller.Properties[kv.Key] = kv.Value;

                            // Notify BUYER: Type=PurchaseRequest -> client does RemoveAmount + Add(item)
                            var closedForBuyer = new ClosedRequest
                            {
                                Id = closeId,
                                OriginId = saleReq.Id,
                                Creator = saleReq.Creator,
                                ItemDefinitionId = saleReq.ItemDefinitionId,
                                Price = saleReq.Price,
                                CreateDate = saleReq.CreateDate,
                                CloseDate = closeDate,
                                Type = MarketRequestType.PurchaseRequest,
                                Partner = saleReq.Creator, // Correctly set the seller as the partner for the buyer
                                PartnerRequestId = string.Empty,
                                Reason = ClosingReason.SuccessTransaction,
                                Quantity = saleReq.Quantity
                            };
                            foreach (var kv in saleReq.Properties) closedForBuyer.Properties[kv.Key] = kv.Value;
                            // Build InventoryItem proto for buyer (needed for client Add(item))
                            var buyerItemProto = new InventoryItem
                            {
                                Id = newEntry.Id,
                                ItemDefinitionId = newEntry.ItemDefinitionId,
                                Quantity = newEntry.Quantity,
                                Flags = newEntry.Flags,
                                Date = newEntry.Date
                            };
                            foreach (var kv in saleReq.Properties) buyerItemProto.Properties[kv.Key] = kv.Value;

                            // Buyer: onPlayerRequestClosed PurchaseRequest -> RemoveAmount + Add(item)
                            PublishPlayerRequestClosed(ticket, closedForBuyer, buyerItemProto);

                            // Seller: onPlayerRequestClosed SaleRequest -> AddAmount (gold received)
                            var sellerTicket = _activeSessions.FirstOrDefault(x => x.Value != null && x.Value.Id.ToString() == saleReq.Creator?.Id).Key;
                            if (sellerTicket != null)
                                PublishPlayerRequestClosed(sellerTicket, closedForSeller);

                            // Broadcast trade update to all subscribers
                            PublishTradeClosed(closedForSeller);
                            PublishTradeUpdated(saleReq.ItemDefinitionId);

                            // ALSO broadcast trade closed to the buyer specifically if they are a subscriber 
                            // (already handled by PublishPlayerRequestClosed for UI logic, 
                            // but PublishTradeClosed handles the Trade Log / History)
                            PublishEvent(TradeTopic(saleReq.ItemDefinitionId), new Axlebolt.RpcSupport.Protobuf.EventResponse
                            {
                                ListenerName = "MarketplaceRemoteEventListener",
                                EventName = "onTradeRequestClosed",
                                Params = { new Axlebolt.RpcSupport.Protobuf.BinaryValue { One = new OnTradeRequestClosedEvent { Request = closedForBuyer }.ToByteString() } }
                            });

                            Console.WriteLine("[Market] Transaction complete: buyer=" + buyer.Id + " paid " + saleReq.Price + "G, seller=" + saleReq.Creator?.Id + " received gold, sellerOnline=" + (sellerTicket != null));
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine("[Market] createPurchaseRequestBySale error: " + ex.Message + "\n" + ex.StackTrace);
                        }
                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                    }
                case "createPurchaseRequest":
                    {
                        try
                        {
                            if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                            {
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                            }

                            var args = CreatePurchaseRequestArgs.Parser.ParseFrom(request.Params[0].One);
                            var itemDefId = args.ItemDefinitionId;
                            var maxPrice = args.Price;
                            var qty = args.Quantity <= 0 ? 1 : args.Quantity;

                            // Try instant match against cheapest sale.
                            OpenRequest bestSale = null;
                            if (_saleRequestsByDefinitionId.TryGetValue(itemDefId, out var saleList))
                            {
                                lock (saleList)
                                {
                                    bestSale = saleList.FirstOrDefault(r => r != null && r.Quantity > 0 && r.Price <= maxPrice);
                                }
                            }

                            if (bestSale != null && _saleRequestsById.TryGetValue(bestSale.Id, out var saleReq) && saleReq != null)
                            {
                                // Buy up to available quantity.
                                var buyQty = Math.Min(qty, saleReq.Quantity <= 0 ? 1 : saleReq.Quantity);

                                if (buyQty > 0)
                                {
                                    var buyer = await GetCurrentUserAsync(ticket);
                                    var buyerInv = await GetOrCreateInventoryDocAsync(buyer);

                                    // Decrease sale quantity or remove sale request entirely.
                                    if (saleReq.Quantity <= buyQty)
                                    {
                                        RemoveSaleRequest(saleReq);
                                        await _mongo.DeleteMarketplaceSaleAsync(saleReq.Id);
                                    }
                                    else
                                    {
                                        saleReq.Quantity -= buyQty;
                                        // Best-effort update in DB: delete+insert with same id to keep it simple.
                                        await _mongo.DeleteMarketplaceSaleAsync(saleReq.Id);
                                        await _mongo.InsertMarketplaceSaleAsync(new MarketplaceSaleDoc
                                        {
                                            Id = saleReq.Id,
                                            SellerUserId = long.TryParse(saleReq.Creator?.Id, out var sId) ? sId : 0,
                                            ItemDefinitionId = saleReq.ItemDefinitionId,
                                            Price = saleReq.Price,
                                            Quantity = saleReq.Quantity,
                                            CreateDate = saleReq.CreateDate,
                                            Properties = saleReq.Properties.ToDictionary(
                                                kv => kv.Key,
                                                kv => new InventoryItemPropertyEntry
                                                {
                                                    Type = (int)kv.Value.Type,
                                                    IntValue = kv.Value.IntValue,
                                                    FloatValue = kv.Value.FloatValue,
                                                    StringValue = kv.Value.StringValue,
                                                    BoolValue = kv.Value.BooleanValue
                                                })
                                        });
                                    }

                                    // Check if buyer has enough balance
                                    var buyerCurrency = buyerInv.Currencies.FirstOrDefault(c => c.CurrencyId == 101);
                                    if (buyerCurrency == null || buyerCurrency.Value < saleReq.Price * buyQty)
                                    {
                                        return CreateErrorResponse(request.Id, "Not enough gold");
                                    }

                                    // Pay buyer currency.
                                    buyerCurrency.Value -= saleReq.Price * buyQty;

                                    // Add item to buyer inventory (single stack).
                                    var newItemId = NextInventoryItemId(ticket, buyerInv);
                                    var nowSec = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                                    var newEntry = new InventoryItemEntry
                                    {
                                        Id = newItemId,
                                        ItemDefinitionId = saleReq.ItemDefinitionId,
                                        Quantity = buyQty,
                                        Flags = 0,
                                        Date = nowSec
                                    };
                                    foreach (var kv in saleReq.Properties)
                                    {
                                        newEntry.Properties[kv.Key] = new InventoryItemPropertyEntry
                                        {
                                            Type = (int)kv.Value.Type,
                                            IntValue = kv.Value.IntValue,
                                            FloatValue = kv.Value.FloatValue,
                                            StringValue = kv.Value.StringValue,
                                            BoolValue = kv.Value.BooleanValue
                                        };
                                    }
                                    buyerInv.Items.Add(newEntry);
                                    await _mongo.UpsertInventoryAsync(buyerInv);

                                    // Credit seller.
                                    if (long.TryParse(saleReq.Creator?.Id, out var sellerUserId))
                                    {
                                        var sellerInv = await _mongo.GetInventoryAsync(sellerUserId);
                                        if (sellerInv != null)
                                        {
                                            var sellerCurrency = sellerInv.Currencies.FirstOrDefault(c => c.CurrencyId == 101);
                                            if (sellerCurrency == null)
                                            {
                                                sellerCurrency = new InventoryCurrencyEntry { CurrencyId = 101, Value = 0 };
                                                sellerInv.Currencies.Add(sellerCurrency);
                                            }
                                            sellerCurrency.Value += saleReq.Price * buyQty;
                                            await _mongo.UpsertInventoryAsync(sellerInv);
                                        }
                                    }

                                    var closed = new ClosedRequest
                                    {
                                        Id = Guid.NewGuid().ToString("N"),
                                        OriginId = saleReq.Id,
                                        Creator = saleReq.Creator,
                                        ItemDefinitionId = saleReq.ItemDefinitionId,
                                        Price = saleReq.Price,
                                        CreateDate = saleReq.CreateDate,
                                        CloseDate = NowMs(),
                                        Type = MarketRequestType.SaleRequest,
                                        Partner = ToProtoPlayer(buyer),
                                        PartnerRequestId = string.Empty,
                                        Reason = ClosingReason.SuccessTransaction,
                                        Quantity = buyQty
                                    };
                                    foreach (var kv in saleReq.Properties) closed.Properties[kv.Key] = kv.Value;

                                    PublishTradeClosed(closed);
                                    PublishTradeUpdated(saleReq.ItemDefinitionId);

                                    await _mongo.InsertMarketplaceHistoryAsync(new MarketplaceHistoryDoc
                                    {
                                        Id = closed.Id,
                                        OriginId = closed.OriginId,
                                        CreatorUserId = long.TryParse(closed.Creator?.Id, out var cId) ? cId : 0,
                                        ItemDefinitionId = closed.ItemDefinitionId,
                                        Price = closed.Price,
                                        CreateDate = closed.CreateDate,
                                        CloseDate = closed.CloseDate,
                                        PartnerUserId = buyer.Id,
                                        Reason = (int)closed.Reason,
                                        Quantity = closed.Quantity,
                                        Properties = newEntry.Properties.ToDictionary(
                                            kv => kv.Key,
                                            kv => new InventoryItemPropertyEntry
                                            {
                                                Type = kv.Value.Type,
                                                IntValue = kv.Value.IntValue,
                                                FloatValue = kv.Value.FloatValue,
                                                StringValue = kv.Value.StringValue,
                                                BoolValue = kv.Value.BoolValue
                                            })
                                    });

                                    var buyerItem2 = new InventoryItem
                                    {
                                        Id = newItemId,
                                        ItemDefinitionId = closed.ItemDefinitionId,
                                        Quantity = closed.Quantity,
                                        Flags = 0,
                                        Date = closed.CloseDate
                                    };
                                    foreach (var kv in saleReq.Properties)
                                    {
                                        buyerItem2.Properties[kv.Key] = kv.Value;
                                    }
                                    PublishPlayerRequestClosed(ticket, closed, buyerItem2);

                                    var sellerTicket = _activeSessions.FirstOrDefault(x => x.Value.Id.ToString() == saleReq.Creator?.Id).Key;
                                    if (sellerTicket != null)
                                    {
                                        PublishPlayerRequestClosed(sellerTicket, closed);
                                    }

                                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = closed.Id });
                                }
                            }

                            // Otherwise place an open purchase request (bid).
                            var me = await GetCurrentUserAsync(ticket);
                            var reqId = Guid.NewGuid().ToString("N");
                            var openReq = new OpenRequest
                            {
                                Id = reqId,
                                Creator = ToProtoPlayer(me),
                                ItemDefinitionId = itemDefId,
                                Price = maxPrice,
                                CreateDate = NowMs(),
                                Type = MarketRequestType.PurchaseRequest,
                                Quantity = qty
                            };
                            AddPurchaseRequest(openReq);
                            PublishTradeOpened(openReq);
                            PublishPlayerRequestOpened(ticket, openReq);
                            PublishTradeUpdated(itemDefId);
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = reqId });
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Market] createPurchaseRequest error: {ex.Message}");
                            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.String { Value = Guid.NewGuid().ToString("N") });
                        }
                    }

                case "cancelRequest":
                    {
                        // Params: CancelRequestArgs
                        try
                        {
                            if (request.Params.Count == 0 || request.Params[0].One == null || request.Params[0].One.IsEmpty)
                            {
                                return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                            }

                            var args = CancelRequestArgs.Parser.ParseFrom(request.Params[0].One);
                            var id = args?.Id;
                            if (!string.IsNullOrWhiteSpace(id) && _saleRequestsById.TryGetValue(id, out var saleReq) && saleReq != null)
                            {
                                RemoveSaleRequest(saleReq);
                                await _mongo.DeleteMarketplaceSaleAsync(id);

                                // Return item back to creator inventory (undo listing).
                                if (long.TryParse(saleReq.Creator?.Id, out var sellerUserId))
                                {
                                    var sellerInv = await _mongo.GetInventoryAsync(sellerUserId);
                                    if (sellerInv != null)
                                    {
                                        // We restore as a new inventory item entry.
                                        var restored = new InventoryItemEntry
                                        {
                                            Id = NextInventoryItemId(sellerUserId.ToString(), sellerInv),
                                            ItemDefinitionId = saleReq.ItemDefinitionId,
                                            Quantity = saleReq.Quantity <= 0 ? 1 : saleReq.Quantity,
                                            Flags = 0,
                                            Date = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                                        };
                                        foreach (var kv in saleReq.Properties)
                                        {
                                            restored.Properties[kv.Key] = new InventoryItemPropertyEntry
                                            {
                                                Type = (int)kv.Value.Type,
                                                IntValue = kv.Value.IntValue,
                                                FloatValue = kv.Value.FloatValue,
                                                StringValue = kv.Value.StringValue,
                                                BoolValue = kv.Value.BooleanValue
                                            };
                                        }
                                        sellerInv.Items.Add(restored);
                                        await _mongo.UpsertInventoryAsync(sellerInv);

                                        // Update client inventory visually if online
                                        var restoredPb = new InventoryItem {
                                            Id = restored.Id,
                                            ItemDefinitionId = restored.ItemDefinitionId,
                                            Quantity = restored.Quantity,
                                            Flags = restored.Flags,
                                            Date = restored.Date
                                        };
                                        foreach (var kv in saleReq.Properties) restoredPb.Properties[kv.Key] = kv.Value;

                                        var closed = new ClosedRequest
                                        {
                                            Id = Guid.NewGuid().ToString("N"),
                                            OriginId = saleReq.Id,
                                            Creator = saleReq.Creator,
                                            ItemDefinitionId = saleReq.ItemDefinitionId,
                                            Price = saleReq.Price,
                                            CreateDate = saleReq.CreateDate,
                                            CloseDate = NowMs(),
                                            Type = MarketRequestType.SaleRequest,
                                            Partner = null,
                                            PartnerRequestId = string.Empty,
                                            Reason = ClosingReason.Cancelled,
                                            Quantity = saleReq.Quantity
                                        };
                                        foreach (var kv in saleReq.Properties)
                                        {
                                            closed.Properties[kv.Key] = kv.Value;
                                        }

                                        PublishTradeClosed(closed);
                                        PublishTradeUpdated(saleReq.ItemDefinitionId);

                                        // Do not save cancelled requests to history as per user request
                                        // await _mongo.InsertMarketplaceHistoryAsync(historyDoc);

                                        PublishPlayerRequestClosed(ticket, closed, restoredPb);
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine($"[Market] cancelRequest error: {ex.Message}");
                        }

                        return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
                    }

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private async Task<RpcResponse> HandleSettingsAsync(RpcRequest request, string ticket)
        {
            return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
        }

        private async Task<RpcResponse> HandleBoltAsync(RpcRequest request, string ticket)
        {
            // BoltRemoteService handles subscriptions and online/offline status
            switch (request.MethodName)
            {
                case "systemTime":
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Long { Value = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() });

                case "subscribe":
                    try
                    {
                        if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                        {
                            var topic = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            SubscribeTopic(ticket, topic);
                        }
                    }
                    catch
                    {
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                case "unsubscribe":
                    try
                    {
                        if (request.Params.Count > 0 && request.Params[0].One != null && !request.Params[0].One.IsEmpty)
                        {
                            var topic = Axlebolt.RpcSupport.Protobuf.String.Parser.ParseFrom(request.Params[0].One).Value;
                            UnsubscribeTopic(ticket, topic);
                        }
                    }
                    catch
                    {
                    }
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });

                default:
                    return CreateSuccessResponse(request.Id, new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true });
            }
        }

        private RpcResponse CreateSuccessResponse(string callId, IMessage result)
        {
            var rpcResponse = new RpcResponse
            {
                Id = callId,
                Return = new BinaryValue { One = result.ToByteString() }
            };

            return rpcResponse; // Keep returning RpcResponse but we need to wrap it in NetworkPacket
        }

        private RpcResponse CreateSuccessResponseArray(string callId, object items)
        {
            var ret = new BinaryValue();

            if (items is IEnumerable<IMessage> messageList)
            {
                foreach (var msg in messageList)
                {
                    ret.Array.Add(msg.ToByteString());
                }
            }
            else if (items is IMessage singleMsg)
            {
                ret.One = singleMsg.ToByteString();
            }

            return new RpcResponse
            {
                Id = callId,
                Return = ret
            };
        }

        private RpcResponse CreateErrorResponse(string callId, string message)
        {
            Console.WriteLine($"[RPC] Soft error: {message}");
            return new RpcResponse
            {
                Id = callId,
                Return = new BinaryValue { One = new Axlebolt.RpcSupport.Protobuf.Boolean { Value = true }.ToByteString() }
            };
        }
    }
}


