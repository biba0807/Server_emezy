using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class ClanMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static ClanMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJjbGFuX21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhRwbGF5" + "ZXJfbWVzc2FnZS5wcm90bxoWY3VycmVuY3lfbWVzc2FnZS5wcm90byKVAQoE" + "Q2xhbhIKCgJpZBgBIAEoCRIMCgRuYW1lGAIgASgJEgsKA3RhZxgDIAEoCRIt" + "CghjbGFuVHlwZRgEIAEoDjIbLmNvbS5heGxlYm9sdC5ib2x0LkNsYW5UeXBl" + "EhAKCGF2YXRhcklkGAUgASgJEhIKCmNyZWF0ZURhdGUYBiABKAMSEQoJY2xh" + "bkxldmVsGAcgASgFIpgBCgpDbGFuTWVtYmVyEgoKAmlkGAEgASgJEikKBnBs" + "YXllchgCIAEoCzIZLmNvbS5heGxlYm9sdC5ib2x0LlBsYXllchIOCgZjbGFu" + "SWQYAyABKAkSLwoEcm9sZRgEIAEoCzIhLmNvbS5heGxlYm9sdC5ib2x0LkNs" + "YW5NZW1iZXJSb2xlEhIKCmNyZWF0ZURhdGUYBSABKAMiRgoPQ2xhbk9wZW5S" + "ZXF1ZXN0EjMKC2NsYW5SZXF1ZXN0GAEgASgLMh4uY29tLmF4bGVib2x0LmJv" + "bHQuQ2xhblJlcXVlc3QimAEKEUNsYW5DbG9zZWRSZXF1ZXN0EjMKC2NsYW5S" + "ZXF1ZXN0GAEgASgLMh4uY29tLmF4bGVib2x0LmJvbHQuQ2xhblJlcXVlc3QS" + "EQoJY2xvc2VEYXRlGAIgASgDEjsKDWNsb3NpbmdSZWFzb24YAyABKA4yJC5j" + "b20uYXhsZWJvbHQuYm9sdC5DbGFuQ2xvc2luZ1JlYXNvbiLtAQoLQ2xhblJl" + "cXVlc3QSCgoCaWQYASABKAkSJQoEY2xhbhgCIAEoCzIXLmNvbS5heGxlYm9s" + "dC5ib2x0LkNsYW4SMAoNcmVxdWVzdFNlbmRlchgDIAEoCzIZLmNvbS5heGxl" + "Ym9sdC5ib2x0LlBsYXllchISCgpjcmVhdGVEYXRlGAQgASgDEjMKC3JlcXVl" + "c3RUeXBlGAUgASgOMh4uY29tLmF4bGVib2x0LmJvbHQuUmVxdWVzdFR5cGUS" + "MAoNaW52aXRlZFBsYXllchgGIAEoCzIZLmNvbS5heGxlYm9sdC5ib2x0LlBs" + "YXllciKRAQoOQ2xhbk1lbWJlclJvbGUSCgoCaWQYASABKAkSDAoEbmFtZRgC" + "IAEoCRINCgVsZXZlbBgDIAEoBRIUCgxkZXNjcmlwcHRpb24YBCABKAkSQAoL" + "cGVybWlzc2lvbnMYBSADKA4yKy5jb20uYXhsZWJvbHQuYm9sdC5DbGFuTWVt" + "YmVyUm9sZVBlcm1pc3Npb24i5gEKCUNsYW5MZXZlbBITCgtsZXZlbE51bWJl" + "chgBIAEoBRIXCg9tYXhNZW1iZXJzQ291bnQYAiABKAUSNgoLbGV2ZWxVcENv" + "c3QYAyABKAsyIS5jb20uYXhsZWJvbHQuYm9sdC5DdXJyZW5jeUFtb3VudBJA" + "Cgpwcm9wZXJ0aWVzGAQgAygLMiwuY29tLmF4bGVib2x0LmJvbHQuQ2xhbkxl" + "dmVsLlByb3BlcnRpZXNFbnRyeRoxCg9Qcm9wZXJ0aWVzRW50cnkSCwoDa2V5" + "GAEgASgJEg0KBXZhbHVlGAIgASgJOgI4ASJHCg9DbGFuVXNlck1lc3NhZ2US" + "EAoIc2VuZGVySWQYASABKAkSDwoHbWVzc2FnZRgCIAEoCRIRCgl0aW1lc3Rh" + "bXAYAyABKAMiVwoXT25Kb2luUmVxdWVzdFRha2VuRXZlbnQSEQoJcmVxdWVz" + "dElkGAEgASgJEikKBnBsYXllchgCIAEoCzIZLmNvbS5heGxlYm9sdC5ib2x0" + "LlBsYXllciI8ChNPbkpvaW5lZFRvQ2xhbkV2ZW50EiUKBGNsYW4YASABKAsy" + "Fy5jb20uYXhsZWJvbHQuYm9sdC5DbGFuIk4KGU9uTWVtYmVySm9pbmVkVG9D" + "bGFuRXZlbnQSMQoKY2xhbk1lbWJlchgBIAEoCzIdLmNvbS5heGxlYm9sdC5i" + "b2x0LkNsYW5NZW1iZXIiRQoTT25LaWNrZWRNZW1iZXJFdmVudBIWCg5raWNr" + "ZXJNZW1iZXJJZBgBIAEoCRIWCg5raWNrZWRNZW1iZXJJZBgCIAEoCSJ7ChRP" + "bkludml0ZWRUb0NsYW5FdmVudBIRCglyZXF1ZXN0SWQYASABKAkSJQoEY2xh" + "bhgCIAEoCzIXLmNvbS5heGxlYm9sdC5ib2x0LkNsYW4SKQoGcGxheWVyGAMg" + "ASgLMhkuY29tLmF4bGVib2x0LmJvbHQuUGxheWVyIoABChNPbkFzc2lnbmVk" + "Um9sZUV2ZW50EjIKB25ld1JvbGUYASABKAsyIS5jb20uYXhsZWJvbHQuYm9s" + "dC5DbGFuTWVtYmVyUm9sZRIaChJhc3NpZ25hdG9yTWVtYmVySWQYAiABKAkS" + "GQoRYXNzaWduaW5nTWVtYmVySWQYAyABKAkiugEKEU9uUmVxdWVzdERlY2xp" + "bmVkEhEKCXJlcXVlc3RJZBgBIAEoCRIzCgtyZXF1ZXN0VHlwZRgCIAEoDjIe" + "LmNvbS5heGxlYm9sdC5ib2x0LlJlcXVlc3RUeXBlEisKCmNsYW5Ub0pvaW4Y" + "AyABKAsyFy5jb20uYXhsZWJvbHQuYm9sdC5DbGFuEjAKDWludml0ZWRQbGF5" + "ZXIYBCABKAsyGS5jb20uYXhsZWJvbHQuYm9sdC5QbGF5ZXIiRQoRT25DbGFu" + "VHlwZUNoYW5nZWQSMAoLbmV3Q2xhblR5cGUYASABKA4yGy5jb20uYXhsZWJv" + "bHQuYm9sdC5DbGFuVHlwZSIkChFPbkNsYW5OYW1lQ2hhbmdlZBIPCgduZXdO" + "YW1lGAEgASgJIiYKDU9uS2lja2VkRXZlbnQSFQoNa2lja2luZ1JlYXNvbhgB" + "IAEoCSIiCg5PbkxlZnRGcm9tQ2xhbhIQCghtZW1iZXJJZBgBIAEoCSJMChVP" + "bkluY29taW5nQ2xhbk1lc3NhZ2USMwoHbWVzc2FnZRgBIAEoCzIiLmNvbS5h" + "eGxlYm9sdC5ib2x0LkNsYW5Vc2VyTWVzc2FnZSJFChJPbkNsYW5VcGdyYWRl" + "RXZlbnQSLwoJY2xhbkxldmVsGAEgASgLMhwuY29tLmF4bGVib2x0LmJvbHQu" + "Q2xhbkxldmVsKjEKCENsYW5UeXBlEgoKBkNMT1NFRBAAEg8KC0lOVklURV9P" + "TkxZEAESCAoET1BFThACKlAKEUNsYW5DbG9zaW5nUmVhc29uEhIKDkFDQ0VQ" + "VF9SRVFVRVNUEAASEwoPREVDTElORV9SRVFVRVNUEAESEgoOQ0FOQ0VMX1JF" + "UVVFU1QQAipCCgtSZXF1ZXN0VHlwZRINCglOT05FX1RZUEUQABIQCgxKT0lO" + "X1JFUVVFU1QQARISCg5JTlZJVEVfUkVRVUVTVBACKvQBChhDbGFuTWVtYmVy" + "Um9sZVBlcm1pc3Npb24SGAoUQ0hBTkdFX0NMQU5fU0VUVElOR1MQABIRCg1B" + "Q0NFUFRfTUVNQkVSEAESEQoNSU5WSVRFX01FTUJFUhACEhQKEEtJQ0tfTUVN" + "QkVSX0xFU1MQAxIVChFLSUNLX01FTUJFUl9FUVVBTBAEEhQKEEFTU0lHTl9S" + "T0xFX0xFU1MQBRIVChFBU1NJR05fUk9MRV9FUVVBTBAGEhYKEkNSRUFURV9D" + "TEFOX0JBVFRMRRAHEhQKEEpPSU5fQ0xBTl9CQVRUTEUQCBIQCgxVUEdSQURF" + "X0NMQU4QCUI1Chpjb20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVi" + "b2x0LkJvbHQuUHJvdG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[2]
			{
				PlayerMessageReflection.Descriptor,
				CurrencyMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(new Type[4]
			{
				typeof(ClanType),
				typeof(ClanClosingReason),
				typeof(RequestType),
				typeof(ClanMemberRolePermission)
			}, new GeneratedClrTypeInfo[21]
			{
				new GeneratedClrTypeInfo(typeof(Clan), Clan.Parser, new string[7] { "Id", "Name", "Tag", "ClanType", "AvatarId", "CreateDate", "ClanLevel" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanMember), ClanMember.Parser, new string[5] { "Id", "Player", "ClanId", "Role", "CreateDate" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanOpenRequest), ClanOpenRequest.Parser, new string[1] { "ClanRequest" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanClosedRequest), ClanClosedRequest.Parser, new string[3] { "ClanRequest", "CloseDate", "ClosingReason" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanRequest), ClanRequest.Parser, new string[6] { "Id", "Clan", "RequestSender", "CreateDate", "RequestType", "InvitedPlayer" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanMemberRole), ClanMemberRole.Parser, new string[5] { "Id", "Name", "Level", "Descripption", "Permissions" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ClanLevel), ClanLevel.Parser, new string[4] { "LevelNumber", "MaxMembersCount", "LevelUpCost", "Properties" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(ClanUserMessage), ClanUserMessage.Parser, new string[3] { "SenderId", "Message", "Timestamp" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnJoinRequestTakenEvent), OnJoinRequestTakenEvent.Parser, new string[2] { "RequestId", "Player" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnJoinedToClanEvent), OnJoinedToClanEvent.Parser, new string[1] { "Clan" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnMemberJoinedToClanEvent), OnMemberJoinedToClanEvent.Parser, new string[1] { "ClanMember" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnKickedMemberEvent), OnKickedMemberEvent.Parser, new string[2] { "KickerMemberId", "KickedMemberId" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnInvitedToClanEvent), OnInvitedToClanEvent.Parser, new string[3] { "RequestId", "Clan", "Player" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnAssignedRoleEvent), OnAssignedRoleEvent.Parser, new string[3] { "NewRole", "AssignatorMemberId", "AssigningMemberId" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnRequestDeclined), OnRequestDeclined.Parser, new string[4] { "RequestId", "RequestType", "ClanToJoin", "InvitedPlayer" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnClanTypeChanged), OnClanTypeChanged.Parser, new string[1] { "NewClanType" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnClanNameChanged), OnClanNameChanged.Parser, new string[1] { "NewName" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnKickedEvent), OnKickedEvent.Parser, new string[1] { "KickingReason" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnLeftFromClan), OnLeftFromClan.Parser, new string[1] { "MemberId" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnIncomingClanMessage), OnIncomingClanMessage.Parser, new string[1] { "Message" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(OnClanUpgradeEvent), OnClanUpgradeEvent.Parser, new string[1] { "ClanLevel" }, null, null, null)
			}));
		}
	}
}
