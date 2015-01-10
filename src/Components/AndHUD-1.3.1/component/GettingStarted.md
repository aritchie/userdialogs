AndHUD
=======
By default AndHUD is accessible through the AndHUD.Shared static instance of the class.  You are also free to create an instance of AndHUD and use it yourself!  


Getting Started
---------------
```csharp
//Show a simple status message with an indeterminate spinner and a Clear background
AndHUD.Shared.Show(myActivity, "Status Message", MaskType.Clear);

//Show a progress with a filling circle representing the progress amount, showing 60% full
AndHUD.Shared.ShowProgress(myActivity, "Loading… 60%", 60);

//Show a success image with a message, with a Clear background, and auto-dismiss after 2 seconds
AndHUD.Shared.ShowSuccess(myActivity, "It Worked!", MaskType.Clear, TimeSpan.FromSeconds(2));

//Show an error image with a message with a Dimmed background, and auto-dismiss after 2 seconds
AndHUD.Shared.ShowError(myActivity, "It no worked :(", MaskType.Black, TimeSpan.FromSeconds(2));

//Show a toast, similar to Android toasts, but styled as AndHUD, with a clear background, auto-dismiss after 2 seconds
AndHUD.Shared.ShowToast(myActivity, "This is a non-centered Toast…", MaskType.Clear, TimeSpan.FromSeconds(2));

//Show a custom image with text
AndHUD.Shared.ShowImage(myActivity, Resource.Drawable.MyCustomImage, "Custom");

//Dismiss a HUD that will or will not be automatically timed out
AndHUD.Shared.Dismiss(myActivity);

//Show a HUD and only close it when it's clicked
AndHUD.Shared.ShowToast(this, "Click this toast to close it!", MaskType.Clear, null, true, () => AndHUD.Shared.Dismiss(this));
```

Other Options
-------------
 - **MaskType:** By default, MaskType.Black dims the background behind the HUD.  Use MaskType.Clear to prevent the dimming.  Use MaskType.None to allow interaction with views behind the HUD.
 - **Timeout:** If you provide a timeout, the HUD will automatically be dismissed after the timeout elapses, if you have not already dismissed it manually.
 - **Click Callback:** If you provide a clickCallback parameter, when the HUD is tapped by the user, the action supplied will be executed.
 - **Cancel Callback:** If you provide a cancelCallback parameter, the HUD can be cancelled by the user pressing the back button, which will cause the cancelCallback action to be executed.

