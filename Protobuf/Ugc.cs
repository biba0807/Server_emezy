using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Ugc : IMessage<Ugc>, IMessage, IEquatable<Ugc>, IDeepCloneable<Ugc>
	{
		private static readonly MessageParser<Ugc> _parser = new MessageParser<Ugc>(() => new Ugc());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int VersionFieldNumber = 2;

		private string version_ = "";

		public const int DownloadUrlFieldNumber = 3;

		private string downloadUrl_ = "";

		public const int DateFieldNumber = 4;

		private long date_;

		public const int AuthorIdFieldNumber = 5;

		private string authorId_ = "";

		public const int PreviewFieldNumber = 6;

		private ByteString preview_ = ByteString.Empty;

		public const int DescriptionFieldNumber = 7;

		private string description_ = "";

		public const int RatingFieldNumber = 8;

		private float rating_;

		public const int TagsFieldNumber = 9;

		private static readonly FieldCodec<string> _repeated_tags_codec = FieldCodec.ForString(74u);

		private readonly RepeatedField<string> tags_ = new RepeatedField<string>();

		[DebuggerNonUserCode]
		public static MessageParser<Ugc> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => UgcMessageReflection.Descriptor.MessageTypes[0];

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
		public string DownloadUrl
		{
			get
			{
				return downloadUrl_;
			}
			set
			{
				downloadUrl_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public ByteString Preview
		{
			get
			{
				return preview_;
			}
			set
			{
				preview_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Description
		{
			get
			{
				return description_;
			}
			set
			{
				description_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public float Rating
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
		public RepeatedField<string> Tags => tags_;

		[DebuggerNonUserCode]
		public Ugc()
		{
		}

		[DebuggerNonUserCode]
		public Ugc(Ugc other)
			: this()
		{
			name_ = other.name_;
			version_ = other.version_;
			downloadUrl_ = other.downloadUrl_;
			date_ = other.date_;
			authorId_ = other.authorId_;
			preview_ = other.preview_;
			description_ = other.description_;
			rating_ = other.rating_;
			tags_ = other.tags_.Clone();
		}

		[DebuggerNonUserCode]
		public Ugc Clone()
		{
			return new Ugc(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Ugc);
		}

		[DebuggerNonUserCode]
		public bool Equals(Ugc other)
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
			if (DownloadUrl != other.DownloadUrl)
			{
				return false;
			}
			if (Date != other.Date)
			{
				return false;
			}
			if (AuthorId != other.AuthorId)
			{
				return false;
			}
			if (Preview != other.Preview)
			{
				return false;
			}
			if (Description != other.Description)
			{
				return false;
			}
			if (Rating != other.Rating)
			{
				return false;
			}
			if (!tags_.Equals(other.tags_))
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
			if (DownloadUrl.Length != 0)
			{
				num ^= DownloadUrl.GetHashCode();
			}
			if (Date != 0)
			{
				num ^= Date.GetHashCode();
			}
			if (AuthorId.Length != 0)
			{
				num ^= AuthorId.GetHashCode();
			}
			if (Preview.Length != 0)
			{
				num ^= Preview.GetHashCode();
			}
			if (Description.Length != 0)
			{
				num ^= Description.GetHashCode();
			}
			if (Rating != 0f)
			{
				num ^= Rating.GetHashCode();
			}
			return num ^ tags_.GetHashCode();
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
			if (DownloadUrl.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(DownloadUrl);
			}
			if (Date != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(Date);
			}
			if (AuthorId.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(AuthorId);
			}
			if (Preview.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteBytes(Preview);
			}
			if (Description.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(Description);
			}
			if (Rating != 0f)
			{
				output.WriteRawTag(69);
				output.WriteFloat(Rating);
			}
			tags_.WriteTo(output, _repeated_tags_codec);
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
			if (DownloadUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(DownloadUrl);
			}
			if (Date != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Date);
			}
			if (AuthorId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AuthorId);
			}
			if (Preview.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeBytesSize(Preview);
			}
			if (Description.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Description);
			}
			if (Rating != 0f)
			{
				num += 5;
			}
			return num + tags_.CalculateSize(_repeated_tags_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Ugc other)
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
				if (other.DownloadUrl.Length != 0)
				{
					DownloadUrl = other.DownloadUrl;
				}
				if (other.Date != 0)
				{
					Date = other.Date;
				}
				if (other.AuthorId.Length != 0)
				{
					AuthorId = other.AuthorId;
				}
				if (other.Preview.Length != 0)
				{
					Preview = other.Preview;
				}
				if (other.Description.Length != 0)
				{
					Description = other.Description;
				}
				if (other.Rating != 0f)
				{
					Rating = other.Rating;
				}
				tags_.Add(other.tags_);
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
					DownloadUrl = input.ReadString();
					break;
				case 32u:
					Date = input.ReadInt64();
					break;
				case 42u:
					AuthorId = input.ReadString();
					break;
				case 50u:
					Preview = input.ReadBytes();
					break;
				case 58u:
					Description = input.ReadString();
					break;
				case 69u:
					Rating = input.ReadFloat();
					break;
				case 74u:
					tags_.AddEntriesFrom(input, _repeated_tags_codec);
					break;
				}
			}
		}
	}
}
