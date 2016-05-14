using System;
using Acr.UserDialogs;
using Samples.ViewModels;
using Xamarin.Forms;


namespace Samples
{
    public partial class ToastsPage : ContentPage
    {
        public ToastsPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new ToastsViewModel(UserDialogs.Instance);
        }
    }
}
