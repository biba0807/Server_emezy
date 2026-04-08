using System;
using System.Collections.Generic;
using Axlebolt.Bolt.Protobuf;

namespace StandoffServer.Models
{
    // Helper models for RPC handling - using aliases to avoid conflicts
    public class InventoryItemDefinition
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = "";
        public bool CanBeTraded { get; set; }
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
        public List<CurrencyAmount> BuyPrice { get; set; } = new List<CurrencyAmount>();
    }

    public class ServerLobby
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public string OwnerPlayerId { get; set; } = "";
        public long CreatorUserId { get; set; }
        public long CreatedAtMs { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public int MaxPlayers { get; set; } = 10;
        public int MaxMembers { get; set; } = 10;
        public int MaxSpectators { get; set; } = 2;
        public bool Joinable { get; set; } = true;
        public LobbyType LobbyType { get; set; } = LobbyType.Private;
        public LobbyType Type { get; set; } = LobbyType.Private;
        public string GameServerId { get; set; } = "";
        public bool IsOpen { get; set; } = true;
        public GameServer GameServer { get; set; }
        public PhotonGame PhotonGame { get; set; }
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
        public List<PlayerFriend> Members { get; set; } = new List<PlayerFriend>();

        public static explicit operator Lobby(ServerLobby serverLobby)
        {
            var lobby = new Lobby
            {
                Id = serverLobby.Id,
                Name = serverLobby.Name,
                OwnerPlayerId = serverLobby.OwnerPlayerId,
                LobbyType = serverLobby.LobbyType,
                Joinable = serverLobby.Joinable,
                MaxMembers = serverLobby.MaxMembers,
                MaxSpectators = serverLobby.MaxSpectators,
                GameServer = serverLobby.GameServer,
                PhotonGame = serverLobby.PhotonGame
            };
            foreach (var kv in serverLobby.Data) lobby.Data[kv.Key] = kv.Value;
            foreach (var m in serverLobby.Members) lobby.Members.Add(m);
            return lobby;
        }
    }

    public class ServerLobbyInvite
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string LobbyId { get; set; } = "";
        public long InviterUserId { get; set; }
        public long InvitedUserId { get; set; }
        public long CreatedAtMs { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        public long Date { get; set; }
        public bool IsAccepted { get; set; } = false;
        public PlayerFriend InviteCreator { get; set; }
        public LobbyPlayerType PlayerType { get; set; }
    }
}
