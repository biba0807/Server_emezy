using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class AuthFacebook : IMessage<AuthFacebook>, IMessage, IEquatable<AuthFacebook>, IDeepCloneable<AuthFacebook>
	{
		private static readonly MessageParser<AuthFacebook> _parser = new MessageParser<AuthFacebook>(() => new AuthFacebook());

		public const int GameIdFieldNumber = 1;

		private string gameId_ = "";

		public const int GameVersionFieldNumber = 2;

		private string gameVersion_ = "";

		public const int PlatformFieldNumber = 3;

		private Platform platform_ = Platform.Unknown;

		public const int TokenFieldNumber = 4;

		private string token_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<AuthFacebook> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AuthMessageReflection.Descriptor.MessageTypes[2];

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
		public string Token
		{
			get
			{
				return token_;
			}
			set
			{
				token_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public AuthFacebook()
		{
		}

		[DebuggerNonUserCode]
		public AuthFacebook(AuthFacebook other)
			: this()
		{
			gameId_ = other.gameId_;
			gameVersion_ = other.gameVersion_;
			platform_ = other.platform_;
			token_ = other.token_;
		}

		[DebuggerNonUserCode]
		public AuthFacebook Clone()
		{
			return new AuthFacebook(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as AuthFacebook);
		}

		[DebuggerNonUserCode]
		public bool Equals(AuthFacebook other)
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
			if (Token != other.Token)
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
			if (Token.Length != 0)
			{
				num ^= Token.GetHashCode();
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
			if (Token.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(Token);
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
			if (Token.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Token);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(AuthFacebook other)
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
				if (other.Token.Length != 0)
				{
					Token = other.Token;
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
					Token = input.ReadString();
					break;
				}
			}
		}
	}
}
