using System;
using Acr.UserDialogs.Infrastructure;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
using Orientation = Android.Widget.Orientation;
#if ANDROIDX
using Google.Android.Material.BottomSheet;
#else
using Android.Support.Design.Widget;
#endif

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
            var dlg = new BottomSheetDialog(this.Activity, config.AndroidStyleId ?? 0);
            var layout = new LinearLayout(this.Activity)
            {
                Orientation = Orientation.Vertical
            };

            if (!String.IsNullOrWhiteSpace(config.Title))
            {
                layout.AddView(this.GetTitle(config.Title, config.Subtitle, config.TitleIcon, config.TitleIconTint));
                layout.AddView(this.CreateDivider());
            }

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
            row.SetBackgroundResource(Extensions.GetSelectableItemBackground(this.Activity));

            if (action.ItemIcon != null)
                row.AddView(this.GetIcon(action.ItemIcon, action.IconTint));

            row.AddView(this.GetText(action.Text, isDestructive));
            row.Click += (sender, args) =>
            {
                action.Action?.Invoke();
                this.Dismiss();
            };
            return row;
        }

        private Color GetColorFromUint(uint color)
        {
            int a = (int)((color >> 24) & 0xff);
            int r = (int)((color >> 16) & 0xff);
            int g = (int)((color >> 8) & 0xff);
            int b = (int)((color) & 0xff);

            return new Color(r, g, b, a);
        }

        protected virtual View GetTitle(string text, string subtitle, string icon, uint? iconTint = null)
        {
            var hasSubtitle = !string.IsNullOrEmpty(subtitle);
            var heightWithSubtitle = 64;

            var row = new LinearLayout(this.Activity)
            {
                Orientation = Orientation.Horizontal,
                LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(hasSubtitle ? heightWithSubtitle : 48))
            };

            if (icon != null)
            {
                var imageView = this.GetIcon(icon, iconTint);
                row.AddView(imageView);
            }

            if (hasSubtitle)
            {
                var lay = new LinearLayout(this.Activity)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent)
                };
                var textTitle = new TextView(this.Activity)
                {
                    Text = text,
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(heightWithSubtitle / 2 - 8))
                    {
                        TopMargin = this.DpToPixels(8),
                        LeftMargin = this.DpToPixels(16)
                    },
                    Gravity = GravityFlags.CenterVertical
                };
                textTitle.SetTextSize(ComplexUnitType.Sp, 16);
                textTitle.SetMaxLines(1);
                textTitle.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

                var textSubtitle = new TextView(this.Activity)
                {
                    Text = subtitle,
                    LayoutParameters = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, this.DpToPixels(heightWithSubtitle / 2 - 8))
                    {
                        BottomMargin = this.DpToPixels(8),
                        LeftMargin = this.DpToPixels(16)
                    },
                    Gravity = GravityFlags.CenterVertical,
                };
                // this uses the textColorSecondary color, so we can style it easily
                textSubtitle.SetTextAppearance(Android.Resource.Style.TextAppearanceMaterialWidgetActionBarSubtitle);
                textSubtitle.SetTextSize(ComplexUnitType.Sp, 16);
                textSubtitle.SetMaxLines(1);
                textSubtitle.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

                lay.AddView(textTitle);
                lay.AddView(textSubtitle);
                row.AddView(lay);
            }
            else
                row.AddView(this.GetText(text, false));

            return row;
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


        protected virtual ImageView GetIcon(string icon, uint? iconTint = null)
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
            {
                var drawable = ImageLoader.Load(icon);
                if (drawable is object)
                    img.SetImageDrawable(drawable);
                else {
                    var uri = Android.Net.Uri.Parse(icon);
                    if (uri is object)
                        img.SetImageURI(uri);
                }
            }
            if (iconTint.HasValue)
                img.SetColorFilter(GetColorFromUint(iconTint.Value));

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
