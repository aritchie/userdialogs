using System;
using BigTed;

namespace XHUD
{
	public enum MaskType
	{
		None = 1,
		Clear,
		Black,
		Gradient
	}

	public static class HUD
	{
		public static void Show(string message, int progress = -1, MaskType maskType = MaskType.None)
		{
			float p = (float)progress / 100f;
			BTProgressHUD.Show(message, p, (ProgressHUD.MaskType)maskType);
		}

		public static void Dismiss()
		{
			BTProgressHUD.Dismiss();
		}

		public static void ShowToast(string message, bool showToastCentered = true, double timeoutMs = 1000)
		{
			BTProgressHUD.ShowToast(message, showToastCentered, timeoutMs);
		}

		public static void ShowToast(string message, MaskType maskType, bool showToastCentered = true, double timeoutMs = 1000)
		{
			BTProgressHUD.ShowToast(message, (ProgressHUD.MaskType)maskType, showToastCentered, timeoutMs);
		}
	}
}