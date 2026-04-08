using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class Float : IMessage<Float>, IMessage, IEquatable<Float>, IDeepCloneable<Float>
	{
		private static readonly MessageParser<Float> _parser = new MessageParser<Float>(() => new Float());

		public const int ValueFieldNumber = 1;

		private float value_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<Float> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[15];

		[DebuggerNonUserCode]
		public float Value
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
		public Float()
		{
		}

		[DebuggerNonUserCode]
		public Float(Float other)
			: this()
		{
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public Float Clone()
		{
			return new Float(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Float);
		}

		[DebuggerNonUserCode]
		public bool Equals(Float other)
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
			if (Value != 0f)
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
			if (Value != 0f)
			{
				output.WriteRawTag(13);
				output.WriteFloat(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Value != 0f)
			{
				num += 5;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Float other)
		{
			if (other != null && other.Value != 0f)
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
				if (num != 13)
				{
					input.SkipLastField();
				}
				else
				{
					Value = input.ReadFloat();
				}
			}
		}
	}
}
