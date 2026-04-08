using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class OnClanTagAndNameChanged : IMessage<OnClanTagAndNameChanged>, IMessage, IEquatable<OnClanTagAndNameChanged>
    {
        private static readonly MessageParser<OnClanTagAndNameChanged> _parser = new MessageParser<OnClanTagAndNameChanged>(() => new OnClanTagAndNameChanged());

        private string newClanTag_ = "";
        private string newClanName_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<OnClanTagAndNameChanged> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[1]; 

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string NewClanTag
        {
            get => newClanTag_;
            set => newClanTag_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public string NewClanName
        {
            get => newClanName_;
            set => newClanName_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public OnClanTagAndNameChanged() { }

        [DebuggerNonUserCode]
        public OnClanTagAndNameChanged Clone()
        {
            return new OnClanTagAndNameChanged();
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as OnClanTagAndNameChanged);

        [DebuggerNonUserCode]
        public bool Equals(OnClanTagAndNameChanged other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return NewClanTag == other.NewClanTag && NewClanName == other.NewClanName;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= NewClanTag.GetHashCode();
            hash ^= NewClanName.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (NewClanTag.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(NewClanTag);
            }
            if (NewClanName.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(NewClanName);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (NewClanTag.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(NewClanTag);
            }
            if (NewClanName.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(NewClanName);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(OnClanTagAndNameChanged other)
        {
            if (other == null) return;
            if (other.newClanTag_.Length != 0) newClanTag_ = other.newClanTag_;
            if (other.newClanName_.Length != 0) newClanName_ = other.newClanName_;
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
                        NewClanTag = input.ReadString();
                        break;
                    case 18:
                        NewClanName = input.ReadString();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
