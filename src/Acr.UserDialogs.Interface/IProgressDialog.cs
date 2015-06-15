using System;


namespace Acr.UserDialogs {

	public interface IProgressDialog : IDisposable {

        string Title { get; set; }
		int PercentComplete { get; set; }
		bool IsDeterministic { get; set; }
		bool IsShowing { get; }
        MaskType MaskType { get; set; }

		void Show();
		void Hide();
        void SetCancel(Action onCancel, string cancelText = "Cancel");
    }
}
