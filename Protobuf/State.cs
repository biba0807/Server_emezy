using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum State
	{
		[OriginalName("preparing")]
		Preparing = 0,
		[OriginalName("ready")]
		Ready = 1,
		[OriginalName("started")]
		Started = 2,
		[OriginalName("finished")]
		Finished = 3,
		[OriginalName("canceled")]
		Canceled = 4
	}
}
