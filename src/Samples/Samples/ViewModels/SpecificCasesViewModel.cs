using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class SpecificCasesViewModel : AbstractViewModel
    {
        public SpecificCasesViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.LoadingTaskToAlert = new Command(() =>
            {
                this.Dialogs.ShowLoading("You really shouldn't use ShowLoading");
                Task.Delay(TimeSpan.FromSeconds(2))
                    .ContinueWith(x => this.Dialogs.Alert("Do you see me?"));
            });

            this.TwoDatePickers = new Command(async () =>
            {
                var v1 = await this.Dialogs.DatePromptAsync("Date 1 (Past -1 Day)", DateTime.Now.AddDays(-1));
                if (!v1.Ok)
                    return;

                var v2 = await this.Dialogs.DatePromptAsync("Date 2 (Future +1 Day)", DateTime.Now.AddDays(1));
                if (!v2.Ok)
                    return;

                this.Dialogs.Alert($"Date 1: {v1.SelectedDate} - Date 2: {v2.SelectedDate}");
            });
        }


        public ICommand LoadingTaskToAlert { get; }
        public ICommand TwoDatePickers { get; }
    }
}