using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public static class GameEventMessageReflection
    {
        private static FileDescriptor descriptor;

        public static FileDescriptor Descriptor => descriptor;

        static GameEventMessageReflection()
        {
            byte[] descriptorData = Convert.FromBase64String(
                "ChhnYW1lX2V2ZW50X21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0" +
                "GhRjb21tb25fbWVzc2FnZS5wcm90bxoXaW52ZW50b3J5X21lc3NhZ2UucHJv" +
                "dG8aFmN1cnJlbmN5X21lc3NhZ2UucHJvdG8iiQEKCEdhbWVQYXNzEgoKAmlk" +
                "GAEgASgJEgwKBGNvZGUYAiABKAkSGwoTa2V5SXRlbURlZmluaXRpb25JZBgD" +
                "IAEoBRIwCgZsZXZlbHMYBCADKAsyIC5jb20uYXhsZWJvbHQuYm9sdC5HYW1l" +
                "UGFzc0xldmVsEhQKDGN1cnJlbnRMZXZlbBgFIAEoBSJgCg1HYW1lUGFzc0xl" +
                "dmVsEg0KBWxldmVsGAEgASgFEhEKCW1pblBvaW50cxgCIAEoBRItCgZyZXdh" +
                "cmQYAyABKAsyHS5jb20uYXhsZWJvbHQuYm9sdC5SZXdhcmRJbmZvIr0BChBD" +
                "dXJyZW50R2FtZUV2ZW50EgoKAmlkGAEgASgJEgwKBGNvZGUYAiABKAkSEQoJ" +
                "ZGF0ZVNpbmNlGAMgASgDEhEKCWRhdGVVbnRpbBgEIAEoAxIUCgxkdXJhdGlv" +
                "bkRheXMYBSABKAUSLwoKZ2FtZVBhc3NlcxgGIAMoCzIbLmNvbS5heGxlYm9s" +
                "dC5ib2x0LkdhbWVQYXNzEg4KBnBvaW50cxgHIAEoBRISCgpjdXJyZW50RGF5" +
                "GAggASgFIlcKHEdldEN1cnJlbnRHYW1lRXZlbnRzUmVzcG9uc2USNwoKZ2Ft" +
                "ZUV2ZW50cxgBIAMoCzIjLmNvbS5heGxlYm9sdC5ib2x0LkN1cnJlbnRHYW1l" +
                "RXZlbnQiPwoYUHJvZ3Jlc3NHYW1lRXZlbnRSZXF1ZXN0EhMKC2dhbWVFdmVu" +
                "dElkGAEgASgJEg4KBnBvaW50cxgCIAEoBSLPAQoZUHJvZ3Jlc3NHYW1lRXZl" +
                "bnRSZXNwb25zZRIOCgZwb2ludHMYASABKAUSSAoGbGV2ZWxzGAIgAygLMjgu" +
                "Y29tLmF4bGVib2x0LmJvbHQuUHJvZ3Jlc3NHYW1lRXZlbnRSZXNwb25zZS5M" +
                "ZXZlbHNFbnRyeRIpCgZyZXdhcmQYAyABKAsyGS5jb20uYXhsZWJvbHQuYm9s" +
                "dC5SZXdhcmQaLQoLTGV2ZWxzRW50cnkSCwoDa2V5GAEgASgJEg0KBXZhbHVl" +
                "GAIgASgFOgI4ASLaAQoWT25HYW1lUGFzc0NoYW5nZWRFdmVudBIPCgdldmVu" +
                "dElkGAEgASgJEg4KBnBvaW50cxgCIAEoBRJFCgZsZXZlbHMYAyADKAsyNS5j" +
                "b20uYXhsZWJvbHQuYm9sdC5PbkdhbWVQYXNzQ2hhbmdlZEV2ZW50LkxldmVs" +
                "c0VudHJ5EikKBnJld2FyZBgEIAEoCzIZLmNvbS5heGxlYm9sdC5ib2x0LlJl" +
                "d2FyZBotCgtMZXZlbHNFbnRyeRILCgNrZXkYASABKAkSDQoFdmFsdWUYAiAB" +
                "KAU6AjgBQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhsZWJv" +
                "bHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z"
            );

            FileDescriptor[] dependencies = new FileDescriptor[]
            {
                CommonMessageReflection.Descriptor,
                InventoryMessageReflection.Descriptor,
                CurrencyMessageReflection.Descriptor
            };

            GeneratedClrTypeInfo[] generatedClrTypes = new GeneratedClrTypeInfo[]
            {
                new GeneratedClrTypeInfo(typeof(GamePass), GamePass.Parser, new[] { "Id", "Code", "KeyItemDefinitionId", "Levels", "CurrentLevel" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(GamePassLevel), GamePassLevel.Parser, new[] { "Level", "MinPoints", "Reward" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(CurrentGameEvent), CurrentGameEvent.Parser, new[] { "Id", "Code", "DateSince", "DateUntil", "DurationDays", "GamePasses", "Points", "CurrentDay" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(GetCurrentGameEventsResponse), GetCurrentGameEventsResponse.Parser, new[] { "GameEvents" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(ProgressGameEventRequest), ProgressGameEventRequest.Parser, new[] { "GameEventId", "Points" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(ProgressGameEventResponse), ProgressGameEventResponse.Parser, new[] { "Points", "Levels", "Reward" }, null, null, null),
                    //new[] { new GeneratedClrTypeInfo(typeof(ProgressGameEventResponse.Types.LevelsEntry), ProgressGameEventResponse.Types.LevelsEntry.Parser, new[] { "Key", "Value" }, null, null, null) }, null),
                new GeneratedClrTypeInfo(typeof(OnGamePassChangedEvent), OnGamePassChangedEvent.Parser, new[] { "EventId", "Points", "Levels", "Reward" }, null, null, null)
                    //new[] { new GeneratedClrTypeInfo(typeof(OnGamePassChangedEvent.Types.LevelsEntry), OnGamePassChangedEvent.Types.LevelsEntry.Parser, new[] { "Key", "Value" }, null, null, null) }, null)
            };

            descriptor = FileDescriptor.FromGeneratedCode(descriptorData, dependencies, new GeneratedClrTypeInfo(null, generatedClrTypes));
        }
    }
}