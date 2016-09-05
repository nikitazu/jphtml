using System;
using System.Xml.Linq;

namespace jphtml.Core.Html
{
    public static class Dom
    {
        public static readonly XElement Br = new XElement("br");

        public static XElement P(params object[] content) => new XElement("p", content);
        public static XElement Span(params object[] content) => new XElement("span", content);
        public static XElement Ruby(params object[] content) => new XElement("ruby", content);
        public static XElement Rt(params object[] content) => new XElement("rt", content);

        public static XAttribute CssClass(string value) => new XAttribute("class", value);

        public static string EnumToCssClass(Enum value) => $"jp-{value.ToString().ToLowerInvariant()}";
    }
}

