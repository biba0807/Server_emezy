using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class PlayerStatsMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static PlayerStatsMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChpwbGF5ZXJfc3RhdHNfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJv" + "bHQiaQoFU3RhdHMSKwoEc3RhdBgBIAMoCzIdLmNvbS5heGxlYm9sdC5ib2x0" + "LlBsYXllclN0YXQSMwoLYWNoaWV2ZW1lbnQYAiADKAsyHi5jb20uYXhsZWJv" + "bHQuYm9sdC5BY2hpZXZlbWVudCJ+CgpQbGF5ZXJTdGF0EgwKBG5hbWUYASAB" + "KAkSEAoIaW50VmFsdWUYAiABKAUSEgoKZmxvYXRWYWx1ZRgDIAEoAhIOCgZ3" + "aW5kb3cYBCABKAISLAoEdHlwZRgFIAEoDjIeLmNvbS5heGxlYm9sdC5ib2x0" + "LlN0YXREZWZUeXBlIo0BCgtQbGF5ZXJTdGF0cxIQCghwbGF5ZXJJZBgBIAEo" + "CRIxCgVzdGF0cxgCIAMoCzIiLmNvbS5heGxlYm9sdC5ib2x0LlN0b3JlUGxh" + "eWVyU3RhdBI5CgxhY2hpZXZlbWVudHMYAyADKAsyIy5jb20uYXhsZWJvbHQu" + "Ym9sdC5TdG9yZUFjaGlldmVtZW50IpABCgtBY2hpZXZlbWVudBIMCgRuYW1l" + "GAEgASgJEhMKC2Rpc3BsYXlOYW1lGAIgASgJEhoKEmRpc3BsYXlEZXNjcmlw" + "dGlvbhgDIAEoCRISCgp1bmxvY2tUaW1lGAQgASgDEhAKCGFjaGlldmVkGAUg" + "ASgIEgwKBGljb24YBiABKAwSDgoGaGlkZGVuGAcgASgIIkUKD1N0b3JlUGxh" + "eWVyU3RhdBIMCgRuYW1lGAEgASgJEhAKCHN0b3JlSW50GAIgASgFEhIKCnN0" + "b3JlRmxvYXQYAyABKAIiMgoQU3RvcmVBY2hpZXZlbWVudBIMCgRuYW1lGAEg" + "ASgJEhAKCGFjaGlldmVkGAIgASgIKi4KC1N0YXREZWZUeXBlEgcKA0lOVBAA" + "EgkKBUZMT0FUEAESCwoHQVZHUkFURRACQjUKGmNvbS5heGxlYm9sdC5ib2x0" + "LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(new Type[1] { typeof(StatDefType) }, new GeneratedClrTypeInfo[6]
			{
				new GeneratedClrTypeInfo(typeof(Stats), Stats.Parser, new string[2] { "Stat", "Achievement" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PlayerStat), PlayerStat.Parser, new string[5] { "Name", "IntValue", "FloatValue", "Window", "Type" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PlayerStats), PlayerStats.Parser, new string[3] { "PlayerId", "Stats", "Achievements" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Achievement), Achievement.Parser, new string[7] { "Name", "DisplayName", "DisplayDescription", "UnlockTime", "Achieved", "Icon", "Hidden" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(StorePlayerStat), StorePlayerStat.Parser, new string[3] { "Name", "StoreInt", "StoreFloat" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(StoreAchievement), StoreAchievement.Parser, new string[2] { "Name", "Achieved" }, null, null, null)
			}));
		}
	}
}
