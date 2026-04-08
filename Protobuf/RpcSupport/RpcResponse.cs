using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class RpcResponse : IMessage<RpcResponse>, IMessage, IEquatable<RpcResponse>, IDeepCloneable<RpcResponse>
	{
		private static readonly MessageParser<RpcResponse> _parser = new MessageParser<RpcResponse>(() => new RpcResponse());

		public const int IdFieldNumber = 1;

		private string id_ = string.Empty;

		public const int ExceptionFieldNumber = 2;

		private Exception exception_;

		public const int ReturnFieldNumber = 3;

		private BinaryValue return_;

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<RpcResponse> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[3];

		[DebuggerNonUserCode]
		public string Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Exception Exception
		{
			get
			{
				return exception_;
			}
			set
			{
				exception_ = value;
			}
		}

		[DebuggerNonUserCode]
		public BinaryValue Return
		{
			get
			{
				return return_;
			}
			set
			{
				return_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RpcResponse()
		{
		}

		[DebuggerNonUserCode]
		public RpcResponse(RpcResponse other)
			: this()
		{
			id_ = other.id_;
			Exception = ((other.exception_ == null) ? null : other.Exception.Clone());
			Return = ((other.return_ == null) ? null : other.Return.Clone());
		}

		[DebuggerNonUserCode]
		public RpcResponse Clone()
		{
			return new RpcResponse(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as RpcResponse);
		}

		[DebuggerNonUserCode]
		public bool Equals(RpcResponse other)
		{
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(other, this))
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (!object.Equals(Exception, other.Exception))
			{
				return false;
			}
			if (!object.Equals(Return, other.Return))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id.Length != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (exception_ != null)
			{
				num ^= Exception.GetHashCode();
			}
			if (return_ != null)
			{
				num ^= Return.GetHashCode();
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
			if (Id.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Id);
			}
			if (exception_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Exception);
			}
			if (return_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(Return);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			if (exception_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Exception);
			}
			if (return_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Return);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(RpcResponse other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.exception_ != null)
			{
				if (exception_ == null)
				{
					exception_ = new Exception();
				}
				Exception.MergeFrom(other.Exception);
			}
			if (other.return_ != null)
			{
				if (return_ == null)
				{
					return_ = new BinaryValue();
				}
				Return.MergeFrom(other.Return);
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
					Id = input.ReadString();
					break;
				case 18u:
					if (exception_ == null)
					{
						exception_ = new Exception();
					}
					input.ReadMessage(exception_);
					break;
				case 26u:
					if (return_ == null)
					{
						return_ = new BinaryValue();
					}
					input.ReadMessage(return_);
					break;
				}
			}
		}
	}
}
