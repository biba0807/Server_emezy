using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class CommonMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static CommonMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChRjb21tb25fbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQieQoK" + "RGljdGlvbmFyeRI7Cgdjb250ZW50GAEgAygLMiouY29tLmF4bGVib2x0LmJv" + "bHQuRGljdGlvbmFyeS5Db250ZW50RW50cnkaLgoMQ29udGVudEVudHJ5EgsK" + "A2tleRgBIAEoCRINCgV2YWx1ZRgCIAEoCToCOAEiIgoEUGFnZRIMCgRwYWdl" + "GAEgASgFEgwKBHNpemUYAiABKAUiKAoGT2Zmc2V0Eg4KBm9mZnNldBgBIAEo" + "BRIOCgZsZW5ndGgYAiABKAUihAEKBkZpbHRlchIMCgRuYW1lGAEgASgJEhAK" + "CGludFZhbHVlGAIgASgFEhIKCmZsb2F0VmFsdWUYAyABKAISEwoLc3RyaW5n" + "VmFsdWUYBCABKAkSMQoKY29tcGFyaXNvbhgFIAEoDjIdLmNvbS5heGxlYm9s" + "dC5ib2x0LkNvbXBhcmlzb24iKgoMQXZhdGFyQmluYXJ5EgoKAmlkGAEgASgJ" + "Eg4KBmF2YXRhchgCIAEoDCIiChBVc2VyQ29uZmlnQmluYXJ5Eg4KBmNvbmZp" + "ZxgBIAEoDCIyCgpHYW1lU2VydmVyEgoKAmlkGAEgASgJEgoKAmlwGAIgASgJ" + "EgwKBHBvcnQYAyABKAUiyAEKClBob3RvbkdhbWUSDgoGcmVnaW9uGAEgASgJ" + "Eg4KBnJvb21JZBgCIAEoCRISCgphcHBWZXJzaW9uGAMgASgJEk0KEGN1c3Rv" + "bVByb3BlcnRpZXMYBCADKAsyMy5jb20uYXhsZWJvbHQuYm9sdC5QaG90b25H" + "YW1lLkN1c3RvbVByb3BlcnRpZXNFbnRyeRo3ChVDdXN0b21Qcm9wZXJ0aWVz" + "RW50cnkSCwoDa2V5GAEgASgJEg0KBXZhbHVlGAIgASgJOgI4ASqAAQoKQ29t" + "cGFyaXNvbhIZChVFUVVBTF9UT19PUl9MRVNTX1RIQU4QABINCglMRVNTX1RI" + "QU4QARIJCgVFUVVBTBACEhAKDEdSRUFURVJfVEhBThADEhwKGEVRVUFMX1RP" + "X09SX0dSRUFURVJfVEhBThAEEg0KCU5PVF9FUVVBTBAFKi0KCFBsYXRmb3Jt" + "EgsKB1Vua25vd24QABILCgdBbmRyb2lkEAESBwoDSU9TEAIqOwoMUHJvcGVy" + "dHlUeXBlEgcKA0lOVBAAEgkKBUZMT0FUEAESCgoGU1RSSU5HEAISCwoHQk9P" + "TEVBThADQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJv" + "bHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[3]
			{
				typeof(Comparison),
				typeof(Platform),
				typeof(PropertyType)
			}, new GeneratedClrTypeInfo[8]
			{
				new GeneratedClrTypeInfo(typeof(Dictionary), Dictionary.Parser, new string[1] { "Content" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(Page), Page.Parser, new string[2] { "Page_", "Size" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Offset), Offset.Parser, new string[2] { "Offset_", "Length" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Filter), Filter.Parser, new string[5] { "Name", "IntValue", "FloatValue", "StringValue", "Comparison" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(AvatarBinary), AvatarBinary.Parser, new string[2] { "Id", "Avatar" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(UserConfigBinary), UserConfigBinary.Parser, new string[1] { "Config" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GameServer), GameServer.Parser, new string[3] { "Id", "Ip", "Port" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PhotonGame), PhotonGame.Parser, new string[4] { "Region", "RoomId", "AppVersion", "CustomProperties" }, null, null, new GeneratedClrTypeInfo[1])
			}));
		}
	}
}
