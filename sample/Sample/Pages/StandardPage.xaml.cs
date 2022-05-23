using System;
using Acr.UserDialogs;
using Samples.ViewModels;


namespace Samples
{
    public partial class StandardPage : ContentPage
    {
        public StandardPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new StandardViewModel(UserDialogs.Instance);
        }
    }
}
