using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using jphtml.Core.Format;

namespace jphtml.Core.Html
{
    public class HtmlSimplePrinter
    {
        public void PrintDocumentBegin(TextWriter writer)
        {
            writer.WriteLine(@"<!DOCTYPE html>
<html>
<head>
  <title>JpHtml</title>
</head>
<body>

<style type='text/css'>
.jp-text {
  font-size: 30px;
}

.jp-part:hover {
  background: #99C3D1;
}

.jp-contexthelp {
  display: block;
  font-size: 25px;
}

.jp-contexthelp > span {
  display: block;
}

.jp-contexthelp .jp-translation {
  font-size: 16px;
}

.jp-particle {
  color: red;
}

.jp-noun {
  color: darkblue;
}

.jp-auxillaryverb {
  color: orange;
}
</style>
");
        }

        public void PrintDocumentEnd(TextWriter writer)
        {
            writer.WriteLine($@"
</body>
</html>
");
        }

        public void PrintParagraphBegin(TextWriter writer)
        {
            writer.WriteLine("<p class='jp-text'>");
        }

        public void PrintParagraphEnd(TextWriter writer)
        {
            writer.WriteLine("</p>");
        }

        public void PrintWord(TextWriter writer, WordInfo word)
        {
            var cssClass = EnumToCssClass(word.PartOfSpeech);
            var part = Span(CssClass($"jp-part {cssClass}"), RubyWord(word));
            writer.WriteLine(part);
        }

        public void PrintContextHelp(TextWriter writer, IEnumerable<WordInfo> words)
        {
            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word.Translation))
                {
                    var help = ContextHelp(word);
                    writer.WriteLine(help);
                }
            }
        }

        XElement RubyWord(WordInfo word) =>
            word.Text.Equals(word.Furigana) || string.IsNullOrEmpty(word.Furigana) ?
                Ruby(word.Text) :
                Ruby(word.Text, Rt(word.Furigana));

        XElement ContextHelp(WordInfo word) => P(
            CssClass("jp-contexthelp"),
            ContextField(
                $"{word.TextMaybeRootForm} [{word.Furigana}] - ",
                $"({FormatSpeechInfo(word)}) {word.Translation}"));

        string FormatSpeechInfo(WordInfo word) =>
            string.Join("|", word.SpeechInfo.Select(v => v.ToString()).Where(s => !"None".Equals(s)));

        XElement ContextField(string value, string translation) => Span(value, Span(CssClass("jp-translation"), translation));

        string EnumToCssClass(Enum value) => $"jp-{value.ToString().ToLowerInvariant()}";

        XElement P(params object[] content) => new XElement("p", content);
        XElement Span(params object[] content) => new XElement("span", content);
        XElement Ruby(params object[] content) => new XElement("ruby", content);
        XElement Rt(params object[] content) => new XElement("rt", content);
        XAttribute CssClass(string value) => new XAttribute("class", value);
    }
}


