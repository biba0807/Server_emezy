using System;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class GetCurrentGameEventsResponse : IMessage<GetCurrentGameEventsResponse>, IEquatable<GetCurrentGameEventsResponse>
    {
        private static readonly MessageParser<GetCurrentGameEventsResponse> _parser =
            new MessageParser<GetCurrentGameEventsResponse>(() => new GetCurrentGameEventsResponse());

        private static readonly FieldCodec<CurrentGameEvent> _repeated_gameEvents_codec =
            FieldCodec.ForMessage(10, CurrentGameEvent.Parser);

        private readonly RepeatedField<CurrentGameEvent> gameEvents_ = new RepeatedField<CurrentGameEvent>();

        public static MessageParser<GetCurrentGameEventsResponse> Parser => _parser;

        public static MessageDescriptor Descriptor =>
            GameEventMessageReflection.Descriptor.MessageTypes[3];

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public RepeatedField<CurrentGameEvent> GameEvents => gameEvents_;

        public GetCurrentGameEventsResponse() { }

        public GetCurrentGameEventsResponse(GetCurrentGameEventsResponse other)
        {
            gameEvents_ = other.gameEvents_.Clone();
        }

        public GetCurrentGameEventsResponse Clone()
        {
            return new GetCurrentGameEventsResponse(this);
        }

        public override bool Equals(object other)
        {
            return Equals(other as GetCurrentGameEventsResponse);
        }

        public bool Equals(GetCurrentGameEventsResponse other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return gameEvents_.Equals(other.gameEvents_);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= gameEvents_.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        public void WriteTo(CodedOutputStream output)
        {
            gameEvents_.WriteTo(output, _repeated_gameEvents_codec);
        }

        public int CalculateSize()
        {
            return gameEvents_.CalculateSize(_repeated_gameEvents_codec);
        }

        public void MergeFrom(GetCurrentGameEventsResponse other)
        {
            if (other == null) return;
            gameEvents_.Add(other.gameEvents_);
        }

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                {
                    gameEvents_.AddEntriesFrom(input, _repeated_gameEvents_codec);
                }
                else
                {
                    input.SkipLastField();
                }
            }
        }
    }
}
