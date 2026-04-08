using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum PropertySetByType
	{
		[OriginalName("GAME_SERVER")]
		GameServer = 0,
		[OriginalName("OFFICIAL_GAME_SERVER")]
		OfficialGameServer = 1,
		[OriginalName("CLIENT")]
		Client = 2
	}
}
