using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanLogMessage : IMessage<ClanLogMessage>, IMessage, IEquatable<ClanLogMessage>, IDeepCloneable<ClanLogMessage>
    {
        private static readonly MessageParser<ClanLogMessage> _parser = new MessageParser<ClanLogMessage>(() => new ClanLogMessage());

        public const int ClanLogTypeFieldNumber = 1;
        private int clanLogType_;

        public const int ChangedClanTypeFieldNumber = 2;
        private int changedClanType_;

        public const int ChangedClanNameFieldNumber = 3;
        private string changedClanName_ = "";

        public const int ChangedClanTagFieldNumber = 4;
        private string changedClanTag_ = "";

        public const int PrimaryMemberFieldNumber = 5;
        private string primaryMember_ = "";

        public const int SecondaryMemberFieldNumber = 6;
        private string secondaryMember_ = "";

        public const int ChangedMaxMemberCountFieldNumber = 7;
        private int changedMaxMemberCount_;

        public const int AssignedRoleFieldNumber = 8;
        private int assignedRole_;

        [DebuggerNonUserCode]
        public static MessageParser<ClanLogMessage> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[1];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int ClanLogType
        {
            get => clanLogType_;
            set => clanLogType_ = value;
        }

        [DebuggerNonUserCode]
        public int ChangedClanType
        {
            get => changedClanType_;
            set => changedClanType_ = value;
        }

        [DebuggerNonUserCode]
        public string ChangedClanName
        {
            get => changedClanName_;
            set => changedClanName_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public string ChangedClanTag
        {
            get => changedClanTag_;
            set => changedClanTag_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public string PrimaryMember
        {
            get => primaryMember_;
            set => primaryMember_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public string SecondaryMember
        {
            get => secondaryMember_;
            set => secondaryMember_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public int ChangedMaxMemberCount
        {
            get => changedMaxMemberCount_;
            set => changedMaxMemberCount_ = value;
        }

        [DebuggerNonUserCode]
        public int AssignedRole
        {
            get => assignedRole_;
            set => assignedRole_ = value;
        }

        [DebuggerNonUserCode]
        public ClanLogMessage() { }

        [DebuggerNonUserCode]
        public ClanLogMessage(ClanLogMessage other) : this()
        {
            clanLogType_ = other.clanLogType_;
            changedClanType_ = other.changedClanType_;
            changedClanName_ = other.changedClanName_;
            changedClanTag_ = other.changedClanTag_;
            primaryMember_ = other.primaryMember_;
            secondaryMember_ = other.secondaryMember_;
            changedMaxMemberCount_ = other.changedMaxMemberCount_;
            assignedRole_ = other.assignedRole_;
        }

        [DebuggerNonUserCode]
        public ClanLogMessage Clone() => new ClanLogMessage(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ClanLogMessage);

        [DebuggerNonUserCode]
        public bool Equals(ClanLogMessage other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return clanLogType_ == other.clanLogType_ &&
                   changedClanType_ == other.changedClanType_ &&
                   changedClanName_ == other.changedClanName_ &&
                   changedClanTag_ == other.changedClanTag_ &&
                   primaryMember_ == other.primaryMember_ &&
                   secondaryMember_ == other.secondaryMember_ &&
                   changedMaxMemberCount_ == other.changedMaxMemberCount_ &&
                   assignedRole_ == other.assignedRole_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= clanLogType_.GetHashCode();
            hash ^= changedClanType_.GetHashCode();
            hash ^= changedClanName_.GetHashCode();
            hash ^= changedClanTag_.GetHashCode();
            hash ^= primaryMember_.GetHashCode();
            hash ^= secondaryMember_.GetHashCode();
            hash ^= changedMaxMemberCount_.GetHashCode();
            hash ^= assignedRole_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (clanLogType_ != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(clanLogType_);
            }
            if (changedClanType_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(changedClanType_);
            }
            if (changedClanName_.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteString(changedClanName_);
            }
            if (changedClanTag_.Length != 0)
            {
                output.WriteRawTag(34);
                output.WriteString(changedClanTag_);
            }
            if (primaryMember_.Length != 0)
            {
                output.WriteRawTag(42);
                output.WriteString(primaryMember_);
            }
            if (secondaryMember_.Length != 0)
            {
                output.WriteRawTag(50);
                output.WriteString(secondaryMember_);
            }
            if (changedMaxMemberCount_ != 0)
            {
                output.WriteRawTag(56);
                output.WriteInt32(changedMaxMemberCount_);
            }
            if (assignedRole_ != 0)
            {
                output.WriteRawTag(64);
                output.WriteInt32(assignedRole_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (clanLogType_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(clanLogType_);
            if (changedClanType_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(changedClanType_);
            if (changedClanName_.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(changedClanName_);
            if (changedClanTag_.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(changedClanTag_);
            if (primaryMember_.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(primaryMember_);
            if (secondaryMember_.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(secondaryMember_);
            if (changedMaxMemberCount_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(changedMaxMemberCount_);
            if (assignedRole_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(assignedRole_);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanLogMessage other)
        {
            if (other == null) return;
            if (other.clanLogType_ != 0) clanLogType_ = other.clanLogType_;
            if (other.changedClanType_ != 0) changedClanType_ = other.changedClanType_;
            if (other.changedClanName_.Length != 0) changedClanName_ = other.changedClanName_;
            if (other.changedClanTag_.Length != 0) changedClanTag_ = other.changedClanTag_;
            if (other.primaryMember_.Length != 0) primaryMember_ = other.primaryMember_;
            if (other.secondaryMember_.Length != 0) secondaryMember_ = other.secondaryMember_;
            if (other.changedMaxMemberCount_ != 0) changedMaxMemberCount_ = other.changedMaxMemberCount_;
            if (other.assignedRole_ != 0) assignedRole_ = other.assignedRole_;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        clanLogType_ = input.ReadInt32();
                        break;
                    case 16:
                        changedClanType_ = input.ReadInt32();
                        break;
                    case 26:
                        ChangedClanName = input.ReadString();
                        break;
                    case 34:
                        ChangedClanTag = input.ReadString();
                        break;
                    case 42:
                        PrimaryMember = input.ReadString();
                        break;
                    case 50:
                        SecondaryMember = input.ReadString();
                        break;
                    case 56:
                        changedMaxMemberCount_ = input.ReadInt32();
                        break;
                    case 64:
                        assignedRole_ = input.ReadInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}