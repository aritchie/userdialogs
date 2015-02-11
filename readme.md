ACR User Dialogs for Xamarin and Windows
=========================================

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, and Windows Phone 8

To use, simply reference the nuget package in each of your platform projects.
ANDROID USERS: You must call UserDialogs.Init(Activity)
ALL OTHERS: UserDialogs.Init()

* Action Sheet (multiple choice menu)
* Alert
* Confirm
* Loading
* Login
* Network Activity
* Progress
* Prompt
* Toast

[examples](https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs)

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 