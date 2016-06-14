using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class ToastsViewModel : AbstractViewModel
    {
        public ToastsViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.SecondsDuration = 3;

            this.ActionText = "Ok";
            this.ActionTextColor = Color.White.ToString();
            this.Message = "This is a test of the emergency toast system";
            this.MessageTextColor = Color.White.ToString ();

            this.Open = new Command(() => dialogs
                .Toast(new ToastConfig(this.Message)
                    //.SetMessageTextColor(System.Drawing.Color.FromHex(this.MessageTextColor))
                    .SetDuration(TimeSpan.FromSeconds(this.SecondsDuration))
                    .SetPrimaryAction(x => x
                        .SetText(this.ActionText)
                        //.SetTextColor(new System.Drawing.Color.FromHex(this.ActionTextColor))
                        .SetAction(() => dialogs.Alert("You clicked the primary button"))
                    )
                )
            );
        }


        public ICommand Open { get; }


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
