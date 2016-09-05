using System;
using AppKit;
using Foundation;
using JpAnnotator.Logging;
using JpAnnotator.Common.Mac;
using JpAnnotator.Core;

namespace JpAnnotator
{
    public partial class ViewController : NSViewController
    {
        Counter _counter;
        ILogWriter _log;

        public ViewController(IntPtr handle) : base(handle)
        {
            _log = new LoggingConfig(new MacResourceLocator()).CreateRootLogWriter();
            _log.Debug("start");
            _counter = new Counter(_log);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            _log.Debug("view did load");
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void ConvertButtonClicked(NSObject sender)
        {
            _log.Debug($"Convert {FileToConvert.StringValue}");
            _counter.Start();
            _counter.Stop();
        }
    }
}
