using System;
using UIKit;


namespace BigTed
{
	public static class BTProgressHUD
	{
		public static void Show(string status = null, float progress = -1, ProgressHUD.MaskType maskType = ProgressHUD.MaskType.None)
		{
			ProgressHUD.Shared.Show(status, progress, maskType);
		}

		public static void Show(string cancelCaption, Action cancelCallback, string status = null, float progress = -1, ProgressHUD.MaskType maskType = ProgressHUD.MaskType.None)
		{
			ProgressHUD.Shared.Show(cancelCaption, cancelCallback, status, progress, maskType);
		}

		public static void ShowContinuousProgress(string status = null, ProgressHUD.MaskType maskType = ProgressHUD.MaskType.None)
		{
			ProgressHUD.Shared.ShowContinuousProgress(status, maskType);
		}

		public static void ShowToast(string status, bool showToastCentered = false, double timeoutMs = 1000)
		{
			ShowToast(status, showToastCentered ? ProgressHUD.ToastPosition.Center : ProgressHUD.ToastPosition.Bottom, timeoutMs);
		}

		public static void ShowToast(string status, ProgressHUD.ToastPosition toastPosition = ProgressHUD.ToastPosition.Center, double timeoutMs = 1000)
		{
			ProgressHUD.Shared.ShowToast(status, ProgressHUD.MaskType.None, toastPosition, timeoutMs);
		}

		public static void ShowToast(string status, ProgressHUD.MaskType maskType = ProgressHUD.MaskType.None, bool showToastCentered = true, double timeoutMs = 1000)
		{
			ProgressHUD.Shared.ShowToast(status, maskType, showToastCentered ? ProgressHUD.ToastPosition.Center : ProgressHUD.ToastPosition.Bottom, timeoutMs);
		}

		public static void SetStatus(string status)
		{
			ProgressHUD.Shared.SetStatus(status);
		}

		public static void ShowSuccessWithStatus(string status, double timeoutMs = 1000)
		{
			ProgressHUD.Shared.ShowSuccessWithStatus(status, timeoutMs);
		}

		public static void ShowErrorWithStatus(string status, double timeoutMs = 1000)
		{
			ProgressHUD.Shared.ShowErrorWithStatus(status, timeoutMs);
		}

		public static void ShowImage(UIImage image, string status, double timeoutMs = 1000)
		{
			ProgressHUD.Shared.ShowImage(image, status, timeoutMs);
		}

		public static void Dismiss()
		{
			ProgressHUD.Shared.Dismiss();
		}

		public static bool IsVisible
		{
			get
			{
				return ProgressHUD.Shared.IsVisible;
			}
		}

		public static bool ForceiOS6LookAndFeel
		{
			get
			{
				return ProgressHUD.Shared.ForceiOS6LookAndFeel;
			}
			set
			{
				ProgressHUD.Shared.ForceiOS6LookAndFeel = value;
			}
		}
	}
}

