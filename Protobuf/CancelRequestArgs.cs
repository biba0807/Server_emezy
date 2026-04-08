using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CancelRequestArgs : IMessage<CancelRequestArgs>, IMessage, IEquatable<CancelRequestArgs>, IDeepCloneable<CancelRequestArgs>
	{
		private static readonly MessageParser<CancelRequestArgs> _parser = new MessageParser<CancelRequestArgs>(() => new CancelRequestArgs());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<CancelRequestArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[11];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public CancelRequestArgs()
		{
		}

		[DebuggerNonUserCode]
		public CancelRequestArgs(CancelRequestArgs other)
			: this()
		{
			id_ = other.id_;
		}

		[DebuggerNonUserCode]
		public CancelRequestArgs Clone()
		{
			return new CancelRequestArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CancelRequestArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(CancelRequestArgs other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id.Length != 0)
			{
				num ^= Id.GetHashCode();
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
			if (Id.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Id);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CancelRequestArgs other)
		{
			if (other != null && other.Id.Length != 0)
			{
				Id = other.Id;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					Id = input.ReadString();
				}
			}
		}
	}
}
