using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class SimpleLog : IMessage<SimpleLog>, IMessage, IEquatable<SimpleLog>, IDeepCloneable<SimpleLog>
	{
		private static readonly MessageParser<SimpleLog> _parser = new MessageParser<SimpleLog>(() => new SimpleLog());

		public const int TypeFieldNumber = 1;

		private LogType type_ = LogType.Debug;

		public const int LogFieldNumber = 2;

		private string log_ = "";

		public const int CountFieldNumber = 3;

		private int count_;

		[DebuggerNonUserCode]
		public static MessageParser<SimpleLog> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CrashlogMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public LogType Type
		{
			get
			{
				return type_;
			}
			set
			{
				type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Log
		{
			get
			{
				return log_;
			}
			set
			{
				log_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int Count
		{
			get
			{
				return count_;
			}
			set
			{
				count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public SimpleLog()
		{
		}

		[DebuggerNonUserCode]
		public SimpleLog(SimpleLog other)
			: this()
		{
			type_ = other.type_;
			log_ = other.log_;
			count_ = other.count_;
		}

		[DebuggerNonUserCode]
		public SimpleLog Clone()
		{
			return new SimpleLog(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as SimpleLog);
		}

		[DebuggerNonUserCode]
		public bool Equals(SimpleLog other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Type != other.Type)
			{
				return false;
			}
			if (Log != other.Log)
			{
				return false;
			}
			if (Count != other.Count)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Type != 0)
			{
				num ^= Type.GetHashCode();
			}
			if (Log.Length != 0)
			{
				num ^= Log.GetHashCode();
			}
			if (Count != 0)
			{
				num ^= Count.GetHashCode();
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
			if (Type != 0)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)Type);
			}
			if (Log.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Log);
			}
			if (Count != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Count);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Type);
			}
			if (Log.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Log);
			}
			if (Count != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Count);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(SimpleLog other)
		{
			if (other != null)
			{
				if (other.Type != 0)
				{
					Type = other.Type;
				}
				if (other.Log.Length != 0)
				{
					Log = other.Log;
				}
				if (other.Count != 0)
				{
					Count = other.Count;
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
					type_ = (LogType)input.ReadEnum();
					break;
				case 18u:
					Log = input.ReadString();
					break;
				case 24u:
					Count = input.ReadInt32();
					break;
				}
			}
		}
	}
}
