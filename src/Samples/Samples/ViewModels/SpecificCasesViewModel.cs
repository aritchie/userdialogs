using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class SpecificCasesViewModel : AbstractViewModel
    {
        public IList<CommandViewModel> Commands { get; }


        public SpecificCasesViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Commands = new List<CommandViewModel>
            {
                new CommandViewModel
                {
                    Text = "Loading Task to Alert",
                    Command = new Command(() =>
                    {
                        this.Dialogs.ShowLoading("You really shouldn't use ShowLoading");
                        Task.Delay(TimeSpan.FromSeconds(2))
                            .ContinueWith(x => this.Dialogs.Alert("Do you see me?"));
                    })
                },
                new CommandViewModel
                {
                    Text = "Two Date Pickers",
                    Command = new Command(async () =>
                    {
                        var v1 = await this.Dialogs.DatePromptAsync("Date 1 (Past -1 Day)", DateTime.Now.AddDays(-1));
                        if (!v1.Ok)
                            return;

                        var v2 = await this.Dialogs.DatePromptAsync("Date 2 (Future +1 Day)", DateTime.Now.AddDays(1));
                        if (!v2.Ok)
                            return;

                        this.Dialogs.Alert($"Date 1: {v1.SelectedDate} - Date 2: {v2.SelectedDate}");
                    })
                },
                new CommandViewModel
                {
                    Text = "Start Loading Twice",
                    Command = new Command(async () =>
                    {
                        this.Dialogs.ShowLoading("Loading 1");
                        await Task.Delay(1000);
                        this.Dialogs.ShowLoading("Loading 2");
                        await Task.Delay(1000);
                        this.Dialogs.HideLoading();
                    })
                },
                new CommandViewModel
                {
                    Text = "Async & OnAction Fail!",
                    Command = new Command(async () =>
                    {
                        try
                        {
                            await this.Dialogs.AlertAsync(new AlertConfig
                            {
                                OnAction = () => { }
                            });
                        }
                        catch
                        {
                            this.Dialogs.Alert("It failed... GOOOD");
                        }
                    })
                },
                new CommandViewModel
                {
                    Text = "Toast from Background Thread",
                    Command = new Command(() =>
                        Task.Factory.StartNew(() =>
                            this.Dialogs.Toast("Test From Background"),
                            TaskCreationOptions.LongRunning
                        )
                    )
                },
                new CommandViewModel
                {
                    Text = "Alert from Background Thread",
                    Command = new Command(() =>
                        Task.Factory.StartNew(() =>
                            this.Dialogs.Alert("Test From Background"),
                            TaskCreationOptions.LongRunning
                        )
                    )
                },
                new CommandViewModel
                {
                    Text = "Toast (no action)",
                    Command = new Command(() => this.Dialogs.Toast("TEST"))
                }
            };
        }
    }
}