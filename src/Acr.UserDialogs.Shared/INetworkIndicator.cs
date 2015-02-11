using System;


namespace Acr.UserDialogs {

	public interface INetworkIndicator : IDisposable {

		bool IsShowing { get; }
		void Show();
		void Hide();
	}
}

