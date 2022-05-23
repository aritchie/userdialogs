using System;
using Acr.UserDialogs;
using Samples.ViewModels;


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
