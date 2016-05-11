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
        Task AlertAsync(string message, string title = null, string okText = null, CancellationToken? cancelToken = null);
        Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null);

        IDisposable ActionSheet(ActionSheetConfig config);
        Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons);

        IDisposable Confirm(ConfirmConfig config);
        Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, CancellationToken? cancelToken = null);
        Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null);

        IDisposable DatePrompt(DatePromptConfig config);
        Task<DatePromptResult> DatePromptAsync(DatePromptConfig config, CancellationToken? cancelToken = null);
        Task<DatePromptResult> DatePromptAsync(string title = null, CancellationToken? cancelToken = null);

        IDisposable TimePrompt(TimePromptConfig config);
        Task<TimePromptResult> TimePromptAsync(TimePromptConfig config, CancellationToken? cancelToken = null);
        Task<TimePromptResult> TimePromptAsync(string title = null, CancellationToken? cancelToken = null);

        IDisposable Prompt(PromptConfig config);
        Task<PromptResult> PromptAsync(string message, string title = null, string okText = null, string cancelText = null, string placeholder = "", InputType inputType = InputType.Default, CancellationToken? cancelToken = null);
        Task<PromptResult> PromptAsync(PromptConfig config, CancellationToken? cancelToken = null);

        IDisposable Login(LoginConfig config);
        Task<LoginResult> LoginAsync(string title = null, string message = null, CancellationToken? cancelToken = null);
        Task<LoginResult> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null);

        IProgressDialog Progress(ProgressDialogConfig config);
        IProgressDialog Loading(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null);
        IProgressDialog Progress(string title = null, Action onCancel = null, string cancelText = null, bool show = true, MaskType? maskType = null);

        void ShowLoading(string title = null, MaskType? maskType = null);
        void HideLoading();

        void ShowImage(IBitmap image, string message, int timeoutMillis = 2000);
        void ShowSuccess(string message, int timeoutMillis = 2000);
        void ShowError(string message, int timeoutMillis = 2000);

        void InfoToast(string title, string description = null, int timeoutMillis = 3000);
        void SuccessToast(string title, string description = null, int timeoutMillis = 3000);
        void WarnToast(string title, string description = null, int timeoutMillis = 3000);
        void ErrorToast(string title, string description = null, int timeoutMillis = 3000);
        void Toast(ToastConfig cfg);
    }
}