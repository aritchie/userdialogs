using System;


namespace Acr.UserDialogs {

	public interface IProgressDialog : IProgressIndicator {

        string Title { get; set; }
        bool IsDeterministic { get; set; }
        void SetCancel(Action onCancel, string cancelText = "Cancel");
    }
}
