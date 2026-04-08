using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GetTradeOpenPurchaseRequestsArgs : IMessage<GetTradeOpenPurchaseRequestsArgs>, IMessage, IEquatable<GetTradeOpenPurchaseRequestsArgs>, IDeepCloneable<GetTradeOpenPurchaseRequestsArgs>
	{
		private static readonly MessageParser<GetTradeOpenPurchaseRequestsArgs> _parser = new MessageParser<GetTradeOpenPurchaseRequestsArgs>(() => new GetTradeOpenPurchaseRequestsArgs());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int PageFieldNumber = 2;

		private int page_;

		public const int SizeFieldNumber = 3;

		private int size_;

		[DebuggerNonUserCode]
		public static MessageParser<GetTradeOpenPurchaseRequestsArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[10];

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
		public GetTradeOpenPurchaseRequestsArgs()
		{
		}

		[DebuggerNonUserCode]
		public GetTradeOpenPurchaseRequestsArgs(GetTradeOpenPurchaseRequestsArgs other)
			: this()
		{
			id_ = other.id_;
			page_ = other.page_;
			size_ = other.size_;
		}

		[DebuggerNonUserCode]
		public GetTradeOpenPurchaseRequestsArgs Clone()
		{
			return new GetTradeOpenPurchaseRequestsArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetTradeOpenPurchaseRequestsArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetTradeOpenPurchaseRequestsArgs other)
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
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
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
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
			if (Page != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Page);
			}
			if (Size != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(Size);
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
		public void MergeFrom(GetTradeOpenPurchaseRequestsArgs other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
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
					Id = input.ReadInt32();
					break;
				case 16u:
					Page = input.ReadInt32();
					break;
				case 24u:
					Size = input.ReadInt32();
					break;
				}
			}
		}
	}
}
