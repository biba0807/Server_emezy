using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GetClosedRequestsCountArgs : IMessage<GetClosedRequestsCountArgs>, IMessage, IEquatable<GetClosedRequestsCountArgs>, IDeepCloneable<GetClosedRequestsCountArgs>
	{
		private static readonly MessageParser<GetClosedRequestsCountArgs> _parser = new MessageParser<GetClosedRequestsCountArgs>(() => new GetClosedRequestsCountArgs());

		public const int TypeFieldNumber = 1;

		private MarketRequestType type_ = MarketRequestType.NoneType;

		public const int ReasonFieldNumber = 2;

		private ClosingReason reason_ = ClosingReason.NoneReason;

		[DebuggerNonUserCode]
		public static MessageParser<GetClosedRequestsCountArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[5];

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
		public GetClosedRequestsCountArgs()
		{
		}

		[DebuggerNonUserCode]
		public GetClosedRequestsCountArgs(GetClosedRequestsCountArgs other)
			: this()
		{
			type_ = other.type_;
			reason_ = other.reason_;
		}

		[DebuggerNonUserCode]
		public GetClosedRequestsCountArgs Clone()
		{
			return new GetClosedRequestsCountArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetClosedRequestsCountArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetClosedRequestsCountArgs other)
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
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GetClosedRequestsCountArgs other)
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
				}
			}
		}
	}
}
