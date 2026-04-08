using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public sealed class Exception : IMessage<Exception>, IMessage, IEquatable<Exception>, IDeepCloneable<Exception>
	{
		private static readonly MessageParser<Exception> _parser = new MessageParser<Exception>(() => new Exception());

		public const int IdFieldNumber = 1;

		private long id_;

		public const int CodeFieldNumber = 2;

		private int code_;

		public const int ParamsFieldNumber = 3;

		private static readonly MapField<string, string>.Codec _map_params_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 26u);

		private readonly MapField<string, string> params_ = new MapField<string, string>();

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public static MessageParser<Exception> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => RpcMessageReflection.Descriptor.MessageTypes[4];

		[DebuggerNonUserCode]
		public long Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return code_;
			}
			set
			{
				code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> Params => params_;

		[DebuggerNonUserCode]
		public Exception()
		{
		}

		[DebuggerNonUserCode]
		public Exception(Exception other)
			: this()
		{
			id_ = other.id_;
			code_ = other.code_;
			params_ = other.params_.Clone();
		}

		[DebuggerNonUserCode]
		public Exception Clone()
		{
			return new Exception(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Exception);
		}

		[DebuggerNonUserCode]
		public bool Equals(Exception other)
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
			if (Code != other.Code)
			{
				return false;
			}
			if (!Params.Equals(other.Params))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			if (Code != 0)
			{
				num ^= Code.GetHashCode();
			}
			return num ^ Params.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt64(Id);
			}
			if (Code != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Code);
			}
			params_.WriteTo(output, _map_params_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Id);
			}
			if (Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Code);
			}
			return num + params_.CalculateSize(_map_params_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Exception other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				if (other.Code != 0)
				{
					Code = other.Code;
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
				case 8u:
					Id = input.ReadInt64();
					break;
				case 16u:
					Code = input.ReadInt32();
					break;
				case 26u:
					params_.AddEntriesFrom(input, _map_params_codec);
					break;
				}
			}
		}
	}
}
