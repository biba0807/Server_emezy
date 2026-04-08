using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CurrencyAmount : IMessage<CurrencyAmount>, IMessage, IEquatable<CurrencyAmount>, IDeepCloneable<CurrencyAmount>
	{
		private static readonly MessageParser<CurrencyAmount> _parser = new MessageParser<CurrencyAmount>(() => new CurrencyAmount());

		public const int CurrencyIdFieldNumber = 1;

		private int currencyId_;

		public const int OldValueFieldNumber = 2;

		private int oldValue_;

		public const int ValueFieldNumber = 3;

		private float value_;

		[DebuggerNonUserCode]
		public static MessageParser<CurrencyAmount> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CurrencyMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int CurrencyId
		{
			get
			{
				return currencyId_;
			}
			set
			{
				currencyId_ = value;
			}
		}

		[Obsolete]
		[DebuggerNonUserCode]
		public int OldValue
		{
			get
			{
				return oldValue_;
			}
			set
			{
				oldValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float Value
		{
			get
			{
				return value_;
			}
			set
			{
				value_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CurrencyAmount()
		{
		}

		[DebuggerNonUserCode]
		public CurrencyAmount(CurrencyAmount other)
			: this()
		{
			currencyId_ = other.currencyId_;
			oldValue_ = other.oldValue_;
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public CurrencyAmount Clone()
		{
			return new CurrencyAmount(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CurrencyAmount);
		}

		[DebuggerNonUserCode]
		public bool Equals(CurrencyAmount other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (CurrencyId != other.CurrencyId)
			{
				return false;
			}
			if (OldValue != other.OldValue)
			{
				return false;
			}
			if (Value != other.Value)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (CurrencyId != 0)
			{
				num ^= CurrencyId.GetHashCode();
			}
			if (OldValue != 0)
			{
				num ^= OldValue.GetHashCode();
			}
			if (Value != 0f)
			{
				num ^= Value.GetHashCode();
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
			if (CurrencyId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(CurrencyId);
			}
			if (OldValue != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(OldValue);
			}
			if (Value != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (CurrencyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(CurrencyId);
			}
			if (OldValue != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(OldValue);
			}
			if (Value != 0f)
			{
				num += 5;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CurrencyAmount other)
		{
			if (other != null)
			{
				if (other.CurrencyId != 0)
				{
					CurrencyId = other.CurrencyId;
				}
				if (other.OldValue != 0)
				{
					OldValue = other.OldValue;
				}
				if (other.Value != 0f)
				{
					Value = other.Value;
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
				case 8u:
					CurrencyId = input.ReadInt32();
					break;
				case 16u:
					OldValue = input.ReadInt32();
					break;
				case 29u:
					Value = input.ReadFloat();
					break;
				}
			}
		}
	}
}
