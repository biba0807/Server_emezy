using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnRequestDeclined : IMessage<OnRequestDeclined>, IMessage, IEquatable<OnRequestDeclined>, IDeepCloneable<OnRequestDeclined>
	{
		private static readonly MessageParser<OnRequestDeclined> _parser = new MessageParser<OnRequestDeclined>(() => new OnRequestDeclined());

		public const int RequestIdFieldNumber = 1;

		private string requestId_ = "";

		public const int RequestTypeFieldNumber = 2;

		private RequestType requestType_ = RequestType.NoneType;

		public const int ClanToJoinFieldNumber = 3;

		private Clan clanToJoin_;

		public const int InvitedPlayerFieldNumber = 4;

		private Player invitedPlayer_;

		[DebuggerNonUserCode]
		public static MessageParser<OnRequestDeclined> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[14];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string RequestId
		{
			get
			{
				return requestId_;
			}
			set
			{
				requestId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public Clan ClanToJoin
		{
			get
			{
				return clanToJoin_;
			}
			set
			{
				clanToJoin_ = value;
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
		public OnRequestDeclined()
		{
		}

		[DebuggerNonUserCode]
		public OnRequestDeclined(OnRequestDeclined other)
			: this()
		{
			requestId_ = other.requestId_;
			requestType_ = other.requestType_;
			ClanToJoin = ((other.clanToJoin_ != null) ? other.ClanToJoin.Clone() : null);
			InvitedPlayer = ((other.invitedPlayer_ != null) ? other.InvitedPlayer.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnRequestDeclined Clone()
		{
			return new OnRequestDeclined(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnRequestDeclined);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnRequestDeclined other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (RequestId != other.RequestId)
			{
				return false;
			}
			if (RequestType != other.RequestType)
			{
				return false;
			}
			if (!object.Equals(ClanToJoin, other.ClanToJoin))
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
			if (RequestId.Length != 0)
			{
				num ^= RequestId.GetHashCode();
			}
			if (RequestType != 0)
			{
				num ^= RequestType.GetHashCode();
			}
			if (clanToJoin_ != null)
			{
				num ^= ClanToJoin.GetHashCode();
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
			if (RequestId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(RequestId);
			}
			if (RequestType != 0)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)RequestType);
			}
			if (clanToJoin_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(ClanToJoin);
			}
			if (invitedPlayer_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(InvitedPlayer);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (RequestId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(RequestId);
			}
			if (RequestType != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)RequestType);
			}
			if (clanToJoin_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(ClanToJoin);
			}
			if (invitedPlayer_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(InvitedPlayer);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnRequestDeclined other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RequestId.Length != 0)
			{
				RequestId = other.RequestId;
			}
			if (other.RequestType != 0)
			{
				RequestType = other.RequestType;
			}
			if (other.clanToJoin_ != null)
			{
				if (clanToJoin_ == null)
				{
					clanToJoin_ = new Clan();
				}
				ClanToJoin.MergeFrom(other.ClanToJoin);
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
					RequestId = input.ReadString();
					break;
				case 16u:
					requestType_ = (RequestType)input.ReadEnum();
					break;
				case 26u:
					if (clanToJoin_ == null)
					{
						clanToJoin_ = new Clan();
					}
					input.ReadMessage(clanToJoin_);
					break;
				case 34u:
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
