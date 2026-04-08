using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class CurrentGameEvent : IMessage<CurrentGameEvent>, IMessage, IEquatable<CurrentGameEvent>, IDeepCloneable<CurrentGameEvent>
    {
        private static readonly MessageParser<CurrentGameEvent> _parser = new MessageParser<CurrentGameEvent>(() => new CurrentGameEvent());

        public const int IdFieldNumber = 1;

        private string id_ = "";

        public const int CodeFieldNumber = 2;

        private string code_ = "";

        public const int DateSinceFieldNumber = 3;

        private long dateSince_;

        public const int DateUntilFieldNumber = 4;

        private long dateUntil_;

        public const int DurationDaysFieldNumber = 5;

        private int durationDays_;

        public const int GamePassesFieldNumber = 6;

        private static readonly FieldCodec<GamePass> _repeated_gamePasses_codec = FieldCodec.ForMessage(50u, GamePass.Parser);

        private readonly RepeatedField<GamePass> gamePasses_ = new RepeatedField<GamePass>();

        public const int PointsFieldNumber = 7;

        private int points_;

        public const int CurrentDayFieldNumber = 8;

        private int currentDay_;

        [DebuggerNonUserCode]
        public static MessageParser<CurrentGameEvent> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => GameEventMessageReflection.Descriptor.MessageTypes[3];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string Id
        {
            get
            {
                return id_;
            }
            set
            {
                id_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public string Code
        {
            get
            {
                return code_;
            }
            set
            {
                code_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public long DateSince
        {
            get
            {
                return dateSince_;
            }
            set
            {
                dateSince_ = value;
            }
        }

        [DebuggerNonUserCode]
        public long DateUntil
        {
            get
            {
                return dateUntil_;
            }
            set
            {
                dateUntil_ = value;
            }
        }

        [DebuggerNonUserCode]
        public int DurationDays
        {
            get
            {
                return durationDays_;
            }
            set
            {
                durationDays_ = value;
            }
        }

        [DebuggerNonUserCode]
        public RepeatedField<GamePass> GamePasses => gamePasses_;

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
        public int CurrentDay
        {
            get
            {
                return currentDay_;
            }
            set
            {
                currentDay_ = value;
            }
        }

        [DebuggerNonUserCode]
        public CurrentGameEvent()
        {
        }

        [DebuggerNonUserCode]
        public CurrentGameEvent(CurrentGameEvent other)
            : this()
        {
            id_ = other.id_;
            code_ = other.code_;
            dateSince_ = other.dateSince_;
            dateUntil_ = other.dateUntil_;
            durationDays_ = other.durationDays_;
            gamePasses_ = other.gamePasses_.Clone();
            points_ = other.points_;
            currentDay_ = other.currentDay_;
        }

        [DebuggerNonUserCode]
        public CurrentGameEvent Clone()
        {
            return new CurrentGameEvent(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as CurrentGameEvent);
        }

        [DebuggerNonUserCode]
        public bool Equals(CurrentGameEvent other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (Id != other.Id)
            {
                return false;
            }
            if (Code != other.Code)
            {
                return false;
            }
            if (DateSince != other.DateSince)
            {
                return false;
            }
            if (DateUntil != other.DateUntil)
            {
                return false;
            }
            if (DurationDays != other.DurationDays)
            {
                return false;
            }
            if (!gamePasses_.Equals(other.gamePasses_))
            {
                return false;
            }
            if (Points != other.Points)
            {
                return false;
            }
            if (CurrentDay != other.CurrentDay)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Id.Length != 0)
            {
                hash ^= Id.GetHashCode();
            }
            if (Code.Length != 0)
            {
                hash ^= Code.GetHashCode();
            }
            if (DateSince != 0L)
            {
                hash ^= DateSince.GetHashCode();
            }
            if (DateUntil != 0L)
            {
                hash ^= DateUntil.GetHashCode();
            }
            if (DurationDays != 0)
            {
                hash ^= DurationDays.GetHashCode();
            }
            hash ^= gamePasses_.GetHashCode();
            if (Points != 0)
            {
                hash ^= Points.GetHashCode();
            }
            if (CurrentDay != 0)
            {
                hash ^= CurrentDay.GetHashCode();
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
            if (Id.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(Id);
            }
            if (Code.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Code);
            }
            if (DateSince != 0L)
            {
                output.WriteRawTag(24);
                output.WriteInt64(DateSince);
            }
            if (DateUntil != 0L)
            {
                output.WriteRawTag(32);
                output.WriteInt64(DateUntil);
            }
            if (DurationDays != 0)
            {
                output.WriteRawTag(40);
                output.WriteInt32(DurationDays);
            }
            gamePasses_.WriteTo(output, _repeated_gamePasses_codec);
            if (Points != 0)
            {
                output.WriteRawTag(56);
                output.WriteInt32(Points);
            }
            if (CurrentDay != 0)
            {
                output.WriteRawTag(64);
                output.WriteInt32(CurrentDay);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (Id.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(Id);
            }
            if (Code.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(Code);
            }
            if (DateSince != 0L)
            {
                size += 1 + CodedOutputStream.ComputeInt64Size(DateSince);
            }
            if (DateUntil != 0L)
            {
                size += 1 + CodedOutputStream.ComputeInt64Size(DateUntil);
            }
            if (DurationDays != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(DurationDays);
            }
            size += gamePasses_.CalculateSize(_repeated_gamePasses_codec);
            if (Points != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(Points);
            }
            if (CurrentDay != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(CurrentDay);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CurrentGameEvent other)
        {
            if (other != null)
            {
                if (other.Id.Length != 0)
                {
                    Id = other.Id;
                }
                if (other.Code.Length != 0)
                {
                    Code = other.Code;
                }
                if (other.DateSince != 0L)
                {
                    DateSince = other.DateSince;
                }
                if (other.DateUntil != 0L)
                {
                    DateUntil = other.DateUntil;
                }
                if (other.DurationDays != 0)
                {
                    DurationDays = other.DurationDays;
                }
                gamePasses_.Add(other.gamePasses_);
                if (other.Points != 0)
                {
                    Points = other.Points;
                }
                if (other.CurrentDay != 0)
                {
                    CurrentDay = other.CurrentDay;
                }
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
                    case 10u:
                        Id = input.ReadString();
                        break;
                    case 18u:
                        Code = input.ReadString();
                        break;
                    case 24u:
                        DateSince = input.ReadInt64();
                        break;
                    case 32u:
                        DateUntil = input.ReadInt64();
                        break;
                    case 40u:
                        DurationDays = input.ReadInt32();
                        break;
                    case 50u:
                        gamePasses_.AddEntriesFrom(input, _repeated_gamePasses_codec);
                        break;
                    case 56u:
                        Points = input.ReadInt32();
                        break;
                    case 64u:
                        CurrentDay = input.ReadInt32();
                        break;
                }
            }
        }
    }
}
