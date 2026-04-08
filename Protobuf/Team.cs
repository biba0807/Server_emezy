using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Team : IMessage<Team>, IMessage, IEquatable<Team>, IDeepCloneable<Team>
	{
		private static readonly MessageParser<Team> _parser = new MessageParser<Team>(() => new Team());

		public const int NameFieldNumber = 1;

		private string name_ = "";

		public const int PlayerIdFieldNumber = 2;

		private string playerId_ = "";

		public const int PlayersFieldNumber = 3;

		private static readonly FieldCodec<string> _repeated_players_codec = FieldCodec.ForString(26u);

		private readonly RepeatedField<string> players_ = new RepeatedField<string>();

		[DebuggerNonUserCode]
		public static MessageParser<Team> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => TournamentsMessageReflection.Descriptor.MessageTypes[1];

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
		public RepeatedField<string> Players => players_;

		[DebuggerNonUserCode]
		public Team()
		{
		}

		[DebuggerNonUserCode]
		public Team(Team other)
			: this()
		{
			name_ = other.name_;
			playerId_ = other.playerId_;
			players_ = other.players_.Clone();
		}

		[DebuggerNonUserCode]
		public Team Clone()
		{
			return new Team(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Team);
		}

		[DebuggerNonUserCode]
		public bool Equals(Team other)
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
			if (PlayerId != other.PlayerId)
			{
				return false;
			}
			if (!players_.Equals(other.players_))
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
			if (PlayerId.Length != 0)
			{
				num ^= PlayerId.GetHashCode();
			}
			return num ^ players_.GetHashCode();
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
			if (PlayerId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(PlayerId);
			}
			players_.WriteTo(output, _repeated_players_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Name.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Name);
			}
			if (PlayerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(PlayerId);
			}
			return num + players_.CalculateSize(_repeated_players_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Team other)
		{
			if (other != null)
			{
				if (other.Name.Length != 0)
				{
					Name = other.Name;
				}
				if (other.PlayerId.Length != 0)
				{
					PlayerId = other.PlayerId;
				}
				players_.Add(other.players_);
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
					PlayerId = input.ReadString();
					break;
				case 26u:
					players_.AddEntriesFrom(input, _repeated_players_codec);
					break;
				}
			}
		}
	}
}
