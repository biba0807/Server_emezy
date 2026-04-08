using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
    public enum JoinClanType
    {
        [OriginalName("BY_ACCEPT_INVITE_REQUEST")]
        ByAcceptInviteRequest = 0,
        [OriginalName("BY_ACCEPT_JOIN_REQUEST")]
        ByAcceptJoinRequest = 1,
        [OriginalName("JOIN_TO_OPEN_CLAN")]
        JoinToOpenClan = 2,
        [OriginalName("NONE_JOIN_TYPE")]
        NoneJoinType = 3
    }
}
