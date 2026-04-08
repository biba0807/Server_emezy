using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class BinaryValue : IMessage<BinaryValue>, IMessage, IEquatable<BinaryValue>, IDeepCloneable<BinaryValue>
	{
		private static readonly MessageParser<BinaryValue> _parser = new MessageParser<BinaryValue>(() => new BinaryValue());

		public const int IsNullFieldNumber = 1;

		private bool isNull_;

		public const int ArrayFieldNumber = 2;

		private static readonly FieldCodec<ByteString> _repeated_array_codec = FieldCodec.ForBytes(18u);

		private readonly RepeatedField<ByteString> array_ = new RepeatedField<ByteString>();

		public const int OneFieldNumber = 3;

		private ByteString one_ = ByteString.Empty;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<BinaryValue> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[6];

		[DebuggerNonUserCode]
		public bool IsNull
		{
			get
			{
				return isNull_;
			}
			set
			{
				isNull_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ByteString> Array => array_;

		[DebuggerNonUserCode]
		public ByteString One
		{
			get
			{
				return one_;
			}
			set
			{
				one_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public BinaryValue()
		{
		}

		[DebuggerNonUserCode]
		public BinaryValue(BinaryValue other)
			: this()
		{
			isNull_ = other.isNull_;
			array_ = other.array_.Clone();
			one_ = other.one_;
		}

		[DebuggerNonUserCode]
		public BinaryValue Clone()
		{
			return new BinaryValue(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as BinaryValue);
		}

		[DebuggerNonUserCode]
		public bool Equals(BinaryValue other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (IsNull != other.IsNull)
			{
				return false;
			}
			if (!array_.Equals(other.array_))
			{
				return false;
			}
			if (One != other.One)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (IsNull)
			{
				num ^= IsNull.GetHashCode();
			}
			num ^= array_.GetHashCode();
			if (One.Length != 0)
			{
				num ^= One.GetHashCode();
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
			if (IsNull)
			{
				output.WriteRawTag(8);
				output.WriteBool(IsNull);
			}
			array_.WriteTo(output, _repeated_array_codec);
			if (One.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteBytes(One);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (IsNull)
			{
				num += 2;
			}
			num += array_.CalculateSize(_repeated_array_codec);
			if (One.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(One);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(BinaryValue other)
		{
			if (other != null)
			{
				if (other.IsNull)
				{
					IsNull = other.IsNull;
				}
				array_.Add(other.array_);
				if (other.One.Length != 0)
				{
					One = other.One;
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
					IsNull = input.ReadBool();
					break;
				case 18u:
					array_.AddEntriesFrom(input, _repeated_array_codec);
					break;
				case 26u:
					One = input.ReadBytes();
					break;
				}
			}
		}
	}
}
