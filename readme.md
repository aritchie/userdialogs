# ACR User Dialogs for Xamarin and Windows

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, Windows Phone 8.0 (silverlight), and Unified Windows Platform (UWP, UAP)



### Features

---

* Action Sheet (multiple choice menu)
* Alert
* Confirm
* Loading
* Login
* Progress
* Prompt
* Toasts
* [examples](https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs)


## Setup

---

To use, simply reference the nuget package in each of your platform projects.

#### iOS and Windows

    Nothing is necessary any long as of v4.x

#### Android Initialization (In your main activity)

    UserDialogs.Init(this);

#### Android Material Design/AppCompat

    In your android project, use Acr.UserDialogs.Android.AppCompat from nuget instead of Acr.UserDialogs.  Call the Init just like above.
    Xamarin Forms Users: DO NOT use appcompat unless you know how to use AppCompat.  Please wait for Xamarin to update to this or use an open source alternative


## Themes/Defaults

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


## FAQ

    Q) I'm using Xamarin Forms and getting a nullreferenceexception when using loading
    A) This happens when you run loading (or almost any dialog) from the constructor of your page or viewmodel.  The view hasn't been rendered yet, therefore there is nothing to render to.

    Q) Is Windows 8 Support?
    A) No.  I never got around to this, but I'm going to support 10 as soon as I figure it out.  It is in a very betaish mode now.  I'm hoping someone will help in this area!!

    Q) Is AppCompat/Material Dialogs supported on Android?
    A) Yes.  It a separate package.  Please make sure to install Acr.UserDialogs to your PCL, but when installing on your android project.

    Q) A new release just came out and the MvvmCross plugin is broken
    A) I don't update the MvvmCross plugin right along side Acr.UserDialogs.  I'm happy to take a PR

    Q) Do you take pull requests?
    A) Absolutely.  I may not always accept them, but I do appreciate the help.

## Powered By:

    Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
    iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
    iOS - Toasts powered by Xamarin-iOS-MessageBar
    WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/) 