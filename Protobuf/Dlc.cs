using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Dlc : IMessage<Dlc>, IMessage, IEquatable<Dlc>, IDeepCloneable<Dlc>
	{
		private static readonly MessageParser<Dlc> _parser = new MessageParser<Dlc>(() => new Dlc());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int VersionFieldNumber = 2;

		private int version_;

		public const int MinGameVersionFieldNumber = 3;

		private string minGameVersion_ = "";

		public const int DownloadUrlFieldNumber = 4;

		private string downloadUrl_ = "";

		public const int DateFieldNumber = 5;

		private long date_;

		[DebuggerNonUserCode]
		public static MessageParser<Dlc> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => DlcMessageReflection.Descriptor.MessageTypes[0];

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
		public int Version
		{
			get
			{
				return version_;
			}
			set
			{
				version_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string MinGameVersion
		{
			get
			{
				return minGameVersion_;
			}
			set
			{
				minGameVersion_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public Dlc()
		{
		}

		[DebuggerNonUserCode]
		public Dlc(Dlc other)
			: this()
		{
			name_ = other.name_;
			version_ = other.version_;
			minGameVersion_ = other.minGameVersion_;
			downloadUrl_ = other.downloadUrl_;
			date_ = other.date_;
		}

		[DebuggerNonUserCode]
		public Dlc Clone()
		{
			return new Dlc(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Dlc);
		}

		[DebuggerNonUserCode]
		public bool Equals(Dlc other)
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
			if (MinGameVersion != other.MinGameVersion)
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
			if (Version != 0)
			{
				num ^= Version.GetHashCode();
			}
			if (MinGameVersion.Length != 0)
			{
				num ^= MinGameVersion.GetHashCode();
			}
			if (DownloadUrl.Length != 0)
			{
				num ^= DownloadUrl.GetHashCode();
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
			if (Version != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(Version);
			}
			if (MinGameVersion.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(MinGameVersion);
			}
			if (DownloadUrl.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(DownloadUrl);
			}
			if (Date != 0)
			{
				output.WriteRawTag(40);
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
			if (Version != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Version);
			}
			if (MinGameVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(MinGameVersion);
			}
			if (DownloadUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(DownloadUrl);
			}
			if (Date != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(Date);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Dlc other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.Version != 0)
				{
					Version = other.Version;
				}
				if (other.MinGameVersion.Length != 0)
				{
					MinGameVersion = other.MinGameVersion;
				}
				if (other.DownloadUrl.Length != 0)
				{
					DownloadUrl = other.DownloadUrl;
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
				case 16u:
					Version = input.ReadInt32();
					break;
				case 26u:
					MinGameVersion = input.ReadString();
					break;
				case 34u:
					DownloadUrl = input.ReadString();
					break;
				case 40u:
					Date = input.ReadInt64();
					break;
				}
			}
		}
	}
}
