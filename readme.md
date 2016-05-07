# v5 Announcement

Acr.UserDialogs.Android.AppCompat is now obsolete.  v5 has this all internalized and uses the most modern mechanism is can to display dialogs.
Android Support Libraries should be agnostic now.  I will allow nuget to flow up to the next major (v24).

# ACR User Dialogs for Xamarin and Windows

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, and Unified Windows Platform (UWP, UAP)


### Features


* Action Sheet (multiple choice menu)
* Alert
* Confirm
* Date
* Loading
* Login
* Progress
* Prompt
* Toasts
* Time
* [examples](https://github.com/aritchie/userdialogs/blob/master/src/Samples/Samples/MainPage.cs)

## Support Platforms

* Xamarin (iOS Unified/Android)
* Universal Windows Platform (Win10/UWP)
* Portable Class Libraries (Profile 259)
* Windows Phone 8/8.1 - It is here, but parts of unimplemented.  NO FEATURE REQUESTS OR SUPPORT - YOU WANT IT, SUBMIT A TESTED PULL REQUEST!

## Setup

To use, simply reference the nuget package in each of your platform projects.

#### iOS and Windows

    Nothing is necessary any longer as of v4.x

#### Android Initialization (In your main activity)

    UserDialogs.Init(this);

### MvvmCross

    // from your PCL app.cs (remember to Init on android platform project)
    Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);


## Themes/Defaults

All config objects contain static vars that contain defaults which are basically used as a poor man's stylesheet.  These save you time of always have to pass what the text for OK should be.  This is particularily useful for multilingual applications.

- ActionSheetConfig
    - DefaultCancelText
    - DefaultDestructiveText
    - DefaultItemIcon
- AlertConfig
    - DefaultOkText
- ConfirmConfig
    - DefaultYes
    - DefaultNo
    - DefaultOkText
    - DefaultCancelText
- DatePromptConfig
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
- TimePromptConfig
    - DefaultOkText
    - DefaultCancelText
    - DefaultMinuteInterval

## FAQ

1. I'm getting a nullreferenceexception when using loading.
    * This happens when you run loading (or almost any dialog) from the constructor of your page or viewmodel.  The view hasn't been rendered yet, therefore there is nothing to render to.

2. Is Windows 8 Support?
    * No.  I never got around to this, but I'm going to support 10 as soon as I figure it out.  It is in a very betaish mode now.  I'm hoping someone will help in this area!!

3. A new release just came out and the MvvmCross plugin is broken
    * I don't update the MvvmCross plugin right along side Acr.UserDialogs.  I'm happy to take a PR

4. Do you take pull requests?
   * Absolutely.  I may not always accept them, but I do appreciate the help.

5. I'm getting "This is the PCL library, not the platform library.  Did you include the nuget package in your main "executable" project?"
    * Do exactly what it says

6. Navigating while inside of a loading/progress dialog causes exceptions or the progress no longer appears properly
    * Hide the progress dialog before navigating

7. I don't like the way X method works on platform Y
    * No problems.  Override the implementation like below


    on the platform
    public class MyCustomUserDialogs : Acr.UserDialogs.UserDialogImpl {
            public override ..
    }

    in appdelegate or the starting activity
    UserDialogs.Instance = new MyCustomUserDialogs();

8. I'm using Xamarin.Forms and I can't install the latest version! (specifically on Android)
    * Read the error you are getting from nuget!!  As of v4.3, this library now uses the latest android support libraries (23.1.1+).  Please do not file feature requests/defects against this.  This library moves with or without Xamarin Forms.  Use v4.2.1 and bug Xamarin to update their library.

## Powered By:

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* iOS - Toasts powered by Xamarin-iOS-MessageBar by @prashantvc (https://github.com/prashantvc/Xamarin.iOS-MessageBar)
* WinPhone - All dialogs by [WPToolkit](http://coding4fun.codeplex.com/)
* UWP - Coding4Fun Toolkit (http://coding4fun.codeplex.com)
* Splat - Provides a nice layer of xplat stuff by @paulcbetts (https://github.com/paulcbetts) 