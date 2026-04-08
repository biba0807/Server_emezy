using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class InventoryItemProperties : IMessage<InventoryItemProperties>, IMessage, IEquatable<InventoryItemProperties>, IDeepCloneable<InventoryItemProperties>
	{
		private static readonly MessageParser<InventoryItemProperties> _parser = new MessageParser<InventoryItemProperties>(() => new InventoryItemProperties());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int PropertiesFieldNumber = 2;

		private static readonly MapField<string, InventoryItemProperty>.Codec _map_properties_codec = new MapField<string, InventoryItemProperty>.Codec(FieldCodec.ForString(10u), FieldCodec.ForMessage(18u, InventoryItemProperty.Parser), 18u);

		private readonly MapField<string, InventoryItemProperty> properties_ = new MapField<string, InventoryItemProperty>();

		[DebuggerNonUserCode]
		public static MessageParser<InventoryItemProperties> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => InventoryMessageReflection.Descriptor.MessageTypes[7];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return id_;
			}
			set
			{
				id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, InventoryItemProperty> Properties => properties_;

		[DebuggerNonUserCode]
		public InventoryItemProperties()
		{
		}

		[DebuggerNonUserCode]
		public InventoryItemProperties(InventoryItemProperties other)
			: this()
		{
			id_ = other.id_;
			properties_ = other.properties_.Clone();
		}

		[DebuggerNonUserCode]
		public InventoryItemProperties Clone()
		{
			return new InventoryItemProperties(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as InventoryItemProperties);
		}

		[DebuggerNonUserCode]
		public bool Equals(InventoryItemProperties other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (Id != other.Id)
			{
				return false;
			}
			if (!Properties.Equals(other.Properties))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (Id != 0)
			{
				num ^= Id.GetHashCode();
			}
			return num ^ Properties.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(Id);
			}
			properties_.WriteTo(output, _map_properties_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(Id);
			}
			return num + properties_.CalculateSize(_map_properties_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(InventoryItemProperties other)
		{
			if (other != null)
			{
				if (other.Id != 0)
				{
					Id = other.Id;
				}
				properties_.Add(other.properties_);
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
					Id = input.ReadInt32();
					break;
				case 18u:
					properties_.AddEntriesFrom(input, _map_properties_codec);
					break;
				}
			}
		}
	}
}
