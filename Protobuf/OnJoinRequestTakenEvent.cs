using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnJoinRequestTakenEvent : IMessage<OnJoinRequestTakenEvent>, IMessage, IEquatable<OnJoinRequestTakenEvent>, IDeepCloneable<OnJoinRequestTakenEvent>
	{
		private static readonly MessageParser<OnJoinRequestTakenEvent> _parser = new MessageParser<OnJoinRequestTakenEvent>(() => new OnJoinRequestTakenEvent());

		public const int RequestIdFieldNumber = 1;

		private string requestId_ = "";

		public const int PlayerFieldNumber = 2;

		private Player player_;

		[DebuggerNonUserCode]
		public static MessageParser<OnJoinRequestTakenEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[8];

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
		public OnJoinRequestTakenEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnJoinRequestTakenEvent(OnJoinRequestTakenEvent other)
			: this()
		{
			requestId_ = other.requestId_;
			Player = ((other.player_ != null) ? other.Player.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnJoinRequestTakenEvent Clone()
		{
			return new OnJoinRequestTakenEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnJoinRequestTakenEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnJoinRequestTakenEvent other)
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
			if (player_ != null)
			{
				output.WriteRawTag(18);
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
			if (player_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Player);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnJoinRequestTakenEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RequestId.Length != 0)
			{
				RequestId = other.RequestId;
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
