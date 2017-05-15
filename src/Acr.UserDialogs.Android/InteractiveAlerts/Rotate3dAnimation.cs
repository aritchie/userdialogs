using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views.Animations;
using Android.Runtime;

namespace Acr.UserDialogs
{
    public class Rotate3dAnimation : Animation
    {
        private Dimension mPivotXType = Dimension.Absolute;
        private Dimension mPivotYType = Dimension.Absolute;
        private float mPivotXValue = 0.0f;
        private float mPivotYValue = 0.0f;

        private float mFromDegrees;
        private float mToDegrees;
        private float mPivotX;
        private float mPivotY;
        private Camera mCamera;
        private int mRollType;

        public const int ROLL_BY_X = 0;
        public const int ROLL_BY_Y = 1;
        public const int ROLL_BY_Z = 2;

        public Rotate3dAnimation(int rollType, float fromDegrees, float toDegrees)
        {
            mRollType = rollType;
            mFromDegrees = fromDegrees;
            mToDegrees = toDegrees;
            mPivotX = 0.0f;
            mPivotY = 0.0f;
        }

        public Rotate3dAnimation(int rollType, float fromDegrees, float toDegrees, float pivotX, float pivotY)
        {
            mRollType = rollType;
            mFromDegrees = fromDegrees;
            mToDegrees = toDegrees;

            mPivotXType = Dimension.Absolute;
            mPivotYType = Dimension.Absolute;
            mPivotXValue = pivotX;
            mPivotYValue = pivotY;
            initializePivotPoint();
        }

        public Rotate3dAnimation(int rollType, float fromDegrees, float toDegrees, Dimension pivotXType, float pivotXValue, Dimension pivotYType, float pivotYValue)
        {
            mRollType = rollType;
            mFromDegrees = fromDegrees;
            mToDegrees = toDegrees;

            mPivotXValue = pivotXValue;
            mPivotXType = pivotXType;
            mPivotYValue = pivotYValue;
            mPivotYType = pivotYType;
            initializePivotPoint();
        }

        private void initializePivotPoint()
        {
            if (mPivotXType == Dimension.Absolute)
            {
                mPivotX = mPivotXValue;
            }
            if (mPivotYType == Dimension.Absolute)
            {
                mPivotY = mPivotYValue;
            }
        }

        public override void Initialize(int width, int height, int parentWidth, int parentHeight)
        {
            base.Initialize(width, height, parentWidth, parentHeight);
            mCamera = new Camera();
            mPivotX = ResolveSize(mPivotXType, mPivotXValue, width, parentWidth);
            mPivotY = ResolveSize(mPivotYType, mPivotYValue, height, parentHeight);
        }

        protected override void ApplyTransformation(float interpolatedTime, Transformation t)
        {
            base.ApplyTransformation(interpolatedTime, t);
            var fromDegrees = mFromDegrees;
            float degrees = fromDegrees + ((mToDegrees - fromDegrees) * interpolatedTime);
            var matrix = t.Matrix;

            mCamera.Save();
            switch (mRollType)
            {
                case ROLL_BY_X:
                    mCamera.RotateX(degrees);
                    break;
                case ROLL_BY_Y:
                    mCamera.RotateY(degrees);
                    break;
                case ROLL_BY_Z:
                    mCamera.RotateZ(degrees);
                    break;
            }
            mCamera.GetMatrix(matrix);
            mCamera.Restore();

            matrix.PreTranslate(-mPivotX, -mPivotY);
            matrix.PostTranslate(mPivotX, mPivotY);
        }
    }
}