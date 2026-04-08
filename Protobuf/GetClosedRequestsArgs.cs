using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GetClosedRequestsArgs : IMessage<GetClosedRequestsArgs>, IMessage, IEquatable<GetClosedRequestsArgs>, IDeepCloneable<GetClosedRequestsArgs>
	{
		private static readonly MessageParser<GetClosedRequestsArgs> _parser = new MessageParser<GetClosedRequestsArgs>(() => new GetClosedRequestsArgs());

		public const int TypeFieldNumber = 1;

		private MarketRequestType type_ = MarketRequestType.NoneType;

		public const int ReasonFieldNumber = 2;

		private ClosingReason reason_ = ClosingReason.NoneReason;

		public const int PageFieldNumber = 3;

		private int page_;

		public const int SizeFieldNumber = 4;

		private int size_;

		[DebuggerNonUserCode]
		public static MessageParser<GetClosedRequestsArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[4];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public MarketRequestType Type
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
		public ClosingReason Reason
		{
			get
			{
				return reason_;
			}
			set
			{
				reason_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Page
		{
			get
			{
				return page_;
			}
			set
			{
				page_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Size
		{
			get
			{
				return size_;
			}
			set
			{
				size_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GetClosedRequestsArgs()
		{
		}

		[DebuggerNonUserCode]
		public GetClosedRequestsArgs(GetClosedRequestsArgs other)
			: this()
		{
			type_ = other.type_;
			reason_ = other.reason_;
			page_ = other.page_;
			size_ = other.size_;
		}

		[DebuggerNonUserCode]
		public GetClosedRequestsArgs Clone()
		{
			return new GetClosedRequestsArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetClosedRequestsArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetClosedRequestsArgs other)
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
			if (Reason != other.Reason)
			{
				return false;
			}
			if (Page != other.Page)
			{
				return false;
			}
			if (Size != other.Size)
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
			if (Reason != 0)
			{
				num ^= Reason.GetHashCode();
			}
			if (Page != 0)
			{
				num ^= Page.GetHashCode();
			}
			if (Size != 0)
			{
				num ^= Size.GetHashCode();
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
			if (Reason != 0)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)Reason);
			}
			if (Page != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Page);
			}
			if (Size != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(Size);
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
			if (Reason != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Reason);
			}
			if (Page != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Page);
			}
			if (Size != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Size);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GetClosedRequestsArgs other)
		{
			if (other != null)
			{
				if (other.Type != 0)
				{
					Type = other.Type;
				}
				if (other.Reason != 0)
				{
					Reason = other.Reason;
				}
				if (other.Page != 0)
				{
					Page = other.Page;
				}
				if (other.Size != 0)
				{
					Size = other.Size;
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
					type_ = (MarketRequestType)input.ReadEnum();
					break;
				case 16u:
					reason_ = (ClosingReason)input.ReadEnum();
					break;
				case 24u:
					Page = input.ReadInt32();
					break;
				case 32u:
					Size = input.ReadInt32();
					break;
				}
			}
		}
	}
}
