using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanSettings : IMessage<ClanSettings>, IMessage, IEquatable<ClanSettings>, IDeepCloneable<ClanSettings>
    {
        private static readonly MessageParser<ClanSettings> _parser = new MessageParser<ClanSettings>(() => new ClanSettings());

        public const int InitialMembersCountFieldNumber = 1;
        private int initialMembersCount_;

        public const int MembersCountLimitFieldNumber = 2;
        private int membersCountLimit_;

        public const int MemberCountUpgradeCostFieldNumber = 3;
        private CurrencyAmount memberCountUpgradeCost_;

        public const int ChangeClanNameOrTagCostFieldNumber = 4;
        private CurrencyAmount changeClanNameOrTagCost_;

        public const int ClanCreateCostFieldNumber = 5;
        private CurrencyAmount clanCreateCost_;

        [DebuggerNonUserCode]
        public static MessageParser<ClanSettings> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[1];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public int InitialMembersCount
        {
            get => initialMembersCount_;
            set => initialMembersCount_ = value;
        }

        [DebuggerNonUserCode]
        public int MembersCountLimit
        {
            get => membersCountLimit_;
            set => membersCountLimit_ = value;
        }

        [DebuggerNonUserCode]
        public CurrencyAmount MemberCountUpgradeCost
        {
            get => memberCountUpgradeCost_;
            set => memberCountUpgradeCost_ = value;
        }

        [DebuggerNonUserCode]
        public CurrencyAmount ChangeClanNameOrTagCost
        {
            get => changeClanNameOrTagCost_;
            set => changeClanNameOrTagCost_ = value;
        }

        [DebuggerNonUserCode]
        public CurrencyAmount ClanCreateCost
        {
            get => clanCreateCost_;
            set => clanCreateCost_ = value;
        }

        [DebuggerNonUserCode]
        public ClanSettings() { }

        [DebuggerNonUserCode]
        public ClanSettings(ClanSettings other) : this()
        {
            initialMembersCount_ = other.initialMembersCount_;
            membersCountLimit_ = other.membersCountLimit_;
            memberCountUpgradeCost_ = other.memberCountUpgradeCost_?.Clone();
            changeClanNameOrTagCost_ = other.changeClanNameOrTagCost_?.Clone();
            clanCreateCost_ = other.clanCreateCost_?.Clone();
        }

        [DebuggerNonUserCode]
        public ClanSettings Clone() => new ClanSettings(this);

        [DebuggerNonUserCode]
        public override bool Equals(object other) => Equals(other as ClanSettings);

        [DebuggerNonUserCode]
        public bool Equals(ClanSettings other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return initialMembersCount_ == other.initialMembersCount_ &&
                   membersCountLimit_ == other.membersCountLimit_ &&
                   Equals(memberCountUpgradeCost_, other.memberCountUpgradeCost_) &&
                   Equals(changeClanNameOrTagCost_, other.changeClanNameOrTagCost_) &&
                   Equals(clanCreateCost_, other.clanCreateCost_);
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int hash = 1;
            hash ^= initialMembersCount_.GetHashCode();
            hash ^= membersCountLimit_.GetHashCode();
            hash ^= (memberCountUpgradeCost_?.GetHashCode() ?? 0);
            hash ^= (changeClanNameOrTagCost_?.GetHashCode() ?? 0);
            hash ^= (clanCreateCost_?.GetHashCode() ?? 0);
            return hash;
        }

        [DebuggerNonUserCode]
        public override string ToString() => JsonFormatter.ToDiagnosticString(this);

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (initialMembersCount_ != 0)
            {
                output.WriteRawTag(8);
                output.WriteInt32(initialMembersCount_);
            }
            if (membersCountLimit_ != 0)
            {
                output.WriteRawTag(16);
                output.WriteInt32(membersCountLimit_);
            }
            if (memberCountUpgradeCost_ != null)
            {
                output.WriteRawTag(26);
                output.WriteMessage(memberCountUpgradeCost_);
            }
            if (changeClanNameOrTagCost_ != null)
            {
                output.WriteRawTag(34);
                output.WriteMessage(changeClanNameOrTagCost_);
            }
            if (clanCreateCost_ != null)
            {
                output.WriteRawTag(42);
                output.WriteMessage(clanCreateCost_);
            }
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int size = 0;
            if (initialMembersCount_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(initialMembersCount_);
            }
            if (membersCountLimit_ != 0)
            {
                size += 1 + CodedOutputStream.ComputeInt32Size(membersCountLimit_);
            }
            if (memberCountUpgradeCost_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(memberCountUpgradeCost_);
            }
            if (changeClanNameOrTagCost_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(changeClanNameOrTagCost_);
            }
            if (clanCreateCost_ != null)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(clanCreateCost_);
            }
            return size;
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanSettings other)
        {
            if (other == null) return;

            if (other.initialMembersCount_ != 0)
            {
                InitialMembersCount = other.initialMembersCount_;
            }
            if (other.membersCountLimit_ != 0)
            {
                MembersCountLimit = other.membersCountLimit_;
            }
            if (other.memberCountUpgradeCost_ != null)
            {
                if (memberCountUpgradeCost_ == null)
                {
                    memberCountUpgradeCost_ = new CurrencyAmount();
                }
                memberCountUpgradeCost_.MergeFrom(other.memberCountUpgradeCost_);
            }
            if (other.changeClanNameOrTagCost_ != null)
            {
                if (changeClanNameOrTagCost_ == null)
                {
                    changeClanNameOrTagCost_ = new CurrencyAmount();
                }
                changeClanNameOrTagCost_.MergeFrom(other.changeClanNameOrTagCost_);
            }
            if (other.clanCreateCost_ != null)
            {
                if (clanCreateCost_ == null)
                {
                    clanCreateCost_ = new CurrencyAmount();
                }
                clanCreateCost_.MergeFrom(other.clanCreateCost_);
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
                    case 8:
                        initialMembersCount_ = input.ReadInt32();
                        break;
                    case 16:
                        membersCountLimit_ = input.ReadInt32();
                        break;
                    case 26:
                        if (memberCountUpgradeCost_ == null)
                        {
                            memberCountUpgradeCost_ = new CurrencyAmount();
                        }
                        input.ReadMessage(memberCountUpgradeCost_);
                        break;
                    case 34:
                        if (changeClanNameOrTagCost_ == null)
                        {
                            changeClanNameOrTagCost_ = new CurrencyAmount();
                        }
                        input.ReadMessage(changeClanNameOrTagCost_);
                        break;
                    case 42:
                        if (clanCreateCost_ == null)
                        {
                            clanCreateCost_ = new CurrencyAmount();
                        }
                        input.ReadMessage(clanCreateCost_);
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}