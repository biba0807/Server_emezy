using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemDefinition : IMessage<InventoryItemDefinition>, IMessage, IEquatable<InventoryItemDefinition>, IDeepCloneable<InventoryItemDefinition>
	{
		private static readonly MessageParser<InventoryItemDefinition> _parser = new MessageParser<InventoryItemDefinition>(() => new InventoryItemDefinition());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int DisplayNameFieldNumber = 2;

		private string displayName_ = "";

		public const int PropertiesFieldNumber = 3;

		private static readonly MapField<string, string>.Codec _map_properties_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 26u);

		private readonly MapField<string, string> properties_ = new MapField<string, string>();

		public const int BuyPriceFieldNumber = 4;

		private static readonly FieldCodec<CurrencyAmount> _repeated_buyPrice_codec = FieldCodec.ForMessage(34u, CurrencyAmount.Parser);

		private readonly RepeatedField<CurrencyAmount> buyPrice_ = new RepeatedField<CurrencyAmount>();

		public const int SellPriceFieldNumber = 5;

		private static readonly FieldCodec<CurrencyAmount> _repeated_sellPrice_codec = FieldCodec.ForMessage(42u, CurrencyAmount.Parser);

		private readonly RepeatedField<CurrencyAmount> sellPrice_ = new RepeatedField<CurrencyAmount>();

		public const int CanBeTradedFieldNumber = 6;

		private bool canBeTraded_;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemDefinition> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[2];

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
		public string DisplayName
		{
			get
			{
				return displayName_;
			}
			set
			{
				displayName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> Properties => properties_;

		[DebuggerNonUserCode]
		public RepeatedField<CurrencyAmount> BuyPrice => buyPrice_;

		[DebuggerNonUserCode]
		public RepeatedField<CurrencyAmount> SellPrice => sellPrice_;

		[DebuggerNonUserCode]
		public bool CanBeTraded
		{
			get
			{
				return canBeTraded_;
			}
			set
			{
				canBeTraded_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemDefinition()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemDefinition(InventoryItemDefinition other)
			: this()
		{
			id_ = other.id_;
			displayName_ = other.displayName_;
			properties_ = other.properties_.Clone();
			buyPrice_ = other.buyPrice_.Clone();
			sellPrice_ = other.sellPrice_.Clone();
			canBeTraded_ = other.canBeTraded_;
		}

		[DebuggerNonUserCode]
		public InventoryItemDefinition Clone()
		{
			return new InventoryItemDefinition(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemDefinition);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemDefinition other)
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
			if (DisplayName != other.DisplayName)
			{
				return false;
			}
			if (!Properties.Equals(other.Properties))
			{
				return false;
			}
			if (!buyPrice_.Equals(other.buyPrice_))
			{
				return false;
			}
			if (!sellPrice_.Equals(other.sellPrice_))
			{
				return false;
			}
			if (CanBeTraded != other.CanBeTraded)
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
			if (DisplayName.Length != 0)
			{
				num ^= DisplayName.GetHashCode();
			}
			num ^= Properties.GetHashCode();
			num ^= buyPrice_.GetHashCode();
			num ^= sellPrice_.GetHashCode();
			if (CanBeTraded)
			{
				num ^= CanBeTraded.GetHashCode();
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
			if (DisplayName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(DisplayName);
			}
			properties_.WriteTo(output, _map_properties_codec);
			buyPrice_.WriteTo(output, _repeated_buyPrice_codec);
			sellPrice_.WriteTo(output, _repeated_sellPrice_codec);
			if (CanBeTraded)
			{
				output.WriteRawTag(48);
				output.WriteBool(CanBeTraded);
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
			if (DisplayName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(DisplayName);
			}
			num += properties_.CalculateSize(_map_properties_codec);
			num += buyPrice_.CalculateSize(_repeated_buyPrice_codec);
			num += sellPrice_.CalculateSize(_repeated_sellPrice_codec);
			if (CanBeTraded)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemDefinition other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.DisplayName.Length != 0)
				{
					DisplayName = other.DisplayName;
				}
				properties_.Add(other.properties_);
				buyPrice_.Add(other.buyPrice_);
				sellPrice_.Add(other.sellPrice_);
				if (other.CanBeTraded)
				{
					CanBeTraded = other.CanBeTraded;
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
				case 18u:
					DisplayName = input.ReadString();
					break;
				case 26u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				case 34u:
					buyPrice_.AddEntriesFrom(input, _repeated_buyPrice_codec);
					break;
				case 42u:
					sellPrice_.AddEntriesFrom(input, _repeated_sellPrice_codec);
					break;
				case 48u:
					CanBeTraded = input.ReadBool();
					break;
				}
			}
		}
	}
}
