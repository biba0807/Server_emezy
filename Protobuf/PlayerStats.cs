using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class PlayerStats : IMessage<PlayerStats>, IMessage, IEquatable<PlayerStats>, IDeepCloneable<PlayerStats>
	{
		private static readonly MessageParser<PlayerStats> _parser = new MessageParser<PlayerStats>(() => new PlayerStats());

		public const int PlayerIdFieldNumber = 1;

		private string playerId_ = "";

		public const int StatsFieldNumber = 2;

		private static readonly FieldCodec<StorePlayerStat> _repeated_stats_codec = FieldCodec.ForMessage(18u, StorePlayerStat.Parser);

		private readonly RepeatedField<StorePlayerStat> stats_ = new RepeatedField<StorePlayerStat>();

		public const int AchievementsFieldNumber = 3;

		private static readonly FieldCodec<StoreAchievement> _repeated_achievements_codec = FieldCodec.ForMessage(26u, StoreAchievement.Parser);

		private readonly RepeatedField<StoreAchievement> achievements_ = new RepeatedField<StoreAchievement>();

		[DebuggerNonUserCode]
		public static MessageParser<PlayerStats> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerStatsMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string PlayerId
		{
			get
			{
				return playerId_;
			}
			set
			{
				playerId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<StorePlayerStat> Stats => stats_;

		[DebuggerNonUserCode]
		public RepeatedField<StoreAchievement> Achievements => achievements_;

		[DebuggerNonUserCode]
		public PlayerStats()
		{
		}

		[DebuggerNonUserCode]
		public PlayerStats(PlayerStats other)
			: this()
		{
			playerId_ = other.playerId_;
			stats_ = other.stats_.Clone();
			achievements_ = other.achievements_.Clone();
		}

		[DebuggerNonUserCode]
		public PlayerStats Clone()
		{
			return new PlayerStats(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as PlayerStats);
		}

		[DebuggerNonUserCode]
		public bool Equals(PlayerStats other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (PlayerId != other.PlayerId)
			{
				return false;
			}
			if (!stats_.Equals(other.stats_))
			{
				return false;
			}
			if (!achievements_.Equals(other.achievements_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (PlayerId.Length != 0)
			{
				num ^= PlayerId.GetHashCode();
			}
			num ^= stats_.GetHashCode();
			return num ^ achievements_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (PlayerId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(PlayerId);
			}
			stats_.WriteTo(output, _repeated_stats_codec);
			achievements_.WriteTo(output, _repeated_achievements_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (PlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerId);
			}
			num += stats_.CalculateSize(_repeated_stats_codec);
			return num + achievements_.CalculateSize(_repeated_achievements_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(PlayerStats other)
		{
			if (other != null)
			{
				if (other.PlayerId.Length != 0)
				{
					PlayerId = other.PlayerId;
				}
				stats_.Add(other.stats_);
				achievements_.Add(other.achievements_);
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
					PlayerId = input.ReadString();
					break;
				case 18u:
					stats_.AddEntriesFrom(input, _repeated_stats_codec);
					break;
				case 26u:
					achievements_.AddEntriesFrom(input, _repeated_achievements_codec);
					break;
				}
			}
		}
	}
}
