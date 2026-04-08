using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class ResponseMessage : IMessage<ResponseMessage>, IMessage, IEquatable<ResponseMessage>, IDeepCloneable<ResponseMessage>
	{
		private static readonly MessageParser<ResponseMessage> _parser = new MessageParser<ResponseMessage>(() => new ResponseMessage());

		public const int RpcResponseFieldNumber = 1;

		private RpcResponse rpcResponse_;

		public const int EventResponseFieldNumber = 2;

		private EventResponse eventResponse_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<ResponseMessage> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		public RpcResponse RpcResponse
		{
			get
			{
				return rpcResponse_;
			}
			set
			{
				rpcResponse_ = value;
			}
		}

		[DebuggerNonUserCode]
		public EventResponse EventResponse
		{
			get
			{
				return eventResponse_;
			}
			set
			{
				eventResponse_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ResponseMessage()
		{
		}

		[DebuggerNonUserCode]
		public ResponseMessage(ResponseMessage other)
			: this()
		{
			RpcResponse = ((other.rpcResponse_ == null) ? null : other.RpcResponse.Clone());
			EventResponse = ((other.eventResponse_ == null) ? null : other.EventResponse.Clone());
		}

		[DebuggerNonUserCode]
		public ResponseMessage Clone()
		{
			return new ResponseMessage(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ResponseMessage);
		}

		[DebuggerNonUserCode]
		public bool Equals(ResponseMessage other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (!object.Equals(RpcResponse, other.RpcResponse))
			{
				return false;
			}
			if (!object.Equals(EventResponse, other.EventResponse))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (rpcResponse_ != null)
			{
				num ^= RpcResponse.GetHashCode();
			}
			if (eventResponse_ != null)
			{
				num ^= EventResponse.GetHashCode();
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
			if (rpcResponse_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(RpcResponse);
			}
			if (eventResponse_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(EventResponse);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (rpcResponse_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(RpcResponse);
			}
			if (eventResponse_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(EventResponse);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ResponseMessage other)
		{
			if (other == null)
			{
				return;
			}
			if (other.rpcResponse_ != null)
			{
				if (rpcResponse_ == null)
				{
					rpcResponse_ = new RpcResponse();
				}
				RpcResponse.MergeFrom(other.RpcResponse);
			}
			if (other.eventResponse_ != null)
			{
				if (eventResponse_ == null)
				{
					eventResponse_ = new EventResponse();
				}
				EventResponse.MergeFrom(other.EventResponse);
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
					if (rpcResponse_ == null)
					{
						rpcResponse_ = new RpcResponse();
					}
					input.ReadMessage(rpcResponse_);
					break;
				case 18u:
					if (eventResponse_ == null)
					{
						eventResponse_ = new EventResponse();
					}
					input.ReadMessage(eventResponse_);
					break;
				}
			}
		}
	}
}
