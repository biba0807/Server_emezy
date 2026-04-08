using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnTradeRequestClosedEvent : IMessage<OnTradeRequestClosedEvent>, IMessage, IEquatable<OnTradeRequestClosedEvent>, IDeepCloneable<OnTradeRequestClosedEvent>
	{
		private static readonly MessageParser<OnTradeRequestClosedEvent> _parser = new MessageParser<OnTradeRequestClosedEvent>(() => new OnTradeRequestClosedEvent());

		public const int RequestFieldNumber = 1;

		private ClosedRequest request_;

		[DebuggerNonUserCode]
		public static MessageParser<OnTradeRequestClosedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[18];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClosedRequest Request
		{
			get
			{
				return request_;
			}
			set
			{
				request_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnTradeRequestClosedEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnTradeRequestClosedEvent(OnTradeRequestClosedEvent other)
			: this()
		{
			Request = ((other.request_ != null) ? other.Request.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnTradeRequestClosedEvent Clone()
		{
			return new OnTradeRequestClosedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnTradeRequestClosedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnTradeRequestClosedEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Request, other.Request))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (request_ != null)
			{
				num ^= Request.GetHashCode();
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
			if (request_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(Request);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (request_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Request);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnTradeRequestClosedEvent other)
		{
			if (other != null && other.request_ != null)
			{
				if (request_ == null)
				{
					request_ = new ClosedRequest();
				}
				Request.MergeFrom(other.Request);
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
					continue;
				}
				if (request_ == null)
				{
					request_ = new ClosedRequest();
				}
				input.ReadMessage(request_);
			}
		}
	}
}
