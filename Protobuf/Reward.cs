using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class Reward : IMessage<Reward>, IMessage, IEquatable<Reward>, IDeepCloneable<Reward>
    {
        private static readonly MessageParser<Reward> _parser = new MessageParser<Reward>(() => new Reward());

        public const int ItemsFieldNumber = 1;

        private static readonly FieldCodec<InventoryItem> _repeated_items_codec = FieldCodec.ForMessage(10u, InventoryItem.Parser);

        private readonly RepeatedField<InventoryItem> items_ = new RepeatedField<InventoryItem>();

        public const int CurrenciesFieldNumber = 2;

        private static readonly FieldCodec<CurrencyAmount> _repeated_currencies_codec = FieldCodec.ForMessage(18u, CurrencyAmount.Parser);

        private readonly RepeatedField<CurrencyAmount> currencies_ = new RepeatedField<CurrencyAmount>();

        [DebuggerNonUserCode]
        public static MessageParser<Reward> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[14];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public RepeatedField<InventoryItem> Items => items_;

        [DebuggerNonUserCode]
        public RepeatedField<CurrencyAmount> Currencies => currencies_;

        [DebuggerNonUserCode]
        public Reward()
        {
        }

        [DebuggerNonUserCode]
        public Reward(Reward other)
            : this()
        {
            items_ = other.items_.Clone();
            currencies_ = other.currencies_.Clone();
        }

        [DebuggerNonUserCode]
        public Reward Clone()
        {
            return new Reward(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as Reward);
        }

        [DebuggerNonUserCode]
        public bool Equals(Reward other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (!items_.Equals(other.items_))
            {
                return false;
            }
            if (!currencies_.Equals(other.currencies_))
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            return 1 ^ items_.GetHashCode() ^ currencies_.GetHashCode();
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            items_.WriteTo(output, _repeated_items_codec);
            currencies_.WriteTo(output, _repeated_currencies_codec);
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            return 0 + items_.CalculateSize(_repeated_items_codec) + currencies_.CalculateSize(_repeated_currencies_codec);
        }

        [DebuggerNonUserCode]
        public void MergeFrom(Reward other)
        {
            if (other != null)
            {
                items_.Add(other.items_);
                currencies_.Add(other.currencies_);
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
                    default:
                        input.SkipLastField();
                        break;
                    case 10u:
                        items_.AddEntriesFrom(input, _repeated_items_codec);
                        break;
                    case 18u:
                        currencies_.AddEntriesFrom(input, _repeated_currencies_codec);
                        break;
                }
            }
        }
    }
}
