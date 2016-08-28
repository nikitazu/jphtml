using NUnit.Framework;
using jphtml.Core.Text;

namespace jphtml.Tests
{
    [TestFixture]
    public class KanaTest
    {
        const string _hiragana = "あいうえお";
        const string _katakana = "アイウエオ";

        [Test]
        public void HiraganaToKatakanaShouldConvert()
        {
            Assert.AreEqual(_katakana, _hiragana.HiraganaToKatakana());
        }

        [Test]
        public void KatakanaToHiraganaShouldConvert()
        {
            Assert.AreEqual(_hiragana, _katakana.KatakanaToHiragana());
        }
    }
}

