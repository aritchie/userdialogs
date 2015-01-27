ACR User Dialogs for Xamarin and Windows
=========================================

##User Dialogs
Allows for messagebox style dialogs to be called from your shared/PCL code

To use, simply reference the nuget package in each of your platform projects.
ANDROID USERS: You must call UserDialogs.Init(Activity)
ALL OTHERS: UserDialogs.Init()

* Action Sheet (multiple choice menu)
* Alert
* Confirm
* Loading
* Login
* Progress
* Prompt
* Toast

[examples](https://github.com/aritchie/acr-xamarin-forms/blob/master/Samples/Samples/ViewModels/UserDialogViewModel.cs)

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 