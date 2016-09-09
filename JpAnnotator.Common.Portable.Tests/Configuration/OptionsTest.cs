using JpAnnotator.Common.Portable.Configuration;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.Configuration
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
                "--author", "murakami",
                "--bookId", "666",
                "--publisher", "ZStudios"
            });
        }

        [Test]
        public void ProviderInputFileShouldProvide()
        {
            new TestProviderInputFile().AssertProviderInputFile(_options);
        }

        [Test]
        public void ProviderEpubShouldProvide()
        {
            new TestProviderEpub().AssertProviderEpub(_options);
        }

        [Test]
        public void ProviderChapterMarkersShouldProvide()
        {
            new TestProviderChapterMarkers().AssertProviderChapterMarkers(_options);
        }
    }

    class TestProviderInputFile
    {
        public void AssertProviderInputFile(IOptionProviderInputFile provider)
        {
            Assert.AreEqual("path/to/in", provider.InputFile);
        }
    }

    class TestProviderEpub
    {
        public void AssertProviderEpub(IOptionProviderEpub provider)
        {
            Assert.AreEqual("murakami", provider.Author, nameof(provider.Author));
            Assert.AreEqual("666", provider.BookId, nameof(provider.BookId));
            Assert.AreEqual("ZStudios", provider.Publisher, nameof(provider.Publisher));
            Assert.AreEqual("path/to/out", provider.OutputFile, nameof(provider.OutputFile));
        }
    }

    class TestProviderChapterMarkers
    {
        public void AssertProviderChapterMarkers(IOptionProviderChapterMarkers provider)
        {
            Assert.AreEqual(3, provider.ChapterMarkers.Count);
            Assert.AreEqual("a", provider.ChapterMarkers[0]);
            Assert.AreEqual("b", provider.ChapterMarkers[1]);
            Assert.AreEqual("c", provider.ChapterMarkers[2]);
        }
    }
}
