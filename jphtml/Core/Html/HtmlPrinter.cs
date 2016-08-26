using System;
using System.IO;
using jphtml.Core.Format;

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
			writer.WriteLine($"<span class='jp-part {cssClass}'>{RubyText(word)}{ContextHelp(word)}</span>");
		}

		string RubyText(WordInfo word) =>
			word.Text.Equals(word.Reading) ? 
		        $"<ruby>{word.Text}</ruby>" : 
		        $"<ruby>{word.Text}<rt>{word.Reading}</rt></ruby>";

		string ContextHelp(WordInfo word) =>
			$"<span class='jp-contexthelp'>k: {word.Text}<span>r: {word.Reading}</span><span>p: {word.Pronunciation}</span></span>";

		string EnumToCssClass(Enum value) => $"jp-{value.ToString().ToLowerInvariant()}";
	}
}

