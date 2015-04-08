using System;
using System.Threading.Tasks;


namespace Acr.UserDialogs {

    public interface IUserDialogs {

        void Alert(string message, string title = null, string okText = "OK");
        void Alert(AlertConfig config);
        void ActionSheet(ActionSheetConfig config);
        void Confirm(ConfirmConfig config);
        void Prompt(PromptConfig config);
        void Login(LoginConfig config);
        IProgressDialog Progress(ProgressDialogConfig config);
		IProgressDialog Loading(string title = null, Action onCancel = null, string cancelText = "Cancel", bool show = true, MaskType maskType = MaskType.Black);
		IProgressDialog Progress(string title = null, Action onCancel = null, string cancelText = "Cancel", bool show = true, MaskType maskType = MaskType.Black);

		void ShowLoading(string title = "Loading", MaskType maskType = MaskType.Black);
        void HideLoading();
		void Toast(string message, int timeoutSeconds = 3, Action onClick = null, MaskType? maskType = null);

        Task AlertAsync(string message, string title = null, string okText = "OK");
        Task AlertAsync(AlertConfig config);
        Task<bool> ConfirmAsync(string message, string title = null, string okText = "OK", string cancelText = "Cancel");
        Task<bool> ConfirmAsync(ConfirmConfig config);
        Task<LoginResult> LoginAsync(string title = "Login", string message = null);
        Task<LoginResult> LoginAsync(LoginConfig config);
        Task<PromptResult> PromptAsync(string message, string title = null, string okText = "OK", string cancelText = "Cancel", string placeholder = "", InputType inputType = InputType.Default);
        Task<PromptResult> PromptAsync(PromptConfig config);
    }
}