using NUnit.Framework;
using jphtml.Core;

namespace jphtml.Tests
{
    [TestFixture]
    public class OptionsTest
    {
        Options _options;

        [SetUp]
        public void Setup()
        {
            _options = new Options(new string[] {
                "--inputFile", "path/to/in",
                "--outputFile", "path/to/out",
            });
        }

        [Test]
        public void InputFileShouldParse()
        {
            Assert.AreEqual("path/to/in", _options.InputFile);
        }

        [Test]
        public void OutputFileShouldParse()
        {
            Assert.AreEqual("path/to/out", _options.OutputFile);
        }
    }
}

