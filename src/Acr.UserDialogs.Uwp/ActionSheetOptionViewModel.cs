using System;
using System.Windows.Input;
using Windows.UI.Xaml;


namespace Acr.UserDialogs {

    public class ActionSheetOptionViewModel {

        public ActionSheetOptionViewModel(bool visible, string text, Action action) {
            this.Text = text;
            this.Action = new Command(action);
            this.Visible = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility Visible { get; }
        public string Text { get; }
        public ICommand Action { get; }
    }
}
