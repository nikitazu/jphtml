using System.Collections.Generic;
using JpAnnotator.Core;
using NUnit.Framework;

namespace JpAnnotator.Tests.Core
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
            ((IOptionProvider<IOptionConsumerInputFile>)_options).Provide(new AssertOptionConsumerInputFile());
        }

        [Test]
        public void ProviderEpubShouldProvide()
        {
            ((IOptionProvider<IOptionConsumerEpub>)_options).Provide(new AssertOptionConsumerEpub());
        }

        [Test]
        public void ProviderChapterMarkersShouldProvide()
        {
            ((IOptionProvider<IOptionConsumerChapterMarkers>)_options).Provide(new AssertOptionConsumerChapterMarkers());
        }
    }

    class AssertOptionConsumerInputFile : IOptionConsumerInputFile
    {
        void IOptionConsumerInputFile.Consume(string inputFile)
        {
            Assert.AreEqual("path/to/in", inputFile);
        }
    }

    class AssertOptionConsumerEpub : IOptionConsumerEpub
    {
        void IOptionConsumerEpub.Consume(string author, string bookId, string publisher, string outputFile)
        {
            Assert.AreEqual("murakami", author, nameof(author));
            Assert.AreEqual("666", bookId, nameof(bookId));
            Assert.AreEqual("ZStudios", publisher, nameof(publisher));
            Assert.AreEqual("path/to/out", outputFile, nameof(outputFile));
        }
    }

    class AssertOptionConsumerChapterMarkers : IOptionConsumerChapterMarkers
    {
        void IOptionConsumerChapterMarkers.Consume(IReadOnlyList<string> chapterMarkers)
        {
            Assert.AreEqual(3, chapterMarkers.Count);
            Assert.AreEqual("a", chapterMarkers[0]);
            Assert.AreEqual("b", chapterMarkers[1]);
            Assert.AreEqual("c", chapterMarkers[2]);
        }
    }
}
