using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemAmount : IMessage<InventoryItemAmount>, IMessage, IEquatable<InventoryItemAmount>, IDeepCloneable<InventoryItemAmount>
	{
		private static readonly MessageParser<InventoryItemAmount> _parser = new MessageParser<InventoryItemAmount>(() => new InventoryItemAmount());

		public const int InventoryItemDefinitionIdFieldNumber = 1;

		private int inventoryItemDefinitionId_;

		public const int ValueFieldNumber = 2;

		private int value_;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemAmount> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[5];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int InventoryItemDefinitionId
		{
			get
			{
				return inventoryItemDefinitionId_;
			}
			set
			{
				inventoryItemDefinitionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Value
		{
			get
			{
				return value_;
			}
			set
			{
				value_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount(InventoryItemAmount other)
			: this()
		{
			inventoryItemDefinitionId_ = other.inventoryItemDefinitionId_;
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public InventoryItemAmount Clone()
		{
			return new InventoryItemAmount(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemAmount);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemAmount other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (InventoryItemDefinitionId != other.InventoryItemDefinitionId)
			{
				return false;
			}
			if (Value != other.Value)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (InventoryItemDefinitionId != 0)
			{
				num ^= InventoryItemDefinitionId.GetHashCode();
			}
			if (Value != 0)
			{
				num ^= Value.GetHashCode();
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
			if (InventoryItemDefinitionId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(InventoryItemDefinitionId);
			}
			if (Value != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (InventoryItemDefinitionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(InventoryItemDefinitionId);
			}
			if (Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemAmount other)
		{
			if (other != null)
			{
				if (other.InventoryItemDefinitionId != 0)
				{
					InventoryItemDefinitionId = other.InventoryItemDefinitionId;
				}
				if (other.Value != 0)
				{
					Value = other.Value;
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
					InventoryItemDefinitionId = input.ReadInt32();
					break;
				case 16u:
					Value = input.ReadInt32();
					break;
				}
			}
		}
	}
}
