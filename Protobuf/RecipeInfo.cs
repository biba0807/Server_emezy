using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.Collections;
using System;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class RecipeInfo : IMessage<RecipeInfo>, IEquatable<RecipeInfo>
    {
        private static readonly MessageParser<RecipeInfo> _parser = new MessageParser<RecipeInfo>(() => new RecipeInfo());

        private string recipe_;

        private static readonly FieldCodec<ExchangeEntity> _repeated_entities_codec = FieldCodec.ForMessage<ExchangeEntity>(18, ExchangeEntity.Parser);

        private readonly RepeatedField<ExchangeEntity> entities_;

        public static MessageParser<RecipeInfo> Parser => _parser;

        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[11];

        MessageDescriptor IMessage.Descriptor => Descriptor;

        public string Recipe
        {
            get { return recipe_; }
            set { recipe_ = ProtoPreconditions.CheckNotNull(value, "value"); }
        }

        public RepeatedField<ExchangeEntity> Entities => entities_;

        public RecipeInfo()
        {
            recipe_ = "";
            entities_ = new RepeatedField<ExchangeEntity>();
        }

        public RecipeInfo(RecipeInfo other) : this()
        {
            recipe_ = other.recipe_;
            entities_ = other.entities_.Clone();
        }

        public RecipeInfo Clone() => new RecipeInfo(this);

        public override bool Equals(object other)
        {
            return Equals(other as RecipeInfo);
        }

        public bool Equals(RecipeInfo other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            return recipe_ == other.recipe_ && entities_.Equals(other.entities_);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            if (!string.IsNullOrEmpty(recipe_)) hash ^= recipe_.GetHashCode();
            hash ^= entities_.GetHashCode();
            return hash;
        }

        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        public void WriteTo(CodedOutputStream output)
        {
            if (!string.IsNullOrEmpty(recipe_))
            {
                output.WriteRawTag(10);
                output.WriteString(recipe_);
            }
            entities_.WriteTo(output, _repeated_entities_codec);
        }

        public int CalculateSize()
        {
            int size = 0;
            if (!string.IsNullOrEmpty(recipe_)) size += 1 + CodedOutputStream.ComputeStringSize(recipe_);
            size += entities_.CalculateSize(_repeated_entities_codec);
            return size;
        }

        public void MergeFrom(RecipeInfo other)
        {
            if (other == null) return;
            if (!string.IsNullOrEmpty(other.recipe_)) Recipe = other.recipe_;
            entities_.Add(other.entities_);
        }

        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 10:
                        recipe_ = input.ReadString();
                        break;
                    case 18:
                        entities_.AddEntriesFrom(input, _repeated_entities_codec);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
