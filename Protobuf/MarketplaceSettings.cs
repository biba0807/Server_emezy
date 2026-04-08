using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class MarketplaceSettings : IMessage<MarketplaceSettings>, IMessage, IEquatable<MarketplaceSettings>, IDeepCloneable<MarketplaceSettings>
	{
		private static readonly MessageParser<MarketplaceSettings> _parser = new MessageParser<MarketplaceSettings>(() => new MarketplaceSettings());

		public const int CommissionPercentFieldNumber = 1;

		private float commissionPercent_;

		public const int MinCommissionFieldNumber = 2;

		private float minCommission_;

		public const int CurrencyIdFieldNumber = 3;

		private int currencyId_;

		public const int EnabledFieldNumber = 4;

		private bool enabled_;

		[DebuggerNonUserCode]
		public static MessageParser<MarketplaceSettings> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[14];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public float CommissionPercent
		{
			get
			{
				return commissionPercent_;
			}
			set
			{
				commissionPercent_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float MinCommission
		{
			get
			{
				return minCommission_;
			}
			set
			{
				minCommission_ = value;
			}
		}

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

		[DebuggerNonUserCode]
		public bool Enabled
		{
			get
			{
				return enabled_;
			}
			set
			{
				enabled_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MarketplaceSettings()
		{
		}

		[DebuggerNonUserCode]
		public MarketplaceSettings(MarketplaceSettings other)
			: this()
		{
			commissionPercent_ = other.commissionPercent_;
			minCommission_ = other.minCommission_;
			currencyId_ = other.currencyId_;
			enabled_ = other.enabled_;
		}

		[DebuggerNonUserCode]
		public MarketplaceSettings Clone()
		{
			return new MarketplaceSettings(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as MarketplaceSettings);
		}

		[DebuggerNonUserCode]
		public bool Equals(MarketplaceSettings other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (CommissionPercent != other.CommissionPercent)
			{
				return false;
			}
			if (MinCommission != other.MinCommission)
			{
				return false;
			}
			if (CurrencyId != other.CurrencyId)
			{
				return false;
			}
			if (Enabled != other.Enabled)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (CommissionPercent != 0f)
			{
				num ^= CommissionPercent.GetHashCode();
			}
			if (MinCommission != 0f)
			{
				num ^= MinCommission.GetHashCode();
			}
			if (CurrencyId != 0)
			{
				num ^= CurrencyId.GetHashCode();
			}
			if (Enabled)
			{
				num ^= Enabled.GetHashCode();
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
			if (CommissionPercent != 0f)
			{
				output.WriteRawTag(13);
				output.WriteFloat(CommissionPercent);
			}
			if (MinCommission != 0f)
			{
				output.WriteRawTag(21);
				output.WriteFloat(MinCommission);
			}
			if (CurrencyId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(CurrencyId);
			}
			if (Enabled)
			{
				output.WriteRawTag(32);
				output.WriteBool(Enabled);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (CommissionPercent != 0f)
			{
				num += 5;
			}
			if (MinCommission != 0f)
			{
				num += 5;
			}
			if (CurrencyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(CurrencyId);
			}
			if (Enabled)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(MarketplaceSettings other)
		{
			if (other != null)
			{
				if (other.CommissionPercent != 0f)
				{
					CommissionPercent = other.CommissionPercent;
				}
				if (other.MinCommission != 0f)
				{
					MinCommission = other.MinCommission;
				}
				if (other.CurrencyId != 0)
				{
					CurrencyId = other.CurrencyId;
				}
				if (other.Enabled)
				{
					Enabled = other.Enabled;
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
				case 13u:
					CommissionPercent = input.ReadFloat();
					break;
				case 21u:
					MinCommission = input.ReadFloat();
					break;
				case 24u:
					CurrencyId = input.ReadInt32();
					break;
				case 32u:
					Enabled = input.ReadBool();
					break;
				}
			}
		}
	}
}
