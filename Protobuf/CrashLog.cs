using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class CrashLog : IMessage<CrashLog>, IMessage, IEquatable<CrashLog>, IDeepCloneable<CrashLog>
	{
		private static readonly MessageParser<CrashLog> _parser = new MessageParser<CrashLog>(() => new CrashLog());

		public const int PlayerIdFieldNumber = 1;

		private string playerId_ = "";

		public const int PlayerUidFieldNumber = 2;

		private string playerUid_ = "";

		public const int GameIdFieldNumber = 3;

		private string gameId_ = "";

		public const int GameVersionFieldNumber = 4;

		private string gameVersion_ = "";

		public const int PlatformFieldNumber = 5;

		private Platform platform_ = Platform.Unknown;

		public const int LogsFieldNumber = 6;

		private static readonly FieldCodec<SimpleLog> _repeated_logs_codec = FieldCodec.ForMessage(50u, SimpleLog.Parser);

		private readonly RepeatedField<SimpleLog> logs_ = new RepeatedField<SimpleLog>();

		[DebuggerNonUserCode]
		public static MessageParser<CrashLog> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => CrashlogMessageReflection.Descriptor.MessageTypes[0];

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
		public string PlayerUid
		{
			get
			{
				return playerUid_;
			}
			set
			{
				playerUid_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

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
		public RepeatedField<SimpleLog> Logs => logs_;

		[DebuggerNonUserCode]
		public CrashLog()
		{
		}

		[DebuggerNonUserCode]
		public CrashLog(CrashLog other)
			: this()
		{
			playerId_ = other.playerId_;
			playerUid_ = other.playerUid_;
			gameId_ = other.gameId_;
			gameVersion_ = other.gameVersion_;
			platform_ = other.platform_;
			logs_ = other.logs_.Clone();
		}

		[DebuggerNonUserCode]
		public CrashLog Clone()
		{
			return new CrashLog(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as CrashLog);
		}

		[DebuggerNonUserCode]
		public bool Equals(CrashLog other)
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
			if (PlayerUid != other.PlayerUid)
			{
				return false;
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
			if (!logs_.Equals(other.logs_))
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
			if (PlayerUid.Length != 0)
			{
				num ^= PlayerUid.GetHashCode();
			}
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
			return num ^ logs_.GetHashCode();
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
			if (PlayerUid.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(PlayerUid);
			}
			if (GameId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(GameId);
			}
			if (GameVersion.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(GameVersion);
			}
			if (Platform != 0)
			{
				output.WriteRawTag(40);
				output.WriteEnum((int)Platform);
			}
			logs_.WriteTo(output, _repeated_logs_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (PlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerId);
			}
			if (PlayerUid.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerUid);
			}
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
			return num + logs_.CalculateSize(_repeated_logs_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CrashLog other)
		{
			if (other != null)
			{
				if (other.PlayerId.Length != 0)
				{
					PlayerId = other.PlayerId;
				}
				if (other.PlayerUid.Length != 0)
				{
					PlayerUid = other.PlayerUid;
				}
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
				logs_.Add(other.logs_);
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
					PlayerUid = input.ReadString();
					break;
				case 26u:
					GameId = input.ReadString();
					break;
				case 34u:
					GameVersion = input.ReadString();
					break;
				case 40u:
					platform_ = (Platform)input.ReadEnum();
					break;
				case 50u:
					logs_.AddEntriesFrom(input, _repeated_logs_codec);
					break;
				}
			}
		}
	}
}
