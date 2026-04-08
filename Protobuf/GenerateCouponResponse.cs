using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class GenerateCouponResponse : IMessage<GenerateCouponResponse>, IEquatable<GenerateCouponResponse>, IDeepCloneable<GenerateCouponResponse>
    {
        private static readonly MessageParser<GenerateCouponResponse> _parser =
            new MessageParser<GenerateCouponResponse>(() => new GenerateCouponResponse());

        public const int CouponIdFieldNumber = 1;
        private string couponId_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<GenerateCouponResponse> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor =>
            InventoryMessageReflection.Descriptor.MessageTypes[12];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string CouponId
        {
            get => couponId_;
            set => couponId_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public GenerateCouponResponse() { }

        [DebuggerNonUserCode]
        public GenerateCouponResponse(GenerateCouponResponse other) : this()
        {
            couponId_ = other.couponId_;
        }

        [DebuggerNonUserCode]
        public GenerateCouponResponse Clone() => new GenerateCouponResponse(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GenerateCouponResponse);

        [DebuggerNonUserCode]
        public bool Equals(GenerateCouponResponse other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return couponId_ == other.couponId_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            if (couponId_.Length != 0) hash ^= couponId_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (couponId_.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(couponId_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (couponId_.Length != 0)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(couponId_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GenerateCouponResponse other)
        {
            if (other == null) return;
            if (other.couponId_.Length != 0) CouponId = other.couponId_;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                if (tag == 10)
                {
                    CouponId = input.ReadString();
                }
                else
                {
                    input.SkipLastField();
                }
            }
        }
    }
}
