using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public enum ClosingReason
	{
		[OriginalName("NONE_REASON")]
		NoneReason = 0,
		[OriginalName("SUCCESS_TRANSACTION")]
		SuccessTransaction = 1,
		[OriginalName("NOT_ENOUGH_FUNDS")]
		NotEnoughFunds = 2,
		[OriginalName("CANCELLED")]
		Cancelled = 3,
		[OriginalName("SALE_REQUEST_NOT_FOUND")]
		SaleRequestNotFound = 4
	}
}
