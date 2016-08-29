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
                "--chapterMarkers", "a,b,c",
                "--simulation", "true"
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
            Assert.AreEqual("path/to/out", _options.OutputDir);
        }

        [Test]
        public void ChapterMarkersShouldParse()
        {
            Assert.AreEqual(3, _options.ChapterMarkers.Count);
            Assert.AreEqual("a", _options.ChapterMarkers[0]);
            Assert.AreEqual("b", _options.ChapterMarkers[1]);
            Assert.AreEqual("c", _options.ChapterMarkers[2]);
        }

        [Test]
        public void SimulationShouldParse()
        {
            Assert.AreEqual(true, _options.Simulation);
        }
    }
}

