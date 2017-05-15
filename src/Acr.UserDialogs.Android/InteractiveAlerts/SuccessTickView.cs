using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Runtime;
using Android.Support.V4.Content;

namespace Acr.UserDialogs
{
    [Register("scl.alertview.SuccessTickView")]
    public class SuccessTickView : View
    {
        private static float mDensity = -1;
        private Paint mPaint;
        private float CONST_RADIUS => dip2px(1.2f);
        private float CONST_RECT_WEIGHT => dip2px(3);
        private float CONST_LEFT_RECT_W => dip2px(15);
        private float CONST_RIGHT_RECT_W => dip2px(25);
        private float MIN_LEFT_RECT_W => dip2px(3.3f);
        private float MAX_RIGHT_RECT_W => CONST_RIGHT_RECT_W + dip2px(6.7f);

        private float mMaxLeftRectWidth;
        private float mLeftRectWidth;
        private float mRightRectWidth;
        private bool mLeftRectGrowMode;

        public SuccessTickView(Context context) : base(context)
        {
            this.Init();
        }

        public SuccessTickView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.Init();
        }

        private void Init()
        {
            mPaint = new Paint();
            var color = ContextCompat.GetColor(this.Context, Resource.Color.success_stroke_color);
            mPaint.Color = Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
            mLeftRectWidth = CONST_LEFT_RECT_W;
            mRightRectWidth = CONST_RIGHT_RECT_W;
            mLeftRectGrowMode = false;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            float totalW = this.Width;
            float totalH = this.Height;
            // rotate canvas first
            canvas.Rotate(45, totalW / 2, totalH / 2);

            totalW /= 1.2f;
            totalH /= 1.4f;
            mMaxLeftRectWidth = (totalW + CONST_LEFT_RECT_W) / 2 + CONST_RECT_WEIGHT - 1;

            RectF leftRect = new RectF();
            if (mLeftRectGrowMode)
            {
                leftRect.Left = 0;
                leftRect.Right = leftRect.Left + mLeftRectWidth;
                leftRect.Top = (totalH + CONST_RIGHT_RECT_W) / 2;
                leftRect.Bottom = leftRect.Top + CONST_RECT_WEIGHT;
            }
            else
            {
                leftRect.Right = (totalW + CONST_LEFT_RECT_W) / 2 + CONST_RECT_WEIGHT - 1;
                leftRect.Left = leftRect.Right - mLeftRectWidth;
                leftRect.Top = (totalH + CONST_RIGHT_RECT_W) / 2;
                leftRect.Bottom = leftRect.Top + CONST_RECT_WEIGHT;
            }

            canvas.DrawRoundRect(leftRect, CONST_RADIUS, CONST_RADIUS, mPaint);

            RectF rightRect = new RectF();
            rightRect.Bottom = (totalH + CONST_RIGHT_RECT_W) / 2 + CONST_RECT_WEIGHT - 1;
            rightRect.Left = (totalW + CONST_LEFT_RECT_W) / 2;
            rightRect.Right = rightRect.Left + CONST_RECT_WEIGHT;
            rightRect.Top = rightRect.Bottom - mRightRectWidth;
            canvas.DrawRoundRect(rightRect, CONST_RADIUS, CONST_RADIUS, mPaint);
        }

        public float dip2px(float dpValue)
        {
            if (mDensity == -1)
            {
                mDensity = this.Resources.DisplayMetrics.Density;
            }
            return dpValue * mDensity + 0.5f;
        }

        protected class SelfAnimation : Animation
        {
            private readonly SuccessTickView view;

            public SelfAnimation(SuccessTickView view)
            {
                this.view = view;
            }

            protected override void ApplyTransformation(float interpolatedTime, Transformation t)
            {
                base.ApplyTransformation(interpolatedTime, t);
                this.view.mLeftRectWidth = 0;
                this.view.mRightRectWidth = 0;
                if (0.54 < interpolatedTime && 0.7 >= interpolatedTime)
                {  // grow left and right rect to right
                    this.view.mLeftRectGrowMode = true;
                    this.view.mLeftRectWidth = this.view.mMaxLeftRectWidth * ((interpolatedTime - 0.54f) / 0.16f);
                    if (0.65 < interpolatedTime)
                    {
                        this.view.mRightRectWidth = this.view.MAX_RIGHT_RECT_W * ((interpolatedTime - 0.65f) / 0.19f);
                    }
                    this.view.Invalidate();
                }
                else if (0.7 < interpolatedTime && 0.84 >= interpolatedTime)
                { // shorten left rect from right, still grow right rect
                    this.view.mLeftRectGrowMode = false;
                    this.view.mLeftRectWidth = this.view.mMaxLeftRectWidth * (1 - ((interpolatedTime - 0.7f) / 0.14f));
                    this.view.mLeftRectWidth = this.view.mLeftRectWidth < this.view.MIN_LEFT_RECT_W ? this.view.MIN_LEFT_RECT_W : this.view.mLeftRectWidth;
                    this.view.mRightRectWidth = this.view.MAX_RIGHT_RECT_W * ((interpolatedTime - 0.65f) / 0.19f);
                    this.view.Invalidate();
                }
                else if (0.84 < interpolatedTime && 1 >= interpolatedTime)
                { // restore left rect width, shorten right rect to const
                    this.view.mLeftRectGrowMode = false;
                    this.view.mLeftRectWidth = this.view.MIN_LEFT_RECT_W + (this.view.CONST_LEFT_RECT_W - this.view.MIN_LEFT_RECT_W) * ((interpolatedTime - 0.84f) / 0.16f);
                    this.view.mRightRectWidth = this.view.CONST_RIGHT_RECT_W + (this.view.MAX_RIGHT_RECT_W - this.view.CONST_RIGHT_RECT_W) * (1 - ((interpolatedTime - 0.84f) / 0.16f));
                    this.view.Invalidate();
                }
            }
        }

        public void StartTickAnim()
        {
            // hide tick
            this.Invalidate();
            Animation tickAnim = new SelfAnimation(this);
            tickAnim.Duration = 750;
            tickAnim.StartOffset = 100;
            this.StartAnimation(tickAnim);
        }
    }
}