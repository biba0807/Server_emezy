using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class DoubleArray : IMessage<DoubleArray>, IMessage, IEquatable<DoubleArray>, IDeepCloneable<DoubleArray>
	{
		private static readonly MessageParser<DoubleArray> _parser = new MessageParser<DoubleArray>(() => new DoubleArray());

		public const int ValueFieldNumber = 1;

		private static readonly FieldCodec<double> _repeated_value_codec = FieldCodec.ForDouble(10u);

		private readonly RepeatedField<double> value_ = new RepeatedField<double>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<DoubleArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[14];

		[DebuggerNonUserCode]
		public RepeatedField<double> Value => value_;

		[DebuggerNonUserCode]
		public DoubleArray()
		{
		}

		[DebuggerNonUserCode]
		public DoubleArray(DoubleArray other)
			: this()
		{
			value_ = other.value_.Clone();
		}

		[DebuggerNonUserCode]
		public DoubleArray Clone()
		{
			return new DoubleArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as DoubleArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(DoubleArray other)
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
		public void MergeFrom(DoubleArray other)
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
				if (num != 10 && num != 9)
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
