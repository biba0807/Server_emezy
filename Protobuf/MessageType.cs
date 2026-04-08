using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public enum MessageType
    {
        [OriginalName("NONE_TYPE")]
        NoneType = 0,
        [OriginalName("CHAT_MESSAGE")]
        ChatMessage = 1,
        [OriginalName("LOG_MESSAGE")]
        LogMessage = 2
    }
}
