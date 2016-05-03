using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class ProgressViewModel : AbstractViewModel
    {
        public ProgressViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Loading = this.LoadingCommand(MaskType.Black);
            this.LoadingClear = this.LoadingCommand(MaskType.Clear);
            this.LoadingGradient = this.LoadingCommand(MaskType.Gradient);
            this.LoadingNone = this.LoadingCommand(MaskType.None);

            this.ShowError = new Command(() => this.Dialogs.ShowError("TEST ERROR!"));
            this.ShowSuccess = new Command(() => this.Dialogs.ShowError("TEST SUCCESS!"));

            this.Progress = new Command(async () =>
            {
                var cancelled = false;

                using (var dlg = this.Dialogs.Progress("Test Progress"))
                {
                    dlg.SetCancel(() => cancelled = true);
                    while (!cancelled && dlg.PercentComplete < 100)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500));
                        dlg.PercentComplete += 2;
                    }
                }
                this.Result(cancelled ? "Progress Cancelled" : "Progress Complete");
            });

            this.ProgressNoCancel = new Command(async () =>
            {
                using (var dlg = this.Dialogs.Progress("Progress (No Cancel)"))
                {
                    while (dlg.PercentComplete < 100)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        dlg.PercentComplete += 20;
                    }
                }
            });

            this.LoadingNoCancel = new Command(async () =>
            {
                using (this.Dialogs.Loading("Loading (No Cancel)"))
                    await Task.Delay(TimeSpan.FromSeconds(3));
            });

            this.LoadingToSuccess = new Command(async () =>
            {
                using (this.Dialogs.Loading("Test Loading"))
                    await Task.Delay(3000);

                this.Dialogs.ShowSuccess("Success Loading!");
            });

            this.ManualLoading = new Command(async () =>
            {
                this.Dialogs.ShowLoading("Manual Loading");
                await Task.Delay(3000);
                this.Dialogs.HideLoading();
            });
        }



        public ICommand Progress { get; }
        public ICommand ProgressNoCancel { get; }
        public ICommand Loading { get; }
        public ICommand LoadingClear { get; }
        public ICommand LoadingGradient { get; }
        public ICommand LoadingNone { get; }
        public ICommand ShowError { get; }
        public ICommand ShowSuccess { get; }
        public ICommand LoadingToSuccess { get; }
        public ICommand LoadingNoCancel { get; }
        public ICommand ManualLoading { get; }


        ICommand LoadingCommand(MaskType mask)
        {
            return new Command(async () =>
            {
                var cancelSrc = new CancellationTokenSource();

                using (var dlg = this.Dialogs.Loading("Loading", maskType: mask))
                {
                    dlg.SetCancel(cancelSrc.Cancel);

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
