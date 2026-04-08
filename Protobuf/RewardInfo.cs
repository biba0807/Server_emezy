using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class RewardInfo : IMessage<RewardInfo>, IEquatable<RewardInfo>
    {
        private static readonly MessageParser<RewardInfo> _parser = new MessageParser<RewardInfo>(() => new RewardInfo());
        private static readonly FieldCodec<InventoryItemAmount> _repeated_items_codec = FieldCodec.ForMessage(10, InventoryItemAmount.Parser);
        private static readonly FieldCodec<CurrencyAmount> _repeated_currencies_codec = FieldCodec.ForMessage(18, CurrencyAmount.Parser);
        private static readonly FieldCodec<RecipeInfo> _repeated_recipes_codec = FieldCodec.ForMessage(26, RecipeInfo.Parser);

        private readonly RepeatedField<InventoryItemAmount> items_ = new RepeatedField<InventoryItemAmount>();
        private readonly RepeatedField<CurrencyAmount> currencies_ = new RepeatedField<CurrencyAmount>();
        private readonly RepeatedField<RecipeInfo> recipes_ = new RepeatedField<RecipeInfo>();

        public static MessageParser<RewardInfo> Parser => _parser;
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[13];
        MessageDescriptor IMessage.Descriptor => Descriptor;

        public RepeatedField<InventoryItemAmount> Items => items_;
        public RepeatedField<CurrencyAmount> Currencies => currencies_;
        public RepeatedField<RecipeInfo> Recipes => recipes_;

        public RewardInfo()
        {
        }

        public RewardInfo(RewardInfo other)
        {
            items_ = other.items_.Clone();
            currencies_ = other.currencies_.Clone();
            recipes_ = other.recipes_.Clone();
        }

        public RewardInfo Clone()
        {
            return new RewardInfo(this);
        }

        public override bool Equals(object other)
        {
            return Equals(other as RewardInfo);
        }

        public bool Equals(RewardInfo other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return items_.Equals(other.items_) &&
                   currencies_.Equals(other.currencies_) &&
                   recipes_.Equals(other.recipes_);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= items_.GetHashCode();
            hash ^= currencies_.GetHashCode();
            hash ^= recipes_.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        public void WriteTo(CodedOutputStream output)
        {
            items_.WriteTo(output, _repeated_items_codec);
            currencies_.WriteTo(output, _repeated_currencies_codec);
            recipes_.WriteTo(output, _repeated_recipes_codec);
        }

        public int CalculateSize()
        {
            int size = 0;
            size += items_.CalculateSize(_repeated_items_codec);
            size += currencies_.CalculateSize(_repeated_currencies_codec);
            size += recipes_.CalculateSize(_repeated_recipes_codec);
            return size;
        }

        public void MergeFrom(RewardInfo other)
        {
            if (other == null)
                return;

            items_.Add(other.items_);
            currencies_.Add(other.currencies_);
            recipes_.Add(other.recipes_);
        }

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        items_.AddEntriesFrom(input, _repeated_items_codec);
                        break;
                    case 18:
                        currencies_.AddEntriesFrom(input, _repeated_currencies_codec);
                        break;
                    case 26:
                        recipes_.AddEntriesFrom(input, _repeated_recipes_codec);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
