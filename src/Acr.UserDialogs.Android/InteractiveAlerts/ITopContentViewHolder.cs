using System;
using Android.Widget;
using Android.Views;

namespace Acr.UserDialogs
{
    public interface ITopContentViewHolder
    {
        ViewGroup ContentView { get; }

        void OnStart();

        void OnPause();
    }
}