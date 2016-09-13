using FluentAssertions;
using JpAnnotator.Common.Portable.PlainText;
using NUnit.Framework;

namespace JpAnnotator.Common.Portable.Tests.PlainText
{
    [TestFixture]
    public class SentenceBreakerTest
    {
        const string _input = "タクシーのラジオは、FM放送のクラシック音楽番組を流していた。曲はヤナーチェックの『シンフォニエッタ』。渋滞に巻き込まれたタクシーの中で聴くのにうってつけの音楽とは言えないはずだ。";
        const string _sentence1 = "タクシーのラジオは、FM放送のクラシック音楽番組を流していた。";
        const string _sentence2 = "曲はヤナーチェックの『シンフォニエッタ』。";
        const string _sentence3 = "渋滞に巻き込まれたタクシーの中で聴くのにうってつけの音楽とは言えないはずだ。";
        SentenceBreaker _breaker;

        [SetUp]
        public void Setup()
        {
            _breaker = new SentenceBreaker();
        }

        [Test]
        public void BreakToSentencesShouldReturnListOfSentences()
        {
            _breaker.BreakToSentences(_input).Should().BeEquivalentTo(_sentence1, _sentence2, _sentence3);
        }
    }
}
