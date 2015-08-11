using System;
using Windows.UI.Xaml.Controls;


namespace Acr.UserDialogs {

    public sealed partial class ActionSheetContentDialog : ContentDialog {

        public ActionSheetContentDialog() {
            this.InitializeComponent();
            this.List.SelectionChanged += (sender, args) => {
                var vm = this.List.SelectedItem as ActionSheetOptionViewModel;
                vm?.Action?.Execute(null);
            };
        }
    }
}
