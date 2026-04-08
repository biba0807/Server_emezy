using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class DayRange : IMessage<DayRange>, IMessage, IEquatable<DayRange>, IDeepCloneable<DayRange>
    {
        private static readonly MessageParser<DayRange> _parser = new MessageParser<DayRange>(() => new DayRange());

        public const int FromFieldNumber = 1;

        private int from_;

        public const int ToFieldNumber = 2;

        private int to_;

        [DebuggerNonUserCode]
        public static MessageParser<DayRange> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => CommonMessageReflection.Descriptor.MessageTypes[7];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int From
        {
            get
            {
                return from_;
            }
            set
            {
                from_ = value;
            }
        }

        [DebuggerNonUserCode]
        public int To
        {
            get
            {
                return to_;
            }
            set
            {
                to_ = value;
            }
        }

        [DebuggerNonUserCode]
        public DayRange()
        {
        }

        [DebuggerNonUserCode]
        public DayRange(DayRange other)
            : this()
        {
            from_ = other.from_;
            to_ = other.to_;
        }

        [DebuggerNonUserCode]
        public DayRange Clone()
        {
            return new DayRange(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as DayRange);
        }

        [DebuggerNonUserCode]
        public bool Equals(DayRange other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (From != other.From)
            {
                return false;
            }
            if (To != other.To)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (From != 0)
            {
                hash ^= From.GetHashCode();
            }
            if (To != 0)
            {
                hash ^= To.GetHashCode();
            }
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (From != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(From);
            }
            if (To != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(To);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (From != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(From);
            }
            if (To != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(To);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(DayRange other)
        {
            if (other != null)
            {
                if (other.From != 0)
                {
                    From = other.From;
                }
                if (other.To != 0)
                {
                    To = other.To;
                }
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        input.SkipLastField();
                        break;
                    case 8u:
                        From = input.ReadInt32();
                        break;
                    case 16u:
                        To = input.ReadInt32();
                        break;
                }
            }
        }
    }
}
