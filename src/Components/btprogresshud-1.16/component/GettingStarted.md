#Usage

Have a look at the BTProgressHUDDemo project.

Firstly, you need to use the BigTed namespace

```csharp
using BigTed;
```

Then, there are a few main static methods for showing the HUD:

```csharp
BTProgressHUD.Show(); //shows the spinner
BTProgressHUD.Show("Oh hai"); //show spinner + text
BTProgressHUD.ShowSuccessWithStatus("Wow, that worked"); //A big TICK with text
BTProgressHUD.ShowErrorWithStatus("Fail!"); //A big CROSS with text
BTProgressHUD.ShowToast("Hello from Toast"); //show an Android-style toast
```
All of these can be dismissed, once your processing is finished, with

```csharp
BTProgressHUD.Dismiss();
```

ShowToast, ShowSuccessWithStatus, ShowErrorWithStatus, and ShowImage all auto-dismiss.

BTProgressHUD is aware of the thread you are calling from, and ensures that HUDs are always manipulated from the UI thread.

#Using on iOS6

If your app needs to work against the iOS6 SDK, but with the user on iOS7, you can set 

```csharp
BTProgressHUD.ForceiOS6LookAndFeel = true;
```

And all the dialog boxes will look like iOS6 (black with white text, not the other way)

#Other Show options

You can call Show with the following parameters

* status: <string> - show status text
* progress: <float> - show a progress circle with 0.0 - 1.0 of progress. Call again to change the progress.
* maskType: <ProgressHUD.MaskType> - show with the background (the whole window) clear, black or gradient. Default is none, which allows interaction with the underlying elements.

```csharp
public enum MaskType
{
	None = 1, // allow user interactions, don't dim background UI (default)
	Clear, // disable user interactions, don't dim background UI
	Black, // disable user interactions, dim background UI with 50% translucent black
	Gradient // disable user interactions, dim background UI with translucent radial gradient (a-la-alertView)
}
```

#ShowToast
The toast can be centered or at the bottom of the screen, like Android. This is controlled by the second parameter.

```csharp
BTProgressHUD.ShowToast ("Your download finished", showToastCentered: false);
```

#ShowSuccess/Error/ShowImage
This method dismisses the activity after 1 second. You can provide your own images if needed - make them 28x28 white PNGs.

```csharp
BTProgressHUD.ShowSuccessWithStatus ("Wow, that worked"); //A big TICK with text
BTProgressHUD.ShowErrorWithStatus ("Fail!"); //A big CROSS with text
BTProgressHUD.ShowImage (UIImage.FromFile("an-image-file.png"), "Nice one Stu!");
```

You can use the timeout parameter of ShowImage to control the time before it's dismissed.

# Other Resources

* [Source code](https://github.com/nicwise/BTProgressHUD/)
