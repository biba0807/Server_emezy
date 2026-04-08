using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Page : IMessage<Page>, IMessage, IEquatable<Page>, IDeepCloneable<Page>
	{
		private static readonly MessageParser<Page> _parser = new MessageParser<Page>(() => new Page());

		public const int Page_FieldNumber = 1;

		private int page_;

		public const int SizeFieldNumber = 2;

		private int size_;

		[DebuggerNonUserCode]
		public static MessageParser<Page> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CommonMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Page_
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
		public Page()
		{
		}

		[DebuggerNonUserCode]
		public Page(Page other)
			: this()
		{
			page_ = other.page_;
			size_ = other.size_;
		}

		[DebuggerNonUserCode]
		public Page Clone()
		{
			return new Page(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Page);
		}

		[DebuggerNonUserCode]
		public bool Equals(Page other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Page_ != other.Page_)
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
			if (Page_ != 0)
			{
				num ^= Page_.GetHashCode();
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
			if (Page_ != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Page_);
			}
			if (Size != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Size);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Page_ != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Page_);
			}
			if (Size != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Size);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Page other)
		{
			if (other != null)
			{
				if (other.Page_ != 0)
				{
					Page_ = other.Page_;
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
					Page_ = input.ReadInt32();
					break;
				case 16u:
					Size = input.ReadInt32();
					break;
				}
			}
		}
	}
}
