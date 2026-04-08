using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CreatePurchaseBySaleArgs : IMessage<CreatePurchaseBySaleArgs>, IMessage, IEquatable<CreatePurchaseBySaleArgs>, IDeepCloneable<CreatePurchaseBySaleArgs>
	{
		private static readonly MessageParser<CreatePurchaseBySaleArgs> _parser = new MessageParser<CreatePurchaseBySaleArgs>(() => new CreatePurchaseBySaleArgs());

		public const int SaleIdFieldNumber = 1;

		private string saleId_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<CreatePurchaseBySaleArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[8];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string SaleId
		{
			get
			{
				return saleId_;
			}
			set
			{
				saleId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public CreatePurchaseBySaleArgs()
		{
		}

		[DebuggerNonUserCode]
		public CreatePurchaseBySaleArgs(CreatePurchaseBySaleArgs other)
			: this()
		{
			saleId_ = other.saleId_;
		}

		[DebuggerNonUserCode]
		public CreatePurchaseBySaleArgs Clone()
		{
			return new CreatePurchaseBySaleArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CreatePurchaseBySaleArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(CreatePurchaseBySaleArgs other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (SaleId != other.SaleId)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (SaleId.Length != 0)
			{
				num ^= SaleId.GetHashCode();
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
			if (SaleId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(SaleId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (SaleId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(SaleId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CreatePurchaseBySaleArgs other)
		{
			if (other != null && other.SaleId.Length != 0)
			{
				SaleId = other.SaleId;
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
					SaleId = input.ReadString();
				}
			}
		}
	}
}
