using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnTradeUpdatedEvent : IMessage<OnTradeUpdatedEvent>, IMessage, IEquatable<OnTradeUpdatedEvent>, IDeepCloneable<OnTradeUpdatedEvent>
	{
		private static readonly MessageParser<OnTradeUpdatedEvent> _parser = new MessageParser<OnTradeUpdatedEvent>(() => new OnTradeUpdatedEvent());

		public const int TradeFieldNumber = 1;

		private Trade trade_;

		[DebuggerNonUserCode]
		public static MessageParser<OnTradeUpdatedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[19];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public Trade Trade
		{
			get
			{
				return trade_;
			}
			set
			{
				trade_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnTradeUpdatedEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnTradeUpdatedEvent(OnTradeUpdatedEvent other)
			: this()
		{
			Trade = ((other.trade_ != null) ? other.Trade.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnTradeUpdatedEvent Clone()
		{
			return new OnTradeUpdatedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnTradeUpdatedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnTradeUpdatedEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Trade, other.Trade))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (trade_ != null)
			{
				num ^= Trade.GetHashCode();
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
			if (trade_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(Trade);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (trade_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Trade);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnTradeUpdatedEvent other)
		{
			if (other != null && other.trade_ != null)
			{
				if (trade_ == null)
				{
					trade_ = new Trade();
				}
				Trade.MergeFrom(other.Trade);
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
				if (trade_ == null)
				{
					trade_ = new Trade();
				}
				input.ReadMessage(trade_);
			}
		}
	}
}
