#4.0 AND XAMARIN FORMS USERS: You must install NativeCode.Mobile.AppCompat on your android project for the material design support.  This is only until xamarin updates forms to do this!

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

Powered By:

    Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
    iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
    iOS - Toasts powered by Xamarin-iOS-MessageBar
    WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 


###Themes/Defaults

    TODO

##How To Setup

---

To use, simply reference the nuget package in each of your platform projects.

###Android Initialization (In your main activity)
    UserDialogs.Init(this);

