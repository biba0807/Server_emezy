using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Offset : IMessage<Offset>, IMessage, IEquatable<Offset>, IDeepCloneable<Offset>
	{
		private static readonly MessageParser<Offset> _parser = new MessageParser<Offset>(() => new Offset());

		public const int Offset_FieldNumber = 1;

		private int offset_;

		public const int LengthFieldNumber = 2;

		private int length_;

		[DebuggerNonUserCode]
		public static MessageParser<Offset> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CommonMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Offset_
		{
			get
			{
				return offset_;
			}
			set
			{
				offset_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Length
		{
			get
			{
				return length_;
			}
			set
			{
				length_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Offset()
		{
		}

		[DebuggerNonUserCode]
		public Offset(Offset other)
			: this()
		{
			offset_ = other.offset_;
			length_ = other.length_;
		}

		[DebuggerNonUserCode]
		public Offset Clone()
		{
			return new Offset(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Offset);
		}

		[DebuggerNonUserCode]
		public bool Equals(Offset other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Offset_ != other.Offset_)
			{
				return false;
			}
			if (Length != other.Length)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Offset_ != 0)
			{
				num ^= Offset_.GetHashCode();
			}
			if (Length != 0)
			{
				num ^= Length.GetHashCode();
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
			if (Offset_ != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Offset_);
			}
			if (Length != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Length);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Offset_ != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Offset_);
			}
			if (Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Length);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Offset other)
		{
			if (other != null)
			{
				if (other.Offset_ != 0)
				{
					Offset_ = other.Offset_;
				}
				if (other.Length != 0)
				{
					Length = other.Length;
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
					Offset_ = input.ReadInt32();
					break;
				case 16u:
					Length = input.ReadInt32();
					break;
				}
			}
		}
	}
}
