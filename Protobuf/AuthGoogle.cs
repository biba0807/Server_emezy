using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class AuthGoogle : IMessage<AuthGoogle>, IMessage, IEquatable<AuthGoogle>, IDeepCloneable<AuthGoogle>
	{
		private static readonly MessageParser<AuthGoogle> _parser = new MessageParser<AuthGoogle>(() => new AuthGoogle());

		public const int GameIdFieldNumber = 1;

		private string gameId_ = "";

		public const int GameVersionFieldNumber = 2;

		private string gameVersion_ = "";

		public const int PlatformFieldNumber = 3;

		private Platform platform_ = Platform.Unknown;

		public const int AuthCodeFieldNumber = 4;

		private string authCode_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<AuthGoogle> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AuthMessageReflection.Descriptor.MessageTypes[1];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string GameId
		{
			get
			{
				return gameId_;
			}
			set
			{
				gameId_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string GameVersion
		{
			get
			{
				return gameVersion_;
			}
			set
			{
				gameVersion_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Platform Platform
		{
			get
			{
				return platform_;
			}
			set
			{
				platform_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AuthCode
		{
			get
			{
				return authCode_;
			}
			set
			{
				authCode_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public AuthGoogle()
		{
		}

		[DebuggerNonUserCode]
		public AuthGoogle(AuthGoogle other)
			: this()
		{
			gameId_ = other.gameId_;
			gameVersion_ = other.gameVersion_;
			platform_ = other.platform_;
			authCode_ = other.authCode_;
		}

		[DebuggerNonUserCode]
		public AuthGoogle Clone()
		{
			return new AuthGoogle(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AuthGoogle);
		}

		[DebuggerNonUserCode]
		public bool Equals(AuthGoogle other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (GameId != other.GameId)
			{
				return false;
			}
			if (GameVersion != other.GameVersion)
			{
				return false;
			}
			if (Platform != other.Platform)
			{
				return false;
			}
			if (AuthCode != other.AuthCode)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (GameId.Length != 0)
			{
				num ^= GameId.GetHashCode();
			}
			if (GameVersion.Length != 0)
			{
				num ^= GameVersion.GetHashCode();
			}
			if (Platform != 0)
			{
				num ^= Platform.GetHashCode();
			}
			if (AuthCode.Length != 0)
			{
				num ^= AuthCode.GetHashCode();
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
			if (GameId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(GameId);
			}
			if (GameVersion.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(GameVersion);
			}
			if (Platform != 0)
			{
				output.WriteRawTag(24);
				output.WriteEnum((int)Platform);
			}
			if (AuthCode.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(AuthCode);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (GameId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(GameId);
			}
			if (GameVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(GameVersion);
			}
			if (Platform != 0)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)Platform);
			}
			if (AuthCode.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(AuthCode);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AuthGoogle other)
		{
			if (other != null)
			{
				if (other.GameId.Length != 0)
				{
					GameId = other.GameId;
				}
				if (other.GameVersion.Length != 0)
				{
					GameVersion = other.GameVersion;
				}
				if (other.Platform != 0)
				{
					Platform = other.Platform;
				}
				if (other.AuthCode.Length != 0)
				{
					AuthCode = other.AuthCode;
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
					GameId = input.ReadString();
					break;
				case 18u:
					GameVersion = input.ReadString();
					break;
				case 24u:
					platform_ = (Platform)input.ReadEnum();
					break;
				case 34u:
					AuthCode = input.ReadString();
					break;
				}
			}
		}
	}
}
