using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum ProcessingState
	{
		[OriginalName("CREATING")]
		Creating = 0,
		[OriginalName("CANCELLING")]
		Cancelling = 1
	}
}
