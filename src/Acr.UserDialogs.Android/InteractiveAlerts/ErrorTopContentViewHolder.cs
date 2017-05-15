using System;
using Android.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Acr.UserDialogs
{
    internal class ErrorTopContentViewHolder : BaseTopContentViewHolder
    {
        private Animation mErrorInAnim;
        private AnimationSet mErrorXInAnim;

        private ImageView mErrorX;

        public ErrorTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
        {

        }

        protected override int ContentId => Resource.Layout.error_top_view;

        public override void OnStart()
        {
            mErrorInAnim = this.CreateExitAnimation();
            mErrorXInAnim = (AnimationSet)AnimationUtils.LoadAnimation(this.Context, Resource.Animation.error_x_in);

            this.mErrorX = (ImageView)this.ContentView.FindViewById(Resource.Id.error_top_x);

            this.ContentView.StartAnimation(this.mErrorInAnim);
            this.mErrorX.StartAnimation(this.mErrorXInAnim);
        }

        protected Animation CreateExitAnimation()
        {
            var exitAnimSet = new AnimationSet(true);
            exitAnimSet.AddAnimation(new AlphaAnimation(0, 1) { Duration = 400 });
            exitAnimSet.AddAnimation(new Rotate3dAnimation(0, 100, 0, 50, 50) { Duration = 400 });

            return exitAnimSet;
        }
    }
}