using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ProgressChallengeRequest : IMessage<ProgressChallengeRequest>, IEquatable<ProgressChallengeRequest>
    {
        private static readonly MessageParser<ProgressChallengeRequest> _parser = new MessageParser<ProgressChallengeRequest>(() => new ProgressChallengeRequest());

        public const int GameEventChallengeIdFieldNumber = 1;
        private string gameEventChallengeId_ = "";

        public const int PointsFieldNumber = 2;
        private int points_;

        public static MessageParser<ProgressChallengeRequest> Parser => _parser;

        public static MessageDescriptor Descriptor => ChallengesMessageReflection.Descriptor.MessageTypes[3];

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public string GameEventChallengeId
        {
            get => gameEventChallengeId_;
            set => gameEventChallengeId_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public int Points
        {
            get => points_;
            set => points_ = value;
        }

        public ProgressChallengeRequest() { }

        public ProgressChallengeRequest(ProgressChallengeRequest other) : this()
        {
            gameEventChallengeId_ = other.gameEventChallengeId_;
            points_ = other.points_;
        }

        public ProgressChallengeRequest Clone() => new ProgressChallengeRequest(this);

        public override bool Equals(object other) => Equals(other as ProgressChallengeRequest);

        public bool Equals(ProgressChallengeRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return GameEventChallengeId == other.GameEventChallengeId && Points == other.Points;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (GameEventChallengeId.Length != 0) hash ^= GameEventChallengeId.GetHashCode();
            if (Points != 0) hash ^= Points.GetHashCode();
            return hash;
        }

        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        public void WriteTo(CodedOutputStream output)
        {
            if (GameEventChallengeId.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(GameEventChallengeId);
            }

            if (Points != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(Points);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (GameEventChallengeId.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(GameEventChallengeId);
            }
            if (Points != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(Points);
            }
            return size;
        }

        public void MergeFrom(ProgressChallengeRequest other)
        {
            if (other == null) return;
            if (other.GameEventChallengeId.Length != 0)
            {
                GameEventChallengeId = other.GameEventChallengeId;
            }
            if (other.Points != 0)
            {
                Points = other.Points;
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
                        GameEventChallengeId = input.ReadString();
                        break;
                    case 16:
                        Points = input.ReadInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
