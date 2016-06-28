using System;
using System.Threading;
using System.Threading.Tasks;
using Splat;


namespace Acr.UserDialogs
{

    public abstract class AbstractUserDialogs : IUserDialogs
    {

        public abstract IDisposable Alert(AlertConfig config);
        public abstract IDisposable ActionSheet(ActionSheetConfig config);
        public abstract IDisposable Confirm(ConfirmConfig config);
        public abstract IDisposable DatePrompt(DatePromptConfig config);
        public abstract IDisposable TimePrompt(TimePromptConfig config);
        public abstract IDisposable Login(LoginConfig config);
        public abstract IDisposable Prompt(PromptConfig config);
        public abstract void ShowImage(IBitmap image, string message, int timeoutMillis);
        public abstract void ShowError(string message, int timeoutMillis);
        public abstract void ShowSuccess(string message, int timeoutMillis);
        public abstract void Toast(ToastConfig config);
        protected abstract IProgressDialog CreateDialogInstance();


        public virtual Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons)
        {
            var tcs = new TaskCompletionSource<string>();
            var cfg = new ActionSheetConfig();
            if (title != null)
                cfg.Title = title;

            if (cancel != null)
                cfg.SetCancel(cancel, () => tcs.TrySetResult(cancel));

            if (destructive != null)
                cfg.SetDestructive(destructive, () => tcs.TrySetResult(destructive));

            foreach (var btn in buttons)
                cfg.Add(btn, () => tcs.TrySetResult(btn));

            var disp = this.ActionSheet(cfg);
            cancelToken?.Register(disp.Dispose);

            return tcs.Task;
        }


        public virtual IDisposable Alert(string message, string title, string okText)
        {
            return this.Alert(new AlertConfig
            {
                Message = message,
                Title = title,
                OkText = okText ?? AlertConfig.DefaultOkText
            });
        }


        IProgressDialog loading;
        public virtual void ShowLoading(string title, MaskType? maskType)
        {
            if (this.loading == null)
                this.loading = this.Loading(title, null, null, true, maskType);
        }


        public virtual void HideLoading()
        {
            this.loading?.Dispose();
            this.loading = null;
        }


        public virtual IProgressDialog Loading(string title, Action onCancel, string cancelText, bool show, MaskType? maskType)
        {
            return this.Progress(new ProgressDialogConfig
            {
                Title = title ?? ProgressDialogConfig.DefaultTitle,
                AutoShow = show,
                CancelText = cancelText ?? ProgressDialogConfig.DefaultCancelText,
                MaskType = maskType ?? ProgressDialogConfig.DefaultMaskType,
                IsDeterministic = false,
                OnCancel = onCancel
            });
        }


        public virtual IProgressDialog Progress(string title, Action onCancel, string cancelText, bool show, MaskType? maskType)
        {
            return this.Progress(new ProgressDialogConfig
            {
                Title = title ?? ProgressDialogConfig.DefaultTitle,
                AutoShow = show,
                CancelText = cancelText ?? ProgressDialogConfig.DefaultCancelText,
                MaskType = maskType ?? ProgressDialogConfig.DefaultMaskType,
                IsDeterministic = true,
                OnCancel = onCancel
            });
        }


        public virtual IProgressDialog Progress(ProgressDialogConfig config)
        {
            var dlg = this.CreateDialogInstance();
            dlg.Title = config.Title;
            dlg.IsDeterministic = config.IsDeterministic;
            dlg.MaskType = config.MaskType;

            if (config.OnCancel != null)
                dlg.SetCancel(config.OnCancel, config.CancelText);

            if (config.AutoShow)
                dlg.Show();

            return dlg;
        }


