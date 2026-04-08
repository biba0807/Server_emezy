using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum ClanType
	{
		[OriginalName("CLOSED")]
		Closed = 0,
		[OriginalName("INVITE_ONLY")]
		InviteOnly = 1,
		[OriginalName("OPEN")]
		Open = 2
	}
}
