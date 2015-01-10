BTProgressHUD is a HUD - heads up display - for your application. It allows you to show transient information to the user, to keep them informed of longer running tasks and alerts.

BTProgressHUD is based on the iOS [SVProgressHUD](https://github.com/samvermette/SVProgressHUD) project.

```csharp
using BigTed;
...

public override void ViewDidAppear (bool animated)
{
	base.ViewDidAppear (animated);
	
	//Show a HUD with a progress spinner and the text
	BTProgressHUD.Show("Hello there!");
	
	//you will need to hide it somewhere else
	//BTProgressHUD.Dismiss();
}

```

There are other forms of HUD: 

* Spinner only
* Spinner and text
* Progress
* Image and text
* Toast, modelled after the [Android Toast](http://developer.android.com/guide/topics/ui/notifiers/toasts.html) display.

Source code can be found on [GitHub](https://github.com/nicwise/BTProgressHUD/).
Any bugs, file them on GitHub and [drop me an email](mailto:nicw@fastchicken.co.nz)

Some screenshots assembled with [PlaceIt](http://placeit.breezi.com/).
