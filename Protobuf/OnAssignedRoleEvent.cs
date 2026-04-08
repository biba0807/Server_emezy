using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnAssignedRoleEvent : IMessage<OnAssignedRoleEvent>, IMessage, IEquatable<OnAssignedRoleEvent>, IDeepCloneable<OnAssignedRoleEvent>
	{
		private static readonly MessageParser<OnAssignedRoleEvent> _parser = new MessageParser<OnAssignedRoleEvent>(() => new OnAssignedRoleEvent());

		public const int NewRoleFieldNumber = 1;

		private ClanMemberRole newRole_;

		public const int AssignatorMemberIdFieldNumber = 2;

		private string assignatorMemberId_ = "";

		public const int AssigningMemberIdFieldNumber = 3;

		private string assigningMemberId_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<OnAssignedRoleEvent> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[13];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public ClanMemberRole NewRole
		{
			get
			{
				return newRole_;
			}
			set
			{
				newRole_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AssignatorMemberId
		{
			get
			{
				return assignatorMemberId_;
			}
			set
			{
				assignatorMemberId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AssigningMemberId
		{
			get
			{
				return assigningMemberId_;
			}
			set
			{
				assigningMemberId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public OnAssignedRoleEvent()
		{
		}

		[DebuggerNonUserCode]
		public OnAssignedRoleEvent(OnAssignedRoleEvent other)
			: this()
		{
			NewRole = ((other.newRole_ != null) ? other.NewRole.Clone() : null);
			assignatorMemberId_ = other.assignatorMemberId_;
			assigningMemberId_ = other.assigningMemberId_;
		}

		[DebuggerNonUserCode]
		public OnAssignedRoleEvent Clone()
		{
			return new OnAssignedRoleEvent(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnAssignedRoleEvent);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnAssignedRoleEvent other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!object.Equals(NewRole, other.NewRole))
			{
				return false;
			}
			if (AssignatorMemberId != other.AssignatorMemberId)
			{
				return false;
			}
			if (AssigningMemberId != other.AssigningMemberId)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (newRole_ != null)
			{
				num ^= NewRole.GetHashCode();
			}
			if (AssignatorMemberId.Length != 0)
			{
				num ^= AssignatorMemberId.GetHashCode();
			}
			if (AssigningMemberId.Length != 0)
			{
				num ^= AssigningMemberId.GetHashCode();
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
			if (newRole_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(NewRole);
			}
			if (AssignatorMemberId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(AssignatorMemberId);
			}
			if (AssigningMemberId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(AssigningMemberId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (newRole_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(NewRole);
			}
			if (AssignatorMemberId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AssignatorMemberId);
			}
			if (AssigningMemberId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AssigningMemberId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnAssignedRoleEvent other)
		{
			if (other == null)
			{
				return;
			}
			if (other.newRole_ != null)
			{
				if (newRole_ == null)
				{
					newRole_ = new ClanMemberRole();
				}
			//	NewRole.MergeFrom(other.NewRole);
			}
			if (other.AssignatorMemberId.Length != 0)
			{
				AssignatorMemberId = other.AssignatorMemberId;
			}
			if (other.AssigningMemberId.Length != 0)
			{
				AssigningMemberId = other.AssigningMemberId;
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
					if (newRole_ == null)
					{
						newRole_ = new ClanMemberRole();
					}
					input.ReadMessage(newRole_);
					break;
				case 18u:
					AssignatorMemberId = input.ReadString();
					break;
				case 26u:
					AssigningMemberId = input.ReadString();
					break;
				}
			}
		}
	}
}
