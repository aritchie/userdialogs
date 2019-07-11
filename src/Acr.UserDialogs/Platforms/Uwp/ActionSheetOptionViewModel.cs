using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Acr.UserDialogs.Infrastructure;


namespace Acr.UserDialogs
{
    public class ActionSheetOptionViewModel
    {
        public ActionSheetOptionViewModel(bool visible, string text, Action action, string image = null)
        {
            this.Text = text;
            this.Action = new Command(action);
            this.Visible = visible ? Visibility.Visible : Visibility.Collapsed;
            this.ItemIcon = image;
        }


        public Visibility Visible { get; }
        public string Text { get; }
        public ICommand Action { get; }
        public string ItemIcon { get; }
    }
}
