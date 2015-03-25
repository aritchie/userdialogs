using System;
using Acr.UserDialogs;
using Android.App;
using Android.OS;
using Android.Widget;
using System.Threading.Tasks;
using System.Threading;


namespace OldAndroid {

    [Activity(Label = "OldAndroid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
		private TextView lblResult;


        protected override void OnCreate(Bundle bundle) {
            UserDialogs.Init(() => this);
            base.OnCreate(bundle);
            this.SetContentView(Resource.Layout.Main);

			this.lblResult = this.FindViewById<TextView>(Resource.Id.lblResult);
			this.Bind(Resource.Id.btnAlert, async () => await this.Alert());
			this.Bind(Resource.Id.btnActionSheet, this.ActionSheet);
			this.Bind(Resource.Id.btnConfirm, async () => await this.Confirm());
			this.Bind(Resource.Id.btnLogin, async () => await this.Login());
			this.Bind(Resource.Id.btnPrompt, this.Prompt);
			this.Bind(Resource.Id.btnPromptNo, async () => await this.PromptWithTextAndNoCancel());
			this.Bind(Resource.Id.btnLoading, async () => await this.Loading());
			this.Bind(Resource.Id.btnLoadingNoCancel, async () => await this.LoadingNoCancel());
			this.Bind(Resource.Id.btnProgress, async () => await this.Progress());
			this.Bind(Resource.Id.btnProgressNoCancel, async () => await this.ProgressNoCancel());
			this.Bind(Resource.Id.btnToast, this.Toast);
        }


		private void Bind(int id, Action action) {
			this.FindViewById<Button>(id).Click += (sender, e) => action();
		}


		private async Task Alert() {
			await UserDialogs.Instance.AlertAsync("Test alert", "Alert Title", "CHANGE ME!");
			this.lblResult.Text = "Returned from alert!";
		}


		private void ActionSheet() {
			var cfg = new ActionSheetConfig()
				.SetTitle("Test Title");

			for (var i = 0; i < 5; i++) {
				var display = (i + 1);
				cfg.Add(
					"Option " + display, 
					() => this.lblResult.Text = String.Format("Option {0} Selected", display)
				);
			}
			cfg.SetDestructive("BOOM", () => this.lblResult.Text = "Destructive BOOM Selected");
			cfg.SetCancel("Cancel", () => this.lblResult.Text = "Cancel Selected");

			UserDialogs.Instance.ActionSheet(cfg);
		}


		private async Task Confirm() {
			var r = await UserDialogs.Instance.ConfirmAsync("Pick a choice", "Pick Title", "Yes", "No");
			var text = (r ? "Yes" : "No");
			this.lblResult.Text = "Confirmation Choice: " + text;
		}


		private async Task Login() {
			var r = await UserDialogs.Instance.LoginAsync(new LoginConfig {
				Title = "HIGH SECURITY",
				Message = "DANGER",
				LoginPlaceholder = "User Name Placeholder",
				PasswordPlaceholder = "Password Placeholder",
				OkText = "LOGIN",
				CancelText = "NO"
			});
			this.lblResult.Text = String.Format(
				"Login {0} - User Name: {1} - Password: {2}",
				r.Ok ? "Success" : "Cancelled",
				r.LoginText,
				r.Password
			);
		}


		private void Prompt() {
			UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
				.SetTitle("Choose Type")
				.Add("Default", () => this.PromptCommand(InputType.Default))
				.Add("E-Mail", () => this.PromptCommand(InputType.Email))
				.Add("Name", () => this.PromptCommand(InputType.Name))
				.Add("Number", () => this.PromptCommand(InputType.Number))
				.Add("Password", () => this.PromptCommand(InputType.Password))
				.Add("Phone", () => this.PromptCommand(InputType.Phone))
				.Add("Url", () => this.PromptCommand(InputType.Url))
			);
		}


		private async Task PromptWithTextAndNoCancel() {
			var result = await UserDialogs.Instance.PromptAsync(new PromptConfig {
				Title = "PromptWithTextAndNoCancel",
				Text = "Existing Text",
				IsCancellable = false
			});
			this.lblResult.Text = String.Format("Result - {0}", result.Text);
		}


		private async void PromptCommand(InputType inputType) {
			var msg = String.Format("Enter a {0} value", inputType.ToString().ToUpper());
			var r = await UserDialogs.Instance.PromptAsync(msg, inputType: inputType);
			this.lblResult.Text = r.Ok
				? "OK " + r.Text
				: "Prompt Cancelled";
		}


		private async Task Progress() {
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


		private async Task ProgressNoCancel() {
			using (var dlg = UserDialogs.Instance.Progress("Progress (No Cancel)")) {
				while (dlg.PercentComplete < 100) {
					await Task.Delay(TimeSpan.FromSeconds(1));
					dlg.PercentComplete += 20;
				}
			}
		}


		private async Task Loading() {
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


		private async Task LoadingNoCancel() {
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


		private async Task ManualLoading() {
			UserDialogs.Instance.ShowLoading("Manual Loading");
			await Task.Delay(3000);
			UserDialogs.Instance.HideLoading();
		}
    }
}