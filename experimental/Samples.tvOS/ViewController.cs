using System;
using Acr.UserDialogs;
using Samples.ViewModels;
using UIKit;


namespace Samples.tvOS
{
    public partial class ViewController : UIViewController
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }


        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            var vm = new StandardViewModel(UserDialogs.Instance);

            btnAlert.TouchUpInside += (sender, e) => vm.Alert.Execute(null);
            btnAlertLong.TouchUpInside += (sender, e) => vm.AlertLongText.Execute(null);
        }
    }
}