ACR User Dialogs for Xamarin and Windows
=========================================

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, and Windows Phone 8

To use, simply reference the nuget package in each of your platform projects.

Additional Android Initialization (In your main activity)
Android Xamarin Forms:

    UserDialogs.Init(() => (Activity)Forms.Context);

Android MvvmCross:

    UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>.Activity);

Android Manually:

    UserDialogs.Init(Activity Factory Function);


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