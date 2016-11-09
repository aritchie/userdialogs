using System;
using System.Threading;
using System.Threading.Tasks;
using Splat;


namespace Acr.UserDialogs
{

    public abstract class AbstractUserDialogs : IUserDialogs
    {
        const string NO_ONACTION = "OnAction should not be set as async will not use it";

        public abstract IDisposable Alert(AlertConfig config);
        public abstract IDisposable ActionSheet(ActionSheetConfig config);
        public abstract IDisposable DatePrompt(DatePromptConfig config);
        public abstract IDisposable TimePrompt(TimePromptConfig config);
        public abstract IDisposable Login(LoginConfig config);
        public abstract IDisposable Prompt(PromptConfig config);
        public abstract void ShowImage(IBitmap image, string message, int timeoutMillis);
        public abstract void ShowError(string message, int timeoutMillis);
        public abstract void ShowSuccess(string message, int timeoutMillis);
        public abstract IDisposable Toast(ToastConfig config);
        protected abstract IProgressDialog CreateDialogInstance(ProgressDialogConfig config);


        public virtual async Task<string> ActionSheetAsync(string title, string cancel, string destructive, CancellationToken? cancelToken = null, params string[] buttons)
        {
            var tcs = new TaskCompletionSource<string>();
            var cfg = new ActionSheetConfig();
            if (title != null)
                cfg.Title = title;

            // you must have a cancel option for actionsheetasync
            cfg.SetCancel(cancel, () => tcs.TrySetResult(cancel));

            if (destructive != null)
                cfg.SetDestructive(destructive, () => tcs.TrySetResult(destructive));

            foreach (var btn in buttons)
                cfg.Add(btn, () => tcs.TrySetResult(btn));

            var disp = this.ActionSheet(cfg);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }


        public virtual IDisposable Alert(string message, string title, string okText)
        {
            return this.Alert(new AlertConfig()
                .SetMessage(message)
                .SetTitle(title)
                .SetText(DialogChoice.Positive, okText ?? AlertConfig.DefaultPositive.Text)
            );
        }


        IProgressDialog loading;
        public virtual void ShowLoading(string title, MaskType? maskType)
        {
            if (this.loading != null)
                this.HideLoading();

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
            var dlg = this.CreateDialogInstance(config);
            dlg.Title = config.Title;

            if (config.AutoShow)
                dlg.Show();

            return dlg;
        }


        public virtual async Task<DialogChoice> AlertAsync(AlertConfig config, CancellationToken? cancelToken = null)
        {
            if (config.OnAction != null)
                throw new ArgumentException(NO_ONACTION);

            var tcs = new TaskCompletionSource<DialogChoice>();
            config.OnAction = x => tcs.TrySetResult(x);

            var disp = this.Alert(config);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }

        public virtual Task<DialogChoice> AlertAsync(string message, string title, string okText, CancellationToken? cancelToken = null)
        {
            return this.AlertAsync(new AlertConfig()
                .SetMessage(message)
                .SetTitle(title)
                .SetText(DialogChoice.Positive, okText ?? AlertConfig.DefaultPositive.Text),
                cancelToken
            );
        }


        public virtual IDisposable Confirm(string message, Action<bool> onAction, string title, string okText, string cancelText)
        {
            return this.Alert(new AlertConfig()
                .SetText(DialogChoice.Positive, okText ?? AlertConfig.DefaultPositive.Text)
                .SetText(DialogChoice.Neutral, cancelText ?? AlertConfig.DefaultNeutral.Text)
                .SetAction(x => onAction(x == DialogChoice.Positive))
             );
        }


        public virtual async Task<bool> ConfirmAsync(string message, string title, string okText, string cancelText, CancellationToken? cancelToken = null)
        {
            var result = await this.AlertAsync(new AlertConfig()
               .SetText(DialogChoice.Positive, okText ?? AlertConfig.DefaultPositive.Text)
               .SetText(DialogChoice.Neutral, cancelText ?? AlertConfig.DefaultNeutral.Text), 
               cancelToken
            );

            return result == DialogChoice.Positive;
        }


