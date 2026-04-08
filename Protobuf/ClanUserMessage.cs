using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanUserMessage : IMessage<ClanUserMessage>, IMessage, IEquatable<ClanUserMessage>, IDeepCloneable<ClanUserMessage>
    {
        private static readonly MessageParser<ClanUserMessage> _parser = new MessageParser<ClanUserMessage>(() => new ClanUserMessage());

        public const int SenderIdFieldNumber = 1;

        private string senderId_ = "";

        public const int MessageFieldNumber = 2;

        private string message_ = "";

        public const int TimestampFieldNumber = 3;

        private long timestamp_;

        [DebuggerNonUserCode]
        public static MessageParser<ClanUserMessage> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[7];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string SenderId
        {
            get
            {
                return senderId_;
            }
            set
            {
                senderId_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public string Message
        {
            get
            {
                return message_;
            }
            set
            {
                message_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public long Timestamp
        {
            get
            {
                return timestamp_;
            }
            set
            {
                timestamp_ = value;
            }
        }

        [DebuggerNonUserCode]
        public ClanUserMessage()
        {
        }

        [DebuggerNonUserCode]
        public ClanUserMessage(ClanUserMessage other)
            : this()
        {
            senderId_ = other.senderId_;
            message_ = other.message_;
            timestamp_ = other.timestamp_;
        }

        [DebuggerNonUserCode]
        public ClanUserMessage Clone()
        {
            return new ClanUserMessage(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ClanUserMessage);
        }

        [DebuggerNonUserCode]
        public bool Equals(ClanUserMessage other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (SenderId != other.SenderId)
            {
                return false;
            }
            if (Message != other.Message)
            {
                return false;
            }
            if (Timestamp != other.Timestamp)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int num = 1;
            if (SenderId.Length != 0)
            {
                num ^= SenderId.GetHashCode();
            }
            if (Message.Length != 0)
            {
                num ^= Message.GetHashCode();
            }
            if (Timestamp != 0)
            {
                num ^= Timestamp.GetHashCode();
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
            if (SenderId.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(SenderId);
            }
            if (Message.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Message);
            }
            if (Timestamp != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt64(Timestamp);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int num = 0;
            if (SenderId.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(SenderId);
            }
            if (Message.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Message);
            }
            if (Timestamp != 0)
            {
                num += 1 + CodedOutputStream.ComputeInt64Size(Timestamp);
            }
            return num;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanUserMessage other)
        {
            if (other != null)
            {
                if (other.SenderId.Length != 0)
                {
                    SenderId = other.SenderId;
                }
                if (other.Message.Length != 0)
                {
                    Message = other.Message;
                }
                if (other.Timestamp != 0)
                {
                    Timestamp = other.Timestamp;
                }
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
                        SenderId = input.ReadString();
                        break;
                    case 18u:
                        Message = input.ReadString();
                        break;
                    case 24u:
                        Timestamp = input.ReadInt64();
                        break;
                }
            }
        }
    }
}
