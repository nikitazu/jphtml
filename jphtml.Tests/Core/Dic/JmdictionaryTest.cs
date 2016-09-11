using NUnit.Framework;
using JpAnnotator.Core.Dic;
using FluentAssertions;

namespace JpAnnotator.Tests.Core.Dic
{
    [TestFixture]
    public class JmdictionaryTest
    {
        IMultiDictionary _dictionary;

        [SetUp]
        public void Setup()
        {
            _dictionary = new Jmdictionary();
            _dictionary.Append("a", "1");
            _dictionary.Append("b", "2");
            _dictionary.Append("b", "3");
        }

        [Test]
        public void LookupTranslationWhenNone()
        {
            _dictionary.LookupTranslation("x").Should().BeNull();
        }

        [Test]
        public void LookupTranslationWhenOne()
        {
            _dictionary.LookupTranslation("a").Should().Be("1");
        }

        [Test]
        public void LookupTranslationWhenMany()
        {
            _dictionary.LookupTranslation("b").Should().Be("2,3");
        }
    }
}

