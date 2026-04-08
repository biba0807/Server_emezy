using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GameSetting : IMessage<GameSetting>, IMessage, IEquatable<GameSetting>, IDeepCloneable<GameSetting>
	{
		private static readonly MessageParser<GameSetting> _parser = new MessageParser<GameSetting>(() => new GameSetting());

		public const int KeyFieldNumber = 1;

		private string key_ = "";

		public const int ValueFieldNumber = 2;

		private string value_ = "";

		public const int IntValueFieldNumber = 3;

		private int intValue_;

		public const int FloatValueFieldNumber = 4;

		private float floatValue_;

		public const int BoolValueFieldNumber = 5;

		private bool boolValue_;

		public const int TypeFieldNumber = 6;

		private SettingType type_ = SettingType.String;

		[DebuggerNonUserCode]
		public static MessageParser<GameSetting> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => GameSettingsMessageReflection.Descriptor.MessageTypes[0];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Key
		{
			get
			{
				return key_;
			}
			set
			{
				key_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Value
		{
			get
			{
				return value_;
			}
			set
			{
				value_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public bool BoolValue
		{
			get
			{
				return boolValue_;
			}
			set
			{
				boolValue_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SettingType Type
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
		public GameSetting()
		{
		}

		[DebuggerNonUserCode]
		public GameSetting(GameSetting other)
			: this()
		{
			key_ = other.key_;
			value_ = other.value_;
			intValue_ = other.intValue_;
			floatValue_ = other.floatValue_;
			boolValue_ = other.boolValue_;
			type_ = other.type_;
		}

		[DebuggerNonUserCode]
		public GameSetting Clone()
		{
			return new GameSetting(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GameSetting);
		}

		[DebuggerNonUserCode]
		public bool Equals(GameSetting other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Key != other.Key)
			{
				return false;
			}
			if (Value != other.Value)
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
			if (BoolValue != other.BoolValue)
			{
				return false;
			}
			if (Type != other.Type)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Key.Length != 0)
			{
				num ^= Key.GetHashCode();
			}
			if (Value.Length != 0)
			{
				num ^= Value.GetHashCode();
			}
			if (IntValue != 0)
			{
				num ^= IntValue.GetHashCode();
			}
			if (FloatValue != 0f)
			{
				num ^= FloatValue.GetHashCode();
			}
			if (BoolValue)
			{
				num ^= BoolValue.GetHashCode();
			}
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
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
			if (Key.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Key);
			}
			if (Value.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Value);
			}
			if (IntValue != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(IntValue);
			}
			if (FloatValue != 0f)
			{
				output.WriteRawTag(37);
				output.WriteFloat(FloatValue);
			}
			if (BoolValue)
			{
				output.WriteRawTag(40);
				output.WriteBool(BoolValue);
			}
			if (Type != 0)
			{
				output.WriteRawTag(48);
				output.WriteEnum((int)Type);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Key.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Key);
			}
			if (Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Value);
			}
			if (IntValue != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(IntValue);
			}
			if (FloatValue != 0f)
			{
				num += 5;
			}
			if (BoolValue)
			{
				num += 2;
			}
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GameSetting other)
		{
			if (other != null)
			{
				if (other.Key.Length != 0)
				{
					Key = other.Key;
				}
				if (other.Value.Length != 0)
				{
					Value = other.Value;
				}
				if (other.IntValue != 0)
				{
					IntValue = other.IntValue;
				}
				if (other.FloatValue != 0f)
				{
					FloatValue = other.FloatValue;
				}
				if (other.BoolValue)
				{
					BoolValue = other.BoolValue;
				}
				if (other.Type != 0)
				{
					Type = other.Type;
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
					Key = input.ReadString();
					break;
				case 18u:
					Value = input.ReadString();
					break;
				case 24u:
					IntValue = input.ReadInt32();
					break;
				case 37u:
					FloatValue = input.ReadFloat();
					break;
				case 40u:
					BoolValue = input.ReadBool();
					break;
				case 48u:
					type_ = (SettingType)input.ReadEnum();
					break;
				}
			}
		}
	}
}
