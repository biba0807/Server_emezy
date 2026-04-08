using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItem : IMessage<InventoryItem>, IMessage, IEquatable<InventoryItem>, IDeepCloneable<InventoryItem>
	{
		private static readonly MessageParser<InventoryItem> _parser = new MessageParser<InventoryItem>(() => new InventoryItem());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int ItemDefinitionIdFieldNumber = 2;

		private int itemDefinitionId_;

		public const int QuantityFieldNumber = 3;

		private int quantity_;

		public const int FlagsFieldNumber = 4;

		private int flags_;

		public const int DateFieldNumber = 5;

		private long date_;

		public const int PropertiesFieldNumber = 6;

		private static readonly MapField<string, InventoryItemProperty>.Codec _map_properties_codec = new MapField<string, InventoryItemProperty>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemProperty.Parser), 50u);

		private readonly MapField<string, InventoryItemProperty> properties_ = new MapField<string, InventoryItemProperty>();

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItem> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[1];

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
		public int Flags
		{
			get
			{
				return flags_;
			}
			set
			{
				flags_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Date
		{
			get
			{
				return date_;
			}
			set
			{
				date_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, InventoryItemProperty> Properties => properties_;

		[DebuggerNonUserCode]
		public InventoryItem()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItem(InventoryItem other)
			: this()
		{
			id_ = other.id_;
			itemDefinitionId_ = other.itemDefinitionId_;
			quantity_ = other.quantity_;
			flags_ = other.flags_;
			date_ = other.date_;
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public InventoryItem Clone()
		{
			return new InventoryItem(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItem);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItem other)
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
			if (Quantity != other.Quantity)
			{
				return false;
			}
			if (Flags != other.Flags)
			{
				return false;
			}
			if (Date != other.Date)
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
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (ItemDefinitionId != 0)
			{
				num ^= ItemDefinitionId.GetHashCode();
			}
			if (Quantity != 0)
			{
				num ^= Quantity.GetHashCode();
			}
			if (Flags != 0)
			{
				num ^= Flags.GetHashCode();
			}
			if (Date != 0)
			{
				num ^= Date.GetHashCode();
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
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
			if (ItemDefinitionId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(ItemDefinitionId);
			}
			if (Quantity != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Quantity);
			}
			if (Flags != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(Flags);
			}
			if (Date != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt64(Date);
			}
			properties_.WriteTo(output, _map_properties_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			if (ItemDefinitionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ItemDefinitionId);
			}
			if (Quantity != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Quantity);
			}
			if (Flags != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Flags);
			}
			if (Date != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Date);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItem other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.ItemDefinitionId != 0)
				{
					ItemDefinitionId = other.ItemDefinitionId;
				}
				if (other.Quantity != 0)
				{
					Quantity = other.Quantity;
				}
				if (other.Flags != 0)
				{
					Flags = other.Flags;
				}
				if (other.Date != 0)
				{
					Date = other.Date;
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
				case 8u:
					Id = input.ReadInt32();
					break;
				case 16u:
					ItemDefinitionId = input.ReadInt32();
					break;
				case 24u:
					Quantity = input.ReadInt32();
					break;
				case 32u:
					Flags = input.ReadInt32();
					break;
				case 40u:
					Date = input.ReadInt64();
					break;
				case 50u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
