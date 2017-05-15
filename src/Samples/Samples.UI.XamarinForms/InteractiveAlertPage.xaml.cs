using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Samples.ViewModels;
using Acr.UserDialogs;

namespace Samples
{
    public partial class InteractiveAlertPage : ContentPage
    {
        public InteractiveAlertPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new InteractiveAlertViewModel(UserDialogs.Instance);
        }
    }
}
