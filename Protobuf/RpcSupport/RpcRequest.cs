using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class RpcRequest : IMessage<RpcRequest>, IMessage, IEquatable<RpcRequest>, IDeepCloneable<RpcRequest>
	{
		private static readonly MessageParser<RpcRequest> _parser = new MessageParser<RpcRequest>(() => new RpcRequest());

		public const int IdFieldNumber = 1;

		private string id_ = string.Empty;

		public const int ServiceNameFieldNumber = 2;

		private string serviceName_ = string.Empty;

		public const int MethodNameFieldNumber = 3;

		private string methodName_ = string.Empty;

		public const int ParamsFieldNumber = 4;

		private static readonly FieldCodec<BinaryValue> _repeated_params_codec = FieldCodec.ForMessage(34u, BinaryValue.Parser);

		private readonly RepeatedField<BinaryValue> params_ = new RepeatedField<BinaryValue>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<RpcRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[0];

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
		public string ServiceName
		{
			get
			{
				return serviceName_;
			}
			set
			{
				serviceName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string MethodName
		{
			get
			{
				return methodName_;
			}
			set
			{
				methodName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<BinaryValue> Params => params_;

		[DebuggerNonUserCode]
		public RpcRequest()
		{
		}

		[DebuggerNonUserCode]
		public RpcRequest(RpcRequest other)
			: this()
		{
			id_ = other.id_;
			serviceName_ = other.serviceName_;
			methodName_ = other.methodName_;
			params_ = other.params_.Clone();
		}

		[DebuggerNonUserCode]
		public RpcRequest Clone()
		{
			return new RpcRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as RpcRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(RpcRequest other)
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
			if (ServiceName != other.ServiceName)
			{
				return false;
			}
			if (MethodName != other.MethodName)
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
			if (Id.Length != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (ServiceName.Length != 0)
			{
				num ^= ServiceName.GetHashCode();
			}
			if (MethodName.Length != 0)
			{
				num ^= MethodName.GetHashCode();
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
			if (Id.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Id);
			}
			if (ServiceName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(ServiceName);
			}
			if (MethodName.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(MethodName);
			}
			params_.WriteTo(output, _repeated_params_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Id);
			}
			if (ServiceName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(ServiceName);
			}
			if (MethodName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(MethodName);
			}
			return num + params_.CalculateSize(_repeated_params_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(RpcRequest other)
		{
			if (other != null)
			{
				if (other.Id.Length != 0)
				{
					Id = other.Id;
				}
				if (other.ServiceName.Length != 0)
				{
					ServiceName = other.ServiceName;
				}
				if (other.MethodName.Length != 0)
				{
					MethodName = other.MethodName;
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
					Id = input.ReadString();
					break;
				case 18u:
					ServiceName = input.ReadString();
					break;
				case 26u:
					MethodName = input.ReadString();
					break;
				case 34u:
					params_.AddEntriesFrom(input, _repeated_params_codec);
					break;
				}
			}
		}
	}
}
