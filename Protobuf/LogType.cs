using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum LogType
	{
		[OriginalName("DEBUG")]
		Debug = 0,
		[OriginalName("ERROR")]
		Error = 1,
		[OriginalName("EXCEPTION")]
		Exception = 2
	}
}
