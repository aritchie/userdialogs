using System;


namespace Samples.Uwp
{

    public partial class MainPage : Xamarin.Forms.Platform.UWP.WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.LoadApplication(new Samples.App());
        }
    }
}
