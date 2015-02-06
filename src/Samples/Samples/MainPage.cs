using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;


namespace Samples {

    public class MainPage : ContentPage {
        private readonly Label lblResult;

        public MainPage() {
            this.lblResult = new Label();

            this.Content = new StackLayout {
                Children = {
                    this.lblResult,
                    Btn("Alert", this.Alert),
                    Btn("ActionSheet", this.ActionSheet),
                    Btn("Confirm", this.Confirm),
                    Btn("Login", this.Login),
                    Btn("Network Activity", this.NetworkActivity),
                    Btn("Prompt", () => this.PromptCommand(false)),
                    Btn("Prompt (Secure)", () => this.PromptCommand(true)),
                    Btn("Progress", this.Progress),
                    Btn("Progress (No Cancel)", this.ProgressNoCancel),
                    Btn("Loading", this.Loading),
                    Btn("Loading (No Cancel)", this.LoadingNoCancel),
                    Btn("Toast", this.Toast)
                }
            };
        }


        private static Button Btn(string text, Action action) {
            var btn = new Button { Text = text };
            btn.Clicked += (sender, args) => action();
            return btn;
        }


        private async void Alert() {
            await UserDialogs.Instance.AlertAsync("Test alert", "Alert Title", "CHANGE ME!");
            this.lblResult.Text = "Returned from alert!";
        }


        private void ActionSheet() {
            var cfg = new ActionSheetConfig().SetTitle("Test Title");
            for (var i = 0; i < 10; i++) {
                var display = (i + 1);
                cfg.Add("Option " + display, () => this.lblResult.Text = String.Format("Option {0} Selected", display));
            }
            UserDialogs.Instance.ActionSheet(cfg);
        }


        private async void Confirm() {
            var r = await UserDialogs.Instance.ConfirmAsync("Pick a choice", "Pick Title", "Yes", "No");
            var text = (r ? "Yes" : "No");
            this.lblResult.Text = "Confirmation Choice: " + text;
        }


        private async void Login() {
            var r = await UserDialogs.Instance.LoginAsync();
            this.lblResult.Text = String.Format(
                "Login {0} - User Name: {1} - Password: {2}",
                r.Ok ? "Success" : "Cancelled",
                r.LoginText,
                r.Password
            );
        }


        private async void NetworkActivity() {
            using (UserDialogs.Instance.NetworkIndication())
                await Task.Delay(TimeSpan.FromSeconds(3));

            this.lblResult.Text = "Done network activity";
        }


        private async void PromptCommand(bool secure) {
            var type = (secure ? "secure text" : "text");
            var msg = String.Format("Enter a {0} value", type.ToUpper());
            var r = await UserDialogs.Instance.PromptAsync(msg, inputType: InputType.Password);
            this.lblResult.Text = r.Ok
                ? "OK " + r.Text
                : secure + " Prompt Cancelled";
        }


        private async void Progress() {
            var cancelled = false;

            using (var dlg = UserDialogs.Instance.Progress("Test Progress")) {
                dlg.SetCancel(() => cancelled = true);
                while (!cancelled && dlg.PercentComplete < 100) {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    dlg.PercentComplete += 2;
                }
            }
            this.lblResult.Text = (cancelled ? "Progress Cancelled" : "Progress Complete");
        }


        private async void ProgressNoCancel() {
            using (var dlg = UserDialogs.Instance.Progress("Progress (No Cancel)")) {
                while (dlg.PercentComplete < 100) {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    dlg.PercentComplete += 20;
                }
            }
        }


        private async void Loading() {
            var cancelSrc = new CancellationTokenSource();

            using (var dlg = UserDialogs.Instance.Loading("Loading")) {
                dlg.SetCancel(cancelSrc.Cancel);

                try {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancelSrc.Token);
                }
                catch { }
            }
            this.lblResult.Text = (cancelSrc.IsCancellationRequested ? "Loading Cancelled" : "Loading Complete");
        }


        private async void LoadingNoCancel() {
            using (UserDialogs.Instance.Loading("Loading (No Cancel)")) 
                await Task.Delay(TimeSpan.FromSeconds(3));

            this.lblResult.Text = "Loading Complete";
        }


        private void Toast() {
            this.lblResult.Text = "Toast Shown";
            UserDialogs.Instance.Toast("Test Toast", onClick: () => {
                this.lblResult.Text = "Toast Pressed";
            });
        }
    }
}
