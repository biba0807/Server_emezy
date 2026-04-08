using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class MarketplaceMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static MarketplaceMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChltYXJrZXRwbGFjZV9tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9s" + "dBoUcGxheWVyX21lc3NhZ2UucHJvdG8aF2ludmVudG9yeV9tZXNzYWdlLnBy" + "b3RvIukCCgtPcGVuUmVxdWVzdBIKCgJpZBgBIAEoCRIqCgdjcmVhdG9yGAIg" + "ASgLMhkuY29tLmF4bGVib2x0LmJvbHQuUGxheWVyEhgKEGl0ZW1EZWZpbml0" + "aW9uSWQYAyABKAUSDQoFcHJpY2UYBCABKAISEgoKY3JlYXRlRGF0ZRgFIAEo" + "AxIyCgR0eXBlGAYgASgOMiQuY29tLmF4bGVib2x0LmJvbHQuTWFya2V0UmVx" + "dWVzdFR5cGUSEAoIcXVhbnRpdHkYByABKAUSQgoKcHJvcGVydGllcxgIIAMo" + "CzIuLmNvbS5heGxlYm9sdC5ib2x0Lk9wZW5SZXF1ZXN0LlByb3BlcnRpZXNF" + "bnRyeRpbCg9Qcm9wZXJ0aWVzRW50cnkSCwoDa2V5GAEgASgJEjcKBXZhbHVl" + "GAIgASgLMiguY29tLmF4bGVib2x0LmJvbHQuSW52ZW50b3J5SXRlbVByb3Bl" + "cnR5OgI4ASKKBAoNQ2xvc2VkUmVxdWVzdBIKCgJpZBgBIAEoCRIQCghvcmln" + "aW5JZBgCIAEoCRIqCgdjcmVhdG9yGAMgASgLMhkuY29tLmF4bGVib2x0LmJv" + "bHQuUGxheWVyEhgKEGl0ZW1EZWZpbml0aW9uSWQYBCABKAUSDQoFcHJpY2UY" + "BSABKAISEgoKY3JlYXRlRGF0ZRgGIAEoAxIRCgljbG9zZURhdGUYByABKAMS" + "MgoEdHlwZRgIIAEoDjIkLmNvbS5heGxlYm9sdC5ib2x0Lk1hcmtldFJlcXVl" + "c3RUeXBlEioKB3BhcnRuZXIYCSABKAsyGS5jb20uYXhsZWJvbHQuYm9sdC5Q" + "bGF5ZXISGAoQcGFydG5lclJlcXVlc3RJZBgKIAEoCRIwCgZyZWFzb24YCyAB" + "KA4yIC5jb20uYXhsZWJvbHQuYm9sdC5DbG9zaW5nUmVhc29uEhAKCHF1YW50" + "aXR5GAwgASgFEkQKCnByb3BlcnRpZXMYDSADKAsyMC5jb20uYXhsZWJvbHQu" + "Ym9sdC5DbG9zZWRSZXF1ZXN0LlByb3BlcnRpZXNFbnRyeRpbCg9Qcm9wZXJ0" + "aWVzRW50cnkSCwoDa2V5GAEgASgJEjcKBXZhbHVlGAIgASgLMiguY29tLmF4" + "bGVib2x0LmJvbHQuSW52ZW50b3J5SXRlbVByb3BlcnR5OgI4ASKTAwoRUHJv" + "Y2Vzc2luZ1JlcXVlc3QSCgoCaWQYASABKAkSGAoQaXRlbURlZmluaXRpb25J" + "ZBgCIAEoBRINCgVwcmljZRgDIAEoAhISCgpjcmVhdGVEYXRlGAQgASgDEjIK" + "BHR5cGUYBSABKA4yJC5jb20uYXhsZWJvbHQuYm9sdC5NYXJrZXRSZXF1ZXN0" + "VHlwZRIVCg1zYWxlUmVxdWVzdElkGAYgASgJEjEKBXN0YXRlGAcgASgOMiIu" + "Y29tLmF4bGVib2x0LmJvbHQuUHJvY2Vzc2luZ1N0YXRlEhAKCHF1YW50aXR5" + "GAggASgFEkgKCnByb3BlcnRpZXMYCSADKAsyNC5jb20uYXhsZWJvbHQuYm9s" + "dC5Qcm9jZXNzaW5nUmVxdWVzdC5Qcm9wZXJ0aWVzRW50cnkaWwoPUHJvcGVy" + "dGllc0VudHJ5EgsKA2tleRgBIAEoCRI3CgV2YWx1ZRgCIAEoCzIoLmNvbS5h" + "eGxlYm9sdC5ib2x0LkludmVudG9yeUl0ZW1Qcm9wZXJ0eToCOAEiawoFVHJh" + "ZGUSCgoCaWQYASABKAUSEgoKc2FsZXNDb3VudBgCIAEoBRIWCg5wdXJjaGFz" + "ZXNDb3VudBgDIAEoBRISCgpzYWxlc1ByaWNlGAQgASgCEhYKDnB1cmNoYXNl" + "c1ByaWNlGAUgASgCIpkBChVHZXRDbG9zZWRSZXF1ZXN0c0FyZ3MSMgoEdHlw" + "ZRgBIAEoDjIkLmNvbS5heGxlYm9sdC5ib2x0Lk1hcmtldFJlcXVlc3RUeXBl" + "EjAKBnJlYXNvbhgCIAEoDjIgLmNvbS5heGxlYm9sdC5ib2x0LkNsb3NpbmdS" + "ZWFzb24SDAoEcGFnZRgDIAEoBRIMCgRzaXplGAQgASgFIoIBChpHZXRDbG9z" + "ZWRSZXF1ZXN0c0NvdW50QXJncxIyCgR0eXBlGAEgASgOMiQuY29tLmF4bGVi" + "b2x0LmJvbHQuTWFya2V0UmVxdWVzdFR5cGUSMAoGcmVhc29uGAIgASgOMiAu" + "Y29tLmF4bGVib2x0LmJvbHQuQ2xvc2luZ1JlYXNvbiI2ChVDcmVhdGVTYWxl" + "UmVxdWVzdEFyZ3MSDgoGaXRlbUlkGAEgASgFEg0KBXByaWNlGAIgASgCIlYK" + "GUNyZWF0ZVB1cmNoYXNlUmVxdWVzdEFyZ3MSGAoQaXRlbURlZmluaXRpb25J" + "ZBgBIAEoBRINCgVwcmljZRgCIAEoAhIQCghxdWFudGl0eRgDIAEoBSIqChhD" + "cmVhdGVQdXJjaGFzZUJ5U2FsZUFyZ3MSDgoGc2FsZUlkGAEgASgJIkYKHEdl" + "dFRyYWRlT3BlblNhbGVSZXF1ZXN0c0FyZ3MSCgoCaWQYASABKAUSDAoEcGFn" + "ZRgCIAEoBRIMCgRzaXplGAMgASgFIkoKIEdldFRyYWRlT3BlblB1cmNoYXNl" + "UmVxdWVzdHNBcmdzEgoKAmlkGAEgASgFEgwKBHBhZ2UYAiABKAUSDAoEc2l6" + "ZRgDIAEoBSIfChFDYW5jZWxSZXF1ZXN0QXJncxIKCgJpZBgBIAEoCSIaCgxH" + "ZXRUcmFkZUFyZ3MSCgoCaWQYASABKAUiKgoNR2V0VHJhZGVzQXJncxIZChFp" + "dGVtRGVmaW5pdGlvbklkcxgBIAMoBSJsChNNYXJrZXRwbGFjZVNldHRpbmdz" + "EhkKEWNvbW1pc3Npb25QZXJjZW50GAEgASgCEhUKDW1pbkNvbW1pc3Npb24Y" + "AiABKAISEgoKY3VycmVuY3lJZBgDIAEoBRIPCgdlbmFibGVkGAQgASgIIk0K" + "Gk9uUGxheWVyUmVxdWVzdE9wZW5lZEV2ZW50Ei8KB3JlcXVlc3QYASABKAsy" + "Hi5jb20uYXhsZWJvbHQuYm9sdC5PcGVuUmVxdWVzdCJ/ChpPblBsYXllclJl" + "cXVlc3RDbG9zZWRFdmVudBIxCgdyZXF1ZXN0GAEgASgLMiAuY29tLmF4bGVi" + "b2x0LmJvbHQuQ2xvc2VkUmVxdWVzdBIuCgRpdGVtGAIgASgLMiAuY29tLmF4" + "bGVib2x0LmJvbHQuSW52ZW50b3J5SXRlbSJMChlPblRyYWRlUmVxdWVzdE9w" + "ZW5lZEV2ZW50Ei8KB3JlcXVlc3QYASABKAsyHi5jb20uYXhsZWJvbHQuYm9s" + "dC5PcGVuUmVxdWVzdCJOChlPblRyYWRlUmVxdWVzdENsb3NlZEV2ZW50EjEK" + "B3JlcXVlc3QYASABKAsyIC5jb20uYXhsZWJvbHQuYm9sdC5DbG9zZWRSZXF1" + "ZXN0Ij4KE09uVHJhZGVVcGRhdGVkRXZlbnQSJwoFdHJhZGUYASABKAsyGC5j" + "b20uYXhsZWJvbHQuYm9sdC5UcmFkZSpKChFNYXJrZXRSZXF1ZXN0VHlwZRIN" + "CglOT05FX1RZUEUQABIQCgxTQUxFX1JFUVVFU1QQARIUChBQVVJDSEFTRV9S" + "RVFVRVNUEAIqLwoPUHJvY2Vzc2luZ1N0YXRlEgwKCENSRUFUSU5HEAASDgoK" + "Q0FOQ0VMTElORxABKnoKDUNsb3NpbmdSZWFzb24SDwoLTk9ORV9SRUFTT04Q" + "ABIXChNTVUNDRVNTX1RSQU5TQUNUSU9OEAESFAoQTk9UX0VOT1VHSF9GVU5E" + "UxACEg0KCUNBTkNFTExFRBADEhoKFlNBTEVfUkVRVUVTVF9OT1RfRk9VTkQQ" + "BEI1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0LkJv" + "bHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				PlayerMessageReflection.Descriptor,
				InventoryMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[3]
			{
				typeof(MarketRequestType),
				typeof(ProcessingState),
				typeof(ClosingReason)
			}, new GeneratedClrTypeInfo[20]
			{
				new GeneratedClrTypeInfo(typeof(OpenRequest), OpenRequest.Parser, new string[8] { "Id", "Creator", "ItemDefinitionId", "Price", "CreateDate", "Type", "Quantity", "Properties" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(ClosedRequest), ClosedRequest.Parser, new string[13]
				{
					"Id", "OriginId", "Creator", "ItemDefinitionId", "Price", "CreateDate", "CloseDate", "Type", "Partner", "PartnerRequestId",
					"Reason", "Quantity", "Properties"
				}, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(ProcessingRequest), ProcessingRequest.Parser, new string[9] { "Id", "ItemDefinitionId", "Price", "CreateDate", "Type", "SaleRequestId", "State", "Quantity", "Properties" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(Trade), Trade.Parser, new string[5] { "Id", "SalesCount", "PurchasesCount", "SalesPrice", "PurchasesPrice" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetClosedRequestsArgs), GetClosedRequestsArgs.Parser, new string[4] { "Type", "Reason", "Page", "Size" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetClosedRequestsCountArgs), GetClosedRequestsCountArgs.Parser, new string[2] { "Type", "Reason" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CreateSaleRequestArgs), CreateSaleRequestArgs.Parser, new string[2] { "ItemId", "Price" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CreatePurchaseRequestArgs), CreatePurchaseRequestArgs.Parser, new string[3] { "ItemDefinitionId", "Price", "Quantity" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CreatePurchaseBySaleArgs), CreatePurchaseBySaleArgs.Parser, new string[1] { "SaleId" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetTradeOpenSaleRequestsArgs), GetTradeOpenSaleRequestsArgs.Parser, new string[3] { "Id", "Page", "Size" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetTradeOpenPurchaseRequestsArgs), GetTradeOpenPurchaseRequestsArgs.Parser, new string[3] { "Id", "Page", "Size" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CancelRequestArgs), CancelRequestArgs.Parser, new string[1] { "Id" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetTradeArgs), GetTradeArgs.Parser, new string[1] { "Id" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GetTradesArgs), GetTradesArgs.Parser, new string[1] { "ItemDefinitionIds" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(MarketplaceSettings), MarketplaceSettings.Parser, new string[4] { "CommissionPercent", "MinCommission", "CurrencyId", "Enabled" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnPlayerRequestOpenedEvent), OnPlayerRequestOpenedEvent.Parser, new string[1] { "Request" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnPlayerRequestClosedEvent), OnPlayerRequestClosedEvent.Parser, new string[2] { "Request", "Item" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnTradeRequestOpenedEvent), OnTradeRequestOpenedEvent.Parser, new string[1] { "Request" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnTradeRequestClosedEvent), OnTradeRequestClosedEvent.Parser, new string[1] { "Request" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnTradeUpdatedEvent), OnTradeUpdatedEvent.Parser, new string[1] { "Trade" }, null, null, null)
			}));
		}
	}
}
