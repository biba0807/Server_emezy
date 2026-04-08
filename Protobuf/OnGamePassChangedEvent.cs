using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class OnGamePassChangedEvent : IMessage<OnGamePassChangedEvent>, IMessage, IEquatable<OnGamePassChangedEvent>
    {
        private static readonly MessageParser<OnGamePassChangedEvent> _parser =
            new MessageParser<OnGamePassChangedEvent>(() => new OnGamePassChangedEvent());

        private static readonly MapField<string, int>.Codec _mapLevelsCodec =
            new MapField<string, int>.Codec(FieldCodec.ForString(10), FieldCodec.ForInt32(16), 26);

        public const int EventIdFieldNumber = 1;
        public const int PointsFieldNumber = 2;
        public const int LevelsFieldNumber = 3;
        public const int RewardFieldNumber = 4;

        private string eventId_ = "";
        private int points_;
        private readonly MapField<string, int> levels_ = new MapField<string, int>();
        private Reward reward_;

        public static MessageParser<OnGamePassChangedEvent> Parser => _parser;
        public static MessageDescriptor Descriptor => GameEventMessageReflection.Descriptor.MessageTypes[6];
        MessageDescriptor IMessage.Descriptor => Descriptor;

        public string EventId
        {
            get => eventId_;
            set => eventId_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public int Points
        {
            get => points_;
            set => points_ = value;
        }

        public MapField<string, int> Levels => levels_;

        public Reward Reward
        {
            get => reward_;
            set => reward_ = value;
        }

        public OnGamePassChangedEvent() { }

        public OnGamePassChangedEvent(OnGamePassChangedEvent other) : this()
        {
            eventId_ = other.eventId_;
            points_ = other.points_;
            levels_ = other.levels_.Clone();
            reward_ = other.reward_ != null ? other.reward_.Clone() : null;
        }

        public OnGamePassChangedEvent Clone() => new OnGamePassChangedEvent(this);

        public override bool Equals(object other) => Equals(other as OnGamePassChangedEvent);

        public bool Equals(OnGamePassChangedEvent other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            if (EventId != other.EventId) return false;
            if (Points != other.Points) return false;
            if (!Levels.Equals(other.Levels)) return false;
            if (!Equals(Reward, other.Reward)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (EventId.Length != 0) hash ^= EventId.GetHashCode();
            if (Points != 0) hash ^= Points.GetHashCode();
            hash ^= Levels.GetHashCode();
            if (reward_ != null) hash ^= Reward.GetHashCode();
            return hash;
        }

        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        public void WriteTo(CodedOutputStream output)
        {
            if (EventId.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(EventId);
            }

            if (Points != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(Points);
            }

            levels_.WriteTo(output, _mapLevelsCodec);

            if (reward_ != null)
            {
                output.WriteRawTag(34);
                output.WriteMessage(Reward);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (EventId.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(EventId);
            if (Points != 0) size += 1 + CodedOutputStream.ComputeInt32Size(Points);
            size += levels_.CalculateSize(_mapLevelsCodec);
            if (reward_ != null) size += 1 + CodedOutputStream.ComputeMessageSize(Reward);
            return size;
        }

        public void MergeFrom(OnGamePassChangedEvent other)
        {
            if (other == null) return;
            if (other.EventId.Length != 0) EventId = other.EventId;
            if (other.Points != 0) Points = other.Points;
            levels_.Add(other.levels_);
            if (other.reward_ != null)
            {
                if (reward_ == null) reward_ = new Reward();
                reward_.MergeFrom(other.reward_);
            }
        }

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        EventId = input.ReadString();
                        break;
                    case 16:
                        Points = input.ReadInt32();
                        break;
                    case 26:
                        levels_.AddEntriesFrom(input, _mapLevelsCodec);
                        break;
                    case 34:
                        if (reward_ == null) reward_ = new Reward();
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
