using System;
using Xamarin.Forms;

namespace Acr.UserDialogs
{
	/// <summary>
	/// This class is for the internal use by toast.
	/// </summary>
	internal class ToastProxy : IToast
	{
		IToast _toastProxy = null;

		public ToastProxy()
		{
			_toastProxy = DependencyService.Get<IToast>(DependencyFetchTarget.NewInstance);

			if (_toastProxy == null)
				throw new Exception("RealObject is null, Internal instance via DependecyService was not created.");
		}

		public int Duration
		{
			get
			{
				return _toastProxy.Duration;
			}

			set
			{
				_toastProxy.Duration = value;
			}
		}

		public string Text
		{
			get
			{
				return _toastProxy.Text;
			}

			set
			{
				_toastProxy.Text = value;
			}
		}

		public void Dismiss()
		{
			_toastProxy.Dismiss();
		}

		public void Show()
		{
			_toastProxy.Show();
		}
	}
}