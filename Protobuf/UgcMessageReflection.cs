using System;
using Google.Protobuf.Reflection;

namespace Axlebolt.Bolt.Protobuf
{
	public static class UgcMessageReflection
	{
		private static FileDescriptor descriptor;

		public static FileDescriptor Descriptor => descriptor;

		static UgcMessageReflection()
		{
			byte[] descriptorData = Convert.FromBase64String("ChF1Z2NfbWVzc2FnZS5wcm90bxIRY29tLmF4bGVib2x0LmJvbHQinQEKA1Vn" + "YxIMCgRuYW1lGAEgASgJEg8KB3ZlcnNpb24YAiABKAkSEwoLZG93bmxvYWRV" + "cmwYAyABKAkSDAoEZGF0ZRgEIAEoAxIQCghhdXRob3JJZBgFIAEoCRIPCgdw" + "cmV2aWV3GAYgASgMEhMKC2Rlc2NyaXB0aW9uGAcgASgJEg4KBnJhdGluZxgI" + "IAEoAhIMCgR0YWdzGAkgAygJIm4KC1VnY0ZlZWRiYWNrEgwKBG5hbWUYASAB" + "KAkSDwoHdmVyc2lvbhgCIAEoCRIQCghhdXRob3JJZBgDIAEoCRIOCgZyYXRp" + "bmcYBCABKAUSEAoIZmVlZGJhY2sYBSABKAkSDAoEZGF0ZRgGIAEoA0I1Chpj" + "b20uYXhsZWJvbHQuYm9sdC5wcm90b2J1ZqoCFkF4bGVib2x0LkJvbHQuUHJv" + "dG9idWZiBnByb3RvMw==");
			descriptor = FileDescriptor.FromGeneratedCode(descriptorData, new FileDescriptor[0], new GeneratedClrTypeInfo(null, new GeneratedClrTypeInfo[2]
			{
				new GeneratedClrTypeInfo(typeof(Ugc), Ugc.Parser, new string[9] { "Name", "Version", "DownloadUrl", "Date", "AuthorId", "Preview", "Description", "Rating", "Tags" }, null, null, null),
				new GeneratedClrTypeInfo(typeof(UgcFeedback), UgcFeedback.Parser, new string[6] { "Name", "Version", "AuthorId", "Rating", "Feedback", "Date" }, null, null, null)
			}));
		}
	}
}
