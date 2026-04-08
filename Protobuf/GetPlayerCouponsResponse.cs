using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class GetPlayerCouponsResponse : IMessage<GetPlayerCouponsResponse>, IEquatable<GetPlayerCouponsResponse>, IDeepCloneable<GetPlayerCouponsResponse>
    {
        private static readonly MessageParser<GetPlayerCouponsResponse> _parser =
            new MessageParser<GetPlayerCouponsResponse>(() => new GetPlayerCouponsResponse());

        public const int CouponFieldNumber = 1;
        private static readonly FieldCodec<Coupon> _repeated_coupon_codec =
            FieldCodec.ForMessage(10u, Coupon.Parser);

        private readonly RepeatedField<Coupon> coupon_ = new RepeatedField<Coupon>();

        [DebuggerNonUserCode]
        public static MessageParser<GetPlayerCouponsResponse> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor =>
            InventoryMessageReflection.Descriptor.MessageTypes[11];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public RepeatedField<Coupon> Coupons => coupon_;

        [DebuggerNonUserCode]
        public GetPlayerCouponsResponse() { }

        [DebuggerNonUserCode]
        public GetPlayerCouponsResponse(GetPlayerCouponsResponse other) : this()
        {
            coupon_ = other.coupon_.Clone();
        }

        [DebuggerNonUserCode]
        public GetPlayerCouponsResponse Clone() => new GetPlayerCouponsResponse(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GetPlayerCouponsResponse);

        [DebuggerNonUserCode]
        public bool Equals(GetPlayerCouponsResponse other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return coupon_.Equals(other.coupon_);
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= coupon_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            coupon_.WriteTo(output, _repeated_coupon_codec);
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            size += coupon_.CalculateSize(_repeated_coupon_codec);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GetPlayerCouponsResponse other)
        {
            if (other == null) return;
            coupon_.Add(other.coupon_);
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10u)
                {
                    coupon_.AddEntriesFrom(input, _repeated_coupon_codec);
                }
                else
                {
                    input.SkipLastField();
                }
            }
        }
    }
}
