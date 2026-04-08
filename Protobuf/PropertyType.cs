using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum PropertyType
	{
		[OriginalName("INT")]
		Int = 0,
		[OriginalName("FLOAT")]
		Float = 1,
		[OriginalName("STRING")]
		String = 2,
		[OriginalName("BOOLEAN")]
		Boolean = 3
	}
}
