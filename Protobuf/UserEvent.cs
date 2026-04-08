using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class UserEvent : IMessage<UserEvent>, IMessage, IEquatable<UserEvent>, IDeepCloneable<UserEvent>
	{
		private static readonly MessageParser<UserEvent> _parser = new MessageParser<UserEvent>(() => new UserEvent());

		public const int CategoryFieldNumber = 1;

		private string category_ = "";

		public const int EventFieldNumber = 2;

		private string event_ = "";

		public const int ValueFieldNumber = 3;

		private int value_;

		public const int IncrementFieldNumber = 4;

		private bool increment_;

		public const int StoreCountryFieldNumber = 5;

		private bool storeCountry_;

		[DebuggerNonUserCode]
		public static MessageParser<UserEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AnalyticsMessageReflection.Descriptor.MessageTypes[0];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Category
		{
			get
			{
				return category_;
			}
			set
			{
				category_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Event
		{
			get
			{
				return event_;
			}
			set
			{
				event_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public bool Increment
		{
			get
			{
				return increment_;
			}
			set
			{
				increment_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool StoreCountry
		{
			get
			{
				return storeCountry_;
			}
			set
			{
				storeCountry_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserEvent()
		{
		}

		[DebuggerNonUserCode]
		public UserEvent(UserEvent other)
			: this()
		{
			category_ = other.category_;
			event_ = other.event_;
			value_ = other.value_;
			increment_ = other.increment_;
			storeCountry_ = other.storeCountry_;
		}

		[DebuggerNonUserCode]
		public UserEvent Clone()
		{
			return new UserEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as UserEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(UserEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Category != other.Category)
			{
				return false;
			}
			if (Event != other.Event)
			{
				return false;
			}
			if (Value != other.Value)
			{
				return false;
			}
			if (Increment != other.Increment)
			{
				return false;
			}
			if (StoreCountry != other.StoreCountry)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Category.Length != 0)
			{
				num ^= Category.GetHashCode();
			}
			if (Event.Length != 0)
			{
				num ^= Event.GetHashCode();
			}
			if (Value != 0)
			{
				num ^= Value.GetHashCode();
			}
			if (Increment)
			{
				num ^= Increment.GetHashCode();
			}
			if (StoreCountry)
			{
				num ^= StoreCountry.GetHashCode();
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
			if (Category.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Category);
			}
			if (Event.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Event);
			}
			if (Value != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Value);
			}
			if (Increment)
			{
				output.WriteRawTag(32);
				output.WriteBool(Increment);
			}
			if (StoreCountry)
			{
				output.WriteRawTag(40);
				output.WriteBool(StoreCountry);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Category.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Category);
			}
			if (Event.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Event);
			}
			if (Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Value);
			}
			if (Increment)
			{
				num += 2;
			}
			if (StoreCountry)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(UserEvent other)
		{
			if (other != null)
			{
				if (other.Category.Length != 0)
				{
					Category = other.Category;
				}
				if (other.Event.Length != 0)
				{
					Event = other.Event;
				}
				if (other.Value != 0)
				{
					Value = other.Value;
				}
				if (other.Increment)
				{
					Increment = other.Increment;
				}
				if (other.StoreCountry)
				{
					StoreCountry = other.StoreCountry;
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
					Category = input.ReadString();
					break;
				case 18u:
					Event = input.ReadString();
					break;
				case 24u:
					Value = input.ReadInt32();
					break;
				case 32u:
					Increment = input.ReadBool();
					break;
				case 40u:
					StoreCountry = input.ReadBool();
					break;
				}
			}
		}
	}
}
