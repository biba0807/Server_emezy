using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class CrashlogMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static CrashlogMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChZjcmFzaGxvZ19tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9sdBoU" + "Y29tbW9uX21lc3NhZ2UucHJvdG8irwEKCENyYXNoTG9nEhAKCHBsYXllcklk" + "GAEgASgJEhEKCXBsYXllclVpZBgCIAEoCRIOCgZnYW1lSWQYAyABKAkSEwoL" + "Z2FtZVZlcnNpb24YBCABKAkSLQoIcGxhdGZvcm0YBSABKA4yGy5jb20uYXhs" + "ZWJvbHQuYm9sdC5QbGF0Zm9ybRIqCgRsb2dzGAYgAygLMhwuY29tLmF4bGVi" + "b2x0LmJvbHQuU2ltcGxlTG9nIlEKCVNpbXBsZUxvZxIoCgR0eXBlGAEgASgO" + "MhouY29tLmF4bGVib2x0LmJvbHQuTG9nVHlwZRILCgNsb2cYAiABKAkSDQoF" + "Y291bnQYAyABKAUqLgoHTG9nVHlwZRIJCgVERUJVRxAAEgkKBUVSUk9SEAES" + "DQoJRVhDRVBUSU9OEAJCNQoaY29tLmF4bGVib2x0LmJvbHQucHJvdG9idWaq" + "AhZBeGxlYm9sdC5Cb2x0LlByb3RvYnVmYgZwcm90bzM=");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { CommonMessageReflection.Descriptor }, new GeneratedClrTypeInfo(new Type[1] { typeof(LogType) }, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(CrashLog), CrashLog.Parser, new string[6] { "PlayerId", "PlayerUid", "GameId", "GameVersion", "Platform", "Logs" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(SimpleLog), SimpleLog.Parser, new string[3] { "Type", "Log", "Count" }, null, null, null)
			}));
		}
	}
}
