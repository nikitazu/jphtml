using System;
using System.IO;
using System.Text;
using jphtml.Core;
using jphtml.Core.Ipc;
using jphtml.Core.Html;
using System.Collections.Generic;
using jphtml.Core.Format;
using jphtml.Core.Dic;

namespace jphtml
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("start");

            var runner = new MecabRunner();
            var reader = new MecabReader();
            var parser = new MecabParser();
            var printer = new HtmlSimplePrinter();
            var dicReader = new JmdicFastReader("../../../data/dic/JMdict_e", new Jmdictionary());

            runner.RunMecab(process =>
            {
                //process.StandardInput.WriteLine("ウィキペディアは誰でも編集できるフリー百科事典です");

                process.StandardInput.WriteLine("タクシーのラジオは、FM放送のクラシック音楽番組を流していた。曲はヤナーチェックの『シンフォニエッタ』。渋滞に巻き込まれたタクシーの中で聴くのにうってつけの音楽とは言えないはずだ。運転手もとくに熱心にその音楽に耳を澄ませているようには見えなかった。中年の運転手は、まるで舳先{へさき}に立って不吉な潮目を読む老練な漁師のように、前方に途切れなく並んだ車の列を、ただ口を閉ざして見つめていた。青豆{あおまめ}は後部席のシートに深くもたれ、軽く目をつむって音楽を聴いていた。");

                var lines = reader.ReadResponse(process.StandardOutput);
                Console.WriteLine(string.Join("\n", lines));

                using (var fileWriter = new StreamWriter("jp.html", false, Encoding.UTF8))
                {
                    printer.PrintDocumentBegin(fileWriter);
                    bool isNewParagraph = true;
                    var words = new List<WordInfo>();
                    foreach (var line in lines)
                    {
                        var word = parser.ParseWord(line);
                        words.Add(word);

                        if (isNewParagraph)
                        {
                            printer.PrintParagraphBegin(fileWriter);
                            isNewParagraph = false;
                        }

                        word.Translation = dicReader.Lookup(word.RootForm);

                        printer.PrintWord(fileWriter, word);

                        if (word.Text.Equals("。"))
                        {
                            printer.PrintParagraphEnd(fileWriter);
                            printer.PrintContextHelp(fileWriter, words);
                            isNewParagraph = true;
                            words.Clear();
                        }

                    }
                    printer.PrintDocumentEnd(fileWriter);
                }
            });

            Console.WriteLine("end");
        }
    }
}
