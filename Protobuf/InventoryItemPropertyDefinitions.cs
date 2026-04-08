using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemPropertyDefinitions : IMessage<InventoryItemPropertyDefinitions>, IMessage, IEquatable<InventoryItemPropertyDefinitions>, IDeepCloneable<InventoryItemPropertyDefinitions>
	{
		private static readonly MessageParser<InventoryItemPropertyDefinitions> _parser = new MessageParser<InventoryItemPropertyDefinitions>(() => new InventoryItemPropertyDefinitions());

		public const int ItemDefinitionIdFieldNumber = 1;

		private int itemDefinitionId_;

		public const int DefinitionsFieldNumber = 2;

		private static readonly MapField<string, InventoryItemPropertyDefinition>.Codec _map_definitions_codec = new MapField<string, InventoryItemPropertyDefinition>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemPropertyDefinition.Parser), 18u);

		private readonly MapField<string, InventoryItemPropertyDefinition> definitions_ = new MapField<string, InventoryItemPropertyDefinition>();

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemPropertyDefinitions> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[3];

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
		public MapField<string, InventoryItemPropertyDefinition> Definitions => definitions_;

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinitions()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinitions(InventoryItemPropertyDefinitions other)
			: this()
		{
			itemDefinitionId_ = other.itemDefinitionId_;
			definitions_ = other.definitions_.Clone();
		}

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinitions Clone()
		{
			return new InventoryItemPropertyDefinitions(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemPropertyDefinitions);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemPropertyDefinitions other)
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
			if (!Definitions.Equals(other.Definitions))
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
			return num ^ Definitions.GetHashCode();
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
			definitions_.WriteTo(output, _map_definitions_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (ItemDefinitionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(ItemDefinitionId);
			}
			return num + definitions_.CalculateSize(_map_definitions_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemPropertyDefinitions other)
		{
			if (other != null)
			{
				if (other.ItemDefinitionId != 0)
				{
					ItemDefinitionId = other.ItemDefinitionId;
				}
				definitions_.Add(other.definitions_);
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
				case 18u:
					definitions_.AddEntriesFrom(input, _map_definitions_codec);
					break;
				}
			}
		}
	}
}
