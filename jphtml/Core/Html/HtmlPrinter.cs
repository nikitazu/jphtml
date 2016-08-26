using System;
using System.IO;
using System.Xml.Linq;
using jphtml.Core.Format;
using System.Linq;

namespace jphtml.Core.Html
{
    public class HtmlPrinter
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

.jp-text {
  outline: 1px solid blue;
}

.jp-part:hover {
  background: #99C3D1;
}

.jp-part > .jp-contexthelp {
  display: none;
}

.jp-part:hover > .jp-contexthelp {
  display: block;
  position: absolute;
  left: 71%;
  top: 10px;
  font-size: 35px;
  font-family: monospace;
}

.jp-part:hover > .jp-contexthelp > span {
  display: block;
}

.jp-part:hover > .jp-contexthelp .jp-contextlable {
  font-size: 10px;
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

<div style='display:inline-block;margin:0;padding:0;width:70%;border:1px solid red;'>
");
        }

        public void PrintDocumentEnd(TextWriter writer)
        {
            writer.WriteLine($@"
</div>

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
            var part = Span(CssClass($"jp-part {cssClass}"), RubyWord(word), ContextHelp(word));
            writer.WriteLine(part);
        }

        XElement RubyWord(WordInfo word) =>
            word.Text.Equals(word.Furigana) || string.IsNullOrEmpty(word.Furigana) ?
                Ruby(word.Text) :
                Ruby(word.Text, Rt(word.Furigana));

        XElement ContextHelp(WordInfo word) => Span(
            CssClass("jp-contexthelp"),
            ContextField("text", word.Text),
            ContextFieldMaybe("furigana", word.Furigana),
            ContextFieldMaybeStar("root", word.RootForm),
            ContextFieldMaybeStar("conj", word.Conjugation),
            ContextFieldMaybeStar("infl", word.Inflection),
            ContextLablePartOfSpeech(word.PartOfSpeech, word.Subclass1, word.Subclass2, word.Subclass3));

        XElement ContextLablePartOfSpeech(params Enum[] values) =>
            ContextLable(string.Join("|", values.Select(v => v.ToString()).Where(s => !"None".Equals(s))));

        XElement ContextFieldMaybeStar(string name, string value) => "*" == value ? null : ContextFieldMaybe(name, value);
        XElement ContextFieldMaybe(string name, string value) => string.IsNullOrEmpty(value) ? null : ContextField(name, value);
        XElement ContextField(string name, string value) => Span(ContextLable(name), Br, value);
        XElement ContextLable(string name) => Span(CssClass("jp-contextlable"), name);

        string EnumToCssClass(Enum value) => $"jp-{value.ToString().ToLowerInvariant()}";

        static readonly XElement Br = new XElement("br");
        XElement Span(params object[] content) => new XElement("span", content);
        XElement Ruby(params object[] content) => new XElement("ruby", content);
        XElement Rt(params object[] content) => new XElement("rt", content);
        XAttribute CssClass(string value) => new XAttribute("class", value);
    }
}

