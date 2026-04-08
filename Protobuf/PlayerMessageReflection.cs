using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class PlayerMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static PlayerMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChRwbGF5ZXJfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQaFGNv" + "bW1vbl9tZXNzYWdlLnByb3RvIncKClBsYXlJbkdhbWUSEAoIZ2FtZUNvZGUY" + "ASABKAkSEwoLZ2FtZVZlcnNpb24YAiABKAkSDwoHbG9iYnlJZBgDIAEoCRIx" + "CgpwaG90b25HYW1lGAQgASgLMh0uY29tLmF4bGVib2x0LmJvbHQuUGhvdG9u" + "R2FtZSKKAQoMUGxheWVyU3RhdHVzEhAKCHBsYXllcklkGAEgASgJEjEKCnBs" + "YXlJbkdhbWUYAiABKAsyHS5jb20uYXhsZWJvbHQuYm9sdC5QbGF5SW5HYW1l" + "EjUKDG9ubGluZVN0YXR1cxgDIAEoDjIfLmNvbS5heGxlYm9sdC5ib2x0Lk9u" + "bGluZVN0YXR1cyKLAQoKQXR0cmlidXRlcxIzCgNtYXAYASADKAsyJi5jb20u" + "YXhsZWJvbHQuYm9sdC5BdHRyaWJ1dGVzLk1hcEVudHJ5GkgKCE1hcEVudHJ5" + "EgsKA2tleRgBIAEoCRIrCgV2YWx1ZRgCIAEoCzIcLmNvbS5heGxlYm9sdC5i" + "b2x0LkF0dHJpYnV0ZToCOAEidwoJQXR0cmlidXRlEi0KBHR5cGUYASABKA4y" + "Hy5jb20uYXhsZWJvbHQuYm9sdC5Qcm9wZXJ0eVR5cGUSCwoDaW50GAIgASgF" + "Eg0KBWZsb2F0GAMgASgCEg4KBnN0cmluZxgEIAEoCRIPCgdib29sZWFuGAUg" + "ASgIIu0BCgZQbGF5ZXISCgoCaWQYASABKAkSCwoDdWlkGAIgASgJEgwKBG5h" + "bWUYAyABKAkSEAoIYXZhdGFySWQYBCABKAkSEgoKdGltZUluR2FtZRgFIAEo" + "BRI1CgxwbGF5ZXJTdGF0dXMYBiABKAsyHy5jb20uYXhsZWJvbHQuYm9sdC5Q" + "bGF5ZXJTdGF0dXMSEgoKbG9nb3V0RGF0ZRgHIAEoAxIYChByZWdpc3RyYXRp" + "b25EYXRlGAggASgDEjEKCmF0dHJpYnV0ZXMYCSABKAsyHS5jb20uYXhsZWJv" + "bHQuYm9sdC5BdHRyaWJ1dGVzImAKGU9uUGxheWVyQXR0cmlidXRlc0NoYW5n" + "ZWQSEAoIcGxheWVySWQYASABKAkSMQoKYXR0cmlidXRlcxgCIAEoCzIdLmNv" + "bS5heGxlYm9sdC5ib2x0LkF0dHJpYnV0ZXMqTwoIQXV0aFR5cGUSCAoEVEVT" + "VBAAEgkKBUdVRVNUEAESDwoLR09PR0xFX1BMQVkQAhIPCgtHQU1FX0NFTlRF" + "UhADEgwKCEZBQ0VCT09LEAQqkQEKDE9ubGluZVN0YXR1cxIQCgxTdGF0ZU9m" + "ZmxpbmUQABIPCgtTdGF0ZU9ubGluZRABEg0KCVN0YXRlQnVzeRACEg0KCVN0" + "YXRlQXdheRADEg8KC1N0YXRlU25vb3plEAQSFwoTU3RhdGVMb29raW5nVG9U" + "cmFkZRAFEhYKElN0YXRlTG9va2luZ1RvUGxheRAGQjUKGmNvbS5heGxlYm9s" + "dC5ib2x0LnByb3RvYnVmqgIWQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJv" + "dG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[1] { CommonMessageReflection.Descriptor }, new GeneratedClrTypeInfo(new Type[2]
			{
				typeof(AuthType),
				typeof(OnlineStatus)
			}, new GeneratedClrTypeInfo[6]
			{
				new GeneratedClrTypeInfo(typeof(PlayInGame), PlayInGame.Parser, new string[4] { "GameCode", "GameVersion", "LobbyId", "PhotonGame" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(PlayerStatus), PlayerStatus.Parser, new string[3] { "PlayerId", "PlayInGame", "OnlineStatus" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Attributes), Attributes.Parser, new string[1] { "Map" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(Attribute), Attribute.Parser, new string[5] { "Type", "Int", "Float", "String", "Boolean" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Player), Player.Parser, new string[9] { "Id", "Uid", "Name", "AvatarId", "TimeInGame", "PlayerStatus", "LogoutDate", "RegistrationDate", "Attributes" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnPlayerAttributesChanged), OnPlayerAttributesChanged.Parser, new string[2] { "PlayerId", "Attributes" }, null, null, null)
			}));
		}
	}
}
