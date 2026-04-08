using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class LocalizedTitle : IMessage<LocalizedTitle>, IMessage, IEquatable<LocalizedTitle>, IDeepCloneable<LocalizedTitle>
    {
        private static readonly MessageParser<LocalizedTitle> _parser = new MessageParser<LocalizedTitle>(() => new LocalizedTitle());

        public const int NameFieldNumber = 1;

        private string name_ = "";

        public const int DescriptionFieldNumber = 2;

        private string description_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<LocalizedTitle> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => CommonMessageReflection.Descriptor.MessageTypes[8];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

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
        public string Description
        {
            get
            {
                return description_;
            }
            set
            {
                description_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public LocalizedTitle()
        {
        }

        [DebuggerNonUserCode]
        public LocalizedTitle(LocalizedTitle other)
            : this()
        {
            name_ = other.name_;
            description_ = other.description_;
        }

        [DebuggerNonUserCode]
        public LocalizedTitle Clone()
        {
            return new LocalizedTitle(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as LocalizedTitle);
        }

        [DebuggerNonUserCode]
        public bool Equals(LocalizedTitle other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (Name != other.Name)
            {
                return false;
            }
            if (Description != other.Description)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Name.Length != 0)
            {
                hash ^= Name.GetHashCode();
            }
            if (Description.Length != 0)
            {
                hash ^= Description.GetHashCode();
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
            if (Name.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(Name);
            }
            if (Description.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Description);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (Name.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(Name);
            }
            if (Description.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(Description);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(LocalizedTitle other)
        {
            if (other != null)
            {
                if (other.Name.Length != 0)
                {
                    Name = other.Name;
                }
                if (other.Description.Length != 0)
                {
                    Description = other.Description;
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
                    case 10u:
                        Name = input.ReadString();
                        break;
                    case 18u:
                        Description = input.ReadString();
                        break;
                }
            }
        }
    }
}
