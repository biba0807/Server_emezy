using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class OnClanMaxMembersCountIncreased : IMessage<OnClanMaxMembersCountIncreased>, IMessage, IEquatable<OnClanMaxMembersCountIncreased>
    {
        private static readonly MessageParser<OnClanMaxMembersCountIncreased> _parser = new MessageParser<OnClanMaxMembersCountIncreased>(() => new OnClanMaxMembersCountIncreased());

        public const int NewMembersCountValueFieldNumber = 1;
        private int newMembersCountValue_;

        [DebuggerNonUserCode]
        public static MessageParser<OnClanMaxMembersCountIncreased> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[22];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int NewMembersCountValue
        {
            get => newMembersCountValue_;
            set => newMembersCountValue_ = value;
        }

        [DebuggerNonUserCode]
        public OnClanMaxMembersCountIncreased() { }

        [DebuggerNonUserCode]
        public OnClanMaxMembersCountIncreased(OnClanMaxMembersCountIncreased other) : this()
        {
            newMembersCountValue_ = other.newMembersCountValue_;
        }

        [DebuggerNonUserCode]
        public OnClanMaxMembersCountIncreased Clone() => new OnClanMaxMembersCountIncreased(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as OnClanMaxMembersCountIncreased);

        [DebuggerNonUserCode]
        public bool Equals(OnClanMaxMembersCountIncreased other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return NewMembersCountValue == other.NewMembersCountValue;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= NewMembersCountValue.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (NewMembersCountValue != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(NewMembersCountValue);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (NewMembersCountValue != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(NewMembersCountValue);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(OnClanMaxMembersCountIncreased other)
        {
            if (other == null) return;
            if (other.NewMembersCountValue != 0) NewMembersCountValue = other.NewMembersCountValue;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 8)
                {
                    NewMembersCountValue = input.ReadInt32();
                }
                else
                {
                    input.SkipLastField();
                }
            }
        }
    }
}
