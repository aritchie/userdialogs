using System;
using Android.Content;
using Android.Views;
using Android.Views.Animations;

namespace Acr.UserDialogs
{
    internal class SuccessTopContentViewHolder : BaseTopContentViewHolder
    {
        private AnimationSet mSuccessLayoutAnimSet;
        private Animation mSuccessBowAnim;

        private View mSuccessLeftMask;
        private SuccessTickView mSuccessTick;
        private View mSuccessRightMask;

        public SuccessTopContentViewHolder(Context context, ViewGroup root) : base(context, root)
        {
        }

        protected override int ContentId => Resource.Layout.success_top_view;

        public override void OnStart()
        {
            this.mSuccessTick = (SuccessTickView)this.ContentView.FindViewById(Resource.Id.success_top_tick);
            this.mSuccessLeftMask = this.ContentView.FindViewById(Resource.Id.success_top_mask_left);
            this.mSuccessRightMask = this.ContentView.FindViewById(Resource.Id.success_top_mask_right);
            this.mSuccessLayoutAnimSet = (AnimationSet)AnimationUtils.LoadAnimation(this.Context, Resource.Animation.success_mask_layout);

            this.mSuccessBowAnim = AnimationUtils.LoadAnimation(this.Context, Resource.Animation.success_bow_roate);
            this.mSuccessLeftMask.StartAnimation(mSuccessLayoutAnimSet.Animations[0]);
            this.mSuccessRightMask.StartAnimation(mSuccessLayoutAnimSet.Animations[1]);

            this.mSuccessTick.StartTickAnim();
            this.mSuccessRightMask.StartAnimation(this.mSuccessBowAnim);
        }
    }
}
