using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using StandoffServer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace StandoffServer.Services
{
    public class MongoService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<UserDoc> _users;
        private readonly IMongoCollection<PlayerStatsDoc> _stats;
        private readonly IMongoCollection<CounterDoc> _counters;
        private readonly IMongoCollection<InventoryDoc> _inventories;
        private readonly IMongoCollection<MarketplaceSaleDoc> _marketplaceSales;
        private readonly IMongoCollection<MarketplaceHistoryDoc> _marketplaceHistory;
        private readonly IMongoCollection<BsonDocument> _clans;
        private readonly IMongoCollection<BsonDocument> _friends;
        private readonly IMongoCollection<ChatMessageDoc> _chatMessages;
        private readonly IMongoCollection<GlobalChatMessageDoc> _globalChatMessages;
        private readonly IMongoCollection<StorageFileDoc> _storageFiles;
        private readonly IMongoCollection<FriendLinkDoc> _friendLinks;
        private readonly IMongoCollection<ClanDoc> _clans2;
        private readonly IMongoCollection<ClanMemberDoc> _clanMembers;
        private readonly IMongoCollection<ClanMessageDoc> _clanMessages;
        private readonly IMongoCollection<ClanReadStateDoc> _clanReadStates;
        private readonly IMongoCollection<ClanRequestDoc> _clanRequests;
        private readonly IMongoCollection<PromocodeDoc> _promocodes;
        private readonly IMongoCollection<SettingsDoc> _settings;
        private readonly IMongoCollection<GameVersionDoc> _versions;

        public MongoService(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(dbName);
            _users = _database.GetCollection<UserDoc>("users");
            _stats = _database.GetCollection<PlayerStatsDoc>("playerStats");
            _counters = _database.GetCollection<CounterDoc>("counters");
            _inventories = _database.GetCollection<InventoryDoc>("inventories");
            _marketplaceSales = _database.GetCollection<MarketplaceSaleDoc>("marketplaceSales");
            _marketplaceHistory = _database.GetCollection<MarketplaceHistoryDoc>("marketplaceHistory");
            _clans = _database.GetCollection<BsonDocument>("clans");
            _friends = _database.GetCollection<BsonDocument>("friends");
            _chatMessages = _database.GetCollection<ChatMessageDoc>("chatMessages");
            _globalChatMessages = _database.GetCollection<GlobalChatMessageDoc>("globalChatMessages");
            _storageFiles = _database.GetCollection<StorageFileDoc>("storageFiles");
            _friendLinks = _database.GetCollection<FriendLinkDoc>("friendLinks");
            _clans2 = _database.GetCollection<ClanDoc>("clans2");
            _clanMembers = _database.GetCollection<ClanMemberDoc>("clanMembers");
            _clanMessages = _database.GetCollection<ClanMessageDoc>("clanMessages");
            _clanReadStates = _database.GetCollection<ClanReadStateDoc>("clanReadStates");
            _clanRequests = _database.GetCollection<ClanRequestDoc>("clanRequests");
            _promocodes = _database.GetCollection<PromocodeDoc>("promocodes");
            _settings = _database.GetCollection<SettingsDoc>("version");
            _versions = _database.GetCollection<GameVersionDoc>("gameVersions");
        }

        public async Task<long> GetNextSequenceAsync(string name)
        {
            var filter = Builders<CounterDoc>.Filter.Eq(c => c.Id, name);
            var update = Builders<CounterDoc>.Update.Inc(c => c.Sequence, 1);
            var options = new FindOneAndUpdateOptions<CounterDoc>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var result = await _counters.FindOneAndUpdateAsync(filter, update, options);
            return result.Sequence;
        }

        public async Task<UserDoc> GetUserByUidAsync(string uid)
        {
            return await _users.Find(u => u.Uid == uid).FirstOrDefaultAsync();
        }

        public async Task<UserDoc> GetUserByIdAsync(long id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(UserDoc user)
        {
            if (user.Id == 0)
            {
                user.Id = await GetNextSequenceAsync("userId");
            }
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(UserDoc user)
        {
            await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        public async Task<List<UserDoc>> SearchUsersAsync(string value, int skip, int limit)
        {
            if (limit <= 0) limit = 20;
            if (limit > 100) limit = 100;
            if (skip < 0) skip = 0;

            if (long.TryParse(value, out var id) && id > 0)
            {
                var u = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
                return u != null ? new List<UserDoc> { u } : new List<UserDoc>();
            }

            value ??= "";
            value = value.Trim();
            if (value.Length == 0) return new List<UserDoc>();

            var escaped = Regex.Escape(value);
            var filter = Builders<UserDoc>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(escaped, "i"));
            return await _users.Find(filter).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<long> CountUsersAsync(string value)
        {
            if (long.TryParse(value, out var id) && id > 0)
            {
                var u = await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
                return u != null ? 1 : 0;
            }

            value ??= "";
            value = value.Trim();
            if (value.Length == 0) return 0;

            var escaped = Regex.Escape(value);
            var filter = Builders<UserDoc>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(escaped, "i"));
            return await _users.CountDocumentsAsync(filter);
        }

        public async Task<PlayerStatsDoc> GetStatsAsync(long userId)
        {
            return await _stats.Find(s => s.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task UpdateStatsAsync(PlayerStatsDoc stats)
        {
            var filter = Builders<PlayerStatsDoc>.Filter.Eq(s => s.UserId, stats.UserId);
            await _stats.ReplaceOneAsync(filter, stats, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<InventoryDoc> GetInventoryAsync(long userId)
        {
            try
            {
                return await _inventories.Find(i => i.UserId == userId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Inventory] GetInventoryAsync error for userId={userId}: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> InventoryExistsAsync(long userId)
        {
            try
            {
                var filter = Builders<InventoryDoc>.Filter.Eq("userId", userId);
                return await _inventories.CountDocumentsAsync(filter) > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task UpsertInventoryAsync(InventoryDoc inv)
        {
            if (inv == null || inv.UserId <= 0)
            {
                Console.WriteLine($"[Inventory] UpsertInventoryAsync blocked: invalid UserId={inv?.UserId}");
                return;
            }
            var filter = Builders<InventoryDoc>.Filter.Eq(i => i.UserId, inv.UserId);
            await _inventories.ReplaceOneAsync(filter, inv, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<List<MarketplaceSaleDoc>> GetAllMarketplaceSalesAsync()
        {
            return await _marketplaceSales.Find(_ => true).ToListAsync();
        }

        public async Task InsertMarketplaceSaleAsync(MarketplaceSaleDoc sale)
        {
            await _marketplaceSales.InsertOneAsync(sale);
        }

        public async Task DeleteMarketplaceSaleAsync(string saleId)
        {
            await _marketplaceSales.DeleteOneAsync(s => s.Id == saleId);
        }

        public async Task InsertMarketplaceHistoryAsync(MarketplaceHistoryDoc history)
        {
            await _marketplaceHistory.InsertOneAsync(history);
        }

        public async Task<List<MarketplaceHistoryDoc>> GetMarketplaceHistoryAsync(long userId)
        {
            return await _marketplaceHistory.Find(h => h.CreatorUserId == userId || h.PartnerUserId == userId).ToListAsync();
        }

        public async Task InsertChatMessageAsync(ChatMessageDoc doc)
        {
            await _chatMessages.InsertOneAsync(doc);
        }

        public async Task<List<ChatMessageDoc>> GetChatMessagesAsync(string chatId, int skip, int limit)
        {
            // Fetch newest page descending, then reverse so client gets oldest-first (newest at bottom)
            var docs = await _chatMessages
                .Find(m => m.ChatId == chatId)
                .SortByDescending(m => m.TimestampMs)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
            docs.Reverse();
            return docs;
        }

        public async Task MarkChatAsReadAsync(string chatId, long readerUserId)
        {
            var filter = Builders<ChatMessageDoc>.Filter.Eq(x => x.ChatId, chatId) &
                         Builders<ChatMessageDoc>.Filter.Eq(x => x.ReceiverUserId, readerUserId) &
                         Builders<ChatMessageDoc>.Filter.Eq(x => x.IsRead, false);
            var update = Builders<ChatMessageDoc>.Update.Set(x => x.IsRead, true);
            await _chatMessages.UpdateManyAsync(filter, update);
        }

        public async Task<int> CountUnreadChatsAsync(long userId)
        {
            // Count distinct chatIds where there exists at least one unread msg for user.
            var filter = Builders<ChatMessageDoc>.Filter.Eq(x => x.ReceiverUserId, userId) &
                         Builders<ChatMessageDoc>.Filter.Eq(x => x.IsRead, false);
            var ids = await _chatMessages.DistinctAsync<string>("chatId", filter);
            return (await ids.ToListAsync()).Count;
        }

        public async Task<List<(string chatId, ChatMessageDoc lastMsg, int unread)>> GetChatUsersSummaryAsync(long userId, int limit)
        {
            // Simple implementation: fetch last messages for chats where user participates.
            // For small scale this is fine; can be replaced with aggregation later.
            var filter = Builders<ChatMessageDoc>.Filter.Or(
                Builders<ChatMessageDoc>.Filter.Eq(x => x.SenderUserId, userId),
                Builders<ChatMessageDoc>.Filter.Eq(x => x.ReceiverUserId, userId)
            );

            var recent = await _chatMessages.Find(filter)
                .SortByDescending(x => x.TimestampMs)
                .Limit(500) // scanning window
                .ToListAsync();

            var dict = new Dictionary<string, ChatMessageDoc>();
            foreach (var m in recent)
            {
                if (m?.ChatId == null) continue;
                if (!dict.ContainsKey(m.ChatId))
                {
                    dict[m.ChatId] = m;
                }
            }

            var result = new List<(string, ChatMessageDoc, int)>();
            foreach (var kv in dict.Take(limit))
            {
                var unreadFilter = Builders<ChatMessageDoc>.Filter.Eq(x => x.ChatId, kv.Key) &
                                   Builders<ChatMessageDoc>.Filter.Eq(x => x.ReceiverUserId, userId) &
                                   Builders<ChatMessageDoc>.Filter.Eq(x => x.IsRead, false);
                var unread = (int)await _chatMessages.CountDocumentsAsync(unreadFilter);
                result.Add((kv.Key, kv.Value, unread));
            }
            return result;
        }

        public async Task InsertGlobalChatMessageAsync(GlobalChatMessageDoc doc)
        {
            await _globalChatMessages.InsertOneAsync(doc);
        }

        public async Task<List<GlobalChatMessageDoc>> GetGlobalChatMessagesAsync(string topic, int skip, int limit)
        {
            return await _globalChatMessages
                .Find(m => m.Topic == topic)
                .SortByDescending(m => m.TimestampMs)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task UpsertStorageFileAsync(StorageFileDoc doc)
        {
            var filter = Builders<StorageFileDoc>.Filter.Eq(x => x.UserId, doc.UserId) &
                         Builders<StorageFileDoc>.Filter.Eq(x => x.Filename, doc.Filename);
            await _storageFiles.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true });
        }

        public async Task DeleteStorageFileAsync(long userId, string filename)
        {
            var filter = Builders<StorageFileDoc>.Filter.Eq(x => x.UserId, userId) &
                         Builders<StorageFileDoc>.Filter.Eq(x => x.Filename, filename);
            await _storageFiles.DeleteOneAsync(filter);
        }

        public async Task<StorageFileDoc> GetStorageFileAsync(long userId, string filename)
        {
            var filter = Builders<StorageFileDoc>.Filter.Eq(x => x.UserId, userId) &
                         Builders<StorageFileDoc>.Filter.Eq(x => x.Filename, filename);
            var result = await _storageFiles.Find(filter).FirstOrDefaultAsync();
            // Fallback: ищем только по имени файла если не нашли по userId+filename
            if (result == null)
            {
                result = await _storageFiles.Find(x => x.Filename == filename).FirstOrDefaultAsync();
            }
            return result;
        }

        public async Task<List<StorageFileDoc>> GetAllStorageFilesAsync(long userId)
        {
            return await _storageFiles.Find(x => x.UserId == userId).ToListAsync();
        }

        public async Task UpsertFriendLinkAsync(FriendLinkDoc link)
        {
            var filter = Builders<FriendLinkDoc>.Filter.Eq(x => x.UserId, link.UserId) &
                         Builders<FriendLinkDoc>.Filter.Eq(x => x.FriendUserId, link.FriendUserId);
            await _friendLinks.ReplaceOneAsync(filter, link, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<FriendLinkDoc> GetFriendLinkAsync(long userId, long friendUserId)
        {
            var filter = Builders<FriendLinkDoc>.Filter.Eq(x => x.UserId, userId) &
                         Builders<FriendLinkDoc>.Filter.Eq(x => x.FriendUserId, friendUserId);
            return await _friendLinks.Find(filter).FirstOrDefaultAsync();
        }

        public async Task DeleteFriendLinkAsync(long userId, long friendUserId)
        {
            var filter = Builders<FriendLinkDoc>.Filter.Eq(x => x.UserId, userId) &
                         Builders<FriendLinkDoc>.Filter.Eq(x => x.FriendUserId, friendUserId);
            await _friendLinks.DeleteOneAsync(filter);
        }

        public async Task<List<FriendLinkDoc>> GetFriendLinksAsync(long userId, int[] statuses, int skip, int limit)
        {
            var filter = Builders<FriendLinkDoc>.Filter.Eq(x => x.UserId, userId);
            if (statuses != null && statuses.Length > 0)
            {
                filter &= Builders<FriendLinkDoc>.Filter.In(x => x.Status, statuses);
            }

            return await _friendLinks.Find(filter)
                .SortByDescending(x => x.UpdatedAtMs)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<long> CountFriendLinksAsync(long userId, int[] statuses)
        {
            var filter = Builders<FriendLinkDoc>.Filter.Eq(x => x.UserId, userId);
            if (statuses != null && statuses.Length > 0)
            {
                filter &= Builders<FriendLinkDoc>.Filter.In(x => x.Status, statuses);
            }
            return await _friendLinks.CountDocumentsAsync(filter);
        }

        public async Task<ClanDoc> GetClanByIdAsync(string clanId)
        {
            return await _clans2.Find(x => x.Id == clanId).FirstOrDefaultAsync();
        }

        public async Task<List<ClanDoc>> FindClansAsync(string query, int skip, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await _clans2.Find(_ => true).SortByDescending(x => x.CreatedAtMs).Skip(skip).Limit(limit).ToListAsync();
            }

            query = query.Trim();
            var filter = Builders<ClanDoc>.Filter.Or(
                Builders<ClanDoc>.Filter.Regex(x => x.Name, new BsonRegularExpression(query, "i")),
                Builders<ClanDoc>.Filter.Regex(x => x.Tag, new BsonRegularExpression(query, "i"))
            );

            return await _clans2.Find(filter).SortByDescending(x => x.CreatedAtMs).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task UpsertClanAsync(ClanDoc clan)
        {
            await _clans2.ReplaceOneAsync(x => x.Id == clan.Id, clan, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<ClanMemberDoc> GetClanMemberAsync(string clanId, long userId)
        {
            return await _clanMembers.Find(x => x.ClanId == clanId && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<ClanMemberDoc>> GetClanMembersAsync(string clanId)
        {
            return await _clanMembers.Find(x => x.ClanId == clanId).ToListAsync();
        }

        public async Task SetClanMemberAsync(ClanMemberDoc m)
        {
            var filter = Builders<ClanMemberDoc>.Filter.Eq(x => x.ClanId, m.ClanId) &
                         Builders<ClanMemberDoc>.Filter.Eq(x => x.UserId, m.UserId);
            await _clanMembers.ReplaceOneAsync(filter, m, new ReplaceOptions { IsUpsert = true });
        }

        public async Task RemoveClanMemberAsync(string clanId, long userId)
        {
            await _clanMembers.DeleteOneAsync(x => x.ClanId == clanId && x.UserId == userId);
        }

        public async Task<string> GetPlayerClanIdAsync(long userId)
        {
            var m = await _clanMembers.Find(x => x.UserId == userId).FirstOrDefaultAsync();
            return m?.ClanId;
        }

        public async Task InsertClanMessageAsync(ClanMessageDoc msg)
        {
            await _clanMessages.InsertOneAsync(msg);
        }

        public async Task<List<ClanMessageDoc>> GetClanMessagesAsync(string clanId, int skip, int limit)
        {
            return await _clanMessages.Find(x => x.ClanId == clanId)
                .SortByDescending(x => x.TimestampMs)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }

        public async Task SetClanReadStateAsync(string clanId, long userId, long lastReadTs)
        {
            var filter = Builders<ClanReadStateDoc>.Filter.Eq(x => x.ClanId, clanId) &
                         Builders<ClanReadStateDoc>.Filter.Eq(x => x.UserId, userId);
            var doc = new ClanReadStateDoc { ClanId = clanId, UserId = userId, LastReadTimestampMs = lastReadTs };
            await _clanReadStates.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<long> GetClanLastReadTsAsync(string clanId, long userId)
        {
            var s = await _clanReadStates.Find(x => x.ClanId == clanId && x.UserId == userId).FirstOrDefaultAsync();
            return s?.LastReadTimestampMs ?? 0;
        }

        public async Task<int> GetUnreadClanMessagesCountAsync(string clanId, long userId)
        {
            var lastRead = await GetClanLastReadTsAsync(clanId, userId);
            var filter = Builders<ClanMessageDoc>.Filter.Eq(x => x.ClanId, clanId) &
                         Builders<ClanMessageDoc>.Filter.Gt(x => x.TimestampMs, lastRead);
            var count = await _clanMessages.CountDocumentsAsync(filter);
            return (int)Math.Min(count, int.MaxValue);
        }

        public async Task UpsertClanRequestAsync(ClanRequestDoc req)
        {
            await _clanRequests.ReplaceOneAsync(x => x.Id == req.Id, req, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<ClanRequestDoc> GetClanRequestAsync(string requestId)
        {
            return await _clanRequests.Find(x => x.Id == requestId).FirstOrDefaultAsync();
        }

        public async Task DeleteClanRequestAsync(string requestId)
        {
            await _clanRequests.DeleteOneAsync(x => x.Id == requestId);
        }

        public async Task<List<ClanRequestDoc>> GetOpenClanRequestsForClanAsync(string clanId)
        {
            return await _clanRequests.Find(x => x.ClanId == clanId && x.Closed == false)
                .SortByDescending(x => x.CreateDateMs)
                .ToListAsync();
        }

        public async Task<List<ClanRequestDoc>> GetOpenClanRequestsForPlayerAsync(long userId)
        {
            // Join requests created by this player + invites addressed to this player
            var filter = Builders<ClanRequestDoc>.Filter.Eq(x => x.Closed, false) &
                         Builders<ClanRequestDoc>.Filter.Or(
                             Builders<ClanRequestDoc>.Filter.Eq(x => x.SenderUserId, userId),
                             Builders<ClanRequestDoc>.Filter.Eq(x => x.InvitedUserId, userId)
                         );
            return await _clanRequests.Find(filter).SortByDescending(x => x.CreateDateMs).ToListAsync();
        }

        public async Task<List<ClanRequestDoc>> GetClosedClanRequestsForClanAsync(string clanId)
        {
            return await _clanRequests.Find(x => x.ClanId == clanId && x.Closed == true)
                .SortByDescending(x => x.CloseDateMs)
                .ToListAsync();
        }

        public async Task<List<ClanRequestDoc>> GetClosedClanRequestsForPlayerAsync(long userId)
        {
            var filter = Builders<ClanRequestDoc>.Filter.Eq(x => x.Closed, true) &
                         Builders<ClanRequestDoc>.Filter.Or(
                             Builders<ClanRequestDoc>.Filter.Eq(x => x.SenderUserId, userId),
                             Builders<ClanRequestDoc>.Filter.Eq(x => x.InvitedUserId, userId)
                         );
            return await _clanRequests.Find(filter).SortByDescending(x => x.CloseDateMs).ToListAsync();
        }

        public async Task<PromocodeDoc> GetPromocodeAsync(string code)
        {
            return await _promocodes.Find(x => x.Code == code).FirstOrDefaultAsync();
        }

        public async Task AddActivationToPromocodeAsync(string code, long userId)
        {
            var filter = Builders<PromocodeDoc>.Filter.Eq(x => x.Code, code);
            var update = Builders<PromocodeDoc>.Update.AddToSet(x => x.ActivatedBy, userId);
            await _promocodes.UpdateOneAsync(filter, update);
        }

        public async Task SetUserBanAsync(long userId, bool isBanned)
        {
            var filter = Builders<UserDoc>.Filter.Eq(u => u.Id, userId);
            var update = Builders<UserDoc>.Update.Set(u => u.IsBanned, isBanned);
            await _users.UpdateOneAsync(filter, update);
        }

        public async Task<SettingsDoc> GetSettingsAsync()
        {
            var s = await _settings.Find(x => x.Id == "global").FirstOrDefaultAsync();
            return s ?? new SettingsDoc();
        }

        public async Task UpsertSettingsAsync(SettingsDoc doc)
        {
            await _settings.ReplaceOneAsync(x => x.Id == "global", doc, new ReplaceOptions { IsUpsert = true });
        }

        // Регистрирует версию в коллекции gameVersions (если ещё нет — создаёт с enabled=true)
        // Также обновляет gameVersion в global документе
        public async Task RegisterVersionAsync(string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return;

            try
            {
                // Проверяем есть ли уже документ с этой версией
                var count = await _versions.CountDocumentsAsync(
                    Builders<GameVersionDoc>.Filter.Eq("_id", version));

                if (count == 0)
                {
                    // Создаём новый документ для этой версии
                    await _versions.InsertOneAsync(new GameVersionDoc { Version = version, Enabled = true });
                    Console.WriteLine($"[Version] Created new version document: {version}");
                }
                else
                {
                    Console.WriteLine($"[Version] Version {version} already exists in gameVersions");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Version] RegisterVersionAsync error: {ex.Message}");
            }

            // Обновляем gameVersion в global документе коллекции version
            try
            {
                var srvSettings = await GetSettingsAsync();
                if (srvSettings.GameVersion != version)
                {
                    srvSettings.GameVersion = version;
                    await UpsertSettingsAsync(srvSettings);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[Version] UpdateGlobal error: {ex.Message}");
            }
        }

        // Проверяет разрешена ли версия (true = пускаем, false = блокируем)
        public async Task<bool> IsVersionAllowedAsync(string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return true;
            var filter = Builders<GameVersionDoc>.Filter.Eq("_id", version);
            var doc = await _versions.Find(filter).FirstOrDefaultAsync();
            // Если версия не зарегистрирована — пускаем (новая версия)
            if (doc == null) return true;
            return doc.Enabled;
        }
    }
}
