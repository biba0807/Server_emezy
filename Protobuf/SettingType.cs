using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum SettingType
	{
		[OriginalName("String")]
		String = 0,
		[OriginalName("Integer")]
		Integer = 1,
		[OriginalName("Float")]
		Float = 2,
		[OriginalName("Bool")]
		Bool = 3
	}
}
