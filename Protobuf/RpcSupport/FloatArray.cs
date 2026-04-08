using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class FloatArray : IMessage<FloatArray>, IMessage, IEquatable<FloatArray>, IDeepCloneable<FloatArray>
	{
		private static readonly MessageParser<FloatArray> _parser = new MessageParser<FloatArray>(() => new FloatArray());

		public const int ValueFieldNumber = 1;

		private static readonly FieldCodec<float> _repeated_value_codec = FieldCodec.ForFloat(10u);

		private readonly RepeatedField<float> value_ = new RepeatedField<float>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<FloatArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[16];

		[DebuggerNonUserCode]
		public RepeatedField<float> Value => value_;

		[DebuggerNonUserCode]
		public FloatArray()
		{
		}

		[DebuggerNonUserCode]
		public FloatArray(FloatArray other)
			: this()
		{
			value_ = other.value_.Clone();
		}

		[DebuggerNonUserCode]
		public FloatArray Clone()
		{
			return new FloatArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as FloatArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(FloatArray other)
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
		public void MergeFrom(FloatArray other)
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
				if (num != 10 && num != 13)
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
