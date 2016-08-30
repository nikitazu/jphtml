using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Epub4Net;
using Epub4Net.NCX;
using Epub4Net.OPF;
using Ionic.Zip;
using Ionic.Zlib;

namespace jphtml.Core.BuildEpub.Epub4Net
{
    public class Epub4NetFileSystemManager : IFileSystemManager
    {
        public string ContentDir { get; set; }
        public string BuildDirectory { get; set; }

        public Epub4NetFileSystemManager(string directory)
        {
            BuildDirectory = directory;
        }

        public void SetupOutputDir()
        {
            if (Directory.Exists(BuildDirectory))
            {
                Directory.Delete(BuildDirectory, true);
            }
            Directory.CreateDirectory(BuildDirectory);

            var metainfdir = Path.Combine(BuildDirectory, "META-INF");
            ContentDir = Path.Combine(BuildDirectory, "OEBPS");
            Directory.CreateDirectory(metainfdir);
            Directory.CreateDirectory(ContentDir);

            AddMimeTypeFile();
            AddContainerFile();
        }


        private void AddContainerFile()
        {
            const string fileResource = "Epub4Net.Files.container.xml";
            var targetFile = Path.Combine(BuildDirectory, "META-INF", "container.xml");
            CopyEmbeddedFileToFileSystem(fileResource, targetFile);
        }

        private void AddMimeTypeFile()
        {
            const string fileResource = "Epub4Net.Files.mimetype";
            var targetFile = Path.Combine(BuildDirectory, "mimetype");
            CopyEmbeddedFileToFileSystem(fileResource, targetFile);
        }

        private static void CopyEmbeddedFileToFileSystem(string fileResource, string targetFile)
        {
            var assembly = typeof(IFileSystemManager).Assembly;
            using (var stream = assembly.GetManifestResourceStream(fileResource))
            {
                if (stream != null)
                {
                    using (var fs = new FileStream(targetFile, FileMode.Create))
                    {
                        stream.CopyTo(fs);
                        fs.Close();
                        stream.Close();
                    }
                }
                else
                {
                    System.Console.WriteLine("FUKI");
                    throw new FileNotFoundException(fileResource + " was not found in assembly");
                }
            }
        }

        public void CreateTocFile(Epub epub)
        {
            var toc = new Toc(epub.Title, epub.BookId, epub.Chapters);
            var tocString = toc.XmlSerialze();
            var tocPath = Path.Combine(ContentDir, "toc.ncx");
            File.WriteAllText(tocPath, tocString, new UTF8Encoding(false));
        }

        public void CreateContentOpfFile(Epub epub)
        {
            var opfPackage = new OPFPackage(epub);
            var xml = opfPackage.XmlSerialize();
            var contentOpfPath = Path.Combine(ContentDir, "content.opf");
            File.WriteAllText(contentOpfPath, xml, new UTF8Encoding(false));
        }

        public void CopyChapterFilesToContentFolder(Epub epub)
        {
            foreach (var chapter in epub.Chapters)
            {
                File.Copy(chapter.Path, Path.Combine(ContentDir, chapter.FileName), true);
            }
        }

        public string ZipEpub(Epub epub)
        {
            var epubFilename = epub.Title + ".epub";
            if (File.Exists(epubFilename)) { File.Delete(epubFilename); }
            using (var zip = new ZipFile(epubFilename, Encoding.UTF8))
            {
                zip.EmitTimesInWindowsFormatWhenSaving = false;
                zip.CompressionLevel = CompressionLevel.None;
                zip.AddFile(Path.Combine(BuildDirectory, "mimetype"), Path.DirectorySeparatorChar.ToString());
                zip.Save();
                File.Delete(Path.Combine(BuildDirectory, "mimetype"));
                zip.AddDirectory(BuildDirectory);
                zip.Save();
            }
            return epubFilename;
        }

        public void CopyResourceFilesToContentFolder(Epub epub)
        {
            foreach (var file in epub.ResourceFiles)
            {
                var destPath = Path.Combine(ContentDir, file.FileName);
                File.Copy(file.Path, destPath, true);
            }
        }

        public void ValidatePathsExists(IEnumerable<IPathed> fileList)
        {
            foreach (var pathed in fileList)
            {
                string path = pathed.Path;
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Could not find the file", path);
                }
            }
        }

        public void DeleteBuildDir()
        {
            if (Directory.Exists(BuildDirectory))
            {
                Directory.Delete(BuildDirectory, true);
            }
        }
    }
}

