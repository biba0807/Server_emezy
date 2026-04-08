using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanMember : IMessage<ClanMember>, IMessage, IEquatable<ClanMember>, IDeepCloneable<ClanMember>
    {
        private static readonly MessageParser<ClanMember> _parser = new MessageParser<ClanMember>(() => new ClanMember());

        public const int IdFieldNumber = 1;

        private string id_ = "";

        public const int PlayerFieldNumber = 2;

        private Player player_;

        public const int ClanIdFieldNumber = 3;

        private string clanId_ = "";

        public const int RoleFieldNumber = 4;

        private ClanMemberRole role_;

        public const int CreateDateFieldNumber = 5;

        private long createDate_;

        [DebuggerNonUserCode]
        public static MessageParser<ClanMember> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[1];

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
        public Player Player
        {
            get
            {
                return player_;
            }
            set
            {
                player_ = value;
            }
        }

        [DebuggerNonUserCode]
        public string ClanId
        {
            get
            {
                return clanId_;
            }
            set
            {
                clanId_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public ClanMemberRole Role
        {
            get
            {
                return role_;
            }
            set
            {
                role_ = value;
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
        public ClanMember()
        {
        }

        [DebuggerNonUserCode]
        public ClanMember(ClanMember other)
            : this()
        {
            id_ = other.id_;
            Player = ((other.player_ != null) ? other.Player.Clone() : null);
            clanId_ = other.clanId_;
            Role = ((other.role_ != null) ? other.Role.Clone() : null);
            createDate_ = other.createDate_;
        }

        [DebuggerNonUserCode]
        public ClanMember Clone()
        {
            return new ClanMember(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ClanMember);
        }

        [DebuggerNonUserCode]
        public bool Equals(ClanMember other)
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
            if (!object.Equals(Player, other.Player))
            {
                return false;
            }
            if (ClanId != other.ClanId)
            {
                return false;
            }
            if (!object.Equals(Role, other.Role))
            {
                return false;
            }
            if (CreateDate != other.CreateDate)
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
            if (player_ != null)
            {
                num ^= Player.GetHashCode();
            }
            if (ClanId.Length != 0)
            {
                num ^= ClanId.GetHashCode();
            }
            if (role_ != null)
            {
                num ^= Role.GetHashCode();
            }
            if (CreateDate != 0)
            {
                num ^= CreateDate.GetHashCode();
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
            if (player_ != null)
            {
                output.WriteRawTag(18);
                output.WriteMessage(Player);
            }
            if (ClanId.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteString(ClanId);
            }
            if (role_ != null)
            {
                output.WriteRawTag(34);
                output.WriteMessage(Role);
            }
            if (CreateDate != 0)
            {
                output.WriteRawTag(40);
                output.WriteInt64(CreateDate);
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
            if (player_ != null)
            {
                num += 1 + CodedOutputStream.ComputeMessageSize(Player);
            }
            if (ClanId.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(ClanId);
            }
            if (role_ != null)
            {
                num += 1 + CodedOutputStream.ComputeMessageSize(Role);
            }
            if (CreateDate != 0)
            {
                num += 1 + CodedOutputStream.ComputeInt64Size(CreateDate);
            }
            return num;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanMember other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Id.Length != 0)
            {
                Id = other.Id;
            }
            if (other.player_ != null)
            {
                if (player_ == null)
                {
                    player_ = new Player();
                }
                Player.MergeFrom(other.Player);
            }
            if (other.ClanId.Length != 0)
            {
                ClanId = other.ClanId;
            }
            if (other.role_ != null)
            {
                if (role_ == null)
                {
                    role_ = new ClanMemberRole();
                }
                Role.MergeFrom(other.Role);
            }
            if (other.CreateDate != 0)
            {
                CreateDate = other.CreateDate;
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
                        if (player_ == null)
                        {
                            player_ = new Player();
                        }
                        input.ReadMessage(player_);
                        break;
                    case 26u:
                        ClanId = input.ReadString();
                        break;
                    case 34u:
                        if (role_ == null)
                        {
                            role_ = new ClanMemberRole();
                        }
                        input.ReadMessage(role_);
                        break;
                    case 40u:
                        CreateDate = input.ReadInt64();
                        break;
                }
            }
        }
    }
}
