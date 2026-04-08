using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnKickedMemberEvent : IMessage<OnKickedMemberEvent>, IMessage, IEquatable<OnKickedMemberEvent>, IDeepCloneable<OnKickedMemberEvent>
	{
		private static readonly MessageParser<OnKickedMemberEvent> _parser = new MessageParser<OnKickedMemberEvent>(() => new OnKickedMemberEvent());

		public const int KickerMemberIdFieldNumber = 1;

		private string kickerMemberId_ = "";

		public const int KickedMemberIdFieldNumber = 2;

		private string kickedMemberId_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<OnKickedMemberEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[11];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string KickerMemberId
		{
			get
			{
				return kickerMemberId_;
			}
			set
			{
				kickerMemberId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string KickedMemberId
		{
			get
			{
				return kickedMemberId_;
			}
			set
			{
				kickedMemberId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public OnKickedMemberEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnKickedMemberEvent(OnKickedMemberEvent other)
			: this()
		{
			kickerMemberId_ = other.kickerMemberId_;
			kickedMemberId_ = other.kickedMemberId_;
		}

		[DebuggerNonUserCode]
		public OnKickedMemberEvent Clone()
		{
			return new OnKickedMemberEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnKickedMemberEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnKickedMemberEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (KickerMemberId != other.KickerMemberId)
			{
				return false;
			}
			if (KickedMemberId != other.KickedMemberId)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (KickerMemberId.Length != 0)
			{
				num ^= KickerMemberId.GetHashCode();
			}
			if (KickedMemberId.Length != 0)
			{
				num ^= KickedMemberId.GetHashCode();
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
			if (KickerMemberId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(KickerMemberId);
			}
			if (KickedMemberId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(KickedMemberId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (KickerMemberId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(KickerMemberId);
			}
			if (KickedMemberId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(KickedMemberId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnKickedMemberEvent other)
		{
			if (other != null)
			{
				if (other.KickerMemberId.Length != 0)
				{
					KickerMemberId = other.KickerMemberId;
				}
				if (other.KickedMemberId.Length != 0)
				{
					KickedMemberId = other.KickedMemberId;
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
					KickerMemberId = input.ReadString();
					break;
				case 18u:
					KickedMemberId = input.ReadString();
					break;
				}
			}
		}
	}
}
