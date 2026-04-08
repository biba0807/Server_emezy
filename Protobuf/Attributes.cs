using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class Attributes : IMessage<Attributes>, IMessage, IEquatable<Attributes>, IDeepCloneable<Attributes>
	{
		private static readonly MessageParser<Attributes> _parser = new MessageParser<Attributes>(() => new Attributes());

		public const int MapFieldNumber = 1;

		private static readonly MapField<string, Attribute>.Codec _map_map_codec = new MapField<string, Attribute>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, Attribute.Parser), 10u);

		private readonly MapField<string, Attribute> map_ = new MapField<string, Attribute>();

		[DebuggerNonUserCode]
		public static MessageParser<Attributes> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => PlayerMessageReflection.Descriptor.MessageTypes[2];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public MapField<string, Attribute> Map => map_;

		[DebuggerNonUserCode]
		public Attributes()
		{
		}

		[DebuggerNonUserCode]
		public Attributes(Attributes other)
			: this()
		{
			map_ = other.map_.Clone();
		}

		[DebuggerNonUserCode]
		public Attributes Clone()
		{
			return new Attributes(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as Attributes);
		}

		[DebuggerNonUserCode]
		public bool Equals(Attributes other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!Map.Equals(other.Map))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			return num ^ Map.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			map_.WriteTo(output, _map_map_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			return num + map_.CalculateSize(_map_map_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(Attributes other)
		{
			if (other != null)
			{
				map_.Add(other.map_);
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
					map_.AddEntriesFrom(input, _map_map_codec);
				}
			}
		}
	}
}
