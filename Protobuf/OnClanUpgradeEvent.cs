using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnClanUpgradeEvent : IMessage<OnClanUpgradeEvent>, IMessage, IEquatable<OnClanUpgradeEvent>, IDeepCloneable<OnClanUpgradeEvent>
	{
		private static readonly MessageParser<OnClanUpgradeEvent> _parser = new MessageParser<OnClanUpgradeEvent>(() => new OnClanUpgradeEvent());

		public const int ClanLevelFieldNumber = 1;

		private ClanLevel clanLevel_;

		[DebuggerNonUserCode]
		public static MessageParser<OnClanUpgradeEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[20];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClanLevel ClanLevel
		{
			get
			{
				return clanLevel_;
			}
			set
			{
				clanLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnClanUpgradeEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnClanUpgradeEvent(OnClanUpgradeEvent other)
			: this()
		{
			ClanLevel = ((other.clanLevel_ != null) ? other.ClanLevel.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnClanUpgradeEvent Clone()
		{
			return new OnClanUpgradeEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnClanUpgradeEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnClanUpgradeEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(ClanLevel, other.ClanLevel))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (clanLevel_ != null)
			{
				num ^= ClanLevel.GetHashCode();
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
			if (clanLevel_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(ClanLevel);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (clanLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(ClanLevel);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnClanUpgradeEvent other)
		{
			if (other != null && other.clanLevel_ != null)
			{
				if (clanLevel_ == null)
				{
					clanLevel_ = new ClanLevel();
				}
				ClanLevel.MergeFrom(other.ClanLevel);
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
				if (clanLevel_ == null)
				{
					clanLevel_ = new ClanLevel();
				}
				input.ReadMessage(clanLevel_);
			}
		}
	}
}
