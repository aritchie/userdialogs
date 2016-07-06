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
            //this.LoadingTaskToAlert = new Command(async () =>
            //{
            //    using (this.Dialogs.Loading("Hold Up"))
            //    {
            //        await Task.Delay(TimeSpan.FromSeconds(2));
            //    }
            //    await this.Dialogs.AlertAsync("Do you see me?");
            //});
            this.LoadingTaskToAlert = new Command(() =>
            {
                this.Dialogs.ShowLoading("You really shouldn't use ShowLoading");
                Task.Delay(TimeSpan.FromSeconds(2))
                    .ContinueWith(x =>
                    {
                        this.Dialogs.HideLoading();
                        this.Dialogs.Alert("Do you see me?");
                    });
            });
        }


        public ICommand LoadingTaskToAlert { get; }
    }
}