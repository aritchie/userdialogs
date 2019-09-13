using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Samples.macOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private readonly NSWindow _window;

        public override NSWindow MainWindow => this._window;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 200, 800, 600);
            this._window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            this._window.Title = "User Dialogs";
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            this.LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }
    }
}
