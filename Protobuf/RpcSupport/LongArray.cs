using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class LongArray : IMessage<LongArray>, IMessage, IEquatable<LongArray>, IDeepCloneable<LongArray>
	{
		private static readonly MessageParser<LongArray> _parser = new MessageParser<LongArray>(() => new LongArray());

		public const int ValueFieldNumber = 1;

		private static readonly FieldCodec<long> _repeated_value_codec = FieldCodec.ForInt64(10u);

		private readonly RepeatedField<long> value_ = new RepeatedField<long>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<LongArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[12];

		[DebuggerNonUserCode]
		public RepeatedField<long> Value => value_;

		[DebuggerNonUserCode]
		public LongArray()
		{
		}

		[DebuggerNonUserCode]
		public LongArray(LongArray other)
			: this()
		{
			value_ = other.value_.Clone();
		}

		[DebuggerNonUserCode]
		public LongArray Clone()
		{
			return new LongArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as LongArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(LongArray other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (!value_.Equals(other.value_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			return num ^ value_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			value_.WriteTo(output, _repeated_value_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			return num + value_.CalculateSize(_repeated_value_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(LongArray other)
		{
			if (other != null)
			{
				value_.Add(other.value_);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				if (num != 10 && num != 8)
				{
					input.SkipLastField();
				}
				else
				{
					value_.AddEntriesFrom(input, _repeated_value_codec);
				}
			}
		}
	}
}
