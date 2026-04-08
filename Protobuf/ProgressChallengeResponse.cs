using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ProgressChallengeResponse : IMessage<ProgressChallengeResponse>, IMessage, IEquatable<ProgressChallengeResponse>, IDeepCloneable<ProgressChallengeResponse>
    {
        private static readonly MessageParser<ProgressChallengeResponse> _parser = new MessageParser<ProgressChallengeResponse>(() => new ProgressChallengeResponse());

        private bool completed_;
        private int challengePoints_;
        private Reward challengeReward_;
        private int eventPoints_;
        private static readonly MapField<string, int>.Codec _mapEventGamePassLevelsCodec = new MapField<string, int>.Codec(FieldCodec.ForString(10), FieldCodec.ForInt32(16), 42);
        private readonly MapField<string, int> eventGamePassLevels_ = new MapField<string, int>();
        private Reward eventReward_;

        [DebuggerNonUserCode]
        public static MessageParser<ProgressChallengeResponse> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ChallengesMessageReflection.Descriptor.MessageTypes[4];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public bool Completed
        {
            get => completed_;
            set => completed_ = value;
        }

        [DebuggerNonUserCode]
        public int ChallengePoints
        {
            get => challengePoints_;
            set => challengePoints_ = value;
        }

        [DebuggerNonUserCode]
        public Reward ChallengeReward
        {
            get => challengeReward_;
            set => challengeReward_ = value;
        }

        [DebuggerNonUserCode]
        public int EventPoints
        {
            get => eventPoints_;
            set => eventPoints_ = value;
        }

        [DebuggerNonUserCode]
        public MapField<string, int> EventGamePassLevels => eventGamePassLevels_;

        [DebuggerNonUserCode]
        public Reward EventReward
        {
            get => eventReward_;
            set => eventReward_ = value;
        }

        [DebuggerNonUserCode]
        public ProgressChallengeResponse() { }

        [DebuggerNonUserCode]
        public ProgressChallengeResponse(ProgressChallengeResponse other) : this()
        {
            completed_ = other.completed_;
            challengePoints_ = other.challengePoints_;
            challengeReward_ = other.challengeReward_ != null ? other.challengeReward_.Clone() : null;
            eventPoints_ = other.eventPoints_;
            eventGamePassLevels_ = other.eventGamePassLevels_.Clone();
            eventReward_ = other.eventReward_ != null ? other.eventReward_.Clone() : null;
        }

        [DebuggerNonUserCode]
        public ProgressChallengeResponse Clone() => new ProgressChallengeResponse(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ProgressChallengeResponse);

        [DebuggerNonUserCode]
        public bool Equals(ProgressChallengeResponse other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return completed_ == other.completed_
                && challengePoints_ == other.challengePoints_
                && Equals(challengeReward_, other.challengeReward_)
                && eventPoints_ == other.eventPoints_
                && eventGamePassLevels_.Equals(other.eventGamePassLevels_)
                && Equals(eventReward_, other.eventReward_);
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (completed_) hash ^= Completed.GetHashCode();
            if (challengePoints_ != 0) hash ^= ChallengePoints.GetHashCode();
            if (challengeReward_ != null) hash ^= ChallengeReward.GetHashCode();
            if (eventPoints_ != 0) hash ^= EventPoints.GetHashCode();
            hash ^= EventGamePassLevels.GetHashCode();
            if (eventReward_ != null) hash ^= EventReward.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (completed_)
            {
                output.WriteRawTag(8);
                output.WriteBool(Completed);
            }
            if (challengePoints_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(ChallengePoints);
            }
            if (challengeReward_ != null)
            {
                output.WriteRawTag(26);
                output.WriteMessage(ChallengeReward);
            }
            if (eventPoints_ != 0)
            {
                output.WriteRawTag(32);
                output.WriteInt32(EventPoints);
            }
            eventGamePassLevels_.WriteTo(output, _mapEventGamePassLevelsCodec);
            if (eventReward_ != null)
            {
                output.WriteRawTag(50);
                output.WriteMessage(EventReward);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (completed_) size += 1 + CodedOutputStream.ComputeBoolSize(Completed);
            if (challengePoints_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(ChallengePoints);
            if (challengeReward_ != null) size += 1 + CodedOutputStream.ComputeMessageSize(ChallengeReward);
            if (eventPoints_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(EventPoints);
            size += eventGamePassLevels_.CalculateSize(_mapEventGamePassLevelsCodec);
            if (eventReward_ != null) size += 1 + CodedOutputStream.ComputeMessageSize(EventReward);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ProgressChallengeResponse other)
        {
            if (other == null) return;
            if (other.Completed) Completed = other.Completed;
            if (other.ChallengePoints != 0) ChallengePoints = other.ChallengePoints;
            if (other.ChallengeReward != null)
            {
                if (ChallengeReward == null) ChallengeReward = new Reward();
                ChallengeReward.MergeFrom(other.ChallengeReward);
            }
            if (other.EventPoints != 0) EventPoints = other.EventPoints;
            eventGamePassLevels_.Add(other.eventGamePassLevels_);
            if (other.EventReward != null)
            {
                if (EventReward == null) EventReward = new Reward();
                EventReward.MergeFrom(other.EventReward);
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
                    case 8:
                        Completed = input.ReadBool();
                        break;
                    case 16:
                        ChallengePoints = input.ReadInt32();
                        break;
                    case 26:
                        if (ChallengeReward == null) ChallengeReward = new Reward();
                        input.ReadMessage(ChallengeReward);
                        break;
                    case 32:
                        EventPoints = input.ReadInt32();
                        break;
                    case 42:
                        eventGamePassLevels_.AddEntriesFrom(input, _mapEventGamePassLevelsCodec);
                        break;
                    case 50:
                        if (EventReward == null) EventReward = new Reward();
                        input.ReadMessage(EventReward);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
