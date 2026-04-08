using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class String : IMessage<String>, IMessage, IEquatable<String>, IDeepCloneable<String>
	{
		private static readonly MessageParser<String> _parser = new MessageParser<String>(() => new String());

		public const int ValueFieldNumber = 1;

		private string value_ = string.Empty;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<String> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[7];

		[DebuggerNonUserCode]
		public string Value
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
		public String()
		{
		}

		[DebuggerNonUserCode]
		public String(String other)
			: this()
		{
			value_ = other.value_;
		}

		[DebuggerNonUserCode]
		public String Clone()
		{
			return new String(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as String);
		}

		[DebuggerNonUserCode]
		public bool Equals(String other)
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
				output.WriteString(Value);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Value.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Value);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(String other)
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
					Value = input.ReadString();
				}
			}
		}
	}
}
