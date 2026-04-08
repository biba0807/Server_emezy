using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Trade : IMessage<Trade>, IMessage, IEquatable<Trade>, IDeepCloneable<Trade>
	{
		private static readonly MessageParser<Trade> _parser = new MessageParser<Trade>(() => new Trade());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int SalesCountFieldNumber = 2;

		private int salesCount_;

		public const int PurchasesCountFieldNumber = 3;

		private int purchasesCount_;

		public const int SalesPriceFieldNumber = 4;

		private float salesPrice_;

		public const int PurchasesPriceFieldNumber = 5;

		private float purchasesPrice_;

		[DebuggerNonUserCode]
		public static MessageParser<Trade> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[3];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int SalesCount
		{
			get
			{
				return salesCount_;
			}
			set
			{
				salesCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int PurchasesCount
		{
			get
			{
				return purchasesCount_;
			}
			set
			{
				purchasesCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float SalesPrice
		{
			get
			{
				return salesPrice_;
			}
			set
			{
				salesPrice_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float PurchasesPrice
		{
			get
			{
				return purchasesPrice_;
			}
			set
			{
				purchasesPrice_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Trade()
		{
		}

		[DebuggerNonUserCode]
		public Trade(Trade other)
			: this()
		{
			id_ = other.id_;
			salesCount_ = other.salesCount_;
			purchasesCount_ = other.purchasesCount_;
			salesPrice_ = other.salesPrice_;
			purchasesPrice_ = other.purchasesPrice_;
		}

		[DebuggerNonUserCode]
		public Trade Clone()
		{
			return new Trade(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Trade);
		}

		[DebuggerNonUserCode]
		public bool Equals(Trade other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (SalesCount != other.SalesCount)
			{
				return false;
			}
			if (PurchasesCount != other.PurchasesCount)
			{
				return false;
			}
			if (SalesPrice != other.SalesPrice)
			{
				return false;
			}
			if (PurchasesPrice != other.PurchasesPrice)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (SalesCount != 0)
			{
				num ^= SalesCount.GetHashCode();
			}
			if (PurchasesCount != 0)
			{
				num ^= PurchasesCount.GetHashCode();
			}
			if (SalesPrice != 0f)
			{
				num ^= SalesPrice.GetHashCode();
			}
			if (PurchasesPrice != 0f)
			{
				num ^= PurchasesPrice.GetHashCode();
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
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
			if (SalesCount != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(SalesCount);
			}
			if (PurchasesCount != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(PurchasesCount);
			}
			if (SalesPrice != 0f)
			{
				output.WriteRawTag(37);
				output.WriteFloat(SalesPrice);
			}
			if (PurchasesPrice != 0f)
			{
				output.WriteRawTag(45);
				output.WriteFloat(PurchasesPrice);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			if (SalesCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(SalesCount);
			}
			if (PurchasesCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(PurchasesCount);
			}
			if (SalesPrice != 0f)
			{
				num += 5;
			}
			if (PurchasesPrice != 0f)
			{
				num += 5;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Trade other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.SalesCount != 0)
				{
					SalesCount = other.SalesCount;
				}
				if (other.PurchasesCount != 0)
				{
					PurchasesCount = other.PurchasesCount;
				}
				if (other.SalesPrice != 0f)
				{
					SalesPrice = other.SalesPrice;
				}
				if (other.PurchasesPrice != 0f)
				{
					PurchasesPrice = other.PurchasesPrice;
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
					Id = input.ReadInt32();
					break;
				case 16u:
					SalesCount = input.ReadInt32();
					break;
				case 24u:
					PurchasesCount = input.ReadInt32();
					break;
				case 37u:
					SalesPrice = input.ReadFloat();
					break;
				case 45u:
					PurchasesPrice = input.ReadFloat();
					break;
				}
			}
		}
	}
}
