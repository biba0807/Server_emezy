using Google.Protobuf;
using Google.Protobuf.Reflection;
using System;
using System.Diagnostics;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ProgressGameEventRequest : IMessage<ProgressGameEventRequest>, IMessage, IEquatable<ProgressGameEventRequest>
    {
        private static readonly MessageParser<ProgressGameEventRequest> _parser = new MessageParser<ProgressGameEventRequest>(() => new ProgressGameEventRequest());

        private string gameEventId_ = "";
        private int points_;

        [DebuggerNonUserCode]
        public static MessageParser<ProgressGameEventRequest> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => GameEventMessageReflection.Descriptor.MessageTypes[4];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string GameEventId
        {
            get => gameEventId_;
            set => gameEventId_ = ProtoPreconditions.CheckNotNull(value, "value");
        }

        [DebuggerNonUserCode]
        public int Points
        {
            get => points_;
            set => points_ = value;
        }

        [DebuggerNonUserCode]
        public ProgressGameEventRequest() { }

        [DebuggerNonUserCode]
        public ProgressGameEventRequest(ProgressGameEventRequest other) : this()
        {
            gameEventId_ = other.gameEventId_;
            points_ = other.points_;
        }

        [DebuggerNonUserCode]
        public ProgressGameEventRequest Clone() => new ProgressGameEventRequest(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ProgressGameEventRequest);

        [DebuggerNonUserCode]
        public bool Equals(ProgressGameEventRequest other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return gameEventId_ == other.gameEventId_
                && points_ == other.points_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (gameEventId_ != "") hash ^= gameEventId_.GetHashCode();
            if (points_ != 0) hash ^= points_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (gameEventId_ != "")
            {
                output.WriteRawTag(10);
                output.WriteString(gameEventId_);
            }
            if (points_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(points_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (gameEventId_ != "") size += 1 + CodedOutputStream.ComputeStringSize(gameEventId_);
            if (points_ != 0) size += 1 + CodedOutputStream.ComputeInt32Size(points_);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ProgressGameEventRequest other)
        {
            if (other == null) return;
            if (other.gameEventId_ != "") GameEventId = other.GameEventId;
            if (other.points_ != 0) Points = other.Points;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            while (input.ReadTag() is uint tag && tag != 0)
            {
                switch (tag)
                {
                    case 10:
                        GameEventId = input.ReadString();
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
