using NUnit.Framework;
using JpAnnotator.Core.Text;
using FluentAssertions;

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
            _hiragana.HiraganaToKatakana().Should().Be(_katakana);
        }

        [Test]
        public void KatakanaToHiraganaShouldConvert()
        {
            _katakana.KatakanaToHiragana().Should().Be(_hiragana);
        }

        [Test]
        public void IsHiraganaShouldReturnTrueForHiragana()
        {
            _hiragana.IsHiragana().Should().BeTrue();
        }

        [Test]
        public void IsHiraganaShouldReturnFalseForNotHiragana()
        {
            "fooあ".IsHiragana().Should().BeFalse();
        }

        [Test]
        public void IsKatakanaShouldReturnTrueForKatakana()
        {
            _katakana.IsKatakana().Should().BeTrue();
        }

        [Test]
        public void IsKatakanaShouldReturnFalseForNotKatakana()
        {
            "fooア".IsKatakana().Should().BeFalse();
        }
    }
}

