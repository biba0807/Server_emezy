using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ExchangeEntity : IMessage<ExchangeEntity>, IMessage, IEquatable<ExchangeEntity>, IDeepCloneable<ExchangeEntity>
    {
        private static readonly MessageParser<ExchangeEntity> _parser = new MessageParser<ExchangeEntity>(() => new ExchangeEntity());

        public const int InventoryItemsFieldNumber = 1;

        private static readonly FieldCodec<InventoryItemAmount> _repeated_inventoryItems_codec = FieldCodec.ForMessage(10u, InventoryItemAmount.Parser);

        private readonly RepeatedField<InventoryItemAmount> inventoryItems_ = new RepeatedField<InventoryItemAmount>();

        public const int CurrenciesFieldNumber = 2;

        private static readonly FieldCodec<CurrencyAmount> _repeated_currencies_codec = FieldCodec.ForMessage(18u, CurrencyAmount.Parser);

        private readonly RepeatedField<CurrencyAmount> currencies_ = new RepeatedField<CurrencyAmount>();

        [DebuggerNonUserCode]
        public static MessageParser<ExchangeEntity> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[12];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public RepeatedField<InventoryItemAmount> InventoryItems => inventoryItems_;

        [DebuggerNonUserCode]
        public RepeatedField<CurrencyAmount> Currencies => currencies_;

        [DebuggerNonUserCode]
        public ExchangeEntity()
        {
        }

        [DebuggerNonUserCode]
        public ExchangeEntity(ExchangeEntity other)
            : this()
        {
            inventoryItems_ = other.inventoryItems_.Clone();
            currencies_ = other.currencies_.Clone();
        }

        [DebuggerNonUserCode]
        public ExchangeEntity Clone()
        {
            return new ExchangeEntity(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ExchangeEntity);
        }

        [DebuggerNonUserCode]
        public bool Equals(ExchangeEntity other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (!inventoryItems_.Equals(other.inventoryItems_))
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
            int num = 1;
            num ^= inventoryItems_.GetHashCode();
            return num ^ currencies_.GetHashCode();
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            inventoryItems_.WriteTo(output, _repeated_inventoryItems_codec);
            currencies_.WriteTo(output, _repeated_currencies_codec);
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int num = 0;
            num += inventoryItems_.CalculateSize(_repeated_inventoryItems_codec);
            return num + currencies_.CalculateSize(_repeated_currencies_codec);
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ExchangeEntity other)
        {
            if (other != null)
            {
                inventoryItems_.Add(other.inventoryItems_);
                currencies_.Add(other.currencies_);
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint num;
            while ((num = input.ReadTag()) != 0)
            {
                switch (num)
                {
                    default:
                        input.SkipLastField();
                        break;
                    case 10u:
                        inventoryItems_.AddEntriesFrom(input, _repeated_inventoryItems_codec);
                        break;
                    case 18u:
                        currencies_.AddEntriesFrom(input, _repeated_currencies_codec);
                        break;
                }
            }
        }
    }
}
