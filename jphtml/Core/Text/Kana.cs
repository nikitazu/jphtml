using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace jphtml.Core.Text
{
    public static class Kana
    {
        const int DeltaKana = 'ア' - 'あ';

        static Regex _hiraganaRegex = new Regex("^[ぁ-ゔゞ゛゜ー]+$", RegexOptions.Compiled);
        static Regex _katakanaRegex = new Regex("^[ァ-・ヽヾ゛゜ー]+$", RegexOptions.Compiled);

        public static string HiraganaToKatakana(this string hiragana) => Convert(hiragana, c => (char)(c + DeltaKana));

        public static string KatakanaToHiragana(this string katakana) => Convert(katakana, c => (char)(c - DeltaKana));

        public static bool IsHiragana(this string text) => _hiraganaRegex.IsMatch(text);

        public static bool IsKatakana(this string text) => _katakanaRegex.IsMatch(text);

        static string Convert(string input, Func<char, char> conv) => new string(input.Select(conv).ToArray());
    }
}

