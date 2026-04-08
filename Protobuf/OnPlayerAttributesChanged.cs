using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class OnPlayerAttributesChanged : IMessage<OnPlayerAttributesChanged>, IMessage, IEquatable<OnPlayerAttributesChanged>, IDeepCloneable<OnPlayerAttributesChanged>
	{
		private static readonly MessageParser<OnPlayerAttributesChanged> _parser = new MessageParser<OnPlayerAttributesChanged>(() => new OnPlayerAttributesChanged());

		public const int PlayerIdFieldNumber = 1;

		private string playerId_ = "";

		public const int AttributesFieldNumber = 2;

		private Attributes attributes_;

		[DebuggerNonUserCode]
		public static MessageParser<OnPlayerAttributesChanged> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerMessageReflection.Descriptor.MessageTypes[5];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string PlayerId
		{
			get
			{
				return playerId_;
			}
			set
			{
				playerId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Attributes Attributes
		{
			get
			{
				return attributes_;
			}
			set
			{
				attributes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public OnPlayerAttributesChanged()
		{
		}

		[DebuggerNonUserCode]
		public OnPlayerAttributesChanged(OnPlayerAttributesChanged other)
			: this()
		{
			playerId_ = other.playerId_;
			Attributes = ((other.attributes_ != null) ? other.Attributes.Clone() : null);
		}

		[DebuggerNonUserCode]
		public OnPlayerAttributesChanged Clone()
		{
			return new OnPlayerAttributesChanged(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as OnPlayerAttributesChanged);
		}

		[DebuggerNonUserCode]
		public bool Equals(OnPlayerAttributesChanged other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (PlayerId != other.PlayerId)
			{
				return false;
			}
			if (!object.Equals(Attributes, other.Attributes))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (PlayerId.Length != 0)
			{
				num ^= PlayerId.GetHashCode();
			}
			if (attributes_ != null)
			{
				num ^= Attributes.GetHashCode();
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
			if (PlayerId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(PlayerId);
			}
			if (attributes_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(Attributes);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (PlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerId);
			}
			if (attributes_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(Attributes);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(OnPlayerAttributesChanged other)
		{
			if (other == null)
			{
				return;
			}
			if (other.PlayerId.Length != 0)
			{
				PlayerId = other.PlayerId;
			}
			if (other.attributes_ != null)
			{
				if (attributes_ == null)
				{
					attributes_ = new Attributes();
				}
				Attributes.MergeFrom(other.Attributes);
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
					PlayerId = input.ReadString();
					break;
				case 18u:
					if (attributes_ == null)
					{
						attributes_ = new Attributes();
					}
					input.ReadMessage(attributes_);
					break;
				}
			}
		}
	}
}
