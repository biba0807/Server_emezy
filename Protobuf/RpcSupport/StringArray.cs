using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class StringArray : IMessage<StringArray>, IMessage, IEquatable<StringArray>, IDeepCloneable<StringArray>
	{
		private static readonly MessageParser<StringArray> _parser = new MessageParser<StringArray>(() => new StringArray());

		public const int ValueFieldNumber = 1;

		private static readonly FieldCodec<string> _repeated_value_codec = FieldCodec.ForString(10u);

		private readonly RepeatedField<string> value_ = new RepeatedField<string>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<StringArray> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[8];

		[DebuggerNonUserCode]
		public RepeatedField<string> Value => value_;

		[DebuggerNonUserCode]
		public StringArray()
		{
		}

		[DebuggerNonUserCode]
		public StringArray(StringArray other)
			: this()
		{
			value_ = other.value_.Clone();
		}

		[DebuggerNonUserCode]
		public StringArray Clone()
		{
			return new StringArray(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as StringArray);
		}

		[DebuggerNonUserCode]
		public bool Equals(StringArray other)
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
		public void MergeFrom(StringArray other)
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
				if (num != 10)
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
