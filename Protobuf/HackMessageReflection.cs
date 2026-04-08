using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class HackMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static HackMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJoYWNrX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0IlEKDUhh" + "Y2tEZXRlY3Rpb24SDwoHYXBrSGFzaBgBIAEoCRIQCghpc1Jvb3RlZBgCIAEo" + "CBIMCgRqc29uGAMgAygJEg8KB21vZERhdGUYBCABKANCNQoaY29tLmF4bGVi" + "b2x0LmJvbHQucHJvdG9idWaqAhZBeGxlYm9sdC5Cb2x0LlByb3RvYnVmYgZw" + "cm90bzM=");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(HackDetection), HackDetection.Parser, new string[4] { "ApkHash", "IsRooted", "Json", "ModDate" }, null, null, null)
			}));
		}
	}
}
