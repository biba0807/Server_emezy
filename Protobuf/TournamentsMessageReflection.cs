using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class TournamentsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static TournamentsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("Chl0b3VybmFtZW50c19tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9s" + "dCJmCgpUb3VybmFtZW50EgwKBG5hbWUYASABKAkSDwoHdmVyc2lvbhgCIAEo" + "BRIWCg5taW5HYW1lVmVyc2lvbhgDIAEoCRITCgtkb3dubG9hZFVybBgEIAEo" + "CRIMCgRkYXRlGAUgASgDIjcKBFRlYW0SDAoEbmFtZRgBIAEoCRIQCghwbGF5" + "ZXJJZBgCIAEoCRIPCgdwbGF5ZXJzGAMgAygJKkoKBVN0YXRlEg0KCXByZXBh" + "cmluZxAAEgkKBXJlYWR5EAESCwoHc3RhcnRlZBACEgwKCGZpbmlzaGVkEAMS" + "DAoIY2FuY2VsZWQQBCoZCgRTaWRlEgcKA3JlZBAAEggKBGJsdWUQAUI1Chpj" + "b20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0LkJvbHQuUHJv" + "dG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(State),
				typeof(Side)
			}, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(Tournament), Tournament.Parser, new string[5] { "Name", "Version", "MinGameVersion", "DownloadUrl", "Date" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Team), Team.Parser, new string[3] { "Name", "PlayerId", "Players" }, null, null, null)
			}));
		}
	}
}
