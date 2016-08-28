using System.Linq;
using System;

namespace jphtml.Core.Text
{
    public static class Kana
    {
        const int DeltaKana = 'ア' - 'あ';

        public static string HiraganaToKatakana(this string hiragana) => Convert(hiragana, c => (char)(c + DeltaKana));

        public static string KatakanaToHiragana(this string katakana) => Convert(katakana, c => (char)(c - DeltaKana));

        static string Convert(string input, Func<char, char> conv) => new string(input.Select(conv).ToArray());
    }
}

