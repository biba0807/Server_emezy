using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class GamePassLevel : IMessage<GamePassLevel>, IMessage, IEquatable<GamePassLevel>, IDeepCloneable<GamePassLevel>
    {
        private static readonly MessageParser<GamePassLevel> _parser = new MessageParser<GamePassLevel>(() => new GamePassLevel());

        public const int LevelFieldNumber = 1;
        private int level_;

        public const int MinPointsFieldNumber = 2;
        private int minPoints_;

        public const int RewardFieldNumber = 3;
        private RewardInfo reward_;

        [DebuggerNonUserCode]
        public static MessageParser<GamePassLevel> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => GameEventMessageReflection.Descriptor.MessageTypes[1];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int Level
        {
            get => level_;
            set => level_ = value;
        }

        [DebuggerNonUserCode]
        public int MinPoints
        {
            get => minPoints_;
            set => minPoints_ = value;
        }

        [DebuggerNonUserCode]
        public RewardInfo Reward
        {
            get => reward_;
            set => reward_ = value;
        }

        [DebuggerNonUserCode]
        public GamePassLevel() { }

        [DebuggerNonUserCode]
        public GamePassLevel(GamePassLevel other) : this()
        {
            level_ = other.level_;
            minPoints_ = other.minPoints_;
            reward_ = other.reward_ != null ? other.reward_.Clone() : null;
        }

        [DebuggerNonUserCode]
        public GamePassLevel Clone() => new GamePassLevel(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GamePassLevel);

        [DebuggerNonUserCode]
        public bool Equals(GamePassLevel other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return level_ == other.level_
                && minPoints_ == other.minPoints_
                && Equals(reward_, other.reward_);
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (level_ != 0) hash ^= level_.GetHashCode();
            if (minPoints_ != 0) hash ^= minPoints_.GetHashCode();
            if (reward_ != null) hash ^= reward_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (level_ != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(level_);
            }
            if (minPoints_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(minPoints_);
            }
            if (reward_ != null)
            {
                output.WriteRawTag(26);
                output.WriteMessage(reward_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (level_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(level_);
            if (minPoints_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(minPoints_);
            if (reward_ != null) size += 1 + CodedOutputStream.ComputeMessageSize(reward_);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GamePassLevel other)
        {
            if (other == null) return;
            if (other.level_ != 0) Level = other.Level;
            if (other.minPoints_ != 0) MinPoints = other.MinPoints;
            if (other.reward_ != null)
            {
                if (reward_ == null) Reward = new RewardInfo();
                Reward.MergeFrom(other.Reward);
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            while (input.ReadTag() is uint tag && tag != 0)
            {
                switch (tag)
                {
                    case 8:
                        Level = input.ReadInt32();
                        break;
                    case 16:
                        MinPoints = input.ReadInt32();
                        break;
                    case 26:
                        if (reward_ == null) Reward = new RewardInfo();
                        input.ReadMessage(reward_);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
