using System;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;
using Splat;

namespace Samples
{

    public class MainPage : TabbedPage
    {

        public MainPage()
        {
            Device.OnPlatform(() =>
            {
                this.Padding = new Thickness(0, 30, 0, 0);
            });
            this.AddPage(
                "Standard Dialogs",
                Btn("Alert", this.Alert),
                Btn("Alert (Long Text)", this.AlertLongText),
                Btn("ActionSheet", this.ActionSheet),
                Btn("ActionSheet (async)", this.ActionSheetAsync),
                Btn("Confirm", this.Confirm),
                Btn("Login", this.Login),
                Btn("Prompt", this.Prompt),
                Btn("Prompt /w Text/No Cancel", this.PromptWithTextAndNoCancel),
                Btn("Error", () => UserDialogs.Instance.ShowError("ERROR!")),
                Btn("Success", () => UserDialogs.Instance.ShowSuccess("ERROR!"))
            );

            this.AddPage(
                "Progress",
                Btn("Manual Loading", this.ManualLoading),
                Btn("Progress", this.Progress),
                Btn("Progress (No Cancel)", this.ProgressNoCancel),
                Btn("Loading (Black - Default)", () => this.Loading(MaskType.Black)),
                Btn("Loading (Clear)", () => this.Loading(MaskType.Clear)),
                Btn("Loading (Gradient iOS)", () => this.Loading(MaskType.Gradient)),
                Btn("Loading (None)", () => this.Loading(MaskType.Black)),
                Btn("Loading (No Cancel)", this.LoadingNoCancel),
                Btn("Loading to Success", this.LoadingToSuccess)
            );

            this.AddPage(
                "Toasts",
                Btn("Success", () => this.Toast(ToastEvent.Success)),
                Btn("Info", () => this.Toast(ToastEvent.Info)),
                Btn("Warning", () => this.Toast(ToastEvent.Warn)),
                Btn("Error", () => this.Toast(ToastEvent.Error))
            );

            this.AddPage(
                "Settings/Themes",
                Btn("Change Default Settings", () =>
                {
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
                    ToastConfig.SuccessTextColor = System.Drawing.Color.Blue;
                    ToastConfig.SuccessBackgroundColor = System.Drawing.Color.BurlyWood;
                    ToastConfig.WarnBackgroundColor = System.Drawing.Color.BlueViolet;
                    ToastConfig.ErrorBackgroundColor = System.Drawing.Color.DeepPink;

                    UserDialogs.Instance.Alert("Default Settings Updated - Now run samples");
                }),
                Btn("Reset Default Settings", () =>
                {
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
            );
        }


        void AddPage(string title, params View[] views)
        {
            var content = new StackLayout();
            foreach (var view in views)
                content.Children.Add(view);

            this.Children.Add(new NavigationPage(new ContentPage
            {
                Content = content,
                Title = title
            })
            { Title = title });
        }


        static Button Btn(string text, Action action)
        {
            return new Button
            {
                Text = text,
                Command = new Command(action)
            };
        }


        void Result(string msg)
        {
            UserDialogs.Instance.Alert(msg);
        }


        async void Alert()
        {
            await UserDialogs.Instance.AlertAsync("Test alert", "Alert Title");
            //this.lblResult.Text = "Returned from alert!";
        }


        void AlertLongText()
        {
            UserDialogs.Instance.Alert("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consequat diam nec eros ornare, vitae cursus nunc molestie. Praesent eget lacus non neque cursus posuere. Nunc venenatis quam sed justo bibendum, ut convallis arcu lobortis. Vestibulum in diam nisl. Nulla pulvinar lacus vel laoreet auctor. Morbi mi urna, viverra et accumsan in, pretium vel lorem. Proin elementum viverra commodo. Sed nunc justo, sollicitudin eu fermentum vitae, faucibus a est. Nulla ante turpis, iaculis et magna sed, facilisis blandit dolor. Morbi est felis, semper non turpis non, tincidunt consectetur enim.");
        }

        void ActionSheet()
        {
            var cfg = new ActionSheetConfig()
                .SetTitle("Test Title");

            var testImage = BitmapLoader.Current.LoadFromResource("icon.png", null, null).Result;

            for (var i = 0; i < 5; i++)
            {
                var display = (i + 1);
                cfg.Add(
                    "Option " + display,
                    () => this.Result($"Option {display} Selected"),
                    testImage
                );
            }
            cfg.SetDestructive(action: () => this.Result("Destructive BOOM Selected"));
            cfg.SetCancel(action: () => this.Result("Cancel Selected"));

            UserDialogs.Instance.ActionSheet(cfg);
        }


        public async void ActionSheetAsync()
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("Test Title", "Cancel", "Destroy", "Button1", "Button2", "Button3");
            this.Result(result);
        }


        async void Confirm()
        {
            var r = await UserDialogs.Instance.ConfirmAsync("Pick a choice", "Pick Title");
            var text = (r ? "Yes" : "No");
            this.Result($"Confirmation Choice: {text}");
        }


        async void Login()
        {
            var r = await UserDialogs.Instance.LoginAsync(new LoginConfig
            {
                Message = "DANGER"
            });
            var status = r.Ok ? "Success" : "Cancelled";
            this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
        }


        void Prompt()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                .SetTitle("Choose Type")
                .Add("Default", () => this.PromptCommand(InputType.Default))
                .Add("E-Mail", () => this.PromptCommand(InputType.Email))
                .Add("Name", () => this.PromptCommand(InputType.Name))
                .Add("Number", () => this.PromptCommand(InputType.Number))
                .Add("Number with Decimal", () => this.PromptCommand(InputType.DecimalNumber))
                .Add("Password", () => this.PromptCommand(InputType.Password))
                .Add("Numeric Password (PIN)", () => this.PromptCommand(InputType.NumericPassword))
                .Add("Phone", () => this.PromptCommand(InputType.Phone))
                .Add("Url", () => this.PromptCommand(InputType.Url))
            );
        }


