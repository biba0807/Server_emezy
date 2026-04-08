using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClosedRequest : IMessage<ClosedRequest>, IMessage, IEquatable<ClosedRequest>, IDeepCloneable<ClosedRequest>
	{
		private static readonly MessageParser<ClosedRequest> _parser = new MessageParser<ClosedRequest>(() => new ClosedRequest());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int OriginIdFieldNumber = 2;

		private string originId_ = "";

		public const int CreatorFieldNumber = 3;

		private Player creator_;

		public const int ItemDefinitionIdFieldNumber = 4;

		private int itemDefinitionId_;

		public const int PriceFieldNumber = 5;

		private float price_;

		public const int CreateDateFieldNumber = 6;

		private long createDate_;

		public const int CloseDateFieldNumber = 7;

		private long closeDate_;

		public const int TypeFieldNumber = 8;

		private MarketRequestType type_ = MarketRequestType.NoneType;

		public const int PartnerFieldNumber = 9;

		private Player partner_;

		public const int PartnerRequestIdFieldNumber = 10;

		private string partnerRequestId_ = "";

		public const int ReasonFieldNumber = 11;

		private ClosingReason reason_ = ClosingReason.NoneReason;

		public const int QuantityFieldNumber = 12;

		private int quantity_;

		public const int PropertiesFieldNumber = 13;

		private static readonly MapField<string, InventoryItemProperty>.Codec _map_properties_codec = new MapField<string, InventoryItemProperty>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemProperty.Parser), 106u);

		private readonly MapField<string, InventoryItemProperty> properties_ = new MapField<string, InventoryItemProperty>();

		[DebuggerNonUserCode]
		public static MessageParser<ClosedRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[1];

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
		public string OriginId
		{
			get
			{
				return originId_;
			}
			set
			{
				originId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public long CloseDate
		{
			get
			{
				return closeDate_;
			}
			set
			{
				closeDate_ = value;
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
		public Player Partner
		{
			get
			{
				return partner_;
			}
			set
			{
				partner_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string PartnerRequestId
		{
			get
			{
				return partnerRequestId_;
			}
			set
			{
				partnerRequestId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ClosingReason Reason
		{
			get
			{
				return reason_;
			}
			set
			{
				reason_ = value;
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
		public ClosedRequest()
		{
		}

		[DebuggerNonUserCode]
		public ClosedRequest(ClosedRequest other)
			: this()
		{
			id_ = other.id_;
			originId_ = other.originId_;
			Creator = ((other.creator_ != null) ? other.Creator.Clone() : null);
			itemDefinitionId_ = other.itemDefinitionId_;
			price_ = other.price_;
			createDate_ = other.createDate_;
			closeDate_ = other.closeDate_;
			type_ = other.type_;
			Partner = ((other.partner_ != null) ? other.Partner.Clone() : null);
			partnerRequestId_ = other.partnerRequestId_;
			reason_ = other.reason_;
			quantity_ = other.quantity_;
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public ClosedRequest Clone()
		{
			return new ClosedRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClosedRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClosedRequest other)
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
			if (OriginId != other.OriginId)
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
			if (CloseDate != other.CloseDate)
			{
				return false;
			}
			if (Type != other.Type)
			{
				return false;
			}
			if (!object.Equals(Partner, other.Partner))
			{
				return false;
			}
			if (PartnerRequestId != other.PartnerRequestId)
			{
				return false;
			}
			if (Reason != other.Reason)
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
			if (OriginId.Length != 0)
			{
				num ^= OriginId.GetHashCode();
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
			if (CloseDate != 0)
			{
				num ^= CloseDate.GetHashCode();
			}
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
			}
			if (partner_ != null)
			{
				num ^= Partner.GetHashCode();
			}
			if (PartnerRequestId.Length != 0)
			{
				num ^= PartnerRequestId.GetHashCode();
			}
			if (Reason != 0)
			{
				num ^= Reason.GetHashCode();
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
			if (OriginId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(OriginId);
			}
			if (creator_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(Creator);
			}
			if (ItemDefinitionId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(ItemDefinitionId);
			}
			if (Price != 0f)
			{
				output.WriteRawTag(45);
				output.WriteFloat(Price);
			}
			if (CreateDate != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt64(CreateDate);
			}
			if (CloseDate != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt64(CloseDate);
			}
			if (Type != 0)
			{
				output.WriteRawTag(64);
				output.WriteEnum((int)Type);
			}
			if (partner_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(Partner);
			}
			if (PartnerRequestId.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(PartnerRequestId);
			}
			if (Reason != 0)
			{
				output.WriteRawTag(88);
				output.WriteEnum((int)Reason);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(96);
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
			if (OriginId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(OriginId);
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
			if (CloseDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(CloseDate);
			}
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			if (partner_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Partner);
			}
			if (PartnerRequestId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PartnerRequestId);
			}
			if (Reason != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Reason);
			}
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClosedRequest other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.OriginId.Length != 0)
			{
				OriginId = other.OriginId;
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
			if (other.CloseDate != 0)
			{
				CloseDate = other.CloseDate;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.partner_ != null)
			{
				if (partner_ == null)
				{
					partner_ = new Player();
				}
				Partner.MergeFrom(other.Partner);
			}
			if (other.PartnerRequestId.Length != 0)
			{
				PartnerRequestId = other.PartnerRequestId;
			}
			if (other.Reason != 0)
			{
				Reason = other.Reason;
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
					OriginId = input.ReadString();
					break;
				case 26u:
					if (creator_ == null)
					{
						creator_ = new Player();
					}
					input.ReadMessage(creator_);
					break;
				case 32u:
					ItemDefinitionId = input.ReadInt32();
					break;
				case 45u:
					Price = input.ReadFloat();
					break;
				case 48u:
					CreateDate = input.ReadInt64();
					break;
				case 56u:
					CloseDate = input.ReadInt64();
					break;
				case 64u:
					type_ = (MarketRequestType)input.ReadEnum();
					break;
				case 74u:
					if (partner_ == null)
					{
						partner_ = new Player();
					}
					input.ReadMessage(partner_);
					break;
				case 82u:
					PartnerRequestId = input.ReadString();
					break;
				case 88u:
					reason_ = (ClosingReason)input.ReadEnum();
					break;
				case 96u:
					Quantity = input.ReadInt32();
					break;
				case 106u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
