using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnKickedEvent : IMessage<OnKickedEvent>, IMessage, IEquatable<OnKickedEvent>, IDeepCloneable<OnKickedEvent>
	{
		private static readonly MessageParser<OnKickedEvent> _parser = new MessageParser<OnKickedEvent>(() => new OnKickedEvent());

		public const int KickingReasonFieldNumber = 1;

		private string kickingReason_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<OnKickedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[17];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string KickingReason
		{
			get
			{
				return kickingReason_;
			}
			set
			{
				kickingReason_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public OnKickedEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnKickedEvent(OnKickedEvent other)
			: this()
		{
			kickingReason_ = other.kickingReason_;
		}

		[DebuggerNonUserCode]
		public OnKickedEvent Clone()
		{
			return new OnKickedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnKickedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnKickedEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (KickingReason != other.KickingReason)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (KickingReason.Length != 0)
			{
				num ^= KickingReason.GetHashCode();
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
			if (KickingReason.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(KickingReason);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (KickingReason.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(KickingReason);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnKickedEvent other)
		{
			if (other != null && other.KickingReason.Length != 0)
			{
				KickingReason = other.KickingReason;
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
				}
				else
				{
					KickingReason = input.ReadString();
				}
			}
		}
	}
}
