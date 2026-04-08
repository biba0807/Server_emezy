using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class BooleanArray : IMessage<BooleanArray>, IMessage, IEquatable<BooleanArray>, IDeepCloneable<BooleanArray>
	{
		private static readonly MessageParser<BooleanArray> _parser = new MessageParser<BooleanArray>(() => new BooleanArray());

		public const int ValueFieldNumber = 1;

		private static readonly FieldCodec<bool> _repeated_value_codec = FieldCodec.ForBool(10u);

		private readonly RepeatedField<bool> value_ = new RepeatedField<bool>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<BooleanArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[18];

		[DebuggerNonUserCode]
		public RepeatedField<bool> Value => value_;

		[DebuggerNonUserCode]
		public BooleanArray()
		{
		}

		[DebuggerNonUserCode]
		public BooleanArray(BooleanArray other)
			: this()
		{
			value_ = other.value_.Clone();
		}

		[DebuggerNonUserCode]
		public BooleanArray Clone()
		{
			return new BooleanArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as BooleanArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(BooleanArray other)
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
		public void MergeFrom(BooleanArray other)
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
