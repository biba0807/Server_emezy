using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace StandoffServer.Models
{
    public class UserDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("id")]
        public long Id { get; set; } // Sequential ID starting from 1

        [BsonElement("customId")]
        public string CustomId { get; set; } // Кастомный айди (например "DEV_01")

        [BsonElement("uid")]
        public string Uid { get; set; } // From TokenCredential

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("avatarId")]
        public int AvatarId { get; set; }

        [BsonElement("ban")]
        public bool IsBanned { get; set; }

        [BsonElement("registrationDate")]
        public long RegistrationDate { get; set; }

        [BsonElement("timeInGame")]
        public long TimeInGame { get; set; }

        [BsonElement("attributes")]
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
    }

    public class PlayerStatsDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("stats")]
        public List<StatEntry> Stats { get; set; } = new List<StatEntry>();
    }

    public class StatEntry
    {
        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("iv")]
        public long IntValue { get; set; }

        [BsonElement("fv")]
        public float FloatValue { get; set; }

        [BsonElement("t")]
        public int Type { get; set; } // StatDefType
    }

    public class CounterDoc
    {
        [BsonId]
        public string Id { get; set; } // "userId"

        [BsonElement("seq")]
        public long Sequence { get; set; }
    }

    public class InventoryDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("currencies")]
        public List<InventoryCurrencyEntry> Currencies { get; set; } = new List<InventoryCurrencyEntry>();

        [BsonElement("items")]
        public List<InventoryItemEntry> Items { get; set; } = new List<InventoryItemEntry>();
    }

    public class InventoryCurrencyEntry
    {
        [BsonElement("id")]
        public int CurrencyId { get; set; }

        [BsonElement("v")]
        public float Value { get; set; }
    }

    public class InventoryItemEntry
    {
        [BsonElement("id")]
        public int Id { get; set; }

        [BsonElement("def")]
        public int ItemDefinitionId { get; set; }

        [BsonElement("q")]
        public int Quantity { get; set; }

        [BsonElement("f")]
        public int Flags { get; set; }

        [BsonElement("d")]
        public long Date { get; set; }

        [BsonElement("p")]
        public Dictionary<string, InventoryItemPropertyEntry> Properties { get; set; } = new Dictionary<string, InventoryItemPropertyEntry>();
    }

    public class InventoryItemPropertyEntry
    {
        [BsonElement("t")]
        public int Type { get; set; }

        [BsonElement("iv")]
        public int IntValue { get; set; }

        [BsonElement("fv")]
        public float FloatValue { get; set; }

        private string _stringValue;
        [BsonElement("sv")]
        [BsonIgnoreIfNull]
        public string StringValue
        {
            get => _stringValue ?? "";
            set => _stringValue = value;
        }

        [BsonElement("bv")]
        public bool BoolValue { get; set; }
    }

    public class MarketplaceSaleDoc
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("sellerId")]
        public long SellerUserId { get; set; }

        [BsonElement("defId")]
        public int ItemDefinitionId { get; set; }

        [BsonElement("price")]
        public float Price { get; set; }

        [BsonElement("qty")]
        public int Quantity { get; set; }

        [BsonElement("created")]
        public long CreateDate { get; set; }

        [BsonElement("props")]
        public Dictionary<string, InventoryItemPropertyEntry> Properties { get; set; } = new Dictionary<string, InventoryItemPropertyEntry>();
    }

    public class MarketplaceHistoryDoc
    {
        [BsonId]
        public string Id { get; set; }

        [BsonElement("originId")]
        public string OriginId { get; set; }

        [BsonElement("creatorId")]
        public long CreatorUserId { get; set; }

        [BsonElement("defId")]
        public int ItemDefinitionId { get; set; }

        [BsonElement("price")]
        public float Price { get; set; }

        [BsonElement("created")]
        public long CreateDate { get; set; }

        [BsonElement("closed")]
        public long CloseDate { get; set; }

        [BsonElement("partnerId")]
        public long? PartnerUserId { get; set; }

        [BsonElement("reason")]
        public int Reason { get; set; } // ClosingReason

        [BsonElement("qty")]
        public int Quantity { get; set; }

        [BsonElement("props")]
        public Dictionary<string, InventoryItemPropertyEntry> Properties { get; set; } = new Dictionary<string, InventoryItemPropertyEntry>();
    }

    public class ChatMessageDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("chatId")]
        public string ChatId { get; set; } // stable id: "u_<min>_<max>"
        
        [BsonElement("senderId")]
        public long SenderUserId { get; set; }

        [BsonElement("receiverId")]
        public long ReceiverUserId { get; set; }

        [BsonElement("msg")]
        public string Message { get; set; }

        [BsonElement("ts")]
        public long TimestampMs { get; set; }

        [BsonElement("r")]
        public bool IsRead { get; set; }
    }

    public class GlobalChatMessageDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("topic")]
        public string Topic { get; set; }

        [BsonElement("senderId")]
        public long SenderUserId { get; set; }

        [BsonElement("msg")]
        public string Message { get; set; }

        [BsonElement("ts")]
        public long TimestampMs { get; set; }
    }

    public class StorageFileDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("fn")]
        public string Filename { get; set; }

        [BsonElement("b")]
        public byte[] Bytes { get; set; }

        [BsonElement("u")]
        public long UpdatedAtMs { get; set; }
    }

    public class FriendLinkDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("u")]
        public long UserId { get; set; }

        [BsonElement("f")]
        public long FriendUserId { get; set; }

        [BsonElement("s")]
        public int Status { get; set; } // Axlebolt.Bolt.Protobuf.RelationshipStatus enum value

        [BsonElement("ts")]
        public long UpdatedAtMs { get; set; }
    }

    public class ClanDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("id")]
        public string Id { get; set; } // clanId string

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("avatar")]
        public string AvatarId { get; set; }

        [BsonElement("created")]
        public long CreatedAtMs { get; set; }

        [BsonElement("type")]
        public int JoinType { get; set; } // JoinClanType enum

        [BsonElement("lvl")]
        public int Level { get; set; }

        [BsonElement("ctype")]
        public int ClanType { get; set; } // Axlebolt.Bolt.Protobuf.ClanType
    }

    public class ClanMemberDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("clanId")]
        public string ClanId { get; set; }

        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("role")]
        public int Role { get; set; } // ClanMemberRole

        [BsonElement("joined")]
        public long JoinedAtMs { get; set; }
    }

    public class ClanMessageDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("clanId")]
        public string ClanId { get; set; }

        [BsonElement("senderId")]
        public long SenderUserId { get; set; }

        [BsonElement("msg")]
        public string Message { get; set; }

        [BsonElement("ts")]
        public long TimestampMs { get; set; }
    }

    public class ClanReadStateDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("clanId")]
        public string ClanId { get; set; }

        [BsonElement("userId")]
        public long UserId { get; set; }

        [BsonElement("lastReadTs")]
        public long LastReadTimestampMs { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class SettingsDoc
    {
        [BsonId]
        public string Id { get; set; } = "global";

        [BsonElement("marketplaceEnabled")]
        public bool MarketplaceEnabled { get; set; } = true;

        [BsonElement("gameVersion")]
        public string GameVersion { get; set; } = "";

        [BsonElement("versionEnabled")]
        public bool VersionEnabled { get; set; } = false;
    }

    // Каждая версия игры — отдельный документ в коллекции "version"
    // _id = "0.12.1", enabled = true → пускаем, false → диалог обновления
    public class GameVersionDoc
    {
        [BsonId]
        public string Version { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; } = true;
    }

    public class PromocodeDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("code")]
        public string Code { get; set; }

        // Голда (0 если промокод на скин)
        [BsonElement("goldAmount")]
        public int GoldAmount { get; set; }

        // Скин (0 если промокод на голду)
        [BsonElement("itemDefinitionId")]
        public int ItemDefinitionId { get; set; }

        [BsonElement("itemQuantity")]
        public int ItemQuantity { get; set; }

        // Максимальное кол-во активаций (0 = безлимит)
        [BsonElement("maxActivations")]
        public int MaxActivations { get; set; }

        // Список userId которые уже активировали
        [BsonElement("activatedBy")]
        public List<long> ActivatedBy { get; set; } = new List<long>();

        // Дата истечения в ms (0 = бессрочно)
        [BsonElement("expiresAt")]
        public long ExpiresAt { get; set; }
    }

    public class ClanRequestDoc
    {
        [BsonId]
        public ObjectId InternalId { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("id")]
        public string Id { get; set; } // request id

        [BsonElement("clanId")]
        public string ClanId { get; set; }

        [BsonElement("senderId")]
        public long SenderUserId { get; set; }

        [BsonElement("invitedId")]
        public long? InvitedUserId { get; set; }

        [BsonElement("t")]
        public int RequestType { get; set; } // RequestType

        [BsonElement("created")]
        public long CreateDateMs { get; set; }

        [BsonElement("closed")]
        public bool Closed { get; set; }

        [BsonElement("closeDate")]
        public long? CloseDateMs { get; set; }

        [BsonElement("reason")]
        public int? ClosingReason { get; set; } // ClanClosingReason
    }
}
