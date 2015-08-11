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
* Toasts

[examples](https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs)

Powered By:

    Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
    iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
    iOS - Toasts powered by Xamarin-iOS-MessageBar
    WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 


###Themes/Defaults

All config objects contain static vars that contain defaults which are basically used as a poor man's stylesheet.  These save you time of always have to pass what the text for OK should be.  This is particularily useful for multilingual applications.

- ActionSheetConfig
    - DefaultCancelText
    - DefaultDestructiveText
- AlertConfig
    - DefaultOkText
- ConfirmConfig
    - DefaultYes
    - DefaultNo
    - DefaultOkText
    - DefaultCancelText
- LoginConfig
    - DefaultTitle
    - DefaultOkText
    - DefaultCancelText
    - DefaultLoginPlaceholder
    - DefaultPasswordPlaceholder
- ProgressDialogConfig
    - DefaultCancelText
    - DefaultTitle
    - DefaultMaskType
- PromptConfig
    - DefaultOkText
    - DefaultCancelText
-ToastConfig
    - InfoIcon
    - InfoBackgroundColor
    - InfoTextColor
    - SuccessIcon
    - SuccessBackgroundColor
    - SuccessTextColor
    - WarnIcon
    - WarnBackgroundColor
    - WarnTextColor
    - ErrorIcon
    - ErrorBackgroundColor
    - ErrorTextColor
    - DefaultDuration

##How To Setup

---

To use, simply reference the nuget package in each of your platform projects.

###Android Initialization (In your main activity)
    UserDialogs.Init(this, useAppCompat); // pass true for appcompat/material design

###XAMARIN FORMS USERS: You must install NativeCode.Mobile.AppCompat on your android project for the material design support.  This is only until xamarin updates forms to do this!