using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class ByteArray : IMessage<ByteArray>, IMessage, IEquatable<ByteArray>, IDeepCloneable<ByteArray>
	{
		private static readonly MessageParser<ByteArray> _parser = new MessageParser<ByteArray>(() => new ByteArray());

		public const int ValueFieldNumber = 1;

		private ByteString value_ = ByteString.Empty;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<ByteArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[20];

		[DebuggerNonUserCode]
		public ByteString Value
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
		public ByteArray()
		{
		}

		[DebuggerNonUserCode]
		public ByteArray(ByteArray other)
			: this()
		{
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public ByteArray Clone()
		{
			return new ByteArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ByteArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(ByteArray other)
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
			if (Value.Length != 0)
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
			if (Value.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteBytes(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ByteArray other)
		{
			if (other != null && other.Value.Length != 0)
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
				if (num != 10)
				{
					input.SkipLastField();
				}
				else
				{
					Value = input.ReadBytes();
				}
			}
		}
	}
}
