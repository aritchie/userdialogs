using System;
using System.Threading;
using System.Threading.Tasks;
using Splat;


namespace Acr.UserDialogs
{
    public interface IUserDialogs
    {
        IDisposable Alert(string message, string title = null, string okText = null);
        IDisposable Alert(AlertConfig config);
        Task<DialogChoice> AlertAsync(string message, string title = null, string okText = null, CancellationToken? cancelToken = null);
        Task<DialogChoice> AlertAsync(AlertConfig config, CancellationToken? cancelToken = null);

        IDisposable ActionSheet(ActionSheetConfig config);
        Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons);

        IDisposable Confirm(string message, Action<bool> onAction, string title = null, string okText = null, string cancelText = null);
        Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, CancellationToken? cancelToken = null);

        IDisposable DatePrompt(DatePromptConfig config);
        Task<DialogResult<DateTime>> DatePromptAsync(DatePromptConfig config, CancellationToken? cancelToken = null);
        Task<DialogResult<DateTime>> DatePromptAsync(string title = null, DateTime? selectedDate = null, CancellationToken? cancelToken = null);

        IDisposable TimePrompt(TimePromptConfig config);
        Task<DialogResult<TimeSpan>> TimePromptAsync(TimePromptConfig config, CancellationToken? cancelToken = null);
        Task<DialogResult<TimeSpan>> TimePromptAsync(string title = null, TimeSpan? selectedTime = null, CancellationToken? cancelToken = null);

        IDisposable Prompt(PromptConfig config);
        Task<DialogResult<string>> PromptAsync(string message, string title = null, string okText = null, string cancelText = null, string placeholder = "", InputType inputType = InputType.Default, CancellationToken? cancelToken = null);
        Task<DialogResult<string>> PromptAsync(PromptConfig config, CancellationToken? cancelToken = null);

        IDisposable Login(LoginConfig config);
        Task<DialogResult<Credentials>> LoginAsync(string title = null, string message = null, CancellationToken? cancelToken = null);
        Task<DialogResult<Credentials>> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null);

        IProgressDialog Progress(ProgressDialogConfig config);
        IProgressDialog Loading(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null);
        IProgressDialog Progress(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null);

        void ShowLoading(string title = null, MaskType? maskType = null);
        void HideLoading();

        void ShowImage(IBitmap image, string message, int timeoutMillis = 2000);
        void ShowSuccess(string message, int timeoutMillis = 2000);
        void ShowError(string message, int timeoutMillis = 2000);

        IDisposable Toast(string title, TimeSpan? dismissTimer = null);
        IDisposable Toast(ToastConfig cfg);
    }
}