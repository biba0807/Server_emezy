using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class EventResponse : IMessage<EventResponse>, IMessage, IEquatable<EventResponse>, IDeepCloneable<EventResponse>
	{
		private static readonly MessageParser<EventResponse> _parser = new MessageParser<EventResponse>(() => new EventResponse());

		public const int ListenerNameFieldNumber = 1;

		private string listenerName_ = string.Empty;

		public const int EventNameFieldNumber = 2;

		private string eventName_ = string.Empty;

		public const int ParamsFieldNumber = 3;

		private static readonly FieldCodec<BinaryValue> _repeated_params_codec = FieldCodec.ForMessage(26u, BinaryValue.Parser);

		private readonly RepeatedField<BinaryValue> params_ = new RepeatedField<BinaryValue>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<EventResponse> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[5];

		[DebuggerNonUserCode]
		public string ListenerName
		{
			get
			{
				return listenerName_;
			}
			set
			{
				listenerName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string EventName
		{
			get
			{
				return eventName_;
			}
			set
			{
				eventName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<BinaryValue> Params => params_;

		[DebuggerNonUserCode]
		public EventResponse()
		{
		}

		[DebuggerNonUserCode]
		public EventResponse(EventResponse other)
			: this()
		{
			listenerName_ = other.listenerName_;
			eventName_ = other.eventName_;
			params_ = other.params_.Clone();
		}

		[DebuggerNonUserCode]
		public EventResponse Clone()
		{
			return new EventResponse(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as EventResponse);
		}

		[DebuggerNonUserCode]
		public bool Equals(EventResponse other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (ListenerName != other.ListenerName)
			{
				return false;
			}
			if (EventName != other.EventName)
			{
				return false;
			}
			if (!params_.Equals(other.params_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (ListenerName.Length != 0)
			{
				num ^= ListenerName.GetHashCode();
			}
			if (EventName.Length != 0)
			{
				num ^= EventName.GetHashCode();
			}
			return num ^ params_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (ListenerName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(ListenerName);
			}
			if (EventName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(EventName);
			}
			params_.WriteTo(output, _repeated_params_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (ListenerName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(ListenerName);
			}
			if (EventName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(EventName);
			}
			return num + params_.CalculateSize(_repeated_params_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(EventResponse other)
		{
			if (other != null)
			{
				if (other.ListenerName.Length != 0)
				{
					ListenerName = other.ListenerName;
				}
				if (other.EventName.Length != 0)
				{
					EventName = other.EventName;
				}
				params_.Add(other.params_);
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
					ListenerName = input.ReadString();
					break;
				case 18u:
					EventName = input.ReadString();
					break;
				case 26u:
					params_.AddEntriesFrom(input, _repeated_params_codec);
					break;
				}
			}
		}
	}
}
