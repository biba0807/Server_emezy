using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public sealed class ClanMemberRole : IMessage<ClanMemberRole>, IMessage, IEquatable<ClanMemberRole>, IDeepCloneable<ClanMemberRole>
    {
        private static readonly MessageParser<ClanMemberRole> _parser = new MessageParser<ClanMemberRole>(() => new ClanMemberRole());

        public const int IdFieldNumber = 1;

        private string id_ = "";

        public const int NameFieldNumber = 2;

        private string name_ = "";

        public const int LevelFieldNumber = 3;

        private int level_;

        public const int DescripptionFieldNumber = 4;

        private string descripption_ = "";

        public const int PermissionsFieldNumber = 5;

        private static readonly FieldCodec<ClanMemberRolePermission> _repeated_permissions_codec = FieldCodec.ForEnum(42u, (ClanMemberRolePermission x) => (int)x, (int x) => (ClanMemberRolePermission)x);

        private readonly RepeatedField<ClanMemberRolePermission> permissions_ = new RepeatedField<ClanMemberRolePermission>();

        [DebuggerNonUserCode]
        public static MessageParser<ClanMemberRole> Parser => _parser;

        [DebuggerNonUserCode]
        public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[5];

        [DebuggerNonUserCode]
        MessageDescriptor IMessage.Descriptor => Descriptor;

        [DebuggerNonUserCode]
        public string Id
        {
            get
            {
                return id_;
            }
            set
            {
                id_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public string Name
        {
            get
            {
                return name_;
            }
            set
            {
                name_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public int Level
        {
            get
            {
                return level_;
            }
            set
            {
                level_ = value;
            }
        }

        [DebuggerNonUserCode]
        public string Descripption
        {
            get
            {
                return descripption_;
            }
            set
            {
                descripption_ = ProtoPreconditions.CheckNotNull(value, "value");
            }
        }

        [DebuggerNonUserCode]
        public RepeatedField<ClanMemberRolePermission> Permissions => permissions_;

        [DebuggerNonUserCode]
        public ClanMemberRole()
        {
        }

        [DebuggerNonUserCode]
        public ClanMemberRole(ClanMemberRole other)
            : this()
        {
            id_ = other.id_;
            name_ = other.name_;
            level_ = other.level_;
            descripption_ = other.descripption_;
            permissions_ = other.permissions_.Clone();
        }

        [DebuggerNonUserCode]
        public ClanMemberRole Clone()
        {
            return new ClanMemberRole(this);
        }

        [DebuggerNonUserCode]
        public override bool Equals(object other)
        {
            return Equals(other as ClanMemberRole);
        }

        [DebuggerNonUserCode]
        public bool Equals(ClanMemberRole other)
        {
            if (other == null)
            {
                return false;
            }
            if (other == this)
            {
                return true;
            }
            if (Id != other.Id)
            {
                return false;
            }
            if (Name != other.Name)
            {
                return false;
            }
            if (Level != other.Level)
            {
                return false;
            }
            if (Descripption != other.Descripption)
            {
                return false;
            }
            if (!permissions_.Equals(other.permissions_))
            {
                return false;
            }
            return true;
        }

        [DebuggerNonUserCode]
        public override int GetHashCode()
        {
            int num = 1;
            if (Id.Length != 0)
            {
                num ^= Id.GetHashCode();
            }
            if (Name.Length != 0)
            {
                num ^= Name.GetHashCode();
            }
            if (Level != 0)
            {
                num ^= Level.GetHashCode();
            }
            if (Descripption.Length != 0)
            {
                num ^= Descripption.GetHashCode();
            }
            return num ^ permissions_.GetHashCode();
        }

        [DebuggerNonUserCode]
        public override string ToString()
        {
            return JsonFormatter.ToDiagnosticString(this);
        }

        [DebuggerNonUserCode]
        public void WriteTo(CodedOutputStream output)
        {
            if (Id.Length != 0)
            {
                output.WriteRawTag(10);
                output.WriteString(Id);
            }
            if (Name.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Name);
            }
            if (Level != 0)
            {
                output.WriteRawTag(24);
                output.WriteInt32(Level);
            }
            if (Descripption.Length != 0)
            {
                output.WriteRawTag(34);
                output.WriteString(Descripption);
            }
            permissions_.WriteTo(output, _repeated_permissions_codec);
        }

        [DebuggerNonUserCode]
        public int CalculateSize()
        {
            int num = 0;
            if (Id.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Id);
            }
            if (Name.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Name);
            }
            if (Level != 0)
            {
                num += 1 + CodedOutputStream.ComputeInt32Size(Level);
            }
            if (Descripption.Length != 0)
            {
                num += 1 + CodedOutputStream.ComputeStringSize(Descripption);
            }
            return num + permissions_.CalculateSize(_repeated_permissions_codec);
        }

        [DebuggerNonUserCode]
        public void MergeFrom(ClanMemberRole other)
        {
            if (other != null)
            {
                if (other.Id.Length != 0)
                {
                    Id = other.Id;
                }
                if (other.Name.Length != 0)
                {
                    Name = other.Name;
                }
                if (other.Level != 0)
                {
                    Level = other.Level;
                }
                if (other.Descripption.Length != 0)
                {
                    Descripption = other.Descripption;
                }
                permissions_.Add(other.permissions_);
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
                        Id = input.ReadString();
                        break;
                    case 18u:
                        Name = input.ReadString();
                        break;
                    case 24u:
                        Level = input.ReadInt32();
                        break;
                    case 34u:
                        Descripption = input.ReadString();
                        break;
                    case 40u:
                    case 42u:
                        permissions_.AddEntriesFrom(input, _repeated_permissions_codec);
                        break;
                }
            }
        }
    }
}
