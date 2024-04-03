using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;


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
                    Text = "Two alerts with one Cancellation Token Source",
                    Command = new Command(async () =>
                    {
                        try
                        {
                            var cts = new CancellationTokenSource();

                            await this.Dialogs.AlertAsync("Press ok and then wait", "Hi", null, cts.Token);
                            cts.CancelAfter(TimeSpan.FromSeconds(3));
                            await this.Dialogs.AlertAsync("I'll close soon, just wait", "Hi", null, cts.Token);
                        }
                        catch(OperationCanceledException)
                        {
                        }
                    })
                },
                new CommandViewModel
                {
                    Text = "Large Toast Text",
                    Command = new Command(() =>
                        this.Dialogs.Toast(
                            "This is a really long message to test text wrapping and other such things that are painful for toast dialogs to render fully in two line labels")
                    )
                },
                new CommandViewModel
                {
                    Text = "Toast with image",
                    Command = new Command(() =>
                    {
                        var img = DeviceInfo.Platform == DevicePlatform.WinUI ? "ms-appx:///Assets/emoji_cool_small.png" : "emoji_cool_small.png";
                        this.Dialogs.Toast(new ToastConfig("Wow what a cool guy").SetIcon(img));
                    })
                },
                new CommandViewModel
                {
                    Text = "Toast (no action)",
                    Command = new Command(() => this.Dialogs.Toast("TEST"))
                },
                new CommandViewModel
                {
                    Text = "Prompt OnTextChanged with Initial Value",
                    Command = new Command(async () =>
                    {
                        await this.Dialogs.PromptAsync(new PromptConfig()
                            .SetMessage("GOOD = ENABLED")
                            .SetText("GOOD")
                            .SetOnTextChanged(args =>
                                args.IsValid = args.Value.Equals("GOOD")
                            )
                        );
                        await this.Dialogs.PromptAsync(new PromptConfig()
                            .SetMessage("GOOD = ENABLED")
                            .SetText("BAD")
                            .SetOnTextChanged(args =>
                                args.IsValid = args.Value.Equals("GOOD")
                            )
                        );
                        // TODO
                    })
                }
            };
        }
    }
}
