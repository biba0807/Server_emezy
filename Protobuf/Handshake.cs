using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Handshake : IMessage<Handshake>, IMessage, IEquatable<Handshake>, IDeepCloneable<Handshake>
	{
		private static readonly MessageParser<Handshake> _parser = new MessageParser<Handshake>(() => new Handshake());

		public const int TicketFieldNumber = 1;

		private string ticket_ = "";

		[DebuggerNonUserCode]
		public static MessageParser<Handshake> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => AuthMessageReflection.Descriptor.MessageTypes[0];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public string Ticket
		{
			get
			{
				return ticket_;
			}
			set
			{
				ticket_ = ProtoPreconditions.CheckNotNull(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public Handshake()
		{
		}

		[DebuggerNonUserCode]
		public Handshake(Handshake other)
			: this()
		{
			ticket_ = other.ticket_;
		}

		[DebuggerNonUserCode]
		public Handshake Clone()
		{
			return new Handshake(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Handshake);
		}

		[DebuggerNonUserCode]
		public bool Equals(Handshake other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Ticket != other.Ticket)
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Ticket.Length != 0)
			{
				num ^= Ticket.GetHashCode();
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
			if (Ticket.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(Ticket);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Ticket.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(Ticket);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Handshake other)
		{
			if (other != null && other.Ticket.Length != 0)
			{
				Ticket = other.Ticket;
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					Ticket = input.ReadString();
				}
			}
		}
	}
}
