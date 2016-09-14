using System;
using System.Windows;
using JpAnnotator.Common.Portable.Gui;
using Microsoft.Win32;

namespace JpAnnotator.Common.Windows.Gui
{
    public class WpfDialogCreator : IDialogCreator
    {
        readonly Window _owner;

        public WpfDialogCreator(Window owner)
        {
            _owner = owner;
        }

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
                Filter = extensions,
                Multiselect = false,
            };
            var result = dialog.ShowDialog(_owner);
            if (result.HasValue && result.Value)
            {
                path = dialog.FileName;
            }
            return result.HasValue && result.Value;
        }

        bool IDialogCreator.SaveFile(string title, string extensions, string filename, out string path)
        {
            path = null;
            var dialog = new SaveFileDialog
            {
                Title = title,
                Filter = extensions,
                FileName = filename
            };
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                path = dialog.FileName;
            }
            return result.HasValue && result.Value;
        }

        void IDialogCreator.UnexpectedError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
