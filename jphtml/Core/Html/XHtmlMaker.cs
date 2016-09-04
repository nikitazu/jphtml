using System.Xml.Linq;
using jphtml.Core.Format;
using jphtml.Core.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace jphtml.Core.Html
{
    public class XHtmlMaker
    {
        static class Tag
        {
            static readonly XNamespace _xhtml = "http://www.w3.org/1999/xhtml";

            public static readonly XName Html = _xhtml + "html";
            public static readonly XName Head = _xhtml + "head";
            public static readonly XName Body = _xhtml + "body";
            public static readonly XName Meta = _xhtml + "meta";
            public static readonly XName Link = _xhtml + "link";
            public static readonly XName Title = _xhtml + "title";
            public static readonly XName Paragraph = _xhtml + "p";
            public static readonly XName Span = _xhtml + "span";
            public static readonly XName Ruby = _xhtml + "ruby";
            public static readonly XName RubyText = _xhtml + "rt";
        }

        static class Attr
        {
            public static readonly XAttribute XLink = new XAttribute(XNamespace.Xmlns + "xlink", "http://www.w3.org/1999/xlink");
            public static readonly XAttribute MathML = new XAttribute(XNamespace.Xmlns + "m", "http://www.w3.org/1998/Math/MathML");
            public static readonly XAttribute Epub = new XAttribute(XNamespace.Xmlns + "epub", "http://www.ipdf.org/2007/ops");
            public static readonly XAttribute Lang = new XAttribute(XNamespace.Xml + "lang", "ru");
        }

        public XElement MakeMeta() => new XElement(
            Tag.Meta,
            new XAttribute("http-equiv", "Content-Type"),
            new XAttribute("content", "application/xhtml+xml; charset=utf-8")
        );

        public XElement MakeRootNode(IEnumerable<XNode> subnodes)
        {
            return new XElement(
                Tag.Html,
                Attr.XLink,
                Attr.MathML,
                Attr.Epub,
                Attr.Lang,
                new XElement(
                    Tag.Head,
                    MakeMeta(),
                    MakeCssLink("style.css"),
                    MakeTitle("JpHtml")
                ),
                new XElement(Tag.Body, subnodes.ToArray())
             );
        }

        public XElement MakeCssLink(string href)
        {
            return new XElement(
                Tag.Link,
                new XAttribute("href", href),
                new XAttribute("rel", "stylesheet"),
                new XAttribute("type", "text/css")
            );
        }

        public XElement MakeTitle(string title)
        {
            return new XElement(Tag.Title, title);
        }

        public XElement MakeParagraph(IEnumerable<XNode> subnodes)
        {
            return new XElement(Tag.Paragraph, JoinTextNodes(subnodes));
        }

        public XElement MakeSpan(IEnumerable<XNode> subnodes)
        {
            return new XElement(Tag.Span, JoinTextNodes(subnodes));
        }

        public XElement MakeRuby(string text, string reading)
        {
            return new XElement(Tag.Ruby, text, new XElement(Tag.RubyText, reading));
        }

        public XNode MakeWord(WordInfo word)
        {
            var furigana = word.Furigana;
            return NoFurigana(word.Text, furigana) ? (XNode)new XText(word.Text) : MakeRuby(word.Text, furigana);
        }

        IEnumerable<XNode> JoinTextNodes(IEnumerable<XNode> nodes)
        {
            StringBuilder builder = null;
            foreach (var node in nodes)
            {
                var textNode = node as XText;
                if (textNode != null && builder == null)
                {
                    builder = new StringBuilder(textNode.Value);
                }
                else if (textNode != null && builder != null)
                {
                    builder.Append($" {textNode.Value}");
                }
                else if (textNode == null && builder == null)
                {
                    yield return node;
                }
                else if (textNode == null && builder != null)
                {
                    yield return new XText(builder.ToString());
                    builder = null;
                    yield return node;
                }
            }
            if (builder != null)
            {
                yield return new XText(builder.ToString());
            }
        }

        bool NoFurigana(string text, string furigana) =>
            string.IsNullOrEmpty(furigana)
              || text == furigana
              || Kana.IsHiragana(text)
              || Kana.IsKatakana(text);
    }
}

