using System;
using System.Collections.Generic;

using Acr.UserDialogs;
using Xamarin.Forms;

namespace Samples.ViewModels
{
    public class InteractiveAlertViewModel : AbstractViewModel
    {
        public IList<CommandViewModel> Commands { get; } = new List<CommandViewModel>();

        public InteractiveAlertViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Commands = new List<CommandViewModel>
            {
                new CommandViewModel
                {
                    Text = "Success",
                    Command = this.InteractiveAlertCommand(InteractiveAlertStyle.Success)
                },
                new CommandViewModel
                {
                    Text = "Warning",
                    Command = this.InteractiveAlertCommand(InteractiveAlertStyle.Warning)
                },
                new CommandViewModel
                {
                    Text = "Error",
                    Command = this.InteractiveAlertCommand(InteractiveAlertStyle.Error)
                },
                new CommandViewModel
                {
                    Text = "Wait",
                    Command = this.InteractiveAlertCommand(InteractiveAlertStyle.Wait)
                },
            };
        }

        protected Command InteractiveAlertCommand(InteractiveAlertStyle alertStyle)
        {
            var config = new InteractiveAlertConfig
            {
                Title = "Interactive Alert!",
                Message = "Description for interactive alert",
                Style = alertStyle,
                IsCancellable = true,
                Done = new InteractiveAlertConfig.InteractiveActionButton(),
                CustomButton = new InteractiveAlertConfig.InteractiveActionButton { Title = "Custom button", Action = () => { } }
            };

            return new Command(() => this.Dialogs.InteractiveAlert(config));
        }
    }
}
