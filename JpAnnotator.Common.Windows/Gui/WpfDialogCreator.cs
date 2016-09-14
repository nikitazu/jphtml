using System;
using JpAnnotator.Common.Portable.Gui;
using Microsoft.Win32;

namespace JpAnnotator.Common.Windows.Gui
{
    public class WpfDialogCreator : IDialogCreator
    {
        void IDialogCreator.Info(string title, string message)
        {
            throw new NotImplementedException();
        }

        bool IDialogCreator.OpenFile(string title, string extensions, out string path)
        {
            path = null;
            var dialog = new OpenFileDialog
            {
                Title = title,
                DefaultExt = extensions,
                Multiselect = false,
            };
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                path = dialog.FileName;
            }
            return result.HasValue && result.Value;
        }

        bool IDialogCreator.SaveFile(string title, string extensions, string filename, out string path)
        {
            throw new NotImplementedException();
        }

        void IDialogCreator.UnexpectedError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
