using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OpenRequest : IMessage<OpenRequest>, IMessage, IEquatable<OpenRequest>, IDeepCloneable<OpenRequest>
	{
		private static readonly MessageParser<OpenRequest> _parser = new MessageParser<OpenRequest>(() => new OpenRequest());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int CreatorFieldNumber = 2;

		private Player creator_;

		public const int ItemDefinitionIdFieldNumber = 3;

		private int itemDefinitionId_;

		public const int PriceFieldNumber = 4;

		private float price_;

		public const int CreateDateFieldNumber = 5;

		private long createDate_;

		public const int TypeFieldNumber = 6;

		private MarketRequestType type_ = MarketRequestType.NoneType;

		public const int QuantityFieldNumber = 7;

		private int quantity_;

		public const int PropertiesFieldNumber = 8;

		private static readonly MapField<string, InventoryItemProperty>.Codec _map_properties_codec = new MapField<string, InventoryItemProperty>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemProperty.Parser), 66u);

		private readonly MapField<string, InventoryItemProperty> properties_ = new MapField<string, InventoryItemProperty>();

		[DebuggerNonUserCode]
		public static MessageParser<OpenRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[0];

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
		public Player Creator
		{
			get
			{
				return creator_;
			}
			set
			{
				creator_ = value;
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
		public OpenRequest()
		{
		}

		[DebuggerNonUserCode]
		public OpenRequest(OpenRequest other)
			: this()
		{
			id_ = other.id_;
			Creator = ((other.creator_ != null) ? other.Creator.Clone() : null);
			itemDefinitionId_ = other.itemDefinitionId_;
			price_ = other.price_;
			createDate_ = other.createDate_;
			type_ = other.type_;
			quantity_ = other.quantity_;
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public OpenRequest Clone()
		{
			return new OpenRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OpenRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(OpenRequest other)
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
			if (!object.Equals(Creator, other.Creator))
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
			if (creator_ != null)
			{
				num ^= Creator.GetHashCode();
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
			if (creator_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Creator);
			}
			if (ItemDefinitionId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				output.WriteRawTag(37);
				output.WriteFloat(Price);
			}
			if (CreateDate != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt64(CreateDate);
			}
			if (Type != 0)
			{
				output.WriteRawTag(48);
				output.WriteEnum((int)Type);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(56);
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
			if (creator_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Creator);
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
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OpenRequest other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.creator_ != null)
			{
				if (creator_ == null)
				{
					creator_ = new Player();
				}
				Creator.MergeFrom(other.Creator);
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
			if (other.Quantity != 0)
			{
				Quantity = other.Quantity;
			}
			properties_.Add(other.properties_);
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
				case 18u:
					if (creator_ == null)
					{
						creator_ = new Player();
					}
					input.ReadMessage(creator_);
					break;
				case 24u:
					ItemDefinitionId = input.ReadInt32();
					break;
				case 37u:
					Price = input.ReadFloat();
					break;
				case 40u:
					CreateDate = input.ReadInt64();
					break;
				case 48u:
					type_ = (MarketRequestType)input.ReadEnum();
					break;
				case 56u:
					Quantity = input.ReadInt32();
					break;
				case 66u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
