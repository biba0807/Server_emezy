using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class OnAssignedLeaderRoleEvent : IMessage<OnAssignedLeaderRoleEvent>, IMessage, IEquatable<OnAssignedLeaderRoleEvent>
    {
        private static readonly MessageParser<OnAssignedLeaderRoleEvent> _parser = new MessageParser<OnAssignedLeaderRoleEvent>(() => new OnAssignedLeaderRoleEvent());

        public const int OldLeaderRoleFieldNumber = 1;
        private int oldLeaderRole_;

        public const int NewLeaderIdFieldNumber = 2;
        private string newLeaderId_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<OnAssignedLeaderRoleEvent> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[16];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int OldLeaderRole
        {
            get => oldLeaderRole_;
            set => oldLeaderRole_ = value;
        }

        [DebuggerNonUserCode]
        public string NewLeaderId
        {
            get => newLeaderId_;
            set => newLeaderId_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public OnAssignedLeaderRoleEvent() { }

        [DebuggerNonUserCode]
        public OnAssignedLeaderRoleEvent(OnAssignedLeaderRoleEvent other) : this()
        {
            oldLeaderRole_ = other.oldLeaderRole_;
            newLeaderId_ = other.newLeaderId_;
        }

        [DebuggerNonUserCode]
        public OnAssignedLeaderRoleEvent Clone() => new OnAssignedLeaderRoleEvent(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as OnAssignedLeaderRoleEvent);

        [DebuggerNonUserCode]
        public bool Equals(OnAssignedLeaderRoleEvent other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return OldLeaderRole == other.OldLeaderRole &&
                   NewLeaderId == other.NewLeaderId;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= OldLeaderRole.GetHashCode();
            if (NewLeaderId.Length != 0) hash ^= NewLeaderId.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (OldLeaderRole != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(OldLeaderRole);
            }
            if (NewLeaderId.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(NewLeaderId);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (OldLeaderRole != 0) size += 1 + CodedOutputStream.ComputeInt32Size(OldLeaderRole);
            if (NewLeaderId.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(NewLeaderId);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(OnAssignedLeaderRoleEvent other)
        {
            if (other == null) return;
            if (other.OldLeaderRole != 0) OldLeaderRole = other.OldLeaderRole;
            if (other.NewLeaderId.Length != 0) NewLeaderId = other.NewLeaderId;
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
                        OldLeaderRole = input.ReadInt32();
                        break;
                    case 18:
                        NewLeaderId = input.ReadString();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
