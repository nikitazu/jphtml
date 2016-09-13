using System.Collections.Generic;
using FluentAssertions;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.PlainText;
using NSubstitute;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.PlainText
{
    [TestFixture]
    public class ChapterMarkersProviderTest
    {
        IOptionProviderChapterMarkers _options;
        IContentsDetector _detector;
        ChapterMarkersProvider _provider;

        [SetUp]
        public void Setup()
        {
            _options = Substitute.For<IOptionProviderChapterMarkers>();
            _detector = Substitute.For<IContentsDetector>();
            _provider = new ChapterMarkersProvider(_options, _detector);
        }

        [Test]
        public void ProvideChapterMarkersShouldProvideMarkersFromOptionsWhenPresent()
        {
            _options.ChapterMarkers.Returns(new List<string> { "ch1", "ch2" });

            _provider.ProvideChapterMarkers(null).Should().BeEquivalentTo("ch1", "ch2");
        }

        [Test]
        public void ProvideChapterMarkersShouldProvideMarkersFromDetectorWhenOptionsAreEmpty()
        {
            _options.ChapterMarkers.Returns(new List<string>());
            _detector.DetectContents(null).Returns(new List<string> { "chA", "chB" });

            _provider.ProvideChapterMarkers(null).Should().BeEquivalentTo("chA", "chB");
        }
    }
}
