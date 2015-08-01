using System;
using Acr.UserDialogs;
using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;


namespace Samples.WinPhone {

    public partial class MainPage : FormsApplicationPage {

        public MainPage() {
            this.InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
            Forms.Init();
            this.LoadApplication(new Samples.App());
        }
    }
}
