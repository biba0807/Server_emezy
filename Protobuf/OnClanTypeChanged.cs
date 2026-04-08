using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnClanTypeChanged : IMessage<OnClanTypeChanged>, IMessage, IEquatable<OnClanTypeChanged>, IDeepCloneable<OnClanTypeChanged>
	{
		private static readonly MessageParser<OnClanTypeChanged> _parser = new MessageParser<OnClanTypeChanged>(() => new OnClanTypeChanged());

		public const int NewClanTypeFieldNumber = 1;

		private ClanType newClanType_ = ClanType.Closed;

		[DebuggerNonUserCode]
		public static MessageParser<OnClanTypeChanged> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[15];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClanType NewClanType
		{
			get
			{
				return newClanType_;
			}
			set
			{
				newClanType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnClanTypeChanged()
		{
		}

		[DebuggerNonUserCode]
		public OnClanTypeChanged(OnClanTypeChanged other)
			: this()
		{
			newClanType_ = other.newClanType_;
		}

		[DebuggerNonUserCode]
		public OnClanTypeChanged Clone()
		{
			return new OnClanTypeChanged(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnClanTypeChanged);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnClanTypeChanged other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (NewClanType != other.NewClanType)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (NewClanType != 0)
			{
				num ^= NewClanType.GetHashCode();
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
			if (NewClanType != 0)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)NewClanType);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (NewClanType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)NewClanType);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnClanTypeChanged other)
		{
			if (other != null && other.NewClanType != 0)
			{
				NewClanType = other.NewClanType;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 8)
				{
					input.SkipLastField();
				}
				else
				{
					newClanType_ = (ClanType)input.ReadEnum();
				}
			}
		}
	}
}
