// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace JpAnnotator
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSProgressIndicator ConversionProgress { get; set; }

		[Outlet]
		AppKit.NSTextField ConversionStatus { get; set; }

		[Outlet]
		AppKit.NSButton ConvertButton { get; set; }

		[Outlet]
		AppKit.NSTextField FileToConvert { get; set; }

		[Outlet]
		AppKit.NSButton OpenButton { get; set; }

		[Action ("ConvertButtonClicked:")]
		partial void ConvertButtonClicked (Foundation.NSObject sender);

		[Action ("OpenButtonClicked:")]
		partial void OpenButtonClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ConversionStatus != null) {
				ConversionStatus.Dispose ();
				ConversionStatus = null;
			}

			if (ConversionProgress != null) {
				ConversionProgress.Dispose ();
				ConversionProgress = null;
			}

			if (FileToConvert != null) {
				FileToConvert.Dispose ();
				FileToConvert = null;
			}

			if (ConvertButton != null) {
				ConvertButton.Dispose ();
				ConvertButton = null;
			}

			if (OpenButton != null) {
				OpenButton.Dispose ();
				OpenButton = null;
			}
		}
	}
}
