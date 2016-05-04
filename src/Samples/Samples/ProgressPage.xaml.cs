using System;
using Acr.UserDialogs;
using Samples.ViewModels;
using Xamarin.Forms;


namespace Samples
{
    public partial class ProgressPage : ContentPage
    {
        public ProgressPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new ProgressViewModel(UserDialogs.Instance);
        }
    }
}
