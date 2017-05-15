using System;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Acr.UserDialogs
{
    internal class WaitTopContentViewHolder : BaseTopContentViewHolder
    {
        public WaitTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
        {

        }

        protected override int ContentId => Resource.Layout.wait_top_view;

        public override void OnStart()
        {

        }
    }
}