using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class AnalyticsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static AnalyticsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChdhbmFseXRpY3NfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQi" + "ZAoJVXNlckV2ZW50EhAKCGNhdGVnb3J5GAEgASgJEg0KBWV2ZW50GAIgASgJ" + "Eg0KBXZhbHVlGAMgASgFEhEKCWluY3JlbWVudBgEIAEoCBIUCgxzdG9yZUNv" + "dW50cnkYBSABKAhCNQoaY29tLmF4bGVib2x0LmJvbHQucHJvdG9idWaqAhZB" + "eGxlYm9sdC5Cb2x0LlByb3RvYnVmYgZwcm90bzM=");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(UserEvent), UserEvent.Parser, new string[5] { "Category", "Event", "Value", "Increment", "StoreCountry" }, null, null, null)
			}));
		}
	}
}
