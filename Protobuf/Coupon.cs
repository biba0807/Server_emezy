using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class Coupon : IMessage<Coupon>, IEquatable<Coupon>, IDeepCloneable<Coupon>
    {
        private static readonly MessageParser<Coupon> _parser = new MessageParser<Coupon>(() => new Coupon());

        public const int CouponIdFieldNumber = 1;
        private string couponId_ = "";

        public const int MaxActivationsQuantityFieldNumber = 2;
        private int maxActivationsQuantity_;

        public const int RemainingActivationsQuantityFieldNumber = 3;
        private int remainingActivationsQuantity_;

        public const int RewardInfoFieldNumber = 4;
        private RewardInfo rewardInfo_;

        [DebuggerNonUserCode]
        public static MessageParser<Coupon> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[7];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string CouponId
        {
            get => couponId_;
            set => couponId_ = ProtoPreconditions.CheckNotNull(value, nameof(value));
        }

        [DebuggerNonUserCode]
        public int MaxActivationsQuantity
        {
            get => maxActivationsQuantity_;
            set => maxActivationsQuantity_ = value;
        }

        [DebuggerNonUserCode]
        public int RemainingActivationsQuantity
        {
            get => remainingActivationsQuantity_;
            set => remainingActivationsQuantity_ = value;
        }

        [DebuggerNonUserCode]
        public RewardInfo RewardInfo
        {
            get => rewardInfo_;
            set => rewardInfo_ = value;
        }

        [DebuggerNonUserCode]
        public Coupon() { }

        [DebuggerNonUserCode]
        public Coupon(Coupon other) : this()
        {
            couponId_ = other.couponId_;
            maxActivationsQuantity_ = other.maxActivationsQuantity_;
            remainingActivationsQuantity_ = other.remainingActivationsQuantity_;
            rewardInfo_ = other.rewardInfo_ != null ? other.rewardInfo_.Clone() : null;
        }

        [DebuggerNonUserCode]
        public Coupon Clone() => new Coupon(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as Coupon);

        [DebuggerNonUserCode]
        public bool Equals(Coupon other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return couponId_ == other.couponId_ &&
                   maxActivationsQuantity_ == other.maxActivationsQuantity_ &&
                   remainingActivationsQuantity_ == other.remainingActivationsQuantity_ &&
                   Equals(rewardInfo_, other.rewardInfo_);
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= couponId_.GetHashCode();
            hash ^= maxActivationsQuantity_.GetHashCode();
            hash ^= remainingActivationsQuantity_.GetHashCode();
            if (rewardInfo_ != null)
                hash ^= rewardInfo_.GetHashCode();
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
            if (maxActivationsQuantity_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(maxActivationsQuantity_);
            }
            if (remainingActivationsQuantity_ != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(remainingActivationsQuantity_);
            }
            if (rewardInfo_ != null)
            {
                output.WriteRawTag(34);
                output.WriteMessage(rewardInfo_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (couponId_.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(couponId_);
            if (maxActivationsQuantity_ != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(maxActivationsQuantity_);
            if (remainingActivationsQuantity_ != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(remainingActivationsQuantity_);
            if (rewardInfo_ != null)
                size += 1 + CodedOutputStream.ComputeMessageSize(rewardInfo_);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(Coupon other)
        {
            if (other == null) return;
            if (other.couponId_.Length != 0) CouponId = other.CouponId;
            if (other.maxActivationsQuantity_ != 0) MaxActivationsQuantity = other.MaxActivationsQuantity;
            if (other.remainingActivationsQuantity_ != 0) RemainingActivationsQuantity = other.RemainingActivationsQuantity;
            if (other.rewardInfo_ != null)
            {
                if (rewardInfo_ == null)
                {
                    rewardInfo_ = new RewardInfo();
                }
                rewardInfo_.MergeFrom(other.rewardInfo_);
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
                    case 10:
                        CouponId = input.ReadString();
                        break;
                    case 16:
                        MaxActivationsQuantity = input.ReadInt32();
                        break;
                    case 24:
                        RemainingActivationsQuantity = input.ReadInt32();
                        break;
                    case 34:
                        if (rewardInfo_ == null)
                        {
                            rewardInfo_ = new RewardInfo();
                        }
                        input.ReadMessage(rewardInfo_);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}