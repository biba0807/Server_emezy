using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class InventoryMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static InventoryMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChdpbnZlbnRvcnlfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQa" + "FGNvbW1vbl9tZXNzYWdlLnByb3RvGhZjdXJyZW5jeV9tZXNzYWdlLnByb3Rv" + "IoIBCg9QbGF5ZXJJbnZlbnRvcnkSNQoKY3VycmVuY2llcxgBIAMoCzIhLmNv" + "bS5heGxlYm9sdC5ib2x0LkN1cnJlbmN5QW1vdW50EjgKDmludmVudG9yeUl0" + "ZW1zGAIgAygLMiAuY29tLmF4bGVib2x0LmJvbHQuSW52ZW50b3J5SXRlbSKH" + "AgoNSW52ZW50b3J5SXRlbRIKCgJpZBgBIAEoBRIYChBpdGVtRGVmaW5pdGlv" + "bklkGAIgASgFEhAKCHF1YW50aXR5GAMgASgFEg0KBWZsYWdzGAQgASgFEgwK" + "BGRhdGUYBSABKAMSRAoKcHJvcGVydGllcxgGIAMoCzIwLmNvbS5heGxlYm9s" + "dC5ib2x0LkludmVudG9yeUl0ZW0uUHJvcGVydGllc0VudHJ5GlsKD1Byb3Bl" + "cnRpZXNFbnRyeRILCgNrZXkYASABKAkSNwoFdmFsdWUYAiABKAsyKC5jb20u" + "YXhsZWJvbHQuYm9sdC5JbnZlbnRvcnlJdGVtUHJvcGVydHk6AjgBIr0CChdJ" + "bnZlbnRvcnlJdGVtRGVmaW5pdGlvbhIKCgJpZBgBIAEoBRITCgtkaXNwbGF5" + "TmFtZRgCIAEoCRJOCgpwcm9wZXJ0aWVzGAMgAygLMjouY29tLmF4bGVib2x0" + "LmJvbHQuSW52ZW50b3J5SXRlbURlZmluaXRpb24uUHJvcGVydGllc0VudHJ5" + "EjMKCGJ1eVByaWNlGAQgAygLMiEuY29tLmF4bGVib2x0LmJvbHQuQ3VycmVu" + "Y3lBbW91bnQSNAoJc2VsbFByaWNlGAUgAygLMiEuY29tLmF4bGVib2x0LmJv" + "bHQuQ3VycmVuY3lBbW91bnQSEwoLY2FuQmVUcmFkZWQYBiABKAgaMQoPUHJv" + "cGVydGllc0VudHJ5EgsKA2tleRgBIAEoCRINCgV2YWx1ZRgCIAEoCToCOAEi" + "/wEKIEludmVudG9yeUl0ZW1Qcm9wZXJ0eURlZmluaXRpb25zEhgKEGl0ZW1E" + "ZWZpbml0aW9uSWQYASABKAUSWQoLZGVmaW5pdGlvbnMYAiADKAsyRC5jb20u" + "YXhsZWJvbHQuYm9sdC5JbnZlbnRvcnlJdGVtUHJvcGVydHlEZWZpbml0aW9u" + "cy5EZWZpbml0aW9uc0VudHJ5GmYKEERlZmluaXRpb25zRW50cnkSCwoDa2V5" + "GAEgASgJEkEKBXZhbHVlGAIgASgLMjIuY29tLmF4bGVib2x0LmJvbHQuSW52" + "ZW50b3J5SXRlbVByb3BlcnR5RGVmaW5pdGlvbjoCOAEitAEKH0ludmVudG9y" + "eUl0ZW1Qcm9wZXJ0eURlZmluaXRpb24SDAoEbmFtZRgBIAEoCRI1Cgxwcm9w" + "ZXJ0eVR5cGUYAiABKA4yHy5jb20uYXhsZWJvbHQuYm9sdC5Qcm9wZXJ0eVR5" + "cGUSEwoLc2F2ZUluVHJhZGUYAyABKAgSNwoJc2V0QnlUeXBlGAQgASgOMiQu" + "Y29tLmF4bGVib2x0LmJvbHQuUHJvcGVydHlTZXRCeVR5cGUiRwoTSW52ZW50" + "b3J5SXRlbUFtb3VudBIhChlpbnZlbnRvcnlJdGVtRGVmaW5pdGlvbklkGAEg" + "ASgFEg0KBXZhbHVlGAIgASgFIoEBCg5FeGNoYW5nZVJlc3VsdBI1CgpjdXJy" + "ZW5jaWVzGAEgAygLMiEuY29tLmF4bGVib2x0LmJvbHQuQ3VycmVuY3lBbW91" + "bnQSOAoOaW52ZW50b3J5SXRlbXMYAiADKAsyIC5jb20uYXhsZWJvbHQuYm9s" + "dC5JbnZlbnRvcnlJdGVtItIBChdJbnZlbnRvcnlJdGVtUHJvcGVydGllcxIK" + "CgJpZBgBIAEoBRJOCgpwcm9wZXJ0aWVzGAIgAygLMjouY29tLmF4bGVib2x0" + "LmJvbHQuSW52ZW50b3J5SXRlbVByb3BlcnRpZXMuUHJvcGVydGllc0VudHJ5" + "GlsKD1Byb3BlcnRpZXNFbnRyeRILCgNrZXkYASABKAkSNwoFdmFsdWUYAiAB" + "KAsyKC5jb20uYXhsZWJvbHQuYm9sdC5JbnZlbnRvcnlJdGVtUHJvcGVydHk6" + "AjgBIpcBChVJbnZlbnRvcnlJdGVtUHJvcGVydHkSLQoEdHlwZRgBIAEoDjIf" + "LmNvbS5heGxlYm9sdC5ib2x0LlByb3BlcnR5VHlwZRIQCghpbnRWYWx1ZRgC" + "IAEoBRISCgpmbG9hdFZhbHVlGAMgASgCEhMKC3N0cmluZ1ZhbHVlGAQgASgJ" + "EhQKDGJvb2xlYW5WYWx1ZRgFIAEoCCJxCglJdGVtRmxhZ3MSNgoFZmxhZ3MY" + "ASADKAsyJy5jb20uYXhsZWJvbHQuYm9sdC5JdGVtRmxhZ3MuRmxhZ3NFbnRy" + "eRosCgpGbGFnc0VudHJ5EgsKA2tleRgBIAEoBRINCgV2YWx1ZRgCIAEoBToC" + "OAEqSQoJU3RvcmVDb2RlEhUKEUdPT0dMRV9QTEFZX1NUT1JFEAASEwoPQVBQ" + "TEVfQVBQX1NUT1JFEAESEAoMQU1BWk9OX1NUT1JFEAIqSgoRUHJvcGVydHlT" + "ZXRCeVR5cGUSDwoLR0FNRV9TRVJWRVIQABIYChRPRkZJQ0lBTF9HQU1FX1NF" + "UlZFUhABEgoKBkNMSUVOVBACQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3Rv" + "YnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				CommonMessageReflection.Descriptor,
				CurrencyMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(StoreCode),
				typeof(PropertySetByType)
			}, new GeneratedClrTypeInfo[10]
			{
				new GeneratedClrTypeInfo(typeof(PlayerInventory), PlayerInventory.Parser, new string[2] { "Currencies", "InventoryItems" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(InventoryItem), InventoryItem.Parser, new string[6] { "Id", "ItemDefinitionId", "Quantity", "Flags", "Date", "Properties" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(InventoryItemDefinition), InventoryItemDefinition.Parser, new string[6] { "Id", "DisplayName", "Properties", "BuyPrice", "SellPrice", "CanBeTraded" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(InventoryItemPropertyDefinitions), InventoryItemPropertyDefinitions.Parser, new string[2] { "ItemDefinitionId", "Definitions" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(InventoryItemPropertyDefinition), InventoryItemPropertyDefinition.Parser, new string[4] { "Name", "PropertyType", "SaveInTrade", "SetByType" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(InventoryItemAmount), InventoryItemAmount.Parser, new string[2] { "InventoryItemDefinitionId", "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ExchangeResult), ExchangeResult.Parser, new string[2] { "Currencies", "InventoryItems" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(InventoryItemProperties), InventoryItemProperties.Parser, new string[2] { "Id", "Properties" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(InventoryItemProperty), InventoryItemProperty.Parser, new string[5] { "Type", "IntValue", "FloatValue", "StringValue", "BooleanValue" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ItemFlags), ItemFlags.Parser, new string[1] { "Flags" }, null, null, new GeneratedClrTypeInfo[1])
			}));
		}
	}
}
