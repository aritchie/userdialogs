using System;
using System.Drawing;
using Splat;


namespace Acr.UserDialogs {

    public enum ToastEvent {
        Info,
        Warn,
        Error,
        Success
    }


    public class ToastConfig {

        // icons only on ios
        // action text only on android, tap action is on all!

        public static IBitmap InfoIcon { get; set; }
        public static Color InfoBackgroundColor { get; set; } = Color.Gainsboro; //Color.FromArgb(96, 0, 482, 1);
        public static Color InfoTextColor { get; set; } = Color.Black;

        public static IBitmap SuccessIcon { get; set; }
        public static Color SuccessBackgroundColor { get; set; } = Color.LawnGreen; //Color.FromArgb(96, 0, 831, 176);
        public static Color SuccessTextColor { get; set; } = Color.Black;

        public static IBitmap WarnIcon { get; set; }
        public static Color WarnBackgroundColor { get; set; } = Color.Coral;
        public static Color WarnTextColor { get; set; } = Color.White;

        public static IBitmap ErrorIcon { get; set; }
        public static Color ErrorBackgroundColor { get; set; } = Color.Red;
        public static Color ErrorTextColor { get; set; } = Color.White;

        public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromSeconds(2.5);


        public ToastEvent Event { get; }
        public Color BackgroundColor { get; set; }
        public IBitmap Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Color TextColor { get; set; }
        public TimeSpan Duration { get; set; }
        public Action Action { get; set; }


        public ToastConfig(ToastEvent @event, string title, string description = null) {
            this.Event = @event;
            this.Title = title;
            this.Description = description;
            this.Duration = DefaultDuration;

            switch (@event) {
                case ToastEvent.Info:
                    this.BackgroundColor = InfoBackgroundColor;
                    this.TextColor = InfoTextColor;
                    this.Icon = InfoIcon;
                    break;

                case ToastEvent.Success:
                    this.BackgroundColor = SuccessBackgroundColor;
                    this.TextColor = SuccessTextColor;
                    this.Icon = SuccessIcon;
                    break;

                case ToastEvent.Warn:
                    this.BackgroundColor = WarnBackgroundColor;
                    this.TextColor = WarnTextColor;
                    this.Icon = WarnIcon;
                    break;

                case ToastEvent.Error:
                    this.BackgroundColor = ErrorBackgroundColor;
                    this.TextColor = ErrorTextColor;
                    this.Icon = ErrorIcon;
                    break;
            }
        }


        public ToastConfig SetDescription(string description) {
            this.Description = description;
            return this;
        }


        public ToastConfig SetDuration(int millis) {
            return this.SetDuration(TimeSpan.FromMilliseconds(millis));
        }


        public ToastConfig SetDuration(TimeSpan duration) {
            this.Duration = duration;
            return this;
        }


        public ToastConfig SetIcon(IBitmap bitmap) {
            this.Icon = bitmap;
            return this;
        }


        public ToastConfig SetColorList(Color? bg, Color? text) {
            if (bg != null) this.BackgroundColor = bg.Value;
            if (text != null) this.TextColor = text.Value;
            return this;
        }


        public ToastConfig SetAction(Action action) {
            this.Action = action;
            return this;
        }
    }
}
