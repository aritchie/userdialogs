using System;
using Android.Views;


namespace Acr.UserDialogs {

    public interface IAlertDialog {

        IAlertDialog SetCancelable(bool cancel);
        IAlertDialog SetMessage(string message);
        IAlertDialog SetTitle(string title);

        IAlertDialog SetPositiveButton(string text, Action action);
        IAlertDialog SetNegativeButton(string text, Action action);
        IAlertDialog SetNeutralButton(string text, Action action);
        IAlertDialog SetView(View view);
        IAlertDialog SetItems(string[] items, Action<int> clickIndex);
        IAlertDialog Show();
    }
}