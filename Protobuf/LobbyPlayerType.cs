using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum LobbyPlayerType
	{
		[OriginalName("MEMBER")]
		Member = 0,
		[OriginalName("SPECTATOR")]
		Spectator = 1
	}
}
