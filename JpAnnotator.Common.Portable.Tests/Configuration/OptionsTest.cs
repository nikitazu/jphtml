using JpAnnotator.Common.Portable.Configuration;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.Configuration
{
    [TestFixture]
    public class OptionsTest
    {
        Options _options;
        IOptionProviderInputFile InputFileProvider => _options;
        IOptionProviderEpub EpubProvider => _options;
        IOptionProviderChapterMarkers ChapterMarkersProvider => _options;

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
            Assert.AreEqual("path/to/in", InputFileProvider.InputFile);
        }

        [Test]
        public void ProviderEpubShouldProvide()
        {
            Assert.AreEqual("murakami", EpubProvider.Author, nameof(EpubProvider.Author));
            Assert.AreEqual("666", EpubProvider.BookId, nameof(EpubProvider.BookId));
            Assert.AreEqual("ZStudios", EpubProvider.Publisher, nameof(EpubProvider.Publisher));
            Assert.AreEqual("path/to/out", EpubProvider.OutputFile, nameof(EpubProvider.OutputFile));
        }

        [Test]
        public void ProviderChapterMarkersShouldProvide()
        {
            Assert.AreEqual(3, ChapterMarkersProvider.ChapterMarkers.Count);
            Assert.AreEqual("a", ChapterMarkersProvider.ChapterMarkers[0]);
            Assert.AreEqual("b", ChapterMarkersProvider.ChapterMarkers[1]);
            Assert.AreEqual("c", ChapterMarkersProvider.ChapterMarkers[2]);
        }
    }
}
