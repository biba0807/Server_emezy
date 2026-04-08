using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class Clan : IMessage<Clan>, IMessage, IEquatable<Clan>, IDeepCloneable<Clan>
    {
        private static readonly MessageParser<Clan> _parser = new MessageParser<Clan>(() => new Clan());

        public const int IdFieldNumber = 1;

        private string id_ = "";

        public const int NameFieldNumber = 2;

        private string name_ = "";

        public const int TagFieldNumber = 3;

        private string tag_ = "";

        public const int ClanTypeFieldNumber = 4;

        private ClanType clanType_ = ClanType.Closed;

        public const int AvatarIdFieldNumber = 5;

        private string avatarId_ = "";

        public const int CreateDateFieldNumber = 6;

        private long createDate_;

        public const int ClanLevelFieldNumber = 7;

        private int clanLevel_;

        [DebuggerNonUserCode]
        public static MessageParser<Clan> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[0];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string Id
        {
            get
            {
                return id_;
            }
            set
            {
                id_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public string Name
        {
            get
            {
                return name_;
            }
            set
            {
                name_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public string Tag
        {
            get
            {
                return tag_;
            }
            set
            {
                tag_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public ClanType ClanType
        {
            get
            {
                return clanType_;
            }
            set
            {
                clanType_ = value;
            }
        }

        [DebuggerNonUserCode]
        public string AvatarId
        {
            get
            {
                return avatarId_;
            }
            set
            {
                avatarId_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public long CreateDate
        {
            get
            {
                return createDate_;
            }
            set
            {
                createDate_ = value;
            }
        }

        [DebuggerNonUserCode]
        public int ClanLevel
        {
            get
            {
                return clanLevel_;
            }
            set
            {
                clanLevel_ = value;
            }
        }

        [DebuggerNonUserCode]
        public Clan()
        {
        }

        [DebuggerNonUserCode]
        public Clan(Clan other)
            : this()
        {
            id_ = other.id_;
            name_ = other.name_;
            tag_ = other.tag_;
            clanType_ = other.clanType_;
            avatarId_ = other.avatarId_;
            createDate_ = other.createDate_;
            clanLevel_ = other.clanLevel_;
        }

        [DebuggerNonUserCode]
        public Clan Clone()
        {
            return new Clan(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as Clan);
        }

        [DebuggerNonUserCode]
        public bool Equals(Clan other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (Id != other.Id)
            {
                return false;
            }
            if (Name != other.Name)
            {
                return false;
            }
            if (Tag != other.Tag)
            {
                return false;
            }
            if (ClanType != other.ClanType)
            {
                return false;
            }
            if (AvatarId != other.AvatarId)
            {
                return false;
            }
            if (CreateDate != other.CreateDate)
            {
                return false;
            }
            if (ClanLevel != other.ClanLevel)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int num = 1;
            if (Id.Length != 0)
            {
                num ^= Id.GetHashCode();
            }
            if (Name.Length != 0)
            {
                num ^= Name.GetHashCode();
            }
            if (Tag.Length != 0)
            {
                num ^= Tag.GetHashCode();
            }
            if (ClanType != 0)
            {
                num ^= ClanType.GetHashCode();
            }
            if (AvatarId.Length != 0)
            {
                num ^= AvatarId.GetHashCode();
            }
            if (CreateDate != 0)
            {
                num ^= CreateDate.GetHashCode();
            }
            if (ClanLevel != 0)
            {
                num ^= ClanLevel.GetHashCode();
            }
            return num;
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (Id.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(Id);
            }
            if (Name.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Name);
            }
            if (Tag.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteString(Tag);
            }
            if (ClanType != 0)
            {
                output.WriteRawTag(32);
                output.WriteEnum((int)ClanType);
            }
            if (AvatarId.Length != 0)
            {
                output.WriteRawTag(42);
                output.WriteString(AvatarId);
            }
            if (CreateDate != 0)
            {
                output.WriteRawTag(48);
                output.WriteInt64(CreateDate);
            }
            if (ClanLevel != 0)
            {
                output.WriteRawTag(56);
                output.WriteInt32(ClanLevel);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int num = 0;
            if (Id.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Id);
            }
            if (Name.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Name);
            }
            if (Tag.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Tag);
            }
            if (ClanType != 0)
            {
                num += 1 + CodedOutputStream.ComputeEnumSize((int)ClanType);
            }
            if (AvatarId.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(AvatarId);
            }
            if (CreateDate != 0)
            {
                num += 1 + CodedOutputStream.ComputeInt64Size(CreateDate);
            }
            if (ClanLevel != 0)
            {
                num += 1 + CodedOutputStream.ComputeInt32Size(ClanLevel);
            }
            return num;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(Clan other)
        {
            if (other != null)
            {
                if (other.Id.Length != 0)
                {
                    Id = other.Id;
                }
                if (other.Name.Length != 0)
                {
                    Name = other.Name;
                }
                if (other.Tag.Length != 0)
                {
                    Tag = other.Tag;
                }
                if (other.ClanType != 0)
                {
                    ClanType = other.ClanType;
                }
                if (other.AvatarId.Length != 0)
                {
                    AvatarId = other.AvatarId;
                }
                if (other.CreateDate != 0)
                {
                    CreateDate = other.CreateDate;
                }
                if (other.ClanLevel != 0)
                {
                    ClanLevel = other.ClanLevel;
                }
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint num;
            while ((num = input.ReadTag()) != 0)
            {
                switch (num)
                {
                    default:
                        input.SkipLastField();
                        break;
                    case 10u:
                        Id = input.ReadString();
                        break;
                    case 18u:
                        Name = input.ReadString();
                        break;
                    case 26u:
                        Tag = input.ReadString();
                        break;
                    case 32u:
                        clanType_ = (ClanType)input.ReadEnum();
                        break;
                    case 42u:
                        AvatarId = input.ReadString();
                        break;
                    case 48u:
                        CreateDate = input.ReadInt64();
                        break;
                    case 56u:
                        ClanLevel = input.ReadInt32();
                        break;
                }
            }
        }
    }
}