        async void PromptWithTextAndNoCancel()
        {
            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                Title = "PromptWithTextAndNoCancel",
                Text = "Existing Text",
                IsCancellable = false
            });
            this.Result($"Result - {result.Text}");
        }


        async void PromptCommand(InputType inputType)
        {
            var msg = $"Enter a {inputType.ToString().ToUpper()} value";
            var r = await UserDialogs.Instance.PromptAsync(msg, inputType: inputType);
            this.Result(r.Ok
                ? "OK " + r.Text
                : "Prompt Cancelled");
        }


        async void Progress()
        {
            var cancelled = false;

            using (var dlg = UserDialogs.Instance.Progress("Test Progress"))
            {
                dlg.SetCancel(() => cancelled = true);
                while (!cancelled && dlg.PercentComplete < 100)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(500));
                    dlg.PercentComplete += 2;
                }
            }
            this.Result(cancelled ? "Progress Cancelled" : "Progress Complete");
        }


        async void ProgressNoCancel()
        {
            using (var dlg = UserDialogs.Instance.Progress("Progress (No Cancel)"))
            {
                while (dlg.PercentComplete < 100)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    dlg.PercentComplete += 20;
                }
            }
        }


        async void Loading(MaskType maskType)
        {
            var cancelSrc = new CancellationTokenSource();

            using (var dlg = UserDialogs.Instance.Loading("Loading", maskType: maskType))
            {
                dlg.SetCancel(cancelSrc.Cancel);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancelSrc.Token);
                }
                catch { }
            }
            this.Result(cancelSrc.IsCancellationRequested ? "Loading Cancelled" : "Loading Complete");
        }


        async void LoadingNoCancel()
        {
            using (UserDialogs.Instance.Loading("Loading (No Cancel)"))
                await Task.Delay(TimeSpan.FromSeconds(3));
        }


        void Toast(ToastEvent @event)
        {
            UserDialogs.Instance.Toast(new ToastConfig(@event, @event.ToString(), "Testing toast functionality....fun!")
            {
                Duration = TimeSpan.FromSeconds(3),
                Action = () => this.Result("Toast Pressed")
            });
        }


        async void ManualLoading()
        {
            UserDialogs.Instance.ShowLoading("Manual Loading");
            await Task.Delay(3000);
            UserDialogs.Instance.HideLoading();
        }


        async void LoadingToSuccess()
        {
            using (UserDialogs.Instance.Loading("Test Loading"))
                await Task.Delay(3000);

            UserDialogs.Instance.ShowSuccess("Success Loading!");
        }
    }
}
