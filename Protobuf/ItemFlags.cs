using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class ItemFlags : IMessage<ItemFlags>, IMessage, IEquatable<ItemFlags>, IDeepCloneable<ItemFlags>
	{
		private static readonly MessageParser<ItemFlags> _parser = new MessageParser<ItemFlags>(() => new ItemFlags());

		public const int FlagsFieldNumber = 1;

		private static readonly MapField<int, int>.Codec _map_flags_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8u), FieldCodec.ForInt32(16u), 10u);

		private readonly MapField<int, int> flags_ = new MapField<int, int>();

		[DebuggerNonUserCode]
		public static MessageParser<ItemFlags> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[9];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public MapField<int, int> Flags => flags_;

		[DebuggerNonUserCode]
		public ItemFlags()
		{
		}

		[DebuggerNonUserCode]
		public ItemFlags(ItemFlags other)
			: this()
		{
			flags_ = other.flags_.Clone();
		}

		[DebuggerNonUserCode]
		public ItemFlags Clone()
		{
			return new ItemFlags(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as ItemFlags);
		}

		[DebuggerNonUserCode]
		public bool Equals(ItemFlags other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!Flags.Equals(other.Flags))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			return num ^ Flags.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			flags_.WriteTo(output, _map_flags_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			return num + flags_.CalculateSize(_map_flags_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(ItemFlags other)
		{
			if (other != null)
			{
				flags_.Add(other.flags_);
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
					flags_.AddEntriesFrom(input, _map_flags_codec);
				}
			}
		}
	}
}
