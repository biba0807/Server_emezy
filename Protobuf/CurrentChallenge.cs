using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class CurrentChallenge : IMessage<CurrentChallenge>, IMessage, IEquatable<CurrentChallenge>
    {
        private static readonly MessageParser<CurrentChallenge> _parser =
            new MessageParser<CurrentChallenge>(() => new CurrentChallenge());

        public const int GameEventChallengeIdFieldNumber = 1;
        public const int CodeFieldNumber = 2;
        public const int KeyItemDefinitionIdFieldNumber = 3;
        public const int LocalizedTitleFieldNumber = 4;
        public const int ActionFieldNumber = 5;
        public const int DayRangeFieldNumber = 6;
        public const int TypeFieldNumber = 7;
        public const int EventPointsFieldNumber = 8;
        public const int TargetPointsFieldNumber = 9;
        public const int CurrentPointsFieldNumber = 10;
        public const int RewardFieldNumber = 11;

        private string gameEventChallengeId_ = "";
        private string code_ = "";
        private int keyItemDefinitionId_;
        private LocalizedTitle localizedTitle_;
        private string action_ = "";
        private DayRange dayRange_;
        private string type_ = "";
        private int eventPoints_;
        private int targetPoints_;
        private int currentPoints_;
        private RewardInfo reward_;

        public static MessageParser<CurrentChallenge> Parser => _parser;

        public static MessageDescriptor Descriptor =>
            ChallengesMessageReflection.Descriptor.MessageTypes[0];

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public string GameEventChallengeId
        {
            get => gameEventChallengeId_;
            set => gameEventChallengeId_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public string Code
        {
            get => code_;
            set => code_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public int KeyItemDefinitionId
        {
            get => keyItemDefinitionId_;
            set => keyItemDefinitionId_ = value;
        }

        public LocalizedTitle LocalizedTitle
        {
            get => localizedTitle_;
            set => localizedTitle_ = value;
        }

        public string Action
        {
            get => action_;
            set => action_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public DayRange DayRange
        {
            get => dayRange_;
            set => dayRange_ = value;
        }

        public string Type
        {
            get => type_;
            set => type_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        public int EventPoints
        {
            get => eventPoints_;
            set => eventPoints_ = value;
        }

        public int TargetPoints
        {
            get => targetPoints_;
            set => targetPoints_ = value;
        }

        public int CurrentPoints
        {
            get => currentPoints_;
            set => currentPoints_ = value;
        }

        public RewardInfo Reward
        {
            get => reward_;
            set => reward_ = value;
        }

        public CurrentChallenge() { }

        public CurrentChallenge(CurrentChallenge other)
        {
            gameEventChallengeId_ = other.gameEventChallengeId_;
            code_ = other.code_;
            keyItemDefinitionId_ = other.keyItemDefinitionId_;
            LocalizedTitle = other.LocalizedTitle?.Clone();
            action_ = other.action_;
            DayRange = other.DayRange?.Clone();
            type_ = other.type_;
            eventPoints_ = other.eventPoints_;
            targetPoints_ = other.targetPoints_;
            currentPoints_ = other.currentPoints_;
            Reward = other.Reward?.Clone();
        }

        public CurrentChallenge Clone() => new CurrentChallenge(this);

        public bool Equals(CurrentChallenge other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return GameEventChallengeId == other.GameEventChallengeId &&
                   Code == other.Code &&
                   KeyItemDefinitionId == other.KeyItemDefinitionId &&
                   Equals(LocalizedTitle, other.LocalizedTitle) &&
                   Action == other.Action &&
                   Equals(DayRange, other.DayRange) &&
                   Type == other.Type &&
                   EventPoints == other.EventPoints &&
                   TargetPoints == other.TargetPoints &&
                   CurrentPoints == other.CurrentPoints &&
                   Equals(Reward, other.Reward);
        }

        public override bool Equals(object obj) => Equals(obj as CurrentChallenge);

        public override int GetHashCode()
        {
            var hash = 1;
            if (GameEventChallengeId.Length != 0)
                hash ^= GameEventChallengeId.GetHashCode();
            if (Code.Length != 0)
                hash ^= Code.GetHashCode();
            if (KeyItemDefinitionId != 0)
                hash ^= KeyItemDefinitionId.GetHashCode();
            if (LocalizedTitle != null)
                hash ^= LocalizedTitle.GetHashCode();
            if (Action.Length != 0)
                hash ^= Action.GetHashCode();
            if (DayRange != null)
                hash ^= DayRange.GetHashCode();
            if (Type.Length != 0)
                hash ^= Type.GetHashCode();
            if (EventPoints != 0)
                hash ^= EventPoints.GetHashCode();
            if (TargetPoints != 0)
                hash ^= TargetPoints.GetHashCode();
            if (CurrentPoints != 0)
                hash ^= CurrentPoints.GetHashCode();
            if (Reward != null)
                hash ^= Reward.GetHashCode();
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
            if (Code.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Code);
            }
            if (KeyItemDefinitionId != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(KeyItemDefinitionId);
            }
            if (LocalizedTitle != null)
            {
                output.WriteRawTag(34);
                output.WriteMessage(LocalizedTitle);
            }
            if (Action.Length != 0)
            {
                output.WriteRawTag(42);
                output.WriteString(Action);
            }
            if (DayRange != null)
            {
                output.WriteRawTag(50);
                output.WriteMessage(DayRange);
            }
            if (Type.Length != 0)
            {
                output.WriteRawTag(58);
                output.WriteString(Type);
            }
            if (EventPoints != 0)
            {
                output.WriteRawTag(64);
                output.WriteInt32(EventPoints);
            }
            if (TargetPoints != 0)
            {
                output.WriteRawTag(72);
                output.WriteInt32(TargetPoints);
            }
            if (CurrentPoints != 0)
            {
                output.WriteRawTag(80);
                output.WriteInt32(CurrentPoints);
            }
            if (Reward != null)
            {
                output.WriteRawTag(90);
                output.WriteMessage(Reward);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (GameEventChallengeId.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(GameEventChallengeId);
            if (Code.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Code);
            if (KeyItemDefinitionId != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(KeyItemDefinitionId);
            if (LocalizedTitle != null)
                size += 1 + CodedOutputStream.ComputeMessageSize(LocalizedTitle);
            if (Action.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Action);
            if (DayRange != null)
                size += 1 + CodedOutputStream.ComputeMessageSize(DayRange);
            if (Type.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Type);
            if (EventPoints != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(EventPoints);
            if (TargetPoints != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(TargetPoints);
            if (CurrentPoints != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(CurrentPoints);
            if (Reward != null)
                size += 1 + CodedOutputStream.ComputeMessageSize(Reward);
            return size;
        }

        public void MergeFrom(CurrentChallenge other)
        {
            if (other == null) return;

            GameEventChallengeId = other.GameEventChallengeId;
            Code = other.Code;
            KeyItemDefinitionId = other.KeyItemDefinitionId;
            if (other.LocalizedTitle != null)
            {
                if (LocalizedTitle == null) LocalizedTitle = new LocalizedTitle();
                LocalizedTitle.MergeFrom(other.LocalizedTitle);
            }
            Action = other.Action;
            if (other.DayRange != null)
            {
                if (DayRange == null) DayRange = new DayRange();
                DayRange.MergeFrom(other.DayRange);
            }
            Type = other.Type;
            EventPoints = other.EventPoints;
            TargetPoints = other.TargetPoints;
            CurrentPoints = other.CurrentPoints;
            if (other.Reward != null)
            {
                if (Reward == null) Reward = new RewardInfo();
                Reward.MergeFrom(other.Reward);
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
                    case 18:
                        Code = input.ReadString();
                        break;
                    case 24:
                        KeyItemDefinitionId = input.ReadInt32();
                        break;
                    case 34:
                        if (LocalizedTitle == null)
                            LocalizedTitle = new LocalizedTitle();
                        input.ReadMessage(LocalizedTitle);
                        break;
                    case 42:
                        Action = input.ReadString();
                        break;
                    case 50:
                        if (DayRange == null)
                            DayRange = new DayRange();
                        input.ReadMessage(DayRange);
                        break;
                    case 58:
                        Type = input.ReadString();
                        break;
                    case 64:
                        EventPoints = input.ReadInt32();
                        break;
                    case 72:
                        TargetPoints = input.ReadInt32();
                        break;
                    case 80:
                        CurrentPoints = input.ReadInt32();
                        break;
                    case 90:
                        if (Reward == null)
                            Reward = new RewardInfo();
                        input.ReadMessage(Reward);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}