using System;
using Acr.UserDialogs;
using Samples.ViewModels;


namespace Samples
{
    public partial class SpecificCasesPage : ContentPage
    {
        public SpecificCasesPage()
        {
            InitializeComponent();

            // the idea here is that you would dependency inject userdialogs
            this.BindingContext = new SpecificCasesViewModel(UserDialogs.Instance);
        }
    }
}
