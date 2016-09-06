using System;
using JpAnnotator.Common.Portable.Gui;
using AppKit;

namespace JpAnnotator.Common.Mac.Gui
{
    public class CocoaDialogCreator : IDialogCreator
    {
        bool IDialogCreator.OpenFile(string title, string extensions, out string path)
        {
            path = null;

            var openDialog = NSOpenPanel.OpenPanel;
            openDialog.Title = title;
            openDialog.CanChooseFiles = true;
            openDialog.CanChooseDirectories = false;
            openDialog.AllowsMultipleSelection = false;
            openDialog.AllowedFileTypes = extensions.Split(',');

            if (openDialog.RunModal() == 1)
            {
                var url = openDialog.Urls[0];

                if (url != null)
                {
                    path = url.Path;
                }
            }

            return path != null;
        }

        bool IDialogCreator.SaveFile(string title, string extensions, string filename, out string path)
        {
            path = null;

            var saveDialog = new NSSavePanel()
            {
                Title = title,
                AllowedFileTypes = extensions.Split(',')
            };

            if (!string.IsNullOrWhiteSpace(filename))
            {
                saveDialog.NameFieldStringValue = filename;
            }

            if (saveDialog.RunModal() == 1 && saveDialog.Url != null)
            {
                path = saveDialog.Url.Path;
            }

            return path != null;
        }

        void IDialogCreator.Info(string title, string message)
        {
            Alert(title, message, NSAlertStyle.Informational);
        }

        void IDialogCreator.UnexpectedError(Exception ex)
        {
            Alert("Unexpected error occured", ex.ToString(), NSAlertStyle.Critical);
        }

        void Alert(string title, string message, NSAlertStyle style)
        {
            new NSAlert { AlertStyle = style, InformativeText = message, MessageText = title, }.RunModal();
        }
    }
}
