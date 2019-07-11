using System;
using Acr.UserDialogs.Infrastructure;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Orientation = Android.Widget.Orientation;


namespace Acr.UserDialogs.Fragments
{
    public class BottomSheetDialogFragment : AbstractAppCompatDialogFragment<ActionSheetConfig>
    {
        protected override void SetDialogDefaults(Dialog dialog)
        {
            dialog.KeyPress += this.OnKeyPress;
            if (this.Config.Cancel == null)
            {
                dialog.SetCancelable(false);
                dialog.SetCanceledOnTouchOutside(false);
            }
            else
            {
                dialog.SetCancelable(true);
                dialog.SetCanceledOnTouchOutside(true);
                dialog.CancelEvent += (sender, args) => this.Config.Cancel.Action.Invoke();
            }
        }


        protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        {
            if (args.KeyCode != Keycode.Back)
                return;

            args.Handled = true;
            this.Config?.Cancel?.Action?.Invoke();
            this.Dismiss();
            base.OnKeyPress(sender, args);
        }


        protected override Dialog CreateDialog(ActionSheetConfig config)
        {
            var dlg = new BottomSheetDialog(this.Activity);
            var layout = new LinearLayout(this.Activity)
            {
                Orientation = Orientation.Vertical
            };

            if (!String.IsNullOrWhiteSpace(config.Title))
                layout.AddView(this.GetHeaderText(config.Title));

            foreach (var action in config.Options)
                layout.AddView(this.CreateRow(action, false));

            if (config.Destructive != null)
            {
                layout.AddView(this.CreateDivider());
                layout.AddView(this.CreateRow(config.Destructive, true));
            }
            if (config.Cancel != null)
            {
                if (config.Destructive == null)
                    layout.AddView(this.CreateDivider());

                layout.AddView(this.CreateRow(config.Cancel, false));
            }
            dlg.SetContentView(layout);
            dlg.SetCancelable(false);
            return dlg;
        }


        protected virtual View CreateRow(ActionSheetOption action, bool isDestructive)
        {
            var row = new LinearLayout(this.Activity)
            {
                Clickable = true,
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(48))
            };
            if (action.ItemIcon != null)
                row.AddView(this.GetIcon(action.ItemIcon));

            row.AddView(this.GetText(action.Text, isDestructive));
            row.Click += (sender, args) =>
            {
                action.Action?.Invoke();
                this.Dismiss();
            };
            return row;
        }


        protected virtual TextView GetHeaderText(string text)
        {
            var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(56))
            {
                LeftMargin = this.DpToPixels(16)
            };
            var txt = new TextView(this.Activity)
            {
                Text = text,
                LayoutParameters = layout,
                Gravity = GravityFlags.CenterVertical
            };
            txt.SetTextSize(ComplexUnitType.Sp, 16);
            return txt;
        }


        protected virtual TextView GetText(string text, bool isDestructive)
        {
            var layout = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
            {
                TopMargin = this.DpToPixels(8),
                BottomMargin = this.DpToPixels(8),
                LeftMargin = this.DpToPixels(16)
            };

            var txt = new TextView(this.Activity)
            {
                Text = text,
                LayoutParameters = layout,
                Gravity = GravityFlags.CenterVertical
            };
            txt.SetTextSize(ComplexUnitType.Sp, 16);
            if (isDestructive)
                txt.SetTextColor(Color.Red);

            return txt;
        }


        protected virtual ImageView GetIcon(string icon)
        {
            var layout = new LinearLayout.LayoutParams(this.DpToPixels(24), this.DpToPixels(24))
            {
                TopMargin = this.DpToPixels(8),
                BottomMargin = this.DpToPixels(8),
                LeftMargin = this.DpToPixels(16),
                RightMargin = this.DpToPixels(16),
                Gravity = GravityFlags.Center
            };

            var img = new ImageView(this.Activity)
            {
                LayoutParameters = layout
            };
            if (icon != null)
                img.SetImageDrawable(ImageLoader.Load(icon));

            return img;
        }


        protected virtual View CreateDivider()
        {
            var view = new View(this.Activity)
            {
                Background = new ColorDrawable(System.Drawing.Color.LightGray.ToNative()),
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(1))
            };
            view.SetPadding(0, this.DpToPixels(7), 0, this.DpToPixels(8));
            return view;
        }


        protected virtual int DpToPixels(int dp)
        {
            var value = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, this.Activity.Resources.DisplayMetrics);
            return Convert.ToInt32(value);
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