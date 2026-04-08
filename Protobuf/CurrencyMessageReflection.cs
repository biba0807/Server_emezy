using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class CurrencyMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static CurrencyMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChZjdXJyZW5jeV9tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9sdCJM" + "CghDdXJyZW5jeRIKCgJpZBgBIAEoBRIVCg1leGNoYW5nZVJhdGlvGAIgASgC" + "Eh0KFWV4Y2hhbmdhYmxlQ3VycmVuY2llcxgDIAMoBSJJCg5DdXJyZW5jeUFt" + "b3VudBISCgpjdXJyZW5jeUlkGAEgASgFEhQKCG9sZFZhbHVlGAIgASgFQgIY" + "ARINCgV2YWx1ZRgDIAEoAkI1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1" + "ZqoCFkF4bGVib2x0LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(Currency), Currency.Parser, new string[3] { "Id", "ExchangeRatio", "ExchangableCurrencies" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(CurrencyAmount), CurrencyAmount.Parser, new string[3] { "CurrencyId", "OldValue", "Value" }, null, null, null)
			}));
		}
	}
}
