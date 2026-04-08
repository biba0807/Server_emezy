using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ClanLevel : IMessage<ClanLevel>, IMessage, IEquatable<ClanLevel>, IDeepCloneable<ClanLevel>
	{
		private static readonly MessageParser<ClanLevel> _parser = new MessageParser<ClanLevel>(() => new ClanLevel());

		public const int LevelNumberFieldNumber = 1;

		private int levelNumber_;

		public const int MaxMembersCountFieldNumber = 2;

		private int maxMembersCount_;

		public const int LevelUpCostFieldNumber = 3;

		private CurrencyAmount levelUpCost_;

		public const int PropertiesFieldNumber = 4;

		private static readonly MapField<string, string>.Codec _map_properties_codec = new MapField<string, string>.Codec(FieldCodec.ForString(10u), FieldCodec.ForString(18u), 34u);

		private readonly MapField<string, string> properties_ = new MapField<string, string>();

		[DebuggerNonUserCode]
		public static MessageParser<ClanLevel> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[6];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int LevelNumber
		{
			get
			{
				return levelNumber_;
			}
			set
			{
				levelNumber_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int MaxMembersCount
		{
			get
			{
				return maxMembersCount_;
			}
			set
			{
				maxMembersCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CurrencyAmount LevelUpCost
		{
			get
			{
				return levelUpCost_;
			}
			set
			{
				levelUpCost_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, string> Properties => properties_;

		[DebuggerNonUserCode]
		public ClanLevel()
		{
		}

		[DebuggerNonUserCode]
		public ClanLevel(ClanLevel other)
			: this()
		{
			levelNumber_ = other.levelNumber_;
			maxMembersCount_ = other.maxMembersCount_;
			LevelUpCost = ((other.levelUpCost_ != null) ? other.LevelUpCost.Clone() : null);
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public ClanLevel Clone()
		{
			return new ClanLevel(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ClanLevel);
		}

		[DebuggerNonUserCode]
		public bool Equals(ClanLevel other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (LevelNumber != other.LevelNumber)
			{
				return false;
			}
			if (MaxMembersCount != other.MaxMembersCount)
			{
				return false;
			}
			if (!object.Equals(LevelUpCost, other.LevelUpCost))
			{
				return false;
			}
			if (!Properties.Equals(other.Properties))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (LevelNumber != 0)
			{
				num ^= LevelNumber.GetHashCode();
			}
			if (MaxMembersCount != 0)
			{
				num ^= MaxMembersCount.GetHashCode();
			}
			if (levelUpCost_ != null)
			{
				num ^= LevelUpCost.GetHashCode();
			}
			return num ^ Properties.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (LevelNumber != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(LevelNumber);
			}
			if (MaxMembersCount != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(MaxMembersCount);
			}
			if (levelUpCost_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(LevelUpCost);
			}
			properties_.WriteTo(output, _map_properties_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (LevelNumber != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(LevelNumber);
			}
			if (MaxMembersCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(MaxMembersCount);
			}
			if (levelUpCost_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(LevelUpCost);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ClanLevel other)
		{
			if (other == null)
			{
				return;
			}
			if (other.LevelNumber != 0)
			{
				LevelNumber = other.LevelNumber;
			}
			if (other.MaxMembersCount != 0)
			{
				MaxMembersCount = other.MaxMembersCount;
			}
			if (other.levelUpCost_ != null)
			{
				if (levelUpCost_ == null)
				{
					levelUpCost_ = new CurrencyAmount();
				}
				LevelUpCost.MergeFrom(other.LevelUpCost);
			}
			properties_.Add(other.properties_);
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
					LevelNumber = input.ReadInt32();
					break;
				case 16u:
					MaxMembersCount = input.ReadInt32();
					break;
				case 26u:
					if (levelUpCost_ == null)
					{
						levelUpCost_ = new CurrencyAmount();
					}
					input.ReadMessage(levelUpCost_);
					break;
				case 34u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
