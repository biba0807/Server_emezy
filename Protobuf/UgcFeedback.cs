using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class UgcFeedback : IMessage<UgcFeedback>, IMessage, IEquatable<UgcFeedback>, IDeepCloneable<UgcFeedback>
	{
		private static readonly MessageParser<UgcFeedback> _parser = new MessageParser<UgcFeedback>(() => new UgcFeedback());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int VersionFieldNumber = 2;

		private string version_ = "";

		public const int AuthorIdFieldNumber = 3;

		private string authorId_ = "";

		public const int RatingFieldNumber = 4;

		private int rating_;

		public const int FeedbackFieldNumber = 5;

		private string feedback_ = "";

		public const int DateFieldNumber = 6;

		private long date_;

		[DebuggerNonUserCode]
		public static MessageParser<UgcFeedback> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => UgcMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

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
		public string Version
		{
			get
			{
				return version_;
			}
			set
			{
				version_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AuthorId
		{
			get
			{
				return authorId_;
			}
			set
			{
				authorId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int Rating
		{
			get
			{
				return rating_;
			}
			set
			{
				rating_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Feedback
		{
			get
			{
				return feedback_;
			}
			set
			{
				feedback_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public long Date
		{
			get
			{
				return date_;
			}
			set
			{
				date_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UgcFeedback()
		{
		}

		[DebuggerNonUserCode]
		public UgcFeedback(UgcFeedback other)
			: this()
		{
			name_ = other.name_;
			version_ = other.version_;
			authorId_ = other.authorId_;
			rating_ = other.rating_;
			feedback_ = other.feedback_;
			date_ = other.date_;
		}

		[DebuggerNonUserCode]
		public UgcFeedback Clone()
		{
			return new UgcFeedback(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as UgcFeedback);
		}

		[DebuggerNonUserCode]
		public bool Equals(UgcFeedback other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Name != other.Name)
			{
				return false;
			}
			if (Version != other.Version)
			{
				return false;
			}
			if (AuthorId != other.AuthorId)
			{
				return false;
			}
			if (Rating != other.Rating)
			{
				return false;
			}
			if (Feedback != other.Feedback)
			{
				return false;
			}
			if (Date != other.Date)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Name.Length != 0)
			{
				num ^= Name.GetHashCode();
			}
			if (Version.Length != 0)
			{
				num ^= Version.GetHashCode();
			}
			if (AuthorId.Length != 0)
			{
				num ^= AuthorId.GetHashCode();
			}
			if (Rating != 0)
			{
				num ^= Rating.GetHashCode();
			}
			if (Feedback.Length != 0)
			{
				num ^= Feedback.GetHashCode();
			}
			if (Date != 0)
			{
				num ^= Date.GetHashCode();
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
			if (Name.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Name);
			}
			if (Version.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(Version);
			}
			if (AuthorId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(AuthorId);
			}
			if (Rating != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(Rating);
			}
			if (Feedback.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(Feedback);
			}
			if (Date != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt64(Date);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (Version.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Version);
			}
			if (AuthorId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AuthorId);
			}
			if (Rating != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Rating);
			}
			if (Feedback.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Feedback);
			}
			if (Date != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Date);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(UgcFeedback other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.Version.Length != 0)
				{
					Version = other.Version;
				}
				if (other.AuthorId.Length != 0)
				{
					AuthorId = other.AuthorId;
				}
				if (other.Rating != 0)
				{
					Rating = other.Rating;
				}
				if (other.Feedback.Length != 0)
				{
					Feedback = other.Feedback;
				}
				if (other.Date != 0)
				{
					Date = other.Date;
				}
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
					Name = input.ReadString();
					break;
				case 18u:
					Version = input.ReadString();
					break;
				case 26u:
					AuthorId = input.ReadString();
					break;
				case 32u:
					Rating = input.ReadInt32();
					break;
				case 42u:
					Feedback = input.ReadString();
					break;
				case 48u:
					Date = input.ReadInt64();
					break;
				}
			}
		}
	}
}
