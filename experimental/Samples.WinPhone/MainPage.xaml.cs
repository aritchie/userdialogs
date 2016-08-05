using System;
using Microsoft.Phone.Controls;
using Xamarin.Forms;


namespace Samples.WinPhone {

    public partial class MainPage {

        public MainPage() {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Forms.Init();
            this.LoadApplication(new Samples.App());
        }
    }
}