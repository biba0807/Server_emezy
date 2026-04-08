using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class AppVerification : IMessage<AppVerification>, IMessage, IEquatable<AppVerification>, IDeepCloneable<AppVerification>
	{
		private static readonly MessageParser<AppVerification> _parser = new MessageParser<AppVerification>(() => new AppVerification());

		public const int IsRootedFieldNumber = 1;

		private bool isRooted_;

		public const int ApkHashFieldNumber = 2;

		private string apkHash_ = "";

		public const int JsonForbiddenAppsFieldNumber = 3;

		private static readonly FieldCodec<string> _repeated_jsonForbiddenApps_codec = FieldCodec.ForString(26u);

		private readonly RepeatedField<string> jsonForbiddenApps_ = new RepeatedField<string>();

		[DebuggerNonUserCode]
		public static MessageParser<AppVerification> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AuthMessageReflection.Descriptor.MessageTypes[3];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

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
		public RepeatedField<string> JsonForbiddenApps => jsonForbiddenApps_;

		[DebuggerNonUserCode]
		public AppVerification()
		{
		}

		[DebuggerNonUserCode]
		public AppVerification(AppVerification other)
			: this()
		{
			isRooted_ = other.isRooted_;
			apkHash_ = other.apkHash_;
			jsonForbiddenApps_ = other.jsonForbiddenApps_.Clone();
		}

		[DebuggerNonUserCode]
		public AppVerification Clone()
		{
			return new AppVerification(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AppVerification);
		}

		[DebuggerNonUserCode]
		public bool Equals(AppVerification other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (IsRooted != other.IsRooted)
			{
				return false;
			}
			if (ApkHash != other.ApkHash)
			{
				return false;
			}
			if (!jsonForbiddenApps_.Equals(other.jsonForbiddenApps_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (IsRooted)
			{
				num ^= IsRooted.GetHashCode();
			}
			if (ApkHash.Length != 0)
			{
				num ^= ApkHash.GetHashCode();
			}
			return num ^ jsonForbiddenApps_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (IsRooted)
			{
				output.WriteRawTag(8);
				output.WriteBool(IsRooted);
			}
			if (ApkHash.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(ApkHash);
			}
			jsonForbiddenApps_.WriteTo(output, _repeated_jsonForbiddenApps_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (IsRooted)
			{
				num += 2;
			}
			if (ApkHash.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(ApkHash);
			}
			return num + jsonForbiddenApps_.CalculateSize(_repeated_jsonForbiddenApps_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AppVerification other)
		{
			if (other != null)
			{
				if (other.IsRooted)
				{
					IsRooted = other.IsRooted;
				}
				if (other.ApkHash.Length != 0)
				{
					ApkHash = other.ApkHash;
				}
				jsonForbiddenApps_.Add(other.jsonForbiddenApps_);
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
				case 8u:
					IsRooted = input.ReadBool();
					break;
				case 18u:
					ApkHash = input.ReadString();
					break;
				case 26u:
					jsonForbiddenApps_.AddEntriesFrom(input, _repeated_jsonForbiddenApps_codec);
					break;
				}
			}
		}
	}
}
