using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanJoinRequest : IMessage<ClanJoinRequest>, IMessage, IEquatable<ClanJoinRequest>, IDeepCloneable<ClanJoinRequest>
    {
        private static readonly MessageParser<ClanJoinRequest> _parser = new MessageParser<ClanJoinRequest>(() => new ClanJoinRequest());

        public const int IdFieldNumber = 1;
        private string id_ = "";

        public const int ClanFieldNumber = 2;
        private Clan clan_;

        public const int RequestSenderFieldNumber = 3;
        private Player requestSender_;

        public const int CreateDateFieldNumber = 4;
        private long createDate_;

        public const int CloseDateFieldNumber = 5;
        private long closeDate_;

        public const int RequestTypeFieldNumber = 6;
        private RequestType requestType_;

        [DebuggerNonUserCode]
        public static MessageParser<ClanJoinRequest> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[3];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string Id
        {
            get => id_;
            set => id_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public Clan Clan
        {
            get => clan_;
            set => clan_ = value;
        }

        [DebuggerNonUserCode]
        public Player RequestSender
        {
            get => requestSender_;
            set => requestSender_ = value;
        }

        [DebuggerNonUserCode]
        public long CreateDate
        {
            get => createDate_;
            set => createDate_ = value;
        }

        [DebuggerNonUserCode]
        public long CloseDate
        {
            get => closeDate_;
            set => closeDate_ = value;
        }

        [DebuggerNonUserCode]
        public RequestType RequestType
        {
            get => requestType_;
            set => requestType_ = value;
        }

        [DebuggerNonUserCode]
        public ClanJoinRequest() { }

        [DebuggerNonUserCode]
        public ClanJoinRequest(ClanJoinRequest other) : this()
        {
            id_ = other.id_;
            clan_ = other.clan_?.Clone();
            requestSender_ = other.requestSender_?.Clone();
            createDate_ = other.createDate_;
            closeDate_ = other.closeDate_;
            requestType_ = other.requestType_;
        }

        [DebuggerNonUserCode]
        public ClanJoinRequest Clone() => new ClanJoinRequest(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ClanJoinRequest);

        [DebuggerNonUserCode]
        public bool Equals(ClanJoinRequest other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return id_ == other.id_ &&
                   Equals(clan_, other.clan_) &&
                   Equals(requestSender_, other.requestSender_) &&
                   createDate_ == other.createDate_ &&
                   closeDate_ == other.closeDate_ &&
                   requestType_ == other.requestType_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= id_.GetHashCode();
            hash ^= (clan_?.GetHashCode() ?? 0);
            hash ^= (requestSender_?.GetHashCode() ?? 0);
            hash ^= createDate_.GetHashCode();
            hash ^= closeDate_.GetHashCode();
            hash ^= requestType_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (output == null) throw new NullReferenceException();

            if (id_.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(id_);
            }
            if (clan_ != null)
            {
                output.WriteRawTag(18);
                output.WriteMessage(clan_);
            }
            if (requestSender_ != null)
            {
                output.WriteRawTag(26);
                output.WriteMessage(requestSender_);
            }
            if (createDate_ != 0)
            {
                output.WriteRawTag(32);
                output.WriteInt64(createDate_);
            }
            if (closeDate_ != 0)
            {
                output.WriteRawTag(40);
                output.WriteInt64(closeDate_);
            }
            if (requestType_ != 0)
            {
                output.WriteRawTag(48);
                output.WriteEnum((int)requestType_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (id_.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(id_);
            }
            if (clan_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(clan_);
            }
            if (requestSender_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(requestSender_);
            }
            if (createDate_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt64Size(createDate_);
            }
            if (closeDate_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt64Size(closeDate_);
            }
            if (requestType_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeEnumSize((int)requestType_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanJoinRequest other)
        {
            if (other == null) return;
            if (other.id_.Length != 0) id_ = other.id_;
            if (other.clan_ != null)
            {
                if (clan_ == null) clan_ = new Clan();
                clan_.MergeFrom(other.clan_);
            }
            if (other.requestSender_ != null)
            {
                if (requestSender_ == null) requestSender_ = new Player();
                requestSender_.MergeFrom(other.requestSender_);
            }
            if (other.createDate_ != 0L) createDate_ = other.createDate_;
            if (other.closeDate_ != 0L) closeDate_ = other.closeDate_;
            if (other.requestType_ != 0) requestType_ = other.requestType_;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        Id = input.ReadString();
                        break;
                    case 18:
                        if (clan_ == null)
                        {
                            clan_ = new Clan();
                        }
                        input.ReadMessage(clan_);
                        break;
                    case 26:
                        if (requestSender_ == null)
                        {
                            requestSender_ = new Player();
                        }
                        input.ReadMessage(requestSender_);
                        break;
                    case 32:
                        CreateDate = input.ReadInt64();
                        break;
                    case 40:
                        CloseDate = input.ReadInt64();
                        break;
                    case 48:
                        requestType_ = (RequestType)input.ReadEnum();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}