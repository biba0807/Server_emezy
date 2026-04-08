using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnPlayerRequestClosedEvent : IMessage<OnPlayerRequestClosedEvent>, IMessage, IEquatable<OnPlayerRequestClosedEvent>, IDeepCloneable<OnPlayerRequestClosedEvent>
	{
		private static readonly MessageParser<OnPlayerRequestClosedEvent> _parser = new MessageParser<OnPlayerRequestClosedEvent>(() => new OnPlayerRequestClosedEvent());

		public const int RequestFieldNumber = 1;

		private ClosedRequest request_;

		public const int ItemFieldNumber = 2;

		private InventoryItem item_;

		[DebuggerNonUserCode]
		public static MessageParser<OnPlayerRequestClosedEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[16];

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
		public InventoryItem Item
		{
			get
			{
				return item_;
			}
			set
			{
				item_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnPlayerRequestClosedEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnPlayerRequestClosedEvent(OnPlayerRequestClosedEvent other)
			: this()
		{
			Request = ((other.request_ != null) ? other.Request.Clone() : null);
			Item = ((other.item_ != null) ? other.Item.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnPlayerRequestClosedEvent Clone()
		{
			return new OnPlayerRequestClosedEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnPlayerRequestClosedEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnPlayerRequestClosedEvent other)
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
			if (!object.Equals(Item, other.Item))
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
			if (item_ != null)
			{
				num ^= Item.GetHashCode();
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
			if (item_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Item);
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
			if (item_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Item);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnPlayerRequestClosedEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.request_ != null)
			{
				if (request_ == null)
				{
					request_ = new ClosedRequest();
				}
				Request.MergeFrom(other.Request);
			}
			if (other.item_ != null)
			{
				if (item_ == null)
				{
					item_ = new InventoryItem();
				}
				Item.MergeFrom(other.Item);
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
				case 10u:
					if (request_ == null)
					{
						request_ = new ClosedRequest();
					}
					input.ReadMessage(request_);
					break;
				case 18u:
					if (item_ == null)
					{
						item_ = new InventoryItem();
					}
					input.ReadMessage(item_);
					break;
				}
			}
		}
	}
}
