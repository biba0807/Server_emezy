using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class HackDetection : IMessage<HackDetection>, IMessage, IEquatable<HackDetection>, IDeepCloneable<HackDetection>
	{
		private static readonly MessageParser<HackDetection> _parser = new MessageParser<HackDetection>(() => new HackDetection());

		public const int ApkHashFieldNumber = 1;

		private string apkHash_ = "";

		public const int IsRootedFieldNumber = 2;

		private bool isRooted_;

		public const int JsonFieldNumber = 3;

		private static readonly FieldCodec<string> _repeated_json_codec = FieldCodec.ForString(26u);

		private readonly RepeatedField<string> json_ = new RepeatedField<string>();

		public const int ModDateFieldNumber = 4;

		private long modDate_;

		[DebuggerNonUserCode]
		public static MessageParser<HackDetection> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => HackMessageReflection.Descriptor.MessageTypes[0];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string ApkHash
		{
			get
			{
				return apkHash_;
			}
			set
			{
				apkHash_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public bool IsRooted
		{
			get
			{
				return isRooted_;
			}
			set
			{
				isRooted_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<string> Json => json_;

		[DebuggerNonUserCode]
		public long ModDate
		{
			get
			{
				return modDate_;
			}
			set
			{
				modDate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public HackDetection()
		{
		}

		[DebuggerNonUserCode]
		public HackDetection(HackDetection other)
			: this()
		{
			apkHash_ = other.apkHash_;
			isRooted_ = other.isRooted_;
			json_ = other.json_.Clone();
			modDate_ = other.modDate_;
		}

		[DebuggerNonUserCode]
		public HackDetection Clone()
		{
			return new HackDetection(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as HackDetection);
		}

		[DebuggerNonUserCode]
		public bool Equals(HackDetection other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (ApkHash != other.ApkHash)
			{
				return false;
			}
			if (IsRooted != other.IsRooted)
			{
				return false;
			}
			if (!json_.Equals(other.json_))
			{
				return false;
			}
			if (ModDate != other.ModDate)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (ApkHash.Length != 0)
			{
				num ^= ApkHash.GetHashCode();
			}
			if (IsRooted)
			{
				num ^= IsRooted.GetHashCode();
			}
			num ^= json_.GetHashCode();
			if (ModDate != 0)
			{
				num ^= ModDate.GetHashCode();
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
			if (ApkHash.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(ApkHash);
			}
			if (IsRooted)
			{
				output.WriteRawTag(16);
				output.WriteBool(IsRooted);
			}
			json_.WriteTo(output, _repeated_json_codec);
			if (ModDate != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt64(ModDate);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (ApkHash.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(ApkHash);
			}
			if (IsRooted)
			{
				num += 2;
			}
			num += json_.CalculateSize(_repeated_json_codec);
			if (ModDate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(ModDate);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(HackDetection other)
		{
			if (other != null)
			{
				if (other.ApkHash.Length != 0)
				{
					ApkHash = other.ApkHash;
				}
				if (other.IsRooted)
				{
					IsRooted = other.IsRooted;
				}
				json_.Add(other.json_);
				if (other.ModDate != 0)
				{
					ModDate = other.ModDate;
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
					ApkHash = input.ReadString();
					break;
				case 16u:
					IsRooted = input.ReadBool();
					break;
				case 26u:
					json_.AddEntriesFrom(input, _repeated_json_codec);
					break;
				case 32u:
					ModDate = input.ReadInt64();
					break;
				}
			}
		}
	}
}
