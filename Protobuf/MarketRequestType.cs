using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum MarketRequestType
	{
		[OriginalName("NONE_TYPE")]
		NoneType = 0,
		[OriginalName("SALE_REQUEST")]
		SaleRequest = 1,
		[OriginalName("PURCHASE_REQUEST")]
		PurchaseRequest = 2
	}
}
