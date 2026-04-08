using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class DlcMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static DlcMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChFkbGNfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQiXwoDRGxj" + "EgwKBG5hbWUYASABKAkSDwoHdmVyc2lvbhgCIAEoBRIWCg5taW5HYW1lVmVy" + "c2lvbhgDIAEoCRITCgtkb3dubG9hZFVybBgEIAEoCRIMCgRkYXRlGAUgASgD" + "QjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9s" + "dC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(Dlc), Dlc.Parser, new string[5] { "Name", "Version", "MinGameVersion", "DownloadUrl", "Date" }, null, null, null)
			}));
		}
	}
}
