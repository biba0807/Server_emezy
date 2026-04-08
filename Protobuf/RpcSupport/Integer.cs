using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class Integer : IMessage<Integer>, IMessage, IEquatable<Integer>, IDeepCloneable<Integer>
	{
		private static readonly MessageParser<Integer> _parser = new MessageParser<Integer>(() => new Integer());

		public const int ValueFieldNumber = 1;

		private int value_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<Integer> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[9];

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
		public Integer()
		{
		}

		[DebuggerNonUserCode]
		public Integer(Integer other)
			: this()
		{
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public Integer Clone()
		{
			return new Integer(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Integer);
		}

		[DebuggerNonUserCode]
		public bool Equals(Integer other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
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
			if (Value != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Value != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Integer other)
		{
			if (other != null && other.Value != 0)
			{
				Value = other.Value;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				if (num != 8)
				{
					input.SkipLastField();
				}
				else
				{
					Value = input.ReadInt32();
				}
			}
		}
	}
}
