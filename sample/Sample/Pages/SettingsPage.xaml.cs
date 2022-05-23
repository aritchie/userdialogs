using System;
using Acr.UserDialogs;
using Samples.ViewModels;


namespace Samples
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new SettingsViewModel(UserDialogs.Instance);
        }
    }
}
