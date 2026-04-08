using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum StoreCode
	{
		[OriginalName("GOOGLE_PLAY_STORE")]
		GooglePlayStore = 0,
		[OriginalName("APPLE_APP_STORE")]
		AppleAppStore = 1,
		[OriginalName("AMAZON_STORE")]
		AmazonStore = 2
	}
}
