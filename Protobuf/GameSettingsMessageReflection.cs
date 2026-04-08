using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class GameSettingsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static GameSettingsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChtnYW1lX3NldHRpbmdzX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5i" + "b2x0IpABCgtHYW1lU2V0dGluZxILCgNrZXkYASABKAkSDQoFdmFsdWUYAiAB" + "KAkSEAoIaW50VmFsdWUYAyABKAUSEgoKZmxvYXRWYWx1ZRgEIAEoAhIRCgli" + "b29sVmFsdWUYBSABKAgSLAoEdHlwZRgGIAEoDjIeLmNvbS5heGxlYm9sdC5i" + "b2x0LlNldHRpbmdUeXBlKjsKC1NldHRpbmdUeXBlEgoKBlN0cmluZxAAEgsK" + "B0ludGVnZXIQARIJCgVGbG9hdBACEggKBEJvb2wQA0I1Chpjb20uYXhsZWJv" + "bHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0LkJvbHQuUHJvdG9idWZiBnBy" + "b3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[1] { typeof(SettingType) }, new GeneratedClrTypeInfo[1]
			{
				new GeneratedClrTypeInfo(typeof(GameSetting), GameSetting.Parser, new string[6] { "Key", "Value", "IntValue", "FloatValue", "BoolValue", "Type" }, null, null, null)
			}));
		}
	}
}
