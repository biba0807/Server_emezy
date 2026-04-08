using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class MatchmakingMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static MatchmakingMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChltYXRjaG1ha2luZ19tZXNzYWdlLnByb3RvEhFjb20uYXhsZWJvbHQuYm9s" + "dBoUY29tbW9uX21lc3NhZ2UucHJvdG8aFWZyaWVuZHNfbWVzc2FnZS5wcm90" + "byL7AQoRR2FtZVNlcnZlckRldGFpbHMSCgoCaWQYASABKAkSMQoKZ2FtZVNl" + "cnZlchgCIAEoCzIdLmNvbS5heGxlYm9sdC5ib2x0LkdhbWVTZXJ2ZXISCwoD" + "bWFwGAMgASgJEhYKDmN1cnJlbnRQbGF5ZXJzGAQgASgFEhIKCm1heFBsYXll" + "cnMYBSABKAUSEgoKYm90UGxheWVycxgGIAEoBRIXCg9yZXF1aXJlUGFzc3dv" + "cmQYByABKAgSDwoHdmVyc2lvbhgIIAEoCRIaChJzdWNjZXNzZnVsUmVzcG9u" + "c2UYCSABKAgSFAoMZG9Ob3RSZWZyZXNoGAogASgIIr8ECgVMb2JieRIKCgJp" + "ZBgBIAEoCRIVCg1vd25lclBsYXllcklkGAIgASgJEgwKBG5hbWUYAyABKAkS" + "LwoJbG9iYnlUeXBlGAQgASgOMhwuY29tLmF4bGVib2x0LmJvbHQuTG9iYnlU" + "eXBlEhAKCGpvaW5hYmxlGAUgASgIEhIKCm1heE1lbWJlcnMYBiABKAUSMAoE" + "ZGF0YRgHIAMoCzIiLmNvbS5heGxlYm9sdC5ib2x0LkxvYmJ5LkRhdGFFbnRy" + "eRIwCgdtZW1iZXJzGAggAygLMh8uY29tLmF4bGVib2x0LmJvbHQuUGxheWVy" + "RnJpZW5kEjAKB2ludml0ZXMYCSADKAsyHy5jb20uYXhsZWJvbHQuYm9sdC5Q" + "bGF5ZXJGcmllbmQSMQoKZ2FtZVNlcnZlchgKIAEoCzIdLmNvbS5heGxlYm9s" + "dC5ib2x0LkdhbWVTZXJ2ZXISMQoKcGhvdG9uR2FtZRgLIAEoCzIdLmNvbS5h" + "eGxlYm9sdC5ib2x0LlBob3RvbkdhbWUSFQoNbWF4U3BlY3RhdG9ycxgMIAEo" + "BRIzCgpzcGVjdGF0b3JzGA0gAygLMh8uY29tLmF4bGVib2x0LmJvbHQuUGxh" + "eWVyRnJpZW5kEjkKEHNwZWN0YXRvckludml0ZXMYDiADKAsyHy5jb20uYXhs" + "ZWJvbHQuYm9sdC5QbGF5ZXJGcmllbmQaKwoJRGF0YUVudHJ5EgsKA2tleRgB" + "IAEoCRINCgV2YWx1ZRgCIAEoCToCOAEinAEKC0xvYmJ5SW52aXRlEg8KB2xv" + "YmJ5SWQYASABKAkSNgoNaW52aXRlQ3JlYXRvchgCIAEoCzIfLmNvbS5heGxl" + "Ym9sdC5ib2x0LlBsYXllckZyaWVuZBIMCgRkYXRlGAMgASgDEjYKCnBsYXll" + "clR5cGUYBCABKA4yIi5jb20uYXhsZWJvbHQuYm9sdC5Mb2JieVBsYXllclR5" + "cGUqRQoJTG9iYnlUeXBlEgsKB1BSSVZBVEUQABIQCgxGUklFTkRTX09OTFkQ" + "ARIKCgZQVUJMSUMQAhINCglJTlZJU0lCTEUQAyosCg9Mb2JieVBsYXllclR5" + "cGUSCgoGTUVNQkVSEAASDQoJU1BFQ1RBVE9SEAEqRQoTTG9iYnlEaXN0YW5j" + "ZUZpbHRlchIJCgVDTE9TRRAAEgsKB0RFRkFVTFQQARIHCgNGQVIQAhINCglX" + "T1JMRFdJREUQA0I1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4" + "bGVib2x0LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				CommonMessageReflection.Descriptor,
				FriendsMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[3]
			{
				typeof(LobbyType),
				typeof(LobbyPlayerType),
				typeof(LobbyDistanceFilter)
			}, new GeneratedClrTypeInfo[3]
			{
				new GeneratedClrTypeInfo(typeof(GameServerDetails), GameServerDetails.Parser, new string[10] { "Id", "GameServer", "Map", "CurrentPlayers", "MaxPlayers", "BotPlayers", "RequirePassword", "Version", "SuccessfulResponse", "DoNotRefresh" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Lobby), Lobby.Parser, new string[14]
				{
					"Id", "OwnerPlayerId", "Name", "LobbyType", "Joinable", "MaxMembers", "Data", "Members", "Invites", "GameServer",
					"PhotonGame", "MaxSpectators", "Spectators", "SpectatorInvites"
				}, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(LobbyInvite), LobbyInvite.Parser, new string[4] { "LobbyId", "InviteCreator", "Date", "PlayerType" }, null, null, null)
			}));
		}
	}
}
