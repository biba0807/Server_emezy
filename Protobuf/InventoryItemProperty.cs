using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemProperty : IMessage<InventoryItemProperty>, IMessage, IEquatable<InventoryItemProperty>, IDeepCloneable<InventoryItemProperty>
	{
		private static readonly MessageParser<InventoryItemProperty> _parser = new MessageParser<InventoryItemProperty>(() => new InventoryItemProperty());

		public const int TypeFieldNumber = 1;

		private PropertyType type_ = PropertyType.Int;

		public const int IntValueFieldNumber = 2;

		private int intValue_;

		public const int FloatValueFieldNumber = 3;

		private float floatValue_;

		public const int StringValueFieldNumber = 4;

		private string stringValue_ = "";

		public const int BooleanValueFieldNumber = 5;

		private bool booleanValue_;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemProperty> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[8];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public PropertyType Type
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
		public int IntValue
		{
			get
			{
				return intValue_;
			}
			set
			{
				intValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float FloatValue
		{
			get
			{
				return floatValue_;
			}
			set
			{
				floatValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string StringValue
		{
			get
			{
				return stringValue_;
			}
			set
			{
				stringValue_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool BooleanValue
		{
			get
			{
				return booleanValue_;
			}
			set
			{
				booleanValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemProperty()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemProperty(InventoryItemProperty other)
			: this()
		{
			type_ = other.type_;
			intValue_ = other.intValue_;
			floatValue_ = other.floatValue_;
			stringValue_ = other.stringValue_;
			booleanValue_ = other.booleanValue_;
		}

		[DebuggerNonUserCode]
		public InventoryItemProperty Clone()
		{
			return new InventoryItemProperty(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemProperty);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemProperty other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Type != other.Type)
			{
				return false;
			}
			if (IntValue != other.IntValue)
			{
				return false;
			}
			if (FloatValue != other.FloatValue)
			{
				return false;
			}
			if (StringValue != other.StringValue)
			{
				return false;
			}
			if (BooleanValue != other.BooleanValue)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
			}
			if (IntValue != 0)
			{
				num ^= IntValue.GetHashCode();
			}
			if (FloatValue != 0f)
			{
				num ^= FloatValue.GetHashCode();
			}
			if (StringValue.Length != 0)
			{
				num ^= StringValue.GetHashCode();
			}
			if (BooleanValue)
			{
				num ^= BooleanValue.GetHashCode();
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
			if (Type != 0)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)Type);
			}
			if (IntValue != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(IntValue);
			}
			if (FloatValue != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(FloatValue);
			}
			if (StringValue.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(StringValue);
			}
			if (BooleanValue)
			{
				output.WriteRawTag(40);
				output.WriteBool(BooleanValue);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			if (IntValue != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(IntValue);
			}
			if (FloatValue != 0f)
			{
				num += 5;
			}
			if (StringValue.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(StringValue);
			}
			if (BooleanValue)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemProperty other)
		{
			if (other != null)
			{
				if (other.Type != 0)
				{
					Type = other.Type;
				}
				if (other.IntValue != 0)
				{
					IntValue = other.IntValue;
				}
				if (other.FloatValue != 0f)
				{
					FloatValue = other.FloatValue;
				}
				if (other.StringValue.Length != 0)
				{
					StringValue = other.StringValue;
				}
				if (other.BooleanValue)
				{
					BooleanValue = other.BooleanValue;
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
					type_ = (PropertyType)input.ReadEnum();
					break;
				case 16u:
					IntValue = input.ReadInt32();
					break;
				case 29u:
					FloatValue = input.ReadFloat();
					break;
				case 34u:
					StringValue = input.ReadString();
					break;
				case 40u:
					BooleanValue = input.ReadBool();
					break;
				}
			}
		}
	}
}
