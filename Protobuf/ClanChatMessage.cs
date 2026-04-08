using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanChatMessage : IMessage<ClanChatMessage>, IMessage, IEquatable<ClanChatMessage>, IDeepCloneable<ClanChatMessage>
    {
        private static readonly MessageParser<ClanChatMessage> _parser = new MessageParser<ClanChatMessage>(() => new ClanChatMessage());

        public const int SenderIdFieldNumber = 1;
        private string senderId_ = "";

        public const int MessageFieldNumber = 2;
        private string message_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<ClanChatMessage> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[7];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string SenderId
        {
            get => senderId_;
            set => senderId_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public string Message
        {
            get => message_;
            set => message_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public ClanChatMessage() { }

        [DebuggerNonUserCode]
        public ClanChatMessage(ClanChatMessage other) : this()
        {
            senderId_ = other.senderId_;
            message_ = other.message_;
        }

        [DebuggerNonUserCode]
        public ClanChatMessage Clone() => new ClanChatMessage(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ClanChatMessage);

        [DebuggerNonUserCode]
        public bool Equals(ClanChatMessage other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return senderId_ == other.senderId_ && message_ == other.message_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= senderId_.GetHashCode();
            hash ^= message_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (output == null) throw new NullReferenceException();

            if (senderId_.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(senderId_);
            }
            if (message_.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(message_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (senderId_.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(senderId_);
            }
            if (message_.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(message_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanChatMessage other)
        {
            if (other == null) return;
            if (other.senderId_.Length != 0) senderId_ = other.senderId_;
            if (other.message_.Length != 0) message_ = other.message_;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            if (input == null) throw new NullReferenceException();

            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        SenderId = input.ReadString();
                        break;
                    case 18:
                        Message = input.ReadString();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
