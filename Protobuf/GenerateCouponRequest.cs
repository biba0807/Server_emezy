using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class GenerateCouponRequest : IMessage<GenerateCouponRequest>, IEquatable<GenerateCouponRequest>, IDeepCloneable<GenerateCouponRequest>
    {
        private static readonly MessageParser<GenerateCouponRequest> _parser =
            new MessageParser<GenerateCouponRequest>(() => new GenerateCouponRequest());

        public const int StackAmountFieldNumber = 1;
        private static readonly FieldCodec<InventoryItemStackAmount> _repeated_stackAmount_codec =
            FieldCodec.ForMessage(10u, InventoryItemStackAmount.Parser);
        private readonly RepeatedField<InventoryItemStackAmount> stackAmount_ = new RepeatedField<InventoryItemStackAmount>();

        public const int CurrencyAmountFieldNumber = 2;
        /*private static readonly FieldCodec<CurrencyAmount> _repeated_currencyAmount_codec =
            FieldCodec.ForMessage(18u, CurrencyAmount.Parser);*/

        private static readonly FieldCodec<CurrencyAmount> _repeated_currencyAmount_codec; // delete

        private readonly RepeatedField<CurrencyAmount> currencyAmount_ = new RepeatedField<CurrencyAmount>();

        public const int MaxActivationsQuantityFieldNumber = 3;
        private int maxActivationsQuantity_;

        [DebuggerNonUserCode]
        public static MessageParser<GenerateCouponRequest> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor =>
            InventoryMessageReflection.Descriptor.MessageTypes[8];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public RepeatedField<InventoryItemStackAmount> StackAmount => stackAmount_;

        [DebuggerNonUserCode]
        public  RepeatedField<CurrencyAmount> CurrencyAmount => currencyAmount_;

        [DebuggerNonUserCode]
        public int MaxActivationsQuantity
        {
            get => maxActivationsQuantity_;
            set => maxActivationsQuantity_ = value;
        }

        [DebuggerNonUserCode]
        public GenerateCouponRequest() { }

        [DebuggerNonUserCode]
        public GenerateCouponRequest(GenerateCouponRequest other) : this()
        {
            stackAmount_ = other.stackAmount_.Clone();
            currencyAmount_ = other.currencyAmount_.Clone();
            maxActivationsQuantity_ = other.maxActivationsQuantity_;
        }

        [DebuggerNonUserCode]
        public GenerateCouponRequest Clone() => new GenerateCouponRequest(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as GenerateCouponRequest);

        [DebuggerNonUserCode]
        public bool Equals(GenerateCouponRequest other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return stackAmount_.Equals(other.stackAmount_)
                && currencyAmount_.Equals(other.currencyAmount_)
                && maxActivationsQuantity_ == other.maxActivationsQuantity_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= stackAmount_.GetHashCode();
            hash ^= currencyAmount_.GetHashCode();
            if (maxActivationsQuantity_ != 0)
                hash ^= maxActivationsQuantity_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            stackAmount_.WriteTo(output, _repeated_stackAmount_codec);
            currencyAmount_.WriteTo(output, _repeated_currencyAmount_codec);
            if (maxActivationsQuantity_ != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(maxActivationsQuantity_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            size += stackAmount_.CalculateSize(_repeated_stackAmount_codec);
            size += currencyAmount_.CalculateSize(_repeated_currencyAmount_codec);
            if (maxActivationsQuantity_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(maxActivationsQuantity_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(GenerateCouponRequest other)
        {
            if (other == null) return;
            stackAmount_.Add(other.stackAmount_);
            currencyAmount_.Add(other.currencyAmount_);
            if (other.maxActivationsQuantity_ != 0)
                MaxActivationsQuantity = other.MaxActivationsQuantity;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10u:
                        stackAmount_.AddEntriesFrom(input, _repeated_stackAmount_codec);
                        break;
                    case 18u:
                        currencyAmount_.AddEntriesFrom(input, _repeated_currencyAmount_codec);
                        break;
                    case 24u:
                        MaxActivationsQuantity = input.ReadInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
