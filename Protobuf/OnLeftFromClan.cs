using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnLeftFromClan : IMessage<OnLeftFromClan>, IMessage, IEquatable<OnLeftFromClan>, IDeepCloneable<OnLeftFromClan>
	{
		private static readonly MessageParser<OnLeftFromClan> _parser = new MessageParser<OnLeftFromClan>(() => new OnLeftFromClan());

		public const int MemberIdFieldNumber = 1;

		private string memberId_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<OnLeftFromClan> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[18];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string MemberId
		{
			get
			{
				return memberId_;
			}
			set
			{
				memberId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public OnLeftFromClan()
		{
		}

		[DebuggerNonUserCode]
		public OnLeftFromClan(OnLeftFromClan other)
			: this()
		{
			memberId_ = other.memberId_;
		}

		[DebuggerNonUserCode]
		public OnLeftFromClan Clone()
		{
			return new OnLeftFromClan(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnLeftFromClan);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnLeftFromClan other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (MemberId != other.MemberId)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (MemberId.Length != 0)
			{
				num ^= MemberId.GetHashCode();
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
			if (MemberId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(MemberId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (MemberId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(MemberId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnLeftFromClan other)
		{
			if (other != null && other.MemberId.Length != 0)
			{
				MemberId = other.MemberId;
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
					MemberId = input.ReadString();
				}
			}
		}
	}
}
