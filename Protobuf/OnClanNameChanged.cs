using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnClanNameChanged : IMessage<OnClanNameChanged>, IMessage, IEquatable<OnClanNameChanged>, IDeepCloneable<OnClanNameChanged>
	{
		private static readonly MessageParser<OnClanNameChanged> _parser = new MessageParser<OnClanNameChanged>(() => new OnClanNameChanged());

		public const int NewNameFieldNumber = 1;

		private string newName_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<OnClanNameChanged> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => ClanMessageReflection.Descriptor.MessageTypes[16];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string NewName
		{
			get
			{
				return newName_;
			}
			set
			{
				newName_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public OnClanNameChanged()
		{
		}

		[DebuggerNonUserCode]
		public OnClanNameChanged(OnClanNameChanged other)
			: this()
		{
			newName_ = other.newName_;
		}

		[DebuggerNonUserCode]
		public OnClanNameChanged Clone()
		{
			return new OnClanNameChanged(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnClanNameChanged);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnClanNameChanged other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (NewName != other.NewName)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (NewName.Length != 0)
			{
				num ^= NewName.GetHashCode();
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
			if (NewName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(NewName);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (NewName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(NewName);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnClanNameChanged other)
		{
			if (other != null && other.NewName.Length != 0)
			{
				NewName = other.NewName;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					NewName = input.ReadString();
				}
			}
		}
	}
}
