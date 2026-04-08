using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class OnMemberJoinedToClanEvent : IMessage<OnMemberJoinedToClanEvent>, IMessage, IEquatable<OnMemberJoinedToClanEvent>
    {
        private static readonly MessageParser<OnMemberJoinedToClanEvent> _parser = new MessageParser<OnMemberJoinedToClanEvent>(() => new OnMemberJoinedToClanEvent());

        public const int ClanMemberFieldNumber = 1;
        private ClanMember clanMember_;

        public const int JoinClanTypeFieldNumber = 2;
        private JoinClanType joinClanType_;

        public const int JoinRequestAcceptorFieldNumber = 3;
        private string joinRequestAcceptor_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<OnMemberJoinedToClanEvent> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[12];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public ClanMember ClanMember
        {
            get => clanMember_;
            set => clanMember_ = value;
        }

        [DebuggerNonUserCode]
        public JoinClanType JoinClanType
        {
            get => joinClanType_;
            set => joinClanType_ = value;
        }

        [DebuggerNonUserCode]
        public string JoinRequestAcceptor
        {
            get => joinRequestAcceptor_;
            set => joinRequestAcceptor_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public OnMemberJoinedToClanEvent() { }

        [DebuggerNonUserCode]
        public OnMemberJoinedToClanEvent(OnMemberJoinedToClanEvent other) : this()
        {
            clanMember_ = other.clanMember_ != null ? other.clanMember_.Clone() : null;
            joinClanType_ = other.joinClanType_;
            joinRequestAcceptor_ = other.joinRequestAcceptor_;
        }

        [DebuggerNonUserCode]
        public OnMemberJoinedToClanEvent Clone() => new OnMemberJoinedToClanEvent(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as OnMemberJoinedToClanEvent);

        [DebuggerNonUserCode]
        public bool Equals(OnMemberJoinedToClanEvent other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(clanMember_, other.clanMember_) &&
                   joinClanType_ == other.joinClanType_ &&
                   joinRequestAcceptor_ == other.joinRequestAcceptor_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= (clanMember_ != null ? clanMember_.GetHashCode() : 0);
            hash ^= joinClanType_.GetHashCode();
            hash ^= (joinRequestAcceptor_.Length != 0 ? joinRequestAcceptor_.GetHashCode() : 0);
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (clanMember_ != null)
            {
                output.WriteRawTag(10);
                output.WriteMessage(clanMember_);
            }
            if (joinClanType_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteEnum((int)joinClanType_);
            }
            if (joinRequestAcceptor_.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteString(joinRequestAcceptor_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (clanMember_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(clanMember_);
            }
            if (joinClanType_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeEnumSize((int)joinClanType_);
            }
            if (joinRequestAcceptor_.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(joinRequestAcceptor_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(OnMemberJoinedToClanEvent other)
        {
            if (other == null) return;
            if (other.clanMember_ != null)
            {
                if (clanMember_ == null)
                {
                    clanMember_ = new ClanMember();
                }
              //  clanMember_.MergeFrom(other.clanMember_);
            }
            if (other.joinClanType_ != 0)
            {
                joinClanType_ = other.joinClanType_;
            }
            if (other.joinRequestAcceptor_.Length != 0)
            {
                JoinRequestAcceptor = other.joinRequestAcceptor_;
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            while (input.ReadTag() is uint tag && tag != 0)
            {
                switch (tag)
                {
                    case 10:
                        if (clanMember_ == null)
                        {
                            clanMember_ = new ClanMember();
                        }
                        input.ReadMessage(clanMember_);
                        break;
                    case 16:
                        joinClanType_ = (JoinClanType)input.ReadEnum();
                        break;
                    case 26:
                        JoinRequestAcceptor = input.ReadString();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}