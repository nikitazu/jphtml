using NUnit.Framework;
using jphtml.Core.Dic;

namespace jphtml.Tests
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
            Assert.IsNull(_dictionary.LookupTranslation("x"));
        }

        [Test]
        public void LookupTranslationWhenOne()
        {
            Assert.AreEqual("1", _dictionary.LookupTranslation("a"));
        }

        [Test]
        public void LookupTranslationWhenMany()
        {
            Assert.AreEqual("2,3", _dictionary.LookupTranslation("b"));
        }
    }
}

