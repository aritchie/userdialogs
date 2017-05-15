using System;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Acr.UserDialogs
{
    internal class WarningTopContentViewHolder : BaseTopContentViewHolder
    {
        public WarningTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
        {

        }

        protected override int ContentId => Resource.Layout.warning_top_view;

        public override void OnStart()
        {

        }
    }
}