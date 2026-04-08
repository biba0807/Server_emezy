using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class AuthMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static AuthMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJhdXRoX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhRjb21t" + "b25fbWVzc2FnZS5wcm90byIbCglIYW5kc2hha2USDgoGdGlja2V0GAEgASgJ" + "InIKCkF1dGhHb29nbGUSDgoGZ2FtZUlkGAEgASgJEhMKC2dhbWVWZXJzaW9u" + "GAIgASgJEi0KCHBsYXRmb3JtGAMgASgOMhsuY29tLmF4bGVib2x0LmJvbHQu" + "UGxhdGZvcm0SEAoIYXV0aENvZGUYBCABKAkicQoMQXV0aEZhY2Vib29rEg4K" + "BmdhbWVJZBgBIAEoCRITCgtnYW1lVmVyc2lvbhgCIAEoCRItCghwbGF0Zm9y" + "bRgDIAEoDjIbLmNvbS5heGxlYm9sdC5ib2x0LlBsYXRmb3JtEg0KBXRva2Vu" + "GAQgASgJIk8KD0FwcFZlcmlmaWNhdGlvbhIQCghpc1Jvb3RlZBgBIAEoCBIP" + "CgdhcGtIYXNoGAIgASgJEhkKEWpzb25Gb3JiaWRkZW5BcHBzGAMgAygJQjUK" + "GmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Q" + "cm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { CommonMessageReflection.Descriptor }, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[4]
			{
				new GeneratedClrTypeInfo(typeof(Handshake), Handshake.Parser, new string[1] { "Ticket" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(AuthGoogle), AuthGoogle.Parser, new string[4] { "GameId", "GameVersion", "Platform", "AuthCode" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(AuthFacebook), AuthFacebook.Parser, new string[4] { "GameId", "GameVersion", "Platform", "Token" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(AppVerification), AppVerification.Parser, new string[3] { "IsRooted", "ApkHash", "JsonForbiddenApps" }, null, null, null)
			}));
		}
	}
}
