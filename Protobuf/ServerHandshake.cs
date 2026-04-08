using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ServerHandshake : IMessage<ServerHandshake>, IMessage, IEquatable<ServerHandshake>, IDeepCloneable<ServerHandshake>
	{
		private static readonly MessageParser<ServerHandshake> _parser = new MessageParser<ServerHandshake>(() => new ServerHandshake());

		public const int GameIdFieldNumber = 1;

		private string gameId_ = "";

		public const int ApiKeyFieldNumber = 2;

		private string apiKey_ = "";

		public const int VersionFieldNumber = 3;

		private string version_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<ServerHandshake> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => GameserverMessageReflection.Descriptor.MessageTypes[0];

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
		public string ApiKey
		{
			get
			{
				return apiKey_;
			}
			set
			{
				apiKey_ = ProtoPreconditions.CheckNotNull(value, "value");
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
		public ServerHandshake()
		{
		}

		[DebuggerNonUserCode]
		public ServerHandshake(ServerHandshake other)
			: this()
		{
			gameId_ = other.gameId_;
			apiKey_ = other.apiKey_;
			version_ = other.version_;
		}

		[DebuggerNonUserCode]
		public ServerHandshake Clone()
		{
			return new ServerHandshake(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ServerHandshake);
		}

		[DebuggerNonUserCode]
		public bool Equals(ServerHandshake other)
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
			if (ApiKey != other.ApiKey)
			{
				return false;
			}
			if (Version != other.Version)
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
			if (ApiKey.Length != 0)
			{
				num ^= ApiKey.GetHashCode();
			}
			if (Version.Length != 0)
			{
				num ^= Version.GetHashCode();
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
			if (ApiKey.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(ApiKey);
			}
			if (Version.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(Version);
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
			if (ApiKey.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(ApiKey);
			}
			if (Version.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Version);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ServerHandshake other)
		{
			if (other != null)
			{
				if (other.GameId.Length != 0)
				{
					GameId = other.GameId;
				}
				if (other.ApiKey.Length != 0)
				{
					ApiKey = other.ApiKey;
				}
				if (other.Version.Length != 0)
				{
					Version = other.Version;
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
					ApiKey = input.ReadString();
					break;
				case 26u:
					Version = input.ReadString();
					break;
				}
			}
		}
	}
}
