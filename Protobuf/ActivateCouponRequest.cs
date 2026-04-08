using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ActivateCouponRequest : IMessage<ActivateCouponRequest>, IMessage, IEquatable<ActivateCouponRequest>, IDeepCloneable<ActivateCouponRequest>
    {
        private static readonly MessageParser<ActivateCouponRequest> _parser = new MessageParser<ActivateCouponRequest>(() => new ActivateCouponRequest());

        public const int CouponIdFieldNumber = 1;

        private string couponId_ = "";

        [DebuggerNonUserCode]
        public static MessageParser<ActivateCouponRequest> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[21];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string CouponId
        {
            get
            {
                return couponId_;
            }
            set
            {
                couponId_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public ActivateCouponRequest()
        {
        }

        [DebuggerNonUserCode]
        public ActivateCouponRequest(ActivateCouponRequest other)
            : this()
        {
            couponId_ = other.couponId_;
        }

        [DebuggerNonUserCode]
        public ActivateCouponRequest Clone()
        {
            return new ActivateCouponRequest(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ActivateCouponRequest);
        }

        [DebuggerNonUserCode]
        public bool Equals(ActivateCouponRequest other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (CouponId != other.CouponId)
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int num = 1;
            if (CouponId.Length != 0)
            {
                num ^= CouponId.GetHashCode();
            }
            return num;
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (CouponId.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(CouponId);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int num = 0;
            if (CouponId.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(CouponId);
            }
            return num;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ActivateCouponRequest other)
        {
            if (other != null && other.CouponId.Length != 0)
            {
                CouponId = other.CouponId;
            }
        }

        [DebuggerNonUserCode]
        public void MergeFrom(CodedInputStream input)
        {
            uint num;
            while ((num = input.ReadTag()) != 0)
            {
                uint num2 = num;
                uint num3 = num2;
                if (num3 != 10)
                {
                    input.SkipLastField();
                }
                else
                {
                    CouponId = input.ReadString();
                }
            }
        }
    }
}
