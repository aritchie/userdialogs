using System;
using Android.App;
using Android.Content;
using Android.Views;

namespace Acr.UserDialogs {

    public class StandardAlertDialog : IAlertDialog {
        AlertDialog.Builder builder;


        public StandardAlertDialog(Context context) {
            this.builder = new AlertDialog.Builder(context);
        }


        public IAlertDialog SetCancelable(bool cancel) {
            this.builder = this.builder.SetCancelable(cancel);
            return this;
        }


        public IAlertDialog SetItems(string[] items, Action<int> clickIndex) {
            this.builder = this.builder.SetItems(items, (sender, args) => clickIndex?.Invoke(args.Which));
            return this;
        }


        public IAlertDialog SetMessage(string message) {
            this.builder = this.builder.SetMessage(message);
            return this;
        }


        public IAlertDialog SetNegativeButton(string text, Action action) {
            this.builder = this.builder.SetNegativeButton(text, (sender, args) => action?.Invoke());
            return this;
        }


        public IAlertDialog SetNeutralButton(string text, Action action) {
            this.builder = this.builder.SetNeutralButton(text, (sender, args) => action?.Invoke());
            return this;
        }


        public IAlertDialog SetPositiveButton(string text, Action action) {
            this.builder = this.builder.SetPositiveButton(text, (sender, args) => action?.Invoke());
            return this;
        }


        public IAlertDialog SetTitle(string title) {
            this.builder = this.builder.SetTitle(title);
            return this;
        }


        public IAlertDialog SetView(View view) {
            this.builder = this.builder.SetView(view);
            return this;
        }


        public IAlertDialog Show() {
            var dialog = this.builder.Create();
            dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
            dialog.Show();
            return this;
        }
    }
}