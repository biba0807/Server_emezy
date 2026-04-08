using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanClosedRequest : IMessage<ClanClosedRequest>, IMessage, IEquatable<ClanClosedRequest>, IDeepCloneable<ClanClosedRequest>
	{
		private static readonly MessageParser<ClanClosedRequest> _parser = new MessageParser<ClanClosedRequest>(() => new ClanClosedRequest());

		public const int ClanRequestFieldNumber = 1;

		private ClanRequest clanRequest_;

		public const int CloseDateFieldNumber = 2;

		private long closeDate_;

		public const int ClosingReasonFieldNumber = 3;

		private ClanClosingReason closingReason_ = ClanClosingReason.AcceptRequest;

		[DebuggerNonUserCode]
		public static MessageParser<ClanClosedRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[3];

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
		public long CloseDate
		{
			get
			{
				return closeDate_;
			}
			set
			{
				closeDate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanClosingReason ClosingReason
		{
			get
			{
				return closingReason_;
			}
			set
			{
				closingReason_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanClosedRequest()
		{
		}

		[DebuggerNonUserCode]
		public ClanClosedRequest(ClanClosedRequest other)
			: this()
		{
			ClanRequest = ((other.clanRequest_ != null) ? other.ClanRequest.Clone() : null);
			closeDate_ = other.closeDate_;
			closingReason_ = other.closingReason_;
		}

		[DebuggerNonUserCode]
		public ClanClosedRequest Clone()
		{
			return new ClanClosedRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanClosedRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanClosedRequest other)
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
			if (CloseDate != other.CloseDate)
			{
				return false;
			}
			if (ClosingReason != other.ClosingReason)
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
			if (CloseDate != 0)
			{
				num ^= CloseDate.GetHashCode();
			}
			if (ClosingReason != 0)
			{
				num ^= ClosingReason.GetHashCode();
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
			if (CloseDate != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt64(CloseDate);
			}
			if (ClosingReason != 0)
			{
				output.WriteRawTag(24);
				output.WriteEnum((int)ClosingReason);
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
			if (CloseDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(CloseDate);
			}
			if (ClosingReason != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)ClosingReason);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanClosedRequest other)
		{
			if (other == null)
			{
				return;
			}
			if (other.clanRequest_ != null)
			{
				if (clanRequest_ == null)
				{
					clanRequest_ = new ClanRequest();
				}
				ClanRequest.MergeFrom(other.ClanRequest);
			}
			if (other.CloseDate != 0)
			{
				CloseDate = other.CloseDate;
			}
			if (other.ClosingReason != 0)
			{
				ClosingReason = other.ClosingReason;
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
					if (clanRequest_ == null)
					{
						clanRequest_ = new ClanRequest();
					}
					input.ReadMessage(clanRequest_);
					break;
				case 16u:
					CloseDate = input.ReadInt64();
					break;
				case 24u:
					closingReason_ = (ClanClosingReason)input.ReadEnum();
					break;
				}
			}
		}
	}
}
