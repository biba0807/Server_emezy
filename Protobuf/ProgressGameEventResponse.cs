using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ProgressGameEventResponse : IMessage<ProgressGameEventResponse>, IMessage, IEquatable<ProgressGameEventResponse>, IDeepCloneable<ProgressGameEventResponse>
    {
        private static readonly MessageParser<ProgressGameEventResponse> _parser = new MessageParser<ProgressGameEventResponse>(() => new ProgressGameEventResponse());

        public const int PointsFieldNumber = 1;

        private int points_;

        public const int LevelsFieldNumber = 2;

        private static readonly MapField<string, int>.Codec _map_levels_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10u), FieldCodec.ForInt32(16u), 18u);

        private readonly MapField<string, int> levels_ = new MapField<string, int>();

        public const int RewardFieldNumber = 3;

        private Reward reward_;

        [DebuggerNonUserCode]
        public static MessageParser<ProgressGameEventResponse> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => GameEventMessageReflection.Descriptor.MessageTypes[5];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int Points
        {
            get
            {
                return points_;
            }
            set
            {
                points_ = value;
            }
        }

        [DebuggerNonUserCode]
        public MapField<string, int> Levels => levels_;

        [DebuggerNonUserCode]
        public Reward Reward
        {
            get
            {
                return reward_;
            }
            set
            {
                reward_ = value;
            }
        }

        [DebuggerNonUserCode]
        public ProgressGameEventResponse()
        {
        }

        [DebuggerNonUserCode]
        public ProgressGameEventResponse(ProgressGameEventResponse other)
            : this()
        {
            points_ = other.points_;
            levels_ = other.levels_.Clone();
            Reward = ((other.reward_ != null) ? other.Reward.Clone() : null);
        }

        [DebuggerNonUserCode]
        public ProgressGameEventResponse Clone()
        {
            return new ProgressGameEventResponse(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ProgressGameEventResponse);
        }

        [DebuggerNonUserCode]
        public bool Equals(ProgressGameEventResponse other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (Points != other.Points)
            {
                return false;
            }
            if (!Levels.Equals(other.Levels))
            {
                return false;
            }
            if (!object.Equals(Reward, other.Reward))
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Points != 0)
            {
                hash ^= Points.GetHashCode();
            }
            hash ^= Levels.GetHashCode();
            if (reward_ != null)
            {
                hash ^= Reward.GetHashCode();
            }
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (Points != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(Points);
            }
            levels_.WriteTo(output, _map_levels_codec);
            if (reward_ != null)
            {
                output.WriteRawTag(26);
                output.WriteMessage(Reward);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (Points != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(Points);
            }
            size += levels_.CalculateSize(_map_levels_codec);
            if (reward_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(Reward);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ProgressGameEventResponse other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Points != 0)
            {
                Points = other.Points;
            }
            levels_.Add(other.levels_);
            if (other.reward_ != null)
            {
                if (reward_ == null)
                {
                    reward_ = new Reward();
                }
                Reward.MergeFrom(other.Reward);
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    default:
                        input.SkipLastField();
                        break;
                    case 8u:
                        Points = input.ReadInt32();
                        break;
                    case 18u:
                        levels_.AddEntriesFrom(input, _map_levels_codec);
                        break;
                    case 26u:
                        if (reward_ == null)
                        {
                            reward_ = new Reward();
                        }
                        input.ReadMessage(reward_);
                        break;
                }
            }
        }
    }
}
