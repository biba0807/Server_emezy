using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.RpcSupport.Protobuf
{
	public static class RpcMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static RpcMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChFycGNfbWVzc2FnZS5wcm90bxIUc3RhbmRvZmYucnBjLnN1cHBvcnQidAoK" + "UnBjUmVxdWVzdBIKCgJpZBgBIAEoCRITCgtzZXJ2aWNlTmFtZRgCIAEoCRIS" + "CgptZXRob2ROYW1lGAMgASgJEjEKBnBhcmFtcxgEIAMoCzIhLnN0YW5kb2Zm" + "LnJwYy5zdXBwb3J0LkJpbmFyeVZhbHVlIoUBCg9SZXNwb25zZU1lc3NhZ2US" + "NgoLcnBjUmVzcG9uc2UYASABKAsyIS5zdGFuZG9mZi5ycGMuc3VwcG9ydC5S" + "cGNSZXNwb25zZRI6Cg1ldmVudFJlc3BvbnNlGAIgASgLMiMuc3RhbmRvZmYu" + "cnBjLnN1cHBvcnQuRXZlbnRSZXNwb25zZSKcAQoNUXVldWVkTWVzc2FnZRI2" + "CgdtZXNzYWdlGAEgASgLMiUuc3RhbmRvZmYucnBjLnN1cHBvcnQuUmVzcG9u" + "c2VNZXNzYWdlEg4KBmJvbHRJZBgCIAEoCRIRCgl0aW1lc3RhbXAYAyABKAMS" + "DgoGdXNlcklkGAQgAygJEg0KBXRvcGljGAUgASgJEhEKCWltcG9ydGFudBgG" + "IAEoCCKAAQoLUnBjUmVzcG9uc2USCgoCaWQYASABKAkSMgoJZXhjZXB0aW9u" + "GAIgASgLMh8uc3RhbmRvZmYucnBjLnN1cHBvcnQuRXhjZXB0aW9uEjEKBnJl" + "dHVybhgDIAEoCzIhLnN0YW5kb2ZmLnJwYy5zdXBwb3J0LkJpbmFyeVZhbHVl" + "IpEBCglFeGNlcHRpb24SCgoCaWQYASABKAMSDAoEY29kZRgCIAEoBRI7CgZw" + "YXJhbXMYAyADKAsyKy5zdGFuZG9mZi5ycGMuc3VwcG9ydC5FeGNlcHRpb24u" + "UGFyYW1zRW50cnkaLQoLUGFyYW1zRW50cnkSCwoDa2V5GAEgASgJEg0KBXZh" + "bHVlGAIgASgJOgI4ASJrCg1FdmVudFJlc3BvbnNlEhQKDGxpc3RlbmVyTmFt" + "ZRgBIAEoCRIRCglldmVudE5hbWUYAiABKAkSMQoGcGFyYW1zGAMgAygLMiEu" + "c3RhbmRvZmYucnBjLnN1cHBvcnQuQmluYXJ5VmFsdWUiOQoLQmluYXJ5VmFs" + "dWUSDgoGaXNOdWxsGAEgASgIEg0KBWFycmF5GAIgAygMEgsKA29uZRgDIAEo" + "DCIXCgZTdHJpbmcSDQoFdmFsdWUYASABKAkiHAoLU3RyaW5nQXJyYXkSDQoF" + "dmFsdWUYASADKAkiGAoHSW50ZWdlchINCgV2YWx1ZRgBIAEoBSIdCgxJbnRl" + "Z2VyQXJyYXkSDQoFdmFsdWUYASADKAUiFQoETG9uZxINCgV2YWx1ZRgBIAEo" + "AyIaCglMb25nQXJyYXkSDQoFdmFsdWUYASADKAMiFwoGRG91YmxlEg0KBXZh" + "bHVlGAEgASgBIhwKC0RvdWJsZUFycmF5Eg0KBXZhbHVlGAEgAygBIhYKBUZs" + "b2F0Eg0KBXZhbHVlGAEgASgCIhsKCkZsb2F0QXJyYXkSDQoFdmFsdWUYASAD" + "KAIiGAoHQm9vbGVhbhINCgV2YWx1ZRgBIAEoCCIdCgxCb29sZWFuQXJyYXkS" + "DQoFdmFsdWUYASADKAgiFQoEQnl0ZRINCgV2YWx1ZRgBIAEoDCIaCglCeXRl" + "QXJyYXkSDQoFdmFsdWUYASABKAwiFQoERW51bRINCgV2YWx1ZRgBIAEoBUI6" + "Chljb20uYXhsZWJvbHQucnBjLnByb3RvYnVmqgIcQXhsZWJvbHQuUnBjU3Vw" + "cG9ydC5Qcm90b2J1ZmIGcHJvdG8z");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[22]
			{
				new GeneratedClrTypeInfo(typeof(RpcRequest), RpcRequest.Parser, new string[4] { "Id", "ServiceName", "MethodName", "Params" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ResponseMessage), ResponseMessage.Parser, new string[2] { "RpcResponse", "EventResponse" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(QueuedMessage), QueuedMessage.Parser, new string[6] { "Message", "BoltId", "Timestamp", "UserId", "Topic", "Important" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(RpcResponse), RpcResponse.Parser, new string[3] { "Id", "Exception", "Return" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Exception), Exception.Parser, new string[3] { "Id", "Code", "Params" }, null, null, new GeneratedClrTypeInfo[1]),
				new GeneratedClrTypeInfo(typeof(EventResponse), EventResponse.Parser, new string[3] { "ListenerName", "EventName", "Params" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(BinaryValue), BinaryValue.Parser, new string[3] { "IsNull", "Array", "One" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(String), String.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(StringArray), StringArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Integer), Integer.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(IntegerArray), IntegerArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Long), Long.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(LongArray), LongArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Double), Double.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(DoubleArray), DoubleArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Float), Float.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(FloatArray), FloatArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Boolean), Boolean.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(BooleanArray), BooleanArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Byte), Byte.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(ByteArray), ByteArray.Parser, new string[1] { "Value" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(Enum), Enum.Parser, new string[1] { "Value" }, null, null, null)
			}));
		}
	}
}
