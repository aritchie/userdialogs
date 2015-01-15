using System;


namespace Acr.UserDialogs {
    
	public interface IProgressDialog : IProgressIndicator {

        void SetCancel(Action onCancel, string cancelText = "Cancel");
    }
}