        public virtual Task AlertAsync(AlertConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<object>();
            config.OnOk = () => tcs.TrySetResult(null);

            var disp = this.Alert(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }


        public virtual Task AlertAsync(string message, string title, string okText, CancellationToken? cancelToken = null)
        {
            return this.AlertAsync(new AlertConfig
            {
                Message = message,
                Title = title,
                OkText = okText ?? AlertConfig.DefaultOkText
            }, cancelToken);
        }


        public virtual Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            config.OnConfirm = x => tcs.TrySetResult(x);

            var disp = this.Confirm(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });
            return tcs.Task;
        }


        public virtual Task<bool> ConfirmAsync(string message, string title, string okText, string cancelText, CancellationToken? cancelToken = null)
        {
            return this.ConfirmAsync(new ConfirmConfig
            {
                Message = message,
                Title = title,
                CancelText = cancelText ?? ConfirmConfig.DefaultCancelText,
                OkText = okText ?? ConfirmConfig.DefaultOkText
            }, cancelToken);
        }


        public virtual Task<DatePromptResult> DatePromptAsync(DatePromptConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<DatePromptResult>();
            config.OnResult = x => tcs.TrySetResult(x);

            var disp = this.DatePrompt(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }


        public virtual Task<DatePromptResult> DatePromptAsync(string title, CancellationToken? cancelToken = null)
        {
            return this.DatePromptAsync(
                new DatePromptConfig { Title = title },
                cancelToken
            );
        }


        public virtual Task<TimePromptResult> TimePromptAsync(TimePromptConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<TimePromptResult>();
            config.OnResult = x => tcs.TrySetResult(x);

            var disp = this.TimePrompt(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });;

            return tcs.Task;
        }


        public virtual Task<TimePromptResult> TimePromptAsync(string title, CancellationToken? cancelToken = null)
        {
            return this.TimePromptAsync(
                new TimePromptConfig { Title = title },
                cancelToken
            );
        }


        public virtual Task<LoginResult> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<LoginResult>();
            config.OnResult = x => tcs.TrySetResult(x);

            var disp = this.Login(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }


        public virtual Task<LoginResult> LoginAsync(string title, string message, CancellationToken? cancelToken = null)
        {
            return this.LoginAsync(new LoginConfig
            {
                Title = title ?? LoginConfig.DefaultTitle,
                Message = message
            }, cancelToken);
        }


        public virtual Task<PromptResult> PromptAsync(PromptConfig config, CancellationToken? cancelToken = null)
        {
            var tcs = new TaskCompletionSource<PromptResult>();
            config.OnResult = x => tcs.TrySetResult(x);

            var disp = this.Prompt(config);
            cancelToken?.Register(() =>
            {
                disp.Dispose();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }


        public virtual Task<PromptResult> PromptAsync(string message, string title, string okText, string cancelText, string placeholder, InputType inputType, CancellationToken? cancelToken = null)
        {
            return this.PromptAsync(new PromptConfig
            {
                Message = message,
                Title = title,
                CancelText = cancelText ?? PromptConfig.DefaultCancelText,
                OkText = okText ?? PromptConfig.DefaultOkText,
                Placeholder = placeholder,
                InputType = inputType
            }, cancelToken);
        }


        public virtual void InfoToast(string title, string description, int timeoutMillis)
        {
            this.Toast(ToastEvent.Info, title, description, timeoutMillis);
        }


        public virtual void SuccessToast(string title, string description, int timeoutMillis)
        {
            this.Toast(ToastEvent.Success, title, description, timeoutMillis);
        }


        public virtual void WarnToast(string title, string description, int timeoutMillis)
        {
            this.Toast(ToastEvent.Warn, title, description, timeoutMillis);
        }


        public virtual void ErrorToast(string title, string description, int timeoutMillis)
        {
            this.Toast(ToastEvent.Error, title, description, timeoutMillis);
        }


        public virtual void Toast(ToastEvent toastEvent, string title, string description, int timeoutMillis)
        {
            this.Toast(new ToastConfig(toastEvent, title)
            {
                Description = description,
                Duration = TimeSpan.FromMilliseconds(timeoutMillis)
            });
        }
    }
}
