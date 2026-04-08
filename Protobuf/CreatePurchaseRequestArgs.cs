using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CreatePurchaseRequestArgs : IMessage<CreatePurchaseRequestArgs>, IMessage, IEquatable<CreatePurchaseRequestArgs>, IDeepCloneable<CreatePurchaseRequestArgs>
	{
		private static readonly MessageParser<CreatePurchaseRequestArgs> _parser = new MessageParser<CreatePurchaseRequestArgs>(() => new CreatePurchaseRequestArgs());

		public const int ItemDefinitionIdFieldNumber = 1;

		private int itemDefinitionId_;

		public const int PriceFieldNumber = 2;

		private float price_;

		public const int QuantityFieldNumber = 3;

		private int quantity_;

		[DebuggerNonUserCode]
		public static MessageParser<CreatePurchaseRequestArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[7];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int ItemDefinitionId
		{
			get
			{
				return itemDefinitionId_;
			}
			set
			{
				itemDefinitionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float Price
		{
			get
			{
				return price_;
			}
			set
			{
				price_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Quantity
		{
			get
			{
				return quantity_;
			}
			set
			{
				quantity_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CreatePurchaseRequestArgs()
		{
		}

		[DebuggerNonUserCode]
		public CreatePurchaseRequestArgs(CreatePurchaseRequestArgs other)
			: this()
		{
			itemDefinitionId_ = other.itemDefinitionId_;
			price_ = other.price_;
			quantity_ = other.quantity_;
		}

		[DebuggerNonUserCode]
		public CreatePurchaseRequestArgs Clone()
		{
			return new CreatePurchaseRequestArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CreatePurchaseRequestArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(CreatePurchaseRequestArgs other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (ItemDefinitionId != other.ItemDefinitionId)
			{
				return false;
			}
			if (Price != other.Price)
			{
				return false;
			}
			if (Quantity != other.Quantity)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (ItemDefinitionId != 0)
			{
				num ^= ItemDefinitionId.GetHashCode();
			}
			if (Price != 0f)
			{
				num ^= Price.GetHashCode();
			}
			if (Quantity != 0)
			{
				num ^= Quantity.GetHashCode();
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
			if (ItemDefinitionId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				output.WriteRawTag(21);
				output.WriteFloat(Price);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Quantity);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (ItemDefinitionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				num += 5;
			}
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CreatePurchaseRequestArgs other)
		{
			if (other != null)
			{
				if (other.ItemDefinitionId != 0)
				{
					ItemDefinitionId = other.ItemDefinitionId;
				}
				if (other.Price != 0f)
				{
					Price = other.Price;
				}
				if (other.Quantity != 0)
				{
					Quantity = other.Quantity;
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
					ItemDefinitionId = input.ReadInt32();
					break;
				case 21u:
					Price = input.ReadFloat();
					break;
				case 24u:
					Quantity = input.ReadInt32();
					break;
				}
			}
		}
	}
}
