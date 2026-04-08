using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnInvitedToClanEvent : IMessage<OnInvitedToClanEvent>, IMessage, IEquatable<OnInvitedToClanEvent>, IDeepCloneable<OnInvitedToClanEvent>
	{
		private static readonly MessageParser<OnInvitedToClanEvent> _parser = new MessageParser<OnInvitedToClanEvent>(() => new OnInvitedToClanEvent());

		public const int RequestIdFieldNumber = 1;

		private string requestId_ = "";

		public const int ClanFieldNumber = 2;

		private Clan clan_;

		public const int PlayerFieldNumber = 3;

		private Player player_;

		[DebuggerNonUserCode]
		public static MessageParser<OnInvitedToClanEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[12];

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
		public Player Player
		{
			get
			{
				return player_;
			}
			set
			{
				player_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnInvitedToClanEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnInvitedToClanEvent(OnInvitedToClanEvent other)
			: this()
		{
			requestId_ = other.requestId_;
			Clan = ((other.clan_ != null) ? other.Clan.Clone() : null);
			Player = ((other.player_ != null) ? other.Player.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnInvitedToClanEvent Clone()
		{
			return new OnInvitedToClanEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnInvitedToClanEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnInvitedToClanEvent other)
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
			if (!object.Equals(Clan, other.Clan))
			{
				return false;
			}
			if (!object.Equals(Player, other.Player))
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
			if (clan_ != null)
			{
				num ^= Clan.GetHashCode();
			}
			if (player_ != null)
			{
				num ^= Player.GetHashCode();
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
			if (clan_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Clan);
			}
			if (player_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(Player);
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
			if (clan_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Clan);
			}
			if (player_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Player);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnInvitedToClanEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RequestId.Length != 0)
			{
				RequestId = other.RequestId;
			}
			if (other.clan_ != null)
			{
				if (clan_ == null)
				{
					clan_ = new Clan();
				}
				Clan.MergeFrom(other.Clan);
			}
			if (other.player_ != null)
			{
				if (player_ == null)
				{
					player_ = new Player();
				}
				Player.MergeFrom(other.Player);
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
				case 18u:
					if (clan_ == null)
					{
						clan_ = new Clan();
					}
					input.ReadMessage(clan_);
					break;
				case 26u:
					if (player_ == null)
					{
						player_ = new Player();
					}
					input.ReadMessage(player_);
					break;
				}
			}
		}
	}
}
