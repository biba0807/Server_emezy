using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum Platform
	{
		[OriginalName("Unknown")]
		Unknown = 0,
		[OriginalName("Android")]
		Android = 1,
		[OriginalName("IOS")]
		Ios = 2
	}
}
