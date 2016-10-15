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
* [examples](https://github.com/aritchie/userdialogs/tree/master/src/Samples/Samples)

## Support Platforms

* iOS 7+
* Android
* Universal Windows Platform (Win10/UWP)
* Portable Class Libraries (Profile 259)

## Experimental Platforms (no support)
* tvOS
* WPF/NET45
* Windows Phone 8/8.1
* MacOS - coming soon


## Setup

To use, simply reference the nuget package in each of your platform projects.

#### iOS and Windows

    Nothing is necessary any longer as of v4.x.  There is an Init function for iOS but it is OPTIONAL and only required if you want/need to control
    the top level viewcontroller for things like iOS extensions.  Progress prompts will not use this factory function though!

#### Android Initialization (In your main activity)

    UserDialogs.Init(this);
    OR UserDialogs.Init(() => provide your own top level activity provider)
    OR MvvmCross - UserDialogs.Init(() => Mvx.Resolve<IMvxTopActivity>().Activity)
    OR Xamarin.Forms - UserDialogs.Init(() => (Activity)Forms.Context)

### MvvmCross

    // from your PCL app.cs (remember to Init on android platform project)
    Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);


## Themes/Defaults

All config objects contain static vars that contain defaults which are basically used as a poor man's stylesheet.  These save you time of always have to pass what the text for OK should be.  This is particularily useful for multilingual applications.

- ActionSheetConfig
    - DefaultAndroidStyleId
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
    - DefaultDuration
    - DefaultMessageTextColor
    - DefaultActionTextColor
    - DefaultBackgroundColor

- TimePromptConfig
    - DefaultOkText
    - DefaultCancelText
    - DefaultMinuteInterval

## FAQ

1. I'm getting a nullreferenceexception when using loading.
    * This happens when you run loading (or almost any dialog) from the constructor of your page or viewmodel.  The view hasn't been rendered yet, therefore there is nothing to render to.

2. I'm getting "This is the PCL library, not the platform library.  Did you include the nuget package in your main "executable" project?"
    * Do exactly what it says

3. Navigating while inside of a loading/progress dialog causes exceptions or the progress no longer appears properly
    * Hide the progress dialog before navigating

4. I don't like the way X method works on platform Y
    * No problems.  Override the implementation like below


    on the platform
    public class MyCustomUserDialogs : Acr.UserDialogs.UserDialogImpl {
            public override ..
    }

    in appdelegate or the starting activity
    UserDialogs.Instance = new MyCustomUserDialogs();

## Powered By:

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* iOS - Modal Date/Time Dialogs provided by [SharpMobileCode](https://github.com/SharpMobileCode/ModalPickerViewController/blob/master/ModalPickerSample/ModalPickerSampleViewController.cs)
* iOS - Toasts powered by TTGSnackBar ported by @MarcBruins (https://github.com/MarcBruins/TTGSnackbar-Xamarin-iOS)
* iOS - Date/Time Picker powered by AIDatePicker ported by @MarcBruins (https://github.com/MarcBruins/AIDatePickerController-Xamarin-iOS)
* UWP - Coding4Fun Toolkit (http://coding4fun.codeplex.com)
* Splat - Provides a nice layer of xplat stuff by @paulcbetts (https://github.com/paulcbetts) 
