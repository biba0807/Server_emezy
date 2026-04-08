using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ProcessingRequest : IMessage<ProcessingRequest>, IMessage, IEquatable<ProcessingRequest>, IDeepCloneable<ProcessingRequest>
	{
		private static readonly MessageParser<ProcessingRequest> _parser = new MessageParser<ProcessingRequest>(() => new ProcessingRequest());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int ItemDefinitionIdFieldNumber = 2;

		private int itemDefinitionId_;

		public const int PriceFieldNumber = 3;

		private float price_;

		public const int CreateDateFieldNumber = 4;

		private long createDate_;

		public const int TypeFieldNumber = 5;

		private MarketRequestType type_ = MarketRequestType.NoneType;

		public const int SaleRequestIdFieldNumber = 6;

		private string saleRequestId_ = "";

		public const int StateFieldNumber = 7;

		private ProcessingState state_ = ProcessingState.Creating;

		public const int QuantityFieldNumber = 8;

		private int quantity_;

		public const int PropertiesFieldNumber = 9;

		private static readonly MapField<string, InventoryItemProperty>.Codec _map_properties_codec = new MapField<string, InventoryItemProperty>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemProperty.Parser), 74u);

		private readonly MapField<string, InventoryItemProperty> properties_ = new MapField<string, InventoryItemProperty>();

		[DebuggerNonUserCode]
		public static MessageParser<ProcessingRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

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
		public long CreateDate
		{
			get
			{
				return createDate_;
			}
			set
			{
				createDate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MarketRequestType Type
		{
			get
			{
				return type_;
			}
			set
			{
				type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string SaleRequestId
		{
			get
			{
				return saleRequestId_;
			}
			set
			{
				saleRequestId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ProcessingState State
		{
			get
			{
				return state_;
			}
			set
			{
				state_ = value;
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
		public MapField<string, InventoryItemProperty> Properties => properties_;

		[DebuggerNonUserCode]
		public ProcessingRequest()
		{
		}

		[DebuggerNonUserCode]
		public ProcessingRequest(ProcessingRequest other)
			: this()
		{
			id_ = other.id_;
			itemDefinitionId_ = other.itemDefinitionId_;
			price_ = other.price_;
			createDate_ = other.createDate_;
			type_ = other.type_;
			saleRequestId_ = other.saleRequestId_;
			state_ = other.state_;
			quantity_ = other.quantity_;
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public ProcessingRequest Clone()
		{
			return new ProcessingRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ProcessingRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(ProcessingRequest other)
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
			if (ItemDefinitionId != other.ItemDefinitionId)
			{
				return false;
			}
			if (Price != other.Price)
			{
				return false;
			}
			if (CreateDate != other.CreateDate)
			{
				return false;
			}
			if (Type != other.Type)
			{
				return false;
			}
			if (SaleRequestId != other.SaleRequestId)
			{
				return false;
			}
			if (State != other.State)
			{
				return false;
			}
			if (Quantity != other.Quantity)
			{
				return false;
			}
			if (!Properties.Equals(other.Properties))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id.Length != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (ItemDefinitionId != 0)
			{
				num ^= ItemDefinitionId.GetHashCode();
			}
			if (Price != 0f)
			{
				num ^= Price.GetHashCode();
			}
			if (CreateDate != 0)
			{
				num ^= CreateDate.GetHashCode();
			}
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
			}
			if (SaleRequestId.Length != 0)
			{
				num ^= SaleRequestId.GetHashCode();
			}
			if (State != 0)
			{
				num ^= State.GetHashCode();
			}
			if (Quantity != 0)
			{
				num ^= Quantity.GetHashCode();
			}
			return num ^ Properties.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Id.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Id);
			}
			if (ItemDefinitionId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(Price);
			}
			if (CreateDate != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(CreateDate);
			}
			if (Type != 0)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)Type);
			}
			if (SaleRequestId.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(SaleRequestId);
			}
			if (State != 0)
			{
				output.WriteRawTag(56);
				output.WriteEnum((int)State);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(Quantity);
			}
			properties_.WriteTo(output, _map_properties_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			if (ItemDefinitionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				num += 5;
			}
			if (CreateDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(CreateDate);
			}
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			if (SaleRequestId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(SaleRequestId);
			}
			if (State != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)State);
			}
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ProcessingRequest other)
		{
			if (other != null)
			{
				if (other.Id.Length != 0)
				{
					Id = other.Id;
				}
				if (other.ItemDefinitionId != 0)
				{
					ItemDefinitionId = other.ItemDefinitionId;
				}
				if (other.Price != 0f)
				{
					Price = other.Price;
				}
				if (other.CreateDate != 0)
				{
					CreateDate = other.CreateDate;
				}
				if (other.Type != 0)
				{
					Type = other.Type;
				}
				if (other.SaleRequestId.Length != 0)
				{
					SaleRequestId = other.SaleRequestId;
				}
				if (other.State != 0)
				{
					State = other.State;
				}
				if (other.Quantity != 0)
				{
					Quantity = other.Quantity;
				}
				properties_.Add(other.properties_);
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
					Id = input.ReadString();
					break;
				case 16u:
					ItemDefinitionId = input.ReadInt32();
					break;
				case 29u:
					Price = input.ReadFloat();
					break;
				case 32u:
					CreateDate = input.ReadInt64();
					break;
				case 40u:
					type_ = (MarketRequestType)input.ReadEnum();
					break;
				case 50u:
					SaleRequestId = input.ReadString();
					break;
				case 56u:
					state_ = (ProcessingState)input.ReadEnum();
					break;
				case 64u:
					Quantity = input.ReadInt32();
					break;
				case 74u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
