using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class GameserverMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static GameserverMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChhnYW1lc2VydmVyX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0" + "IkIKD1NlcnZlckhhbmRzaGFrZRIOCgZnYW1lSWQYASABKAkSDgoGYXBpS2V5" + "GAIgASgJEg8KB3ZlcnNpb24YAyABKAlCNQoaY29tLmF4bGVib2x0LmJvbHQu" + "cHJvdG9idWaqAhZBeGxlYm9sdC5Cb2x0LlByb3RvYnVmYgZwcm90bzM=");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(ServerHandshake), ServerHandshake.Parser, new string[3] { "GameId", "ApiKey", "Version" }, null, null, null)
			}));
		}
	}
}
