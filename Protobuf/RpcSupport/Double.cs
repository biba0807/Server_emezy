using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class Double : IMessage<Double>, IMessage, IEquatable<Double>, IDeepCloneable<Double>
	{
		private static readonly MessageParser<Double> _parser = new MessageParser<Double>(() => new Double());

		public const int ValueFieldNumber = 1;

		private double value_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<Double> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[13];

		[DebuggerNonUserCode]
		public double Value
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
		public Double()
		{
		}

		[DebuggerNonUserCode]
		public Double(Double other)
			: this()
		{
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public Double Clone()
		{
			return new Double(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Double);
		}

		[DebuggerNonUserCode]
		public bool Equals(Double other)
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
			if (Value != 0.0)
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
			if (Value != 0.0)
			{
				output.WriteRawTag(9);
				output.WriteDouble(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Value != 0.0)
			{
				num += 9;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Double other)
		{
			if (other != null && other.Value != 0.0)
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
				if (num != 9)
				{
					input.SkipLastField();
				}
				else
				{
					Value = input.ReadDouble();
				}
			}
		}
	}
}
