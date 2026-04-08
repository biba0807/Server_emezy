using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class GetCurrentChallengesRequest : IMessage<GetCurrentChallengesRequest>, IMessage, IEquatable<GetCurrentChallengesRequest>, IDeepCloneable<GetCurrentChallengesRequest>
    {
        private static readonly MessageParser<GetCurrentChallengesRequest> _parser = new MessageParser<GetCurrentChallengesRequest>(() => new GetCurrentChallengesRequest());
        private string gameEventId_ = "";
        private bool completed_;

        public const int GameEventIdFieldNumber = 1;
        public const int CompletedFieldNumber = 2;

        [DebuggerNonUserCode]
        public static MessageParser<GetCurrentChallengesRequest> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ChallengesMessageReflection.Descriptor.MessageTypes[1];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string GameEventId
        {
            get => gameEventId_;
            set => gameEventId_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public bool Completed
        {
            get => completed_;
            set => completed_ = value;
        }

        [DebuggerNonUserCode]
        public GetCurrentChallengesRequest()
        {
        }

        [DebuggerNonUserCode]
        public GetCurrentChallengesRequest(GetCurrentChallengesRequest other) : this()
        {
            gameEventId_ = other.gameEventId_;
            completed_ = other.completed_;
        }

        [DebuggerNonUserCode]
        public GetCurrentChallengesRequest Clone() => new GetCurrentChallengesRequest(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GetCurrentChallengesRequest);

        [DebuggerNonUserCode]
        public bool Equals(GetCurrentChallengesRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return GameEventId == other.GameEventId && Completed == other.Completed;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (GameEventId.Length != 0) hash ^= GameEventId.GetHashCode();
            if (Completed) hash ^= Completed.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (GameEventId.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(GameEventId);
            }
            if (Completed)
            {
                output.WriteRawTag(16);
                output.WriteBool(Completed);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (GameEventId.Length != 0) size += 1 + CodedOutputStream.ComputeStringSize(GameEventId);
            if (Completed) size += 1 + 1;
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GetCurrentChallengesRequest other)
        {
            if (other == null) return;
            if (other.GameEventId.Length != 0) GameEventId = other.GameEventId;
            if (other.Completed) Completed = other.Completed;
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
                        GameEventId = input.ReadString();
                        break;
                    case 16:
                        Completed = input.ReadBool();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
