using System.Windows.Input;
using Acr.UserDialogs;


namespace Samples.ViewModels
{
    public class ProgressViewModel : AbstractViewModel
    {
        public IList<CommandViewModel> Commands { get; } = new List<CommandViewModel>();


        public ProgressViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Commands = new List<CommandViewModel>
            {
                new CommandViewModel
                {
                    Text = "Loading",
                    Command = this.LoadingCommand(MaskType.Black)
                },
                new CommandViewModel
                {
                    Text = "Loading (Clear)",
                    Command = this.LoadingCommand(MaskType.Clear)
                },
                new CommandViewModel
                {
                    Text = "Loading (Gradient)",
                    Command = this.LoadingCommand(MaskType.Gradient)
                },
                new CommandViewModel
                {
                    Text = "Loading (None)",
                    Command = this.LoadingCommand(MaskType.None)
                },
                new CommandViewModel
                {
                    Text = "Progress",
                    Command = new Command(async () =>
                    {
                        var cancelled = false;

                        using (var dlg = this.Dialogs.Progress("Test Progress", () => cancelled = true))
                        {
                            while (!cancelled && dlg.PercentComplete < 100)
                            {
                                await Task.Delay(TimeSpan.FromMilliseconds(500));
                                dlg.PercentComplete += 2;
                            }
                        }
                        this.Result(cancelled ? "Progress Cancelled" : "Progress Complete");
                    })
                },
                new CommandViewModel
                {
                    Text = "Progress (No Cancel)",
                    Command = new Command(async () =>
                    {
                        using (var dlg = this.Dialogs.Progress("Progress (No Cancel)"))
                        {
                            while (dlg.PercentComplete < 100)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(1));
                                dlg.PercentComplete += 20;
                            }
                        }
                    })
                },
                new CommandViewModel
                {
                    Text = "Loading (No Cancel)",
                    Command = new Command(async () =>
                    {
                        using (this.Dialogs.Loading("Loading (No Cancel)"))
                            await Task.Delay(TimeSpan.FromSeconds(3));
                    })
                },
                new CommandViewModel
                {
                    Text = "Loading To Success",
                    Command = new Command(async () =>
                    {
                        using (this.Dialogs.Loading("Test Loading"))
                            await Task.Delay(3000);
                    })
                },
                new CommandViewModel
                {
                    Text = "Manual Loading",
                    Command = new Command(async () =>
                    {
                        this.Dialogs.ShowLoading("Manual Loading");
                        await Task.Delay(3000);
                        this.Dialogs.HideLoading();
                    })
                }
            };
        }


        ICommand LoadingCommand(MaskType mask)
        {
            return new Command(async () =>
            {
                var cancelSrc = new CancellationTokenSource();
                var config = new ProgressDialogConfig()
                    .SetTitle("Loading")
                    .SetIsDeterministic(false)
                    .SetMaskType(mask)
                    .SetCancel(onCancel: cancelSrc.Cancel);

                using (this.Dialogs.Progress(config))
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cancelSrc.Token);
                    }
                    catch { }
                }
                this.Result(cancelSrc.IsCancellationRequested ? "Loading Cancelled" : "Loading Complete");
            });
        }
    }
}
