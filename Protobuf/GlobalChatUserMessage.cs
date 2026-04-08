using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GlobalChatUserMessage : IMessage<GlobalChatUserMessage>, IMessage, IEquatable<GlobalChatUserMessage>, IDeepCloneable<GlobalChatUserMessage>
	{
		private static readonly MessageParser<GlobalChatUserMessage> _parser = new MessageParser<GlobalChatUserMessage>(() => new GlobalChatUserMessage());

		public const int MessageFieldNumber = 1;

		private string message_ = "";

		public const int SenderFieldNumber = 2;

		private Player sender_;

		public const int TimestampFieldNumber = 3;

		private long timestamp_;

		[DebuggerNonUserCode]
		public static MessageParser<GlobalChatUserMessage> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ChatMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

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
		public Player Sender
		{
			get
			{
				return sender_;
			}
			set
			{
				sender_ = value;
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
		public GlobalChatUserMessage()
		{
		}

		[DebuggerNonUserCode]
		public GlobalChatUserMessage(GlobalChatUserMessage other)
			: this()
		{
			message_ = other.message_;
			Sender = ((other.sender_ != null) ? other.Sender.Clone() : null);
			timestamp_ = other.timestamp_;
		}

		[DebuggerNonUserCode]
		public GlobalChatUserMessage Clone()
		{
			return new GlobalChatUserMessage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GlobalChatUserMessage);
		}

		[DebuggerNonUserCode]
		public bool Equals(GlobalChatUserMessage other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Message != other.Message)
			{
				return false;
			}
			if (!object.Equals(Sender, other.Sender))
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
			if (Message.Length != 0)
			{
				num ^= Message.GetHashCode();
			}
			if (sender_ != null)
			{
				num ^= Sender.GetHashCode();
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
			if (Message.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Message);
			}
			if (sender_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Sender);
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
			if (Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Message);
			}
			if (sender_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Sender);
			}
			if (Timestamp != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Timestamp);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GlobalChatUserMessage other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Message.Length != 0)
			{
				Message = other.Message;
			}
			if (other.sender_ != null)
			{
				if (sender_ == null)
				{
					sender_ = new Player();
				}
				Sender.MergeFrom(other.Sender);
			}
			if (other.Timestamp != 0)
			{
				Timestamp = other.Timestamp;
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
					Message = input.ReadString();
					break;
				case 18u:
					if (sender_ == null)
					{
						sender_ = new Player();
					}
					input.ReadMessage(sender_);
					break;
				case 24u:
					Timestamp = input.ReadInt64();
					break;
				}
			}
		}
	}
}
