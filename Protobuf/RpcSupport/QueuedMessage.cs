using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class QueuedMessage : IMessage<QueuedMessage>, IMessage, IEquatable<QueuedMessage>, IDeepCloneable<QueuedMessage>
	{
		private static readonly MessageParser<QueuedMessage> _parser = new MessageParser<QueuedMessage>(() => new QueuedMessage());

		public const int MessageFieldNumber = 1;

		private ResponseMessage message_;

		public const int BoltIdFieldNumber = 2;

		private string boltId_ = string.Empty;

		public const int TimestampFieldNumber = 3;

		private long timestamp_;

		public const int UserIdFieldNumber = 4;

		private static readonly FieldCodec<string> _repeated_userId_codec = FieldCodec.ForString(34u);

		private readonly RepeatedField<string> userId_ = new RepeatedField<string>();

		public const int TopicFieldNumber = 5;

		private string topic_ = string.Empty;

		public const int ImportantFieldNumber = 6;

		private bool important_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<QueuedMessage> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		public ResponseMessage Message
		{
			get
			{
				return message_;
			}
			set
			{
				message_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string BoltId
		{
			get
			{
				return boltId_;
			}
			set
			{
				boltId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public RepeatedField<string> UserId => userId_;

		[DebuggerNonUserCode]
		public string Topic
		{
			get
			{
				return topic_;
			}
			set
			{
				topic_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool Important
		{
			get
			{
				return important_;
			}
			set
			{
				important_ = value;
			}
		}

		[DebuggerNonUserCode]
		public QueuedMessage()
		{
		}

		[DebuggerNonUserCode]
		public QueuedMessage(QueuedMessage other)
			: this()
		{
			Message = ((other.message_ == null) ? null : other.Message.Clone());
			boltId_ = other.boltId_;
			timestamp_ = other.timestamp_;
			userId_ = other.userId_.Clone();
			topic_ = other.topic_;
			important_ = other.important_;
		}

		[DebuggerNonUserCode]
		public QueuedMessage Clone()
		{
			return new QueuedMessage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as QueuedMessage);
		}

		[DebuggerNonUserCode]
		public bool Equals(QueuedMessage other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (!object.Equals(Message, other.Message))
			{
				return false;
			}
			if (BoltId != other.BoltId)
			{
				return false;
			}
			if (Timestamp != other.Timestamp)
			{
				return false;
			}
			if (!userId_.Equals(other.userId_))
			{
				return false;
			}
			if (Topic != other.Topic)
			{
				return false;
			}
			if (Important != other.Important)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (message_ != null)
			{
				num ^= Message.GetHashCode();
			}
			if (BoltId.Length != 0)
			{
				num ^= BoltId.GetHashCode();
			}
			if (Timestamp != 0)
			{
				num ^= Timestamp.GetHashCode();
			}
			num ^= userId_.GetHashCode();
			if (Topic.Length != 0)
			{
				num ^= Topic.GetHashCode();
			}
			if (Important)
			{
				num ^= Important.GetHashCode();
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
			if (message_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(Message);
			}
			if (BoltId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(BoltId);
			}
			if (Timestamp != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt64(Timestamp);
			}
			userId_.WriteTo(output, _repeated_userId_codec);
			if (Topic.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(Topic);
			}
			if (Important)
			{
				output.WriteRawTag(48);
				output.WriteBool(Important);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (message_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Message);
			}
			if (BoltId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(BoltId);
			}
			if (Timestamp != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Timestamp);
			}
			num += userId_.CalculateSize(_repeated_userId_codec);
			if (Topic.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Topic);
			}
			if (Important)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(QueuedMessage other)
		{
			if (other == null)
			{
				return;
			}
			if (other.message_ != null)
			{
				if (message_ == null)
				{
					message_ = new ResponseMessage();
				}
				Message.MergeFrom(other.Message);
			}
			if (other.BoltId.Length != 0)
			{
				BoltId = other.BoltId;
			}
			if (other.Timestamp != 0)
			{
				Timestamp = other.Timestamp;
			}
			userId_.Add(other.userId_);
			if (other.Topic.Length != 0)
			{
				Topic = other.Topic;
			}
			if (other.Important)
			{
				Important = other.Important;
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
					if (message_ == null)
					{
						message_ = new ResponseMessage();
					}
					input.ReadMessage(message_);
					break;
				case 18u:
					BoltId = input.ReadString();
					break;
				case 24u:
					Timestamp = input.ReadInt64();
					break;
				case 34u:
					userId_.AddEntriesFrom(input, _repeated_userId_codec);
					break;
				case 42u:
					Topic = input.ReadString();
					break;
				case 48u:
					Important = input.ReadBool();
					break;
				}
			}
		}
	}
}
