using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum ClanClosingReason
	{
		[OriginalName("ACCEPT_REQUEST")]
		AcceptRequest = 0,
		[OriginalName("DECLINE_REQUEST")]
		DeclineRequest = 1,
		[OriginalName("CANCEL_REQUEST")]
		CancelRequest = 2
	}
}
