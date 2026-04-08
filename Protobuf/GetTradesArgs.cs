using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public sealed class GetTradesArgs : IMessage<GetTradesArgs>, IMessage, IEquatable<GetTradesArgs>, IDeepCloneable<GetTradesArgs>
	{
		private static readonly MessageParser<GetTradesArgs> _parser = new MessageParser<GetTradesArgs>(() => new GetTradesArgs());

		public const int ItemDefinitionIdsFieldNumber = 1;

		private static readonly FieldCodec<int> _repeated_itemDefinitionIds_codec = FieldCodec.ForInt32(10u);

		private readonly RepeatedField<int> itemDefinitionIds_ = new RepeatedField<int>();

		[DebuggerNonUserCode]
		public static MessageParser<GetTradesArgs> Parser => _parser;

		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor => MarketplaceMessageReflection.Descriptor.MessageTypes[13];

		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor => Descriptor;

		[DebuggerNonUserCode]
		public RepeatedField<int> ItemDefinitionIds => itemDefinitionIds_;

		[DebuggerNonUserCode]
		public GetTradesArgs()
		{
		}

		[DebuggerNonUserCode]
		public GetTradesArgs(GetTradesArgs other)
			: this()
		{
			itemDefinitionIds_ = other.itemDefinitionIds_.Clone();
		}

		[DebuggerNonUserCode]
		public GetTradesArgs Clone()
		{
			return new GetTradesArgs(this);
		}

		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return Equals(other as GetTradesArgs);
		}

		[DebuggerNonUserCode]
		public bool Equals(GetTradesArgs other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (!itemDefinitionIds_.Equals(other.itemDefinitionIds_))
			{
				return false;
			}
			return true;
		}

		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			return num ^ itemDefinitionIds_.GetHashCode();
		}

		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			itemDefinitionIds_.WriteTo(output, _repeated_itemDefinitionIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			return num + itemDefinitionIds_.CalculateSize(_repeated_itemDefinitionIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(GetTradesArgs other)
		{
			if (other != null)
			{
				itemDefinitionIds_.Add(other.itemDefinitionIds_);
			}
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0)
			{
				uint num2 = num;
				if (num2 != 8 && num2 != 10)
				{
					input.SkipLastField();
				}
				else
				{
					itemDefinitionIds_.AddEntriesFrom(input, _repeated_itemDefinitionIds_codec);
				}
			}
		}
	}
}
