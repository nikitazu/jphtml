using NUnit.Framework;
using JpAnnotator.Core.Text;

namespace JpAnnotator.Tests
{
    [TestFixture]
    public class KanaTest
    {
        const string _hiragana =
            "あいうえおかきくけこがぎぐげごたちつてとだぢづでどさしすせそざじずぜぞはひふへほぱぴぷぺぽばびぶべぼまみむめもなにぬねのらりるれろわをやゆよゃゅょぁぃぇぉっん";

        const string _katakana =
            "アイウエオカキクケコガギグゲゴタチツテトダヂヅデドサシスセソザジズゼゾハヒフヘホパピプペポバビブベボマミムメモナニヌネノラリルレロワヲヤユヨャュョァィェォッン";

        // ー ?

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

        [Test]
        public void IsHiraganaShouldReturnTrueForHiragana()
        {
            Assert.IsTrue(_hiragana.IsHiragana());
        }

        [Test]
        public void IsHiraganaShouldReturnFalseForNotHiragana()
        {
            Assert.IsFalse("fooあ".IsHiragana());
        }

        [Test]
        public void IsKatakanaShouldReturnTrueForKatakana()
        {
            Assert.IsTrue(_katakana.IsKatakana());
        }

        [Test]
        public void IsKatakanaShouldReturnFalseForNotKatakana()
        {
            Assert.IsFalse("fooア".IsKatakana());
        }
    }
}

