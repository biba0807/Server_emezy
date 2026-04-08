using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanRequest : IMessage<ClanRequest>, IMessage, IEquatable<ClanRequest>, IDeepCloneable<ClanRequest>
	{
		private static readonly MessageParser<ClanRequest> _parser = new MessageParser<ClanRequest>(() => new ClanRequest());

		public const int IdFieldNumber = 1;

		private string id_ = "";

		public const int ClanFieldNumber = 2;

		private Clan clan_;

		public const int RequestSenderFieldNumber = 3;

		private Player requestSender_;

		public const int CreateDateFieldNumber = 4;

		private long createDate_;

		public const int RequestTypeFieldNumber = 5;

		private RequestType requestType_ = RequestType.NoneType;

		public const int InvitedPlayerFieldNumber = 6;

		private Player invitedPlayer_;

		[DebuggerNonUserCode]
		public static MessageParser<ClanRequest> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[4];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

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
		public Clan Clan
		{
			get
			{
				return clan_;
			}
			set
			{
				clan_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Player RequestSender
		{
			get
			{
				return requestSender_;
			}
			set
			{
				requestSender_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long CreateDate
		{
			get
			{
				return createDate_;
			}
			set
			{
				createDate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RequestType RequestType
		{
			get
			{
				return requestType_;
			}
			set
			{
				requestType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Player InvitedPlayer
		{
			get
			{
				return invitedPlayer_;
			}
			set
			{
				invitedPlayer_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ClanRequest()
		{
		}

		[DebuggerNonUserCode]
		public ClanRequest(ClanRequest other)
			: this()
		{
			id_ = other.id_;
			Clan = ((other.clan_ != null) ? other.Clan.Clone() : null);
			RequestSender = ((other.requestSender_ != null) ? other.RequestSender.Clone() : null);
			createDate_ = other.createDate_;
			requestType_ = other.requestType_;
			InvitedPlayer = ((other.invitedPlayer_ != null) ? other.InvitedPlayer.Clone() : null);
		}

		[DebuggerNonUserCode]
		public ClanRequest Clone()
		{
			return new ClanRequest(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanRequest);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanRequest other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (!object.Equals(Clan, other.Clan))
			{
				return false;
			}
			if (!object.Equals(RequestSender, other.RequestSender))
			{
				return false;
			}
			if (CreateDate != other.CreateDate)
			{
				return false;
			}
			if (RequestType != other.RequestType)
			{
				return false;
			}
			if (!object.Equals(InvitedPlayer, other.InvitedPlayer))
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
			if (clan_ != null)
			{
				num ^= Clan.GetHashCode();
			}
			if (requestSender_ != null)
			{
				num ^= RequestSender.GetHashCode();
			}
			if (CreateDate != 0)
			{
				num ^= CreateDate.GetHashCode();
			}
			if (RequestType != 0)
			{
				num ^= RequestType.GetHashCode();
			}
			if (invitedPlayer_ != null)
			{
				num ^= InvitedPlayer.GetHashCode();
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
			if (clan_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Clan);
			}
			if (requestSender_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(RequestSender);
			}
			if (CreateDate != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(CreateDate);
			}
			if (RequestType != 0)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)RequestType);
			}
			if (invitedPlayer_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(InvitedPlayer);
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
			if (clan_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Clan);
			}
			if (requestSender_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(RequestSender);
			}
			if (CreateDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(CreateDate);
			}
			if (RequestType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)RequestType);
			}
			if (invitedPlayer_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InvitedPlayer);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanRequest other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Id.Length != 0)
			{
				Id = other.Id;
			}
			if (other.clan_ != null)
			{
				if (clan_ == null)
				{
					clan_ = new Clan();
				}
				Clan.MergeFrom(other.Clan);
			}
			if (other.requestSender_ != null)
			{
				if (requestSender_ == null)
				{
					requestSender_ = new Player();
				}
				RequestSender.MergeFrom(other.RequestSender);
			}
			if (other.CreateDate != 0)
			{
				CreateDate = other.CreateDate;
			}
			if (other.RequestType != 0)
			{
				RequestType = other.RequestType;
			}
			if (other.invitedPlayer_ != null)
			{
				if (invitedPlayer_ == null)
				{
					invitedPlayer_ = new Player();
				}
				InvitedPlayer.MergeFrom(other.InvitedPlayer);
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
					if (clan_ == null)
					{
						clan_ = new Clan();
					}
					input.ReadMessage(clan_);
					break;
				case 26u:
					if (requestSender_ == null)
					{
						requestSender_ = new Player();
					}
					input.ReadMessage(requestSender_);
					break;
				case 32u:
					CreateDate = input.ReadInt64();
					break;
				case 40u:
					requestType_ = (RequestType)input.ReadEnum();
					break;
				case 50u:
					if (invitedPlayer_ == null)
					{
						invitedPlayer_ = new Player();
					}
					input.ReadMessage(invitedPlayer_);
					break;
				}
			}
		}
	}
}
