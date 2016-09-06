using System;

namespace JpAnnotator.Common.Portable.Gui
{
    public interface IDialogCreator
    {
        bool OpenFile(string title, string extensions, out string path);

        bool SaveFile(string title, string extensions, string filename, out string path);

        void Info(string title, string message);

        void UnexpectedError(Exception ex);
    }
}
