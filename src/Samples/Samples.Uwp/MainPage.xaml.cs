using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Acr.UserDialogs;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Samples.Uwp {

    public sealed partial class MainPage : Page {
        readonly TextBlock lblResult;

        public MainPage() {
            this.InitializeComponent();

			this.lblResult = new TextBlock();
			this.Content = new ScrollViewer {
				Content = new StackPanel {
	                Children = {
	                    this.lblResult,
	                    Btn("Alert", this.Alert),
	                    Btn("ActionSheet", this.ActionSheet),
                        Btn("ActionSheet (async)", this.ActionSheetAsync),
	                    Btn("Confirm", this.Confirm),
	                    Btn("Login", this.Login),
						Btn("Manual Loading", this.ManualLoading),
	                    Btn("Prompt", this.Prompt),
						Btn("Prompt /w Text/No Cancel", this.PromptWithTextAndNoCancel),
	                    Btn("Progress", this.Progress),
	                    Btn("Progress (No Cancel)", this.ProgressNoCancel),
	                    Btn("Loading (Black - Default)", () => this.Loading(MaskType.Black)),
                        Btn("Loading (Clear)", () => this.Loading(MaskType.Clear)),
                        Btn("Loading (Gradient iOS)", () => this.Loading(MaskType.Gradient)),
                        Btn("Loading (None)", () => this.Loading(MaskType.Black)),
	                    Btn("Loading (No Cancel)", this.LoadingNoCancel),
                        Btn("Error", () => UserDialogs.Instance.ShowError("ERROR!")),
                        Btn("Success", () => UserDialogs.Instance.ShowSuccess("ERROR!")),
						Btn("Toast (Success)", () => this.Toast(ToastEvent.Success)),
						Btn("Toast (Info)", () => this.Toast(ToastEvent.Info)),
						Btn("Toast (Warning)", () => this.Toast(ToastEvent.Warn)),
						Btn("Toast (Error)", () => this.Toast(ToastEvent.Error)),
                        Btn("Change Default Settings", () => {
                            // CANCEL
                            ActionSheetConfig.DefaultCancelText = ConfirmConfig.DefaultCancelText = LoginConfig.DefaultCancelText = PromptConfig.DefaultCancelText = ProgressDialogConfig.DefaultCancelText = "NO WAY";

                            // OK
                            AlertConfig.DefaultOkText = ConfirmConfig.DefaultOkText = LoginConfig.DefaultOkText = PromptConfig.DefaultOkText = "Sure";

                            // CUSTOM
                            ActionSheetConfig.DefaultDestructiveText = "BOOM!";
                            ConfirmConfig.DefaultYes = "SIGN LIFE AWAY";
                            ConfirmConfig.DefaultNo = "NO WAY";
                            LoginConfig.DefaultTitle = "HIGH SECURITY";
                            LoginConfig.DefaultLoginPlaceholder = "WHO ARE YOU?";
                            LoginConfig.DefaultPasswordPlaceholder = "SUPER SECRET PASSWORD";
                            ProgressDialogConfig.DefaultTitle = "WAIT A MINUTE";

                            // TOAST
                            ToastConfig.DefaultDuration = TimeSpan.FromSeconds(5);
                            ToastConfig.InfoBackgroundColor = System.Drawing.Color.Aqua;
                            ToastConfig.SuccessBackgroundColor = System.Drawing.Color.BurlyWood;
                            ToastConfig.WarnBackgroundColor = System.Drawing.Color.BlueViolet;
                            ToastConfig.ErrorBackgroundColor = System.Drawing.Color.DeepPink;

                            UserDialogs.Instance.Alert("Default Settings Updated - Now run samples");
                        }),
                        Btn("Reset Default Settings", () => {
                            // CANCEL
                            ActionSheetConfig.DefaultCancelText = ConfirmConfig.DefaultCancelText = LoginConfig.DefaultCancelText = PromptConfig.DefaultCancelText = ProgressDialogConfig.DefaultCancelText = "Cancel";

                            // OK
                            AlertConfig.DefaultOkText = ConfirmConfig.DefaultOkText = LoginConfig.DefaultOkText = PromptConfig.DefaultOkText = "Ok";

                            // CUSTOM
                            ActionSheetConfig.DefaultDestructiveText = "Remove";
                            ConfirmConfig.DefaultYes = "Yes";
                            ConfirmConfig.DefaultNo = "No";
                            LoginConfig.DefaultTitle = "Login";
                            LoginConfig.DefaultLoginPlaceholder = "User Name";
                            LoginConfig.DefaultPasswordPlaceholder = "Password";
                            ProgressDialogConfig.DefaultTitle = "Loading";
                            ToastConfig.DefaultDuration = TimeSpan.FromSeconds(3);

                            UserDialogs.Instance.Alert("Default Settings Restored");

                            // TODO: toast defaults
                        })
	                }
				}
            };
        }


        static Button Btn(string text, Action action) {
            return new Button {
                Content = text,
                Command = new Command(action)
            };
        }


        async void Alert() {
            await UserDialogs.Instance.AlertAsync("Test alert", "Alert Title");
            this.lblResult.Text = "Returned from alert!";
        }


        void ActionSheet() {
			var cfg = new ActionSheetConfig()
				.SetTitle("Test Title");

            for (var i = 0; i < 5; i++) {
                var display = (i + 1);
                cfg.Add(
					"Option " + display,
					() => this.lblResult.Text = $"Option {display} Selected"
				);
            }
			cfg.SetDestructive(action: () => this.lblResult.Text = "Destructive BOOM Selected");
			cfg.SetCancel(action: () => this.lblResult.Text = "Cancel Selected");

            UserDialogs.Instance.ActionSheet(cfg);
        }


        public async void ActionSheetAsync() {
            var result = await UserDialogs.Instance.ActionSheetAsync("Test Title", "Cancel", "Destroy", "Button1", "Button2", "Button3");
            this.lblResult.Text = result;
        }


        async void Confirm() {
            var r = await UserDialogs.Instance.ConfirmAsync("Pick a choice", "Pick Title");
            var text = (r ? "Yes" : "No");
            this.lblResult.Text = "Confirmation Choice: " + text;
        }


        async void Login() {
			var r = await UserDialogs.Instance.LoginAsync(new LoginConfig {
				Message = "DANGER"
			});
            var status = r.Ok ? "Success" : "Cancelled";
            this.lblResult.Text = $"Login {status} - User Name: {r.LoginText} - Password: {r.Password}";
        }


		void Prompt() {
			UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
				.SetTitle("Choose Type")
				.Add("Default", () => this.PromptCommand(InputType.Default))
				.Add("E-Mail", () => this.PromptCommand(InputType.Email))
                .Add("Name", () => this.PromptCommand(InputType.Name))
				.Add("Number", () => this.PromptCommand(InputType.Number))
				.Add("Password", () => this.PromptCommand(InputType.Password))
                .Add("Numeric Password (PIN)", () => this.PromptCommand(InputType.NumericPassword))
                .Add("Phone", () => this.PromptCommand(InputType.Phone))
                .Add("Url", () => this.PromptCommand(InputType.Url))
			);
		}


		async void PromptWithTextAndNoCancel() {
			var result = await UserDialogs.Instance.PromptAsync(new PromptConfig {
				Title = "PromptWithTextAndNoCancel",
				Text = "Existing Text",
				IsCancellable = false
			});
			this.lblResult.Text = $"Result - {result.Text}";
		}


		async void PromptCommand(InputType inputType) {
			var msg = $"Enter a {inputType.ToString().ToUpper()} value";
			var r = await UserDialogs.Instance.PromptAsync(msg, inputType: inputType);
            this.lblResult.Text = r.Ok
                ? "OK " + r.Text
                : "Prompt Cancelled";
        }


        async void Progress() {
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


        async void ProgressNoCancel() {
            using (var dlg = UserDialogs.Instance.Progress("Progress (No Cancel)")) {
                while (dlg.PercentComplete < 100) {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    dlg.PercentComplete += 20;
                }
            }
        }


        async void Loading(MaskType maskType) {
            var cancelSrc = new CancellationTokenSource();

			using (var dlg = UserDialogs.Instance.Loading("Loading", maskType: maskType)) {
                dlg.SetCancel(cancelSrc.Cancel);

                try {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancelSrc.Token);
                }
                catch { }
            }
            this.lblResult.Text = (cancelSrc.IsCancellationRequested ? "Loading Cancelled" : "Loading Complete");
        }


        async void LoadingNoCancel() {
            using (UserDialogs.Instance.Loading("Loading (No Cancel)"))
                await Task.Delay(TimeSpan.FromSeconds(3));

            this.lblResult.Text = "Loading Complete";
        }


		void Toast(ToastEvent @event) {
            UserDialogs.Instance.Toast(new ToastConfig(@event, "Test Toast") {
                Duration = TimeSpan.FromSeconds(3),
                Action = () => this.lblResult.Text = "Toast Pressed"
            });
        }


		async void ManualLoading() {
			UserDialogs.Instance.ShowLoading("Manual Loading");
			await Task.Delay(3000);
			UserDialogs.Instance.HideLoading();
		}
    }
}
