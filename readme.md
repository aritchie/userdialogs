#ACR User Dialogs for Xamarin and Windows

---

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, and Windows Phone 8

##Features

---

* Action Sheet (multiple choice menu)
* Alert
* Confirm
* Loading
* Login
* Progress
* Prompt
* Toast

[examples](https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs)

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 



##How To Setup

---

To use, simply reference the nuget package in each of your platform projects.

###iOS Initialization

    public class AppDelegate : UIApplicationDelegate {  // or your custom appdelegate inheritance (xamarin forms, mvvmcross)
        public override bool FinishedLaunching(UIApplication app, NSDictionary options) {
            UserDialogs.Init();
            .. your init logic
            return base.FinishedLaunching(app, options);
        }
    }

###Android Initialization (In your main activity)

    // Xamarin Forms
    UserDialogs.Init(() => (Activity)Forms.Context);

    // MvvmCross
    UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>.Activity);

    // Using your own activity provider (you need to manage what the top activity is)
    UserDialogs.Init(Activity Factory Function);

###Windows Phone

    // in your mainpage constructor
    UserDialogs.Init();

