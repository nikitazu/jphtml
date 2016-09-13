using System.Collections.Generic;
using FluentAssertions;
using JpAnnotator.Common.Portable.Configuration;
using JpAnnotator.Common.Portable.PlainText;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.PlainText
{
    [TestFixture]
    public class ChapterMarkersProviderTest
    {
        ChapterMarkersProvider _provider;
        ChapterMarkersProvider _emptyOptionsProvider;
        IOptionProviderChapterMarkers _fullOptions;
        IOptionProviderChapterMarkers _emptyOptions;
        IContentsDetector _detector;

        [SetUp]
        public void Setup()
        {
            _fullOptions = new TestMarkersProvider();
            _emptyOptions = new TestEmptyMarkersProvider();
            _detector = new TestDetector();
            _provider = new ChapterMarkersProvider(_fullOptions, _detector);
            _emptyOptionsProvider = new ChapterMarkersProvider(_emptyOptions, _detector);
        }

        [Test]
        public void ProvideChapterMarkersShouldProvideMarkersFromOptionsWhenPresent()
        {
            _provider.ProvideChapterMarkers(null).Should().BeEquivalentTo("ch1", "ch2");
        }

        [Test]
        public void ProvideChapterMarkersShouldProvideMarkersFromDetectorWhenOptionsAreEmpty()
        {
            _emptyOptionsProvider.ProvideChapterMarkers(null).Should().BeEquivalentTo("chA", "chB");
        }
    }

    class TestMarkersProvider : IOptionProviderChapterMarkers
    {
        IReadOnlyList<string> IOptionProviderChapterMarkers.ChapterMarkers => new List<string>
        {
            "ch1", "ch2"
        };
    }

    class TestEmptyMarkersProvider : IOptionProviderChapterMarkers
    {
        IReadOnlyList<string> IOptionProviderChapterMarkers.ChapterMarkers => new List<string>();
    }

    class TestDetector : IContentsDetector
    {
        IEnumerable<string> IContentsDetector.DetectContents(List<string> textLines)
        {
            return new List<string> { "chA", "chB" };
        }
    }
}

