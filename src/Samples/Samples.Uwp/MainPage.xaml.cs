using System;


namespace Samples.Uwp {

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.LoadApplication(new Samples.App());
        }
    }
}
