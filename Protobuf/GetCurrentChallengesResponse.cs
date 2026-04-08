using System;
using System.Collections.Generic;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class GetCurrentChallengesResponse : IMessage<GetCurrentChallengesResponse>, IEquatable<GetCurrentChallengesResponse>
    {
        private static readonly MessageParser<GetCurrentChallengesResponse> _parser =
            new MessageParser<GetCurrentChallengesResponse>(() => new GetCurrentChallengesResponse());

        private static readonly FieldCodec<CurrentChallenge> _repeated_challenges_codec =
            FieldCodec.ForMessage(10, CurrentChallenge.Parser);

        private readonly RepeatedField<CurrentChallenge> challenges_ = new RepeatedField<CurrentChallenge>();

        public static MessageParser<GetCurrentChallengesResponse> Parser => _parser;

        public static MessageDescriptor Descriptor => ChallengesMessageReflection.Descriptor.MessageTypes[2];

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public RepeatedField<CurrentChallenge> Challenges => challenges_;

        public GetCurrentChallengesResponse() { }

        public GetCurrentChallengesResponse(GetCurrentChallengesResponse other)
        {
            challenges_ = other.challenges_.Clone();
        }

        public GetCurrentChallengesResponse Clone()
        {
            return new GetCurrentChallengesResponse(this);
        }

        public override bool Equals(object other)
        {
            return Equals(other as GetCurrentChallengesResponse);
        }

        public bool Equals(GetCurrentChallengesResponse other)
        {
            return other != null && challenges_.Equals(other.challenges_);
        }

        public override int GetHashCode()
        {
            return challenges_.GetHashCode();
        }

        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        public void WriteTo(CodedOutputStream output)
        {
            challenges_.WriteTo(output, _repeated_challenges_codec);
        }

        public int CalculateSize()
        {
            return challenges_.CalculateSize(_repeated_challenges_codec);
        }

        public void MergeFrom(GetCurrentChallengesResponse other)
        {
            if (other == null) return;
            challenges_.Add(other.challenges_);
        }

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                {
                    challenges_.AddEntriesFrom(input, _repeated_challenges_codec);
                }
                else
                {
                    input.SkipLastField();
                }
            }
        }
    }
}
