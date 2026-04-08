using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum RequestType
	{
		[OriginalName("NONE_TYPE")]
		NoneType = 0,
		[OriginalName("JOIN_REQUEST")]
		JoinRequest = 1,
		[OriginalName("INVITE_REQUEST")]
		InviteRequest = 2
	}
}
