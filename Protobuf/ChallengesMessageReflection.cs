using System;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public static class ChallengesMessageReflection
    {
        private static FileDescriptor descriptor;

        public static FileDescriptor Descriptor => descriptor;

        static ChallengesMessageReflection()
        {
            descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChdDaGFsbGVuZ2VzTWVzc2FnZS5wcm90bxIXQXhsZWJvbHQuQm9sdC5Qcm90" + "b2J1ZjIaE0NvbW1vbk1lc3NhZ2UucHJvdG8aFUN1cnJlbmN5TWVzc2FnZS5w" + "cm90bxoWSW52ZW50b3J5TWVzc2FnZS5wcm90byLmAgoQQ3VycmVudENoYWxs" + "ZW5nZRIcChRnYW1lRXZlbnRDaGFsbGVuZ2VJZBgBIAEoCRIMCgRjb2RlGAIg" + "ASgJEhsKE2tleUl0ZW1EZWZpbml0aW9uSWQYAyABKAUSPwoObG9jYWxpemVk" + "VGl0bGUYBCABKAsyJy5BeGxlYm9sdC5Cb2x0LlByb3RvYnVmMi5Mb2NhbGl6" + "ZWRUaXRsZRIOCgZhY3Rpb24YBSABKAkSMwoIZGF5UmFuZ2UYBiABKAsyIS5B" + "eGxlYm9sdC5Cb2x0LlByb3RvYnVmMi5EYXlSYW5nZRIMCgR0eXBlGAcgASgJ" + "EhMKC2V2ZW50UG9pbnRzGAggASgFEhQKDHRhcmdldFBvaW50cxgJIAEoBRIV" + "Cg1jdXJyZW50UG9pbnRzGAogASgFEjMKBnJld2FyZBgLIAEoCzIjLkF4bGVi" + "b2x0LkJvbHQuUHJvdG9idWYyLlJld2FyZEluZm8iRQobR2V0Q3VycmVudENo" + "YWxsZW5nZXNSZXF1ZXN0EhMKC2dhbWVFdmVudElkGAEgASgJEhEKCWNvbXBs" + "ZXRlZBgCIAEoCCJdChxHZXRDdXJyZW50Q2hhbGxlbmdlc1Jlc3BvbnNlEj0K" + "CmNoYWxsZW5nZXMYASADKAsyKS5BeGxlYm9sdC5Cb2x0LlByb3RvYnVmMi5D" + "dXJyZW50Q2hhbGxlbmdlIkgKGFByb2dyZXNzQ2hhbGxlbmdlUmVxdWVzdBIc" + "ChRnYW1lRXZlbnRDaGFsbGVuZ2VJZBgBIAEoCRIOCgZwb2ludHMYAiABKAUi" + "8gIKGVByb2dyZXNzQ2hhbGxlbmdlUmVzcG9uc2USEQoJY29tcGxldGVkGAEg" + "ASgIEhcKD2NoYWxsZW5nZVBvaW50cxgCIAEoBRI4Cg9jaGFsbGVuZ2VSZXdh" + "cmQYAyABKAsyHy5BeGxlYm9sdC5Cb2x0LlByb3RvYnVmMi5SZXdhcmQSEwoL" + "RXZlbnRQb2ludHMYBCABKAUSaAoTZXZlbnRHYW1lUGFzc0xldmVscxgFIAMo" + "CzJLLkF4bGVib2x0LkJvbHQuUHJvdG9idWYyLlByb2dyZXNzQ2hhbGxlbmdl" + "UmVzcG9uc2UuRXZlbnRHYW1lUGFzc0xldmVsc0VudHJ5EjQKC0V2ZW50UmV3" + "YXJkGAYgASgLMh8uQXhsZWJvbHQuQm9sdC5Qcm90b2J1ZjIuUmV3YXJkGjoK" + "GEV2ZW50R2FtZVBhc3NMZXZlbHNFbnRyeRILCgNrZXkYASABKAkSDQoFdmFs" + "dWUYAiABKAU6AjgBYgZwcm90bzM="), new FileDescriptor[3]
            {
                CommonMessageReflection.Descriptor,
                CurrencyMessageReflection.Descriptor,
                InventoryMessageReflection.Descriptor
            }, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[5]
            {
                new GeneratedClrTypeInfo(typeof(CurrentChallenge), CurrentChallenge.Parser, new string[11]
                {
                    "GameEventChallengeId", "Code", "KeyItemDefinitionId", "LocalizedTitle", "Action", "DayRange", "Type", "EventPoints", "TargetPoints", "CurrentPoints",
                    "Reward"
                }, null, null, null),
                new GeneratedClrTypeInfo(typeof(GetCurrentChallengesRequest), GetCurrentChallengesRequest.Parser, new string[2] { "GameEventId", "Completed" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(GetCurrentChallengesResponse), GetCurrentChallengesResponse.Parser, new string[1] { "Challenges" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(ProgressChallengeRequest), ProgressChallengeRequest.Parser, new string[2] { "GameEventChallengeId", "Points" }, null, null, null),
                new GeneratedClrTypeInfo(typeof(ProgressChallengeResponse), ProgressChallengeResponse.Parser, new string[6] { "Completed", "ChallengePoints", "ChallengeReward", "EventPoints", "EventGamePassLevels", "EventReward" }, null, null, new GeneratedClrTypeInfo[1])
            }));
        }
    }
}
