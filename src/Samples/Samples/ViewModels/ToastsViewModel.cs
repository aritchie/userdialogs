using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Splat;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class ToastsViewModel : AbstractViewModel
    {
        public ToastsViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.SecondsDuration = 3;

            this.ActionText = "Ok";
            this.Message = "This is a test of the emergency toast system";

            this.ActionTextColor = ToHex(Color.White);
            this.MessageTextColor = ToHex(Color.White);
            this.BackgroundColor = ToHex(Color.Blue);

            this.Open = new Command(() =>
            {
                // var icon = await BitmapLoader.Current.LoadFromResource("emoji_cool_small.png", null, null);

                ToastConfig.DefaultBackgroundColor = System.Drawing.Color.AliceBlue;
                ToastConfig.DefaultMessageTextColor = System.Drawing.Color.Red;
                ToastConfig.DefaultActionTextColor = System.Drawing.Color.DarkRed;
                //var bgColor = FromHex(this.BackgroundColor);
                //var msgColor = FromHex(this.MessageTextColor);
                //var actionColor = FromHex(this.ActionTextColor);

                dialogs.Toast(new ToastConfig(this.Message)
                    //.SetBackgroundColor(bgColor)
                    //.SetMessageTextColor(msgColor)
                    .SetDuration(TimeSpan.FromSeconds(this.SecondsDuration))
                    .SetPosition(this.ShowOnTop ? ToastPosition.Top : ToastPosition.Bottom)
                    //.SetIcon(icon)
                    .SetAction(x => x
                        .SetText(this.ActionText)
                        //.SetTextColor(actionColor)
                        .SetAction(() => dialogs.Alert("You clicked the primary toast button"))
                    )
                );
            });
        }


        static System.Drawing.Color FromHex(string hex)
        {
            var c = Color.FromHex(hex);
            var dc = System.Drawing.Color.FromArgb((int)c.A, (int)c.R, (int)c.G, (int)c.B);
            return dc;
        }


        static string ToHex(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            //var alpha = (int)(color.A * 255);
            //var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}{alpha:X2}");
            var hex = String.Format($"#{red:X2}{green:X2}{blue:X2}");
            return hex;
        }


        public ICommand Open { get; }


        string backgroundColor;
        public string BackgroundColor
        {
            get { return this.backgroundColor; }
            set
            {
                if (this.backgroundColor == value)
                    return;

                this.backgroundColor = value;
                this.OnPropertyChanged();
            }
        }


        int secondsDuration;
        public int SecondsDuration
        {
            get { return this.secondsDuration; }
            set
            {
                if (this.secondsDuration == value)
                    return;

                this.secondsDuration = value;
                this.OnPropertyChanged();
            }
        }


        bool showOnTop;
        public bool ShowOnTop
        {
            get => this.showOnTop;
            set
            {
                if (this.showOnTop == value)
                    return;

                this.showOnTop = true;
                this.OnPropertyChanged();
            }
        }


        string actionText;
        public string ActionText
        {
            get { return this.actionText; }
            set
            {
                if (this.actionText == value)
                    return;

                this.actionText = value;
                this.OnPropertyChanged();
            }
        }


        string actionTextColor;
        public string ActionTextColor
        {
            get { return this.actionTextColor; }
            set
            {
                if (this.actionTextColor == value)
                    return;

                this.actionTextColor = value;
                this.OnPropertyChanged();
            }
        }


        string messageTextColor;
        public string MessageTextColor
        {
            get { return this.messageTextColor; }
            set
            {
                if (this.messageTextColor == value)
                    return;

                this.messageTextColor = value;
                this.OnPropertyChanged();
            }
        }


        string message;
        public string Message
        {
            get { return this.message; }
            set
            {
                if (this.message == value)
                    return;

                this.message = value;
                this.OnPropertyChanged();
            }
        }
    }
}
