using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemPropertyDefinition : IMessage<InventoryItemPropertyDefinition>, IMessage, IEquatable<InventoryItemPropertyDefinition>, IDeepCloneable<InventoryItemPropertyDefinition>
	{
		private static readonly MessageParser<InventoryItemPropertyDefinition> _parser = new MessageParser<InventoryItemPropertyDefinition>(() => new InventoryItemPropertyDefinition());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int PropertyTypeFieldNumber = 2;

		private PropertyType propertyType_ = PropertyType.Int;

		public const int SaveInTradeFieldNumber = 3;

		private bool saveInTrade_;

		public const int SetByTypeFieldNumber = 4;

		private PropertySetByType setByType_ = PropertySetByType.GameServer;

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemPropertyDefinition> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[4];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Name
		{
			get
			{
				return name_;
			}
			set
			{
				name_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public PropertyType PropertyType
		{
			get
			{
				return propertyType_;
			}
			set
			{
				propertyType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool SaveInTrade
		{
			get
			{
				return saveInTrade_;
			}
			set
			{
				saveInTrade_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PropertySetByType SetByType
		{
			get
			{
				return setByType_;
			}
			set
			{
				setByType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinition()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinition(InventoryItemPropertyDefinition other)
			: this()
		{
			name_ = other.name_;
			propertyType_ = other.propertyType_;
			saveInTrade_ = other.saveInTrade_;
			setByType_ = other.setByType_;
		}

		[DebuggerNonUserCode]
		public InventoryItemPropertyDefinition Clone()
		{
			return new InventoryItemPropertyDefinition(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemPropertyDefinition);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemPropertyDefinition other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (PropertyType != other.PropertyType)
			{
				return false;
			}
			if (SaveInTrade != other.SaveInTrade)
			{
				return false;
			}
			if (SetByType != other.SetByType)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (PropertyType != 0)
			{
				num ^= PropertyType.GetHashCode();
			}
			if (SaveInTrade)
			{
				num ^= SaveInTrade.GetHashCode();
			}
			if (SetByType != 0)
			{
				num ^= SetByType.GetHashCode();
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
			if (Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Name);
			}
			if (PropertyType != 0)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)PropertyType);
			}
			if (SaveInTrade)
			{
				output.WriteRawTag(24);
				output.WriteBool(SaveInTrade);
			}
			if (SetByType != 0)
			{
				output.WriteRawTag(32);
				output.WriteEnum((int)SetByType);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (PropertyType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)PropertyType);
			}
			if (SaveInTrade)
			{
				num += 2;
			}
			if (SetByType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)SetByType);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemPropertyDefinition other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.PropertyType != 0)
				{
					PropertyType = other.PropertyType;
				}
				if (other.SaveInTrade)
				{
					SaveInTrade = other.SaveInTrade;
				}
				if (other.SetByType != 0)
				{
					SetByType = other.SetByType;
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
				case 10u:
					Name = input.ReadString();
					break;
				case 16u:
					propertyType_ = (PropertyType)input.ReadEnum();
					break;
				case 24u:
					SaveInTrade = input.ReadBool();
					break;
				case 32u:
					setByType_ = (PropertySetByType)input.ReadEnum();
					break;
				}
			}
		}
	}
}
