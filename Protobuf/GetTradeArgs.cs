using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GetTradeArgs : IMessage<GetTradeArgs>, IMessage, IEquatable<GetTradeArgs>, IDeepCloneable<GetTradeArgs>
	{
		private static readonly MessageParser<GetTradeArgs> _parser = new MessageParser<GetTradeArgs>(() => new GetTradeArgs());

		public const int IdFieldNumber = 1;

		private int id_;

		[DebuggerNonUserCode]
		public static MessageParser<GetTradeArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[12];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GetTradeArgs()
		{
		}

		[DebuggerNonUserCode]
		public GetTradeArgs(GetTradeArgs other)
			: this()
		{
			id_ = other.id_;
		}

		[DebuggerNonUserCode]
		public GetTradeArgs Clone()
		{
			return new GetTradeArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetTradeArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetTradeArgs other)
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
			if (Id != 0)
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
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GetTradeArgs other)
		{
			if (other != null && other.Id != 0)
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
				if (num2 != 8)
				{
					input.SkipLastField();
				}
				else
				{
					Id = input.ReadInt32();
				}
			}
		}
	}
}
