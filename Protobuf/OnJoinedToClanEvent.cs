using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnJoinedToClanEvent : IMessage<OnJoinedToClanEvent>, IMessage, IEquatable<OnJoinedToClanEvent>, IDeepCloneable<OnJoinedToClanEvent>
	{
		private static readonly MessageParser<OnJoinedToClanEvent> _parser = new MessageParser<OnJoinedToClanEvent>(() => new OnJoinedToClanEvent());

		public const int ClanFieldNumber = 1;

		private Clan clan_;

		[DebuggerNonUserCode]
		public static MessageParser<OnJoinedToClanEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[9];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

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
		public OnJoinedToClanEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnJoinedToClanEvent(OnJoinedToClanEvent other)
			: this()
		{
			Clan = ((other.clan_ != null) ? other.Clan.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnJoinedToClanEvent Clone()
		{
			return new OnJoinedToClanEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnJoinedToClanEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnJoinedToClanEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(Clan, other.Clan))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (clan_ != null)
			{
				num ^= Clan.GetHashCode();
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
			if (clan_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(Clan);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (clan_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Clan);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnJoinedToClanEvent other)
		{
			if (other != null && other.clan_ != null)
			{
				if (clan_ == null)
				{
					clan_ = new Clan();
				}
				Clan.MergeFrom(other.Clan);
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
				if (clan_ == null)
				{
					clan_ = new Clan();
				}
				input.ReadMessage(clan_);
			}
		}
	}
}
