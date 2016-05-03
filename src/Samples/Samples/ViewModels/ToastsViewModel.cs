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
            this.Success = this.ToastCommand(ToastEvent.Success);
            this.Warn = this.ToastCommand(ToastEvent.Warn);
            this.Info = this.ToastCommand(ToastEvent.Info);
            this.Error = this.ToastCommand(ToastEvent.Error);
        }


        public ICommand Success { get; }
        public ICommand Info { get; }
        public ICommand Warn { get; }
        public ICommand Error { get; }


        ICommand ToastCommand(ToastEvent @event)
        {
            return new Command(() =>
                this.Dialogs.Toast(new ToastConfig(@event, @event.ToString(), "Testing toast functionality....fun!")
                {
                    Duration = TimeSpan.FromSeconds(3),
                    Action = () => this.Result("Toast Pressed")
                })
            );
        }
    }
}
