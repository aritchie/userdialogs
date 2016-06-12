using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Splat;
using Orientation = Android.Widget.Orientation;


namespace Acr.UserDialogs.Fragments
{
    public class BottomSheetDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.SetCancelable(false);
            dialog.SetCanceledOnTouchOutside(false);
        }


        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            var dlg = new BottomSheetDialog(this.Activity);

            var layout = new LinearLayout(this.Activity)
            {
                Orientation = Orientation.Vertical
            };
            layout.LayoutParameters.Height = this.DpToPixels(56);

            if (!String.IsNullOrWhiteSpace(config.Title))
            {
                layout.AddView(this.GetHeaderText(config.Title));
            }

            foreach (var action in config.Options)
                layout.AddView(this.CreateRow(action));

            if (config.Destructive != null)
            {
                layout.AddView(this.CreateDivider());
                layout.AddView(this.CreateRow(config.Destructive));
            }
            if (config.Cancel != null)
            {
                if (config.Destructive == null)
                    layout.AddView(this.CreateDivider());

                layout.AddView(this.CreateRow(config.Cancel));
            }
            dlg.SetContentView(layout);
            dlg.SetCancelable(false);
            return dlg;
        }


        protected virtual View CreateRow(ActionSheetOption action)
        {
            var row = new LinearLayout(this.Activity)
            {
                Clickable = true,
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(48))
            };
            row.AddView(this.GetIcon(action.ItemIcon));
            row.AddView(this.GetText(action.Text));
            row.Click += (sender, args) =>
            {
                action.Action?.Invoke();
                this.Dismiss();
            };
            return row;
        }


        protected virtual TextView GetText(string text)
        {
            var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
            {
                Gravity = GravityFlags.CenterVertical
            };
            layout.SetMargins(this.DpToPixels(16), 0, this.DpToPixels(16), 0);

            return new TextView(this.Activity)
            {
                Text = text,
                TextSize = 16,
                LayoutParameters = layout
            };
        }


        protected virtual TextView GetHeaderText(string text)
        {
            var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
            {
                Gravity = GravityFlags.CenterVertical
            };
            layout.SetMargins(this.DpToPixels(16), 0, this.DpToPixels(16), 0);

            return new TextView(this.Activity)
            {
                Text = text,
                TextSize = 32,
                LayoutParameters = layout
            };
        }


        protected virtual ImageView GetIcon(IBitmap icon)
        {
            var layout = new LinearLayout.LayoutParams(this.DpToPixels(24), this.DpToPixels(24))
            {
               Gravity = GravityFlags.CenterVertical
            };
            layout.SetMargins(this.DpToPixels(16), 0, this.DpToPixels(16), 0);

            var img = new ImageView(this.Activity)
            {
                LayoutParameters = layout
            };
            if (icon != null)
            {
                img.SetImageDrawable(icon.ToNative());
            }
            return img;
        }


        protected virtual View CreateDivider()
        {
            return new View(this.Activity)
            {
                Background = new ColorDrawable(System.Drawing.Color.DarkGray.ToNative()),
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(1))
            };
        }


        protected virtual int DpToPixels(int dp)
        {
            var px = dp * ((int)this.Activity.Resources.DisplayMetrics.DensityDpi / (int)DisplayMetrics.DensityDefault);
            return Convert.ToInt32(px);
        }

        /*
 public boolean onTouch(View v, MotionEvent event) {

        final int DELAY = 100;

        if(event.getAction() == MotionEvent.ACTION_UP) {


            RelativeLayout fondo = (RelativeLayout) findViewById(R.id.fondo);

            ColorDrawable f = new ColorDrawable(0xff00ff00);
            ColorDrawable f2 = new ColorDrawable(0xffff0000);
            ColorDrawable f3 = new ColorDrawable(0xff0000ff);
            ColorDrawable f4 = new ColorDrawable(0xff0000ff);

            AnimationDrawable a = new AnimationDrawable();
            a.addFrame(f, DELAY);
            a.addFrame(f2, DELAY);
            a.addFrame(f3, DELAY);
            a.addFrame(f4, DELAY);
            a.setOneShot(false);

            fondo.setBackgroundDrawable(a); // This method is deprecated in API 16
            // fondo.setBackground(a); // Use this method if you're using API 16
            a.start();
         }
         return true;
    }
         */
    }
}