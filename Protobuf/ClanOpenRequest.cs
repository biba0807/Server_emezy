using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanOpenRequest : IMessage<ClanOpenRequest>, IMessage, IEquatable<ClanOpenRequest>, IDeepCloneable<ClanOpenRequest>
	{
		private static readonly MessageParser<ClanOpenRequest> _parser = new MessageParser<ClanOpenRequest>(() => new ClanOpenRequest());

		public const int ClanRequestFieldNumber = 1;

		private ClanRequest clanRequest_;

		[DebuggerNonUserCode]
		public static MessageParser<ClanOpenRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClanRequest ClanRequest
		{
			get
			{
				return clanRequest_;
			}
			set
			{
				clanRequest_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanOpenRequest()
		{
		}

		[DebuggerNonUserCode]
		public ClanOpenRequest(ClanOpenRequest other)
			: this()
		{
			ClanRequest = ((other.clanRequest_ != null) ? other.ClanRequest.Clone() : null);
		}

		[DebuggerNonUserCode]
		public ClanOpenRequest Clone()
		{
			return new ClanOpenRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanOpenRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanOpenRequest other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(ClanRequest, other.ClanRequest))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (clanRequest_ != null)
			{
				num ^= ClanRequest.GetHashCode();
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
			if (clanRequest_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(ClanRequest);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (clanRequest_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(ClanRequest);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanOpenRequest other)
		{
			if (other != null && other.clanRequest_ != null)
			{
				if (clanRequest_ == null)
				{
					clanRequest_ = new ClanRequest();
				}
				ClanRequest.MergeFrom(other.ClanRequest);
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
				if (clanRequest_ == null)
				{
					clanRequest_ = new ClanRequest();
				}
				input.ReadMessage(clanRequest_);
			}
		}
	}
}
