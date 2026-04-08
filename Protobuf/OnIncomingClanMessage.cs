using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnIncomingClanMessage : IMessage<OnIncomingClanMessage>, IMessage, IEquatable<OnIncomingClanMessage>, IDeepCloneable<OnIncomingClanMessage>
	{
		private static readonly MessageParser<OnIncomingClanMessage> _parser = new MessageParser<OnIncomingClanMessage>(() => new OnIncomingClanMessage());

		public const int MessageFieldNumber = 1;

		private ClanUserMessage message_;

		[DebuggerNonUserCode]
		public static MessageParser<OnIncomingClanMessage> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[19];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClanUserMessage Message
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
		public OnIncomingClanMessage()
		{
		}

		[DebuggerNonUserCode]
		public OnIncomingClanMessage(OnIncomingClanMessage other)
			: this()
		{
			Message = ((other.message_ != null) ? other.Message.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnIncomingClanMessage Clone()
		{
			return new OnIncomingClanMessage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnIncomingClanMessage);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnIncomingClanMessage other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Message, other.Message))
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
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (message_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Message);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnIncomingClanMessage other)
		{
			if (other != null && other.message_ != null)
			{
				if (message_ == null)
				{
					message_ = new ClanUserMessage();
				}
			//	Message.MergeFrom(other.Message);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
					continue;
				}
				if (message_ == null)
				{
					message_ = new ClanUserMessage();
				}
				input.ReadMessage(message_);
			}
		}
	}
}
