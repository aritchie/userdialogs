using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Splat;
using Xamarin.Forms;


namespace Samples.ViewModels
{
    public class StandardViewModel : AbstractViewModel
    {
        public StandardViewModel(IUserDialogs dialogs) : base(dialogs)
        {
            this.Alert = new Command(() => this.Dialogs.Alert("Test alert", "Alert Title"));
            this.AlertLongText = new Command(() =>
                this.Dialogs.Alert("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consequat diam nec eros ornare, vitae cursus nunc molestie. Praesent eget lacus non neque cursus posuere. Nunc venenatis quam sed justo bibendum, ut convallis arcu lobortis. Vestibulum in diam nisl. Nulla pulvinar lacus vel laoreet auctor. Morbi mi urna, viverra et accumsan in, pretium vel lorem. Proin elementum viverra commodo. Sed nunc justo, sollicitudin eu fermentum vitae, faucibus a est. Nulla ante turpis, iaculis et magna sed, facilisis blandit dolor. Morbi est felis, semper non turpis non, tincidunt consectetur enim.")
            );

            this.ActionSheet = new Command(() =>
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

                this.Dialogs.ActionSheet(cfg);
            });

            this.ActionSheetAsync = new Command(async () =>
            {
                var result = await this.Dialogs.ActionSheetAsync("Test Title", "Cancel", "Destroy", null, "Button1", "Button2", "Button3");
                this.Result(result);
            });

            this.Confirm = new Command(async () =>
            {
                var r = await this.Dialogs.ConfirmAsync("Pick a choice", "Pick Title");
                var text = (r ? "Yes" : "No");
                this.Result($"Confirmation Choice: {text}");
            });

            this.Login = new Command(async () =>
            {
                var r = await this.Dialogs.LoginAsync(new LoginConfig
                {
                    Message = "DANGER"
                });
                var status = r.Ok ? "Success" : "Cancelled";
                this.Result($"Login {status} - User Name: {r.LoginText} - Password: {r.Password}");
            });

            this.Prompt = new Command(() => this.Dialogs.ActionSheet(new ActionSheetConfig()
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
                .SetCancel()
            ));
            this.PromptNoTextOrCancel = new Command(async () =>
            {
                var result = await this.Dialogs.PromptAsync(new PromptConfig
                {
                    Title = "PromptWithTextAndNoCancel",
                    Text = "Existing Text",
                    IsCancellable = false
                });
                this.Result($"Result - {result.Text}");
            });

            this.Date = new Command(async () =>
            {
                var result = await this.Dialogs.DatePromptAsync(new DatePromptConfig
                {
                    IsCancellable = true,
                    MinimumDate = DateTime.Now.AddDays(-3),
                    MaximumDate = DateTime.Now.AddDays(1)
                });
                this.Result($"Date Prompt: {result.Ok} - Value: {result.SelectedDate}");
            });
            this.Time = new Command(async () =>
            {
                var result = await this.Dialogs.TimePromptAsync(new TimePromptConfig
                {
                    IsCancellable = true
                });
                this.Result($"Time Prompt: {result.Ok} - Value: {result.SelectedTime}");
            });
        }


        public ICommand Alert { get; }
        public ICommand AlertLongText { get; }
        public ICommand ActionSheet { get; }
        public ICommand ActionSheetAsync { get; }
        public ICommand Confirm { get; }
        public ICommand Login { get; }
        public ICommand Prompt { get; }
        public ICommand PromptNoTextOrCancel { get; }
        public ICommand Date { get; }
        public ICommand Time { get; }


        async Task PromptCommand(InputType inputType)
        {
            var msg = $"Enter a {inputType.ToString().ToUpper()} value";
            var r = await UserDialogs.Instance.PromptAsync(msg, inputType: inputType);
            this.Result(r.Ok
                ? "OK " + r.Text
                : "Prompt Cancelled");
        }
    }
}
