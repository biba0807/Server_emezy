using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class ChatMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static ChatMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChJjaGF0X21lc3NhZ2UucHJvdG8SEWNvbS5heGxlYm9sdC5ib2x0GhRncm91" + "cHNfbWVzc2FnZS5wcm90bxoVZnJpZW5kc19tZXNzYWdlLnByb3RvGhRwbGF5" + "ZXJfbWVzc2FnZS5wcm90byKhAQoIQ2hhdFVzZXISLwoGcGxheWVyGAEgASgL" + "Mh8uY29tLmF4bGVib2x0LmJvbHQuUGxheWVyRnJpZW5kEicKBWdyb3VwGAIg" + "ASgLMhguY29tLmF4bGVib2x0LmJvbHQuR3JvdXASDwoHbWVzc2FnZRgDIAEo" + "CRIRCgl0aW1lc3RhbXAYBCABKAMSFwoPdW5yZWFkTXNnc0NvdW50GAUgASgF" + "IlMKC1VzZXJNZXNzYWdlEhAKCHNlbmRlcklkGAEgASgJEg8KB21lc3NhZ2UY" + "AiABKAkSEQoJdGltZXN0YW1wGAMgASgDEg4KBmlzUmVhZBgEIAEoCCJmChVH" + "bG9iYWxDaGF0VXNlck1lc3NhZ2USDwoHbWVzc2FnZRgBIAEoCRIpCgZzZW5k" + "ZXIYAiABKAsyGS5jb20uYXhsZWJvbHQuYm9sdC5QbGF5ZXISEQoJdGltZXN0" + "YW1wGAMgASgDQjUKGmNvbS5heGxlYm9sdC5ib2x0LnByb3RvYnVmqgIWQXhs" + "ZWJvbHQuQm9sdC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[3]
			{
				GroupsMessageReflection.Descriptor,
				FriendsMessageReflection.Descriptor,
				PlayerMessageReflection.Descriptor
			}, new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[3]
			{
				new GeneratedClrTypeInfo(typeof(ChatUser), ChatUser.Parser, new string[5] { "Player", "Group", "Message", "Timestamp", "UnreadMsgsCount" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(UserMessage), UserMessage.Parser, new string[4] { "SenderId", "Message", "Timestamp", "IsRead" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(GlobalChatUserMessage), GlobalChatUserMessage.Parser, new string[3] { "Message", "Sender", "Timestamp" }, null, null, null)
			}));
		}
	}
}
