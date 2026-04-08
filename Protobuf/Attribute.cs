using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Attribute : IMessage<Attribute>, IMessage, IEquatable<Attribute>, IDeepCloneable<Attribute>
	{
		private static readonly MessageParser<Attribute> _parser = new MessageParser<Attribute>(() => new Attribute());

		public const int TypeFieldNumber = 1;

		private PropertyType type_ = PropertyType.Int;

		public const int IntFieldNumber = 2;

		private int int_;

		public const int FloatFieldNumber = 3;

		private float float_;

		public const int StringFieldNumber = 4;

		private string string_ = "";

		public const int BooleanFieldNumber = 5;

		private bool boolean_;

		[DebuggerNonUserCode]
		public static MessageParser<Attribute> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerMessageReflection.Descriptor.MessageTypes[3];

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
		public int Int
		{
			get
			{
				return int_;
			}
			set
			{
				int_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float Float
		{
			get
			{
				return float_;
			}
			set
			{
				float_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string String
		{
			get
			{
				return string_;
			}
			set
			{
				string_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool Boolean
		{
			get
			{
				return boolean_;
			}
			set
			{
				boolean_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Attribute()
		{
		}

		[DebuggerNonUserCode]
		public Attribute(Attribute other)
			: this()
		{
			type_ = other.type_;
			int_ = other.int_;
			float_ = other.float_;
			string_ = other.string_;
			boolean_ = other.boolean_;
		}

		[DebuggerNonUserCode]
		public Attribute Clone()
		{
			return new Attribute(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Attribute);
		}

		[DebuggerNonUserCode]
		public bool Equals(Attribute other)
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
			if (Int != other.Int)
			{
				return false;
			}
			if (Float != other.Float)
			{
				return false;
			}
			if (String != other.String)
			{
				return false;
			}
			if (Boolean != other.Boolean)
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
			if (Int != 0)
			{
				num ^= Int.GetHashCode();
			}
			if (Float != 0f)
			{
				num ^= Float.GetHashCode();
			}
			if (String.Length != 0)
			{
				num ^= String.GetHashCode();
			}
			if (Boolean)
			{
				num ^= Boolean.GetHashCode();
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
			if (Int != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Int);
			}
			if (Float != 0f)
			{
				output.WriteRawTag(29);
				output.WriteFloat(Float);
			}
			if (String.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(String);
			}
			if (Boolean)
			{
				output.WriteRawTag(40);
				output.WriteBool(Boolean);
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
			if (Int != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Int);
			}
			if (Float != 0f)
			{
				num += 5;
			}
			if (String.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(String);
			}
			if (Boolean)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Attribute other)
		{
			if (other != null)
			{
				if (other.Type != 0)
				{
					Type = other.Type;
				}
				if (other.Int != 0)
				{
					Int = other.Int;
				}
				if (other.Float != 0f)
				{
					Float = other.Float;
				}
				if (other.String.Length != 0)
				{
					String = other.String;
				}
				if (other.Boolean)
				{
					Boolean = other.Boolean;
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
					Int = input.ReadInt32();
					break;
				case 29u:
					Float = input.ReadFloat();
					break;
				case 34u:
					String = input.ReadString();
					break;
				case 40u:
					Boolean = input.ReadBool();
					break;
				}
			}
		}
	}
}