        public virtual async Task<DialogResult<DateTime>> DatePromptAsync(DatePromptConfig config, CancellationToken? cancelToken = null)
        {
            if (config.OnAction != null)
                throw new ArgumentException(NO_ONACTION);

            var tcs = new TaskCompletionSource<DialogResult<DateTime>>();
            config.OnAction = x => tcs.TrySetResult(x);

            var disp = this.DatePrompt(config);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }


        public virtual Task<DialogResult<DateTime>> DatePromptAsync(string title, DateTime? selectedDate, CancellationToken? cancelToken = null)
        {
            return this.DatePromptAsync(
                new DatePromptConfig
                {
                    Title = title,
                    SelectedDate = selectedDate
                },
                cancelToken
            );
        }


        public virtual async Task<DialogResult<TimeSpan>> TimePromptAsync(TimePromptConfig config, CancellationToken? cancelToken = null)
        {
            if (config.OnAction != null)
                throw new ArgumentException(NO_ONACTION);

            var tcs = new TaskCompletionSource<DialogResult<TimeSpan>>();
            config.OnAction = x => tcs.TrySetResult(x);

            var disp = this.TimePrompt(config);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }


        public virtual Task<DialogResult<TimeSpan>> TimePromptAsync(string title, TimeSpan? selectedTime, CancellationToken? cancelToken = null)
        {
            return this.TimePromptAsync(
                new TimePromptConfig
                {
                    Title = title,
                    SelectedTime = selectedTime
                },
                cancelToken
            );
        }


        public virtual async Task<DialogResult<Credentials>> LoginAsync(LoginConfig config, CancellationToken? cancelToken = null)
        {
            if (config.OnAction != null)
                throw new ArgumentException(NO_ONACTION);

            var tcs = new TaskCompletionSource<DialogResult<Credentials>>();
            config.OnAction = x => tcs.TrySetResult(x);

            var disp = this.Login(config);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }


        public virtual Task<DialogResult<Credentials>> LoginAsync(string title, string message, CancellationToken? cancelToken = null)
        {
            return this.LoginAsync(new LoginConfig
            {
                Title = title ?? LoginConfig.DefaultTitle,
                Message = message
            }, cancelToken);
        }


        public virtual async Task<DialogResult<string>> PromptAsync(PromptConfig config, CancellationToken? cancelToken = null)
        {
            if (config.OnAction != null)
                throw new ArgumentException(NO_ONACTION);

            var tcs = new TaskCompletionSource<DialogResult<string>>();
            config.OnAction = x => tcs.TrySetResult(x);

            var disp = this.Prompt(config);
            using (cancelToken?.Register(() => Cancel(disp, tcs)))
            {
                return await tcs.Task;
            }
        }


        public virtual Task<DialogResult<string>> PromptAsync(string message, string title, string okText, string cancelText, string placeholder, InputType inputType, CancellationToken? cancelToken = null)
        {
            return this.PromptAsync(new PromptConfig()
                .SetMessage(message)
                .SetTitle(title)
                .SetText(DialogChoice.Neutral, cancelText ?? PromptConfig.DefaultNeutral.Text)
                .SetText(DialogChoice.Positive, okText ?? PromptConfig.DefaultPositive.Text)
                .SetPlaceholder(placeholder)
                .SetInputType(inputType),
                cancelToken
            );
        }


        public virtual IDisposable Toast(string message, TimeSpan? dismissTimer)
        {
            return this.Toast(new ToastConfig(message)
            {
                Duration = dismissTimer ?? ToastConfig.DefaultDuration
            });
        }


        static void Cancel<TResult>(IDisposable disp, TaskCompletionSource<TResult> tcs)
        {
            disp.Dispose();
            tcs.TrySetCanceled();
        }
    }
}
