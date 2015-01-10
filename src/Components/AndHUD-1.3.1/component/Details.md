AndHUD
==========

AndHUD is a Progress / HUD library for Android which allows you to easily add amazing HUDs to your app!

Features
--------
 - Spinner (with and without Text)
 - Progress (with and without Text)
 - Image (with and without Text)
 - Success / Error (with and without Text)
 - Toasts
 - Xamarin.Android Support
 - Xamarin Component store
 - Similar API and functionality to BTProgressHUD for iOS
 - XHUD Optional API that is in parity with BTProgressHUD's XHUD API
 

Quick and Simple
----------------
```csharp
//Show a simple status message with an indeterminate spinner and a Clear background
AndHUD.Shared.Show(myActivity, "Status Message", MaskType.Clear);

//Show a progress with a filling circle representing the progress amount, showing 60% full
AndHUD.Shared.ShowProgress(myActivity, "Loading… 60%", 60);

//Show a success image with a message, with a Clear background, and auto-dismiss after 2 seconds
AndHUD.Shared.ShowSuccess(myActivity, "It Worked!", MaskType.Clear, TimeSpan.FromSeconds(2));
```


Thanks
------
Thanks to Nic Wise (@fastchicken) who inspired the creation of this with his component BTProgressHUD https://components.xamarin.com/view/btprogresshud/

It was so awesome for iOS that I needed to have it on Android as well :)

