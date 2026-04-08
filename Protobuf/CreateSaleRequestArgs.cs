using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CreateSaleRequestArgs : IMessage<CreateSaleRequestArgs>, IMessage, IEquatable<CreateSaleRequestArgs>, IDeepCloneable<CreateSaleRequestArgs>
	{
		private static readonly MessageParser<CreateSaleRequestArgs> _parser = new MessageParser<CreateSaleRequestArgs>(() => new CreateSaleRequestArgs());

		public const int ItemIdFieldNumber = 1;

		private int itemId_;

		public const int PriceFieldNumber = 2;

		private float price_;

		[DebuggerNonUserCode]
		public static MessageParser<CreateSaleRequestArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[6];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int ItemId
		{
			get
			{
				return itemId_;
			}
			set
			{
				itemId_ = value;
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
		public CreateSaleRequestArgs()
		{
		}

		[DebuggerNonUserCode]
		public CreateSaleRequestArgs(CreateSaleRequestArgs other)
			: this()
		{
			itemId_ = other.itemId_;
			price_ = other.price_;
		}

		[DebuggerNonUserCode]
		public CreateSaleRequestArgs Clone()
		{
			return new CreateSaleRequestArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CreateSaleRequestArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(CreateSaleRequestArgs other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (ItemId != other.ItemId)
			{
				return false;
			}
			if (Price != other.Price)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (ItemId != 0)
			{
				num ^= ItemId.GetHashCode();
			}
			if (Price != 0f)
			{
				num ^= Price.GetHashCode();
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
			if (ItemId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(ItemId);
			}
			if (Price != 0f)
			{
				output.WriteRawTag(21);
				output.WriteFloat(Price);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (ItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ItemId);
			}
			if (Price != 0f)
			{
				num += 5;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CreateSaleRequestArgs other)
		{
			if (other != null)
			{
				if (other.ItemId != 0)
				{
					ItemId = other.ItemId;
				}
				if (other.Price != 0f)
				{
					Price = other.Price;
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
					ItemId = input.ReadInt32();
					break;
				case 21u:
					Price = input.ReadFloat();
					break;
				}
			}
		}
	}
}
