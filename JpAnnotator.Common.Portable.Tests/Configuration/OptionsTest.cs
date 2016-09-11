using JpAnnotator.Common.Portable.Configuration;
using NUnit.Framework;
using FluentAssertions;

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
            _options = new Options(
                "--inputFile", "path/to/in",
                "--outputFile", "path/to/out",
                "--author", "murakami",
                "--bookId", "666",
                "--publisher", "ZStudios",
                "--chapterMarkers", "a,b,c"
            );
        }

        [Test]
        public void ProviderInputFileShouldProvide()
        {
            InputFileProvider.InputFile.Should().Be("path/to/in");
        }

        [Test]
        public void ProviderEpubShouldProvide()
        {
            EpubProvider.Author.Should().Be("murakami");
            EpubProvider.BookId.Should().Be("666");
            EpubProvider.Publisher.Should().Be("ZStudios");
            EpubProvider.OutputFile.Should().Be("path/to/out");
        }

        [Test]
        public void ProviderChapterMarkersShouldProvide()
        {
            ChapterMarkersProvider.ChapterMarkers.Should().BeEquivalentTo("a", "b", "c");
        }
    }
}
