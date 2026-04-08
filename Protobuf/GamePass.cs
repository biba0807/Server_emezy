using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class GamePass : IMessage<GamePass>, IMessage, IEquatable<GamePass>
    {
        private static readonly MessageParser<GamePass> _parser =
            new MessageParser<GamePass>(() => new GamePass());

        public const int IdFieldNumber = 1;
        public const int CodeFieldNumber = 2;
        public const int KeyItemDefinitionIdFieldNumber = 3;
        public const int LevelsFieldNumber = 4;
        public const int CurrentLevelFieldNumber = 5;

        private string id_ = "";
        private string code_ = "";
        private int keyItemDefinitionId_;
        private readonly RepeatedField<GamePassLevel> levels_ =
            new RepeatedField<GamePassLevel>();
        private int currentLevel_;

        [DebuggerNonUserCode]
        public static MessageParser<GamePass> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor =>
            GameEventMessageReflection.Descriptor.MessageTypes[0];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string Id
        {
            get => id_;
            set => id_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public string Code
        {
            get => code_;
            set => code_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public int KeyItemDefinitionId
        {
            get => keyItemDefinitionId_;
            set => keyItemDefinitionId_ = value;
        }

        [DebuggerNonUserCode]
        public RepeatedField<GamePassLevel> Levels => levels_;

        [DebuggerNonUserCode]
        public int CurrentLevel
        {
            get => currentLevel_;
            set => currentLevel_ = value;
        }

        [DebuggerNonUserCode]
        public GamePass() { }

        [DebuggerNonUserCode]
        public GamePass(GamePass other) : this()
        {
            id_ = other.id_;
            code_ = other.code_;
            keyItemDefinitionId_ = other.keyItemDefinitionId_;
            levels_ = other.levels_.Clone();
            currentLevel_ = other.currentLevel_;
        }

        [DebuggerNonUserCode]
        public GamePass Clone() => new GamePass(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GamePass);

        [DebuggerNonUserCode]
        public bool Equals(GamePass other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;

            return Id == other.Id &&
                   Code == other.Code &&
                   KeyItemDefinitionId == other.KeyItemDefinitionId &&
                   levels_.Equals(other.levels_) &&
                   CurrentLevel == other.CurrentLevel;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (Id.Length != 0) hash ^= Id.GetHashCode();
            if (Code.Length != 0) hash ^= Code.GetHashCode();
            if (KeyItemDefinitionId != 0) hash ^= KeyItemDefinitionId.GetHashCode();
            hash ^= levels_.GetHashCode();
            if (CurrentLevel != 0) hash ^= CurrentLevel.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() =>
            JsonFormatter.ToDiagnosticString(this);

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
            if (KeyItemDefinitionId != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(KeyItemDefinitionId);
            }
            levels_.WriteTo(output, _repeated_levels_codec);
            if (CurrentLevel != 0)
            {
                output.WriteRawTag(40);
                output.WriteInt32(CurrentLevel);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (Id.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Id);
            if (Code.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Code);
            if (KeyItemDefinitionId != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(KeyItemDefinitionId);
            size += levels_.CalculateSize(_repeated_levels_codec);
            if (CurrentLevel != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(CurrentLevel);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GamePass other)
        {
            if (other == null) return;

            Id = other.Id;
            Code = other.Code;
            KeyItemDefinitionId = other.KeyItemDefinitionId;
            levels_.Add(other.levels_);
            CurrentLevel = other.CurrentLevel;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        Id = input.ReadString();
                        break;
                    case 18:
                        Code = input.ReadString();
                        break;
                    case 24:
                        KeyItemDefinitionId = input.ReadInt32();
                        break;
                    case 34:
                        levels_.AddEntriesFrom(input, _repeated_levels_codec);
                        break;
                    case 40:
                        CurrentLevel = input.ReadInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }

        static GamePass()
        {
            _repeated_levels_codec =
                FieldCodec.ForMessage(34, GamePassLevel.Parser);
        }

        private static readonly FieldCodec<GamePassLevel> _repeated_levels_codec;
    }
}