using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    [DebuggerNonUserCode]
    public sealed class InventoryItemStackAmount : IMessage<InventoryItemStackAmount>, IEquatable<InventoryItemStackAmount>, IDeepCloneable<InventoryItemStackAmount>
    {
        private static readonly MessageParser<InventoryItemStackAmount> _parser = new MessageParser<InventoryItemStackAmount>(() => new InventoryItemStackAmount());

        public const int InventoryItemStackIdFieldNumber = 1;
        private int inventoryItemStackId_;

        public const int ValueFieldNumber = 2;
        private int value_;

        [DebuggerNonUserCode]
        public static MessageParser<InventoryItemStackAmount> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[6];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int InventoryItemStackId
        {
            get => inventoryItemStackId_;
            set => inventoryItemStackId_ = value;
        }

        [DebuggerNonUserCode]
        public int Value
        {
            get => value_;
            set => value_ = value;
        }

        [DebuggerNonUserCode]
        public InventoryItemStackAmount() { }

        [DebuggerNonUserCode]
        public InventoryItemStackAmount(InventoryItemStackAmount other) : this()
        {
            inventoryItemStackId_ = other.inventoryItemStackId_;
            value_ = other.value_;
        }

        [DebuggerNonUserCode]
        public InventoryItemStackAmount Clone() => new InventoryItemStackAmount(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as InventoryItemStackAmount);

        [DebuggerNonUserCode]
        public bool Equals(InventoryItemStackAmount other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return inventoryItemStackId_ == other.inventoryItemStackId_ && value_ == other.value_;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= inventoryItemStackId_.GetHashCode();
            hash ^= value_.GetHashCode();
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (inventoryItemStackId_ != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(inventoryItemStackId_);
            }
            if (value_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(value_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (inventoryItemStackId_ != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(inventoryItemStackId_);
            if (value_ != 0)
                size += 1 + CodedOutputStream.ComputeInt32Size(value_);
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(InventoryItemStackAmount other)
        {
            if (other == null) return;
            if (other.inventoryItemStackId_ != 0) InventoryItemStackId = other.InventoryItemStackId;
            if (other.value_ != 0) Value = other.Value;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint tag;
            while ((tag = input.ReadTag()) != 0)
            {
                switch (tag)
                {
                    case 8:
                        InventoryItemStackId = input.ReadInt32();
                        break;
                    case 16:
                        Value = input.ReadInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}
