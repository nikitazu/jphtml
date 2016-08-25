using NUnit.Framework;
using System.IO;
using jphtml.Core;

namespace jphtml.Tests.Core
{
	[TestFixture]
	public class MecabReaderTest
	{
		MecabReader _reader;

		[SetUp]
		public void Setup()
		{
			_reader = new MecabReader();
		}

		[Test]
		public void ReadResponseShouldReturnResponseUntioEOS()
		{
			using (var input = new StringReader("abc\nxyz\nEOS\n"))
			{
				var result = _reader.ReadResponse(input);
				Assert.AreEqual(result.ToString(), "abc\nxyz\n");
			}
		}
	}
}

