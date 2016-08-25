using System;
using System.IO;
using jphtml.Core.Format;

namespace jphtml.Core.Html
{
	public class HtmlPrinter
	{
		public void PrintDocument(TextWriter writer, Func<string> getBody)
		{
			writer.WriteLine(@"<!DOCTYPE html>
<html>
<head>
  <title>JpHtml</title>
</head>
<body>

<style type='text/css'>
.jp-text {
  font-size: 26px;
}

.jp-part:hover {
  background: darkcyan;
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

<p class='jp-text'>
");
			string bodyPart;
			while (!string.IsNullOrEmpty(bodyPart = getBody()))
			{
				writer.WriteLine($"  {bodyPart}");
			}
			writer.WriteLine($@"
</p>
</body>
</html>
");
		}

		public string FormatWord(WordInfo word)
		{
			var cssClass = EnumToCssClass(word.PartOfSpeech);
			return $"<span class='jp-part {cssClass}'>{word.Text}</span>";
		}

		string EnumToCssClass(Enum value) => $"jp-{value.ToString().ToLowerInvariant()}";
	}
}

