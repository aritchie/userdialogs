# As of March 5, 2021 - this library is now in lockdown.  You are welcome to submit PR's for issues you may be having and they will be reviewed, but I will no longer be adding features or frontlining issues any longer.

# <img src="icon.png" width="71" height="71"/> ACR User Dialogs

A cross platform library that allows you to call for standard user dialogs from a shared/portable library.
Supports Android, iOS, and Unified Windows Platform (UWP, UAP)

[![NuGet](https://img.shields.io/nuget/v/Acr.UserDialogs.svg?maxAge=2592000)](https://www.nuget.org/packages/Acr.UserDialogs/)
[![Build status](https://dev.azure.com/allanritchie/Plugins/_apis/build/status/UserDialogs)](https://dev.azure.com/allanritchie/Plugins/_build/latest?definitionId=8)


### Features

* Action Sheets
* Alert
* Confirm
* Date
* Loading/Progress
* Login
* Prompt
* Toasts
* Time
* [Samples](https://github.com/aritchie/userdialogs/tree/master/src/Samples/Samples)


## Support Platforms

* iOS 8+
* Android
* Universal Windows Platform (Win10/UWP)
* NET Standard 2.0

## Setup

To use, simply reference the nuget package in each of your platform projects.  If you are getting issues with System.Drawing.Color, please make sure you are using the latest version of Xamarin

#### iOS and Windows

    Nothing is necessary any longer as of v4.x.  There is an Init function for iOS but it is OPTIONAL and only required if you want/need to control
    the top level viewcontroller for things like iOS extensions.  Progress prompts will not use this factory function though!

#### Android Initialization (In your main activity)

```csharp
UserDialogs.Init(this);
OR UserDialogs.Init(() => provide your own top level activity provider)
```

## Powered By:

* Android - Progress/Loading uses Redth's [AndHUD](https://github.com/Redth/AndHUD)
* iOS - Progress/Loading uses Nic Wise's [BTProgressHUD](https://github.com/nicwise/BTProgressHUD)
* iOS - Toasts powered by TTGSnackBar ported by @MarcBruins (https://github.com/MarcBruins/TTGSnackbar-Xamarin-iOS)
* iOS - Date/Time Picker powered by AIDatePicker ported by @MarcBruins (https://github.com/MarcBruins/AIDatePickerController-Xamarin-iOS)
* UWP - Coding4Fun Toolkit (http://coding4fun.codeplex.com)

# Frequently Asked Questions

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

5. Why don't you support the latest Android support libraries?

    * Because Xamarin breaks these frequently, one way or another - every... single... major release.  Be patient and wait!

6. Why don't you cancel a dialog when the app goes to the background (AND) why do I get an exception when I call for a dialog?

    * USER DIALOGS DOES NOT SOLVE WORLD PEACE! Guess what - most android API version and iOS don't call this.  This library is not a window state manager, if you call for a dialog, 
        it will try to present one.  If your app goes to the background and you call for a dialog, iOS & Android are tossing you the exception.  The library isn't here to save you from bad design choices.  
        Call us an anti-pattern if you want, we present dialogs!

7. Why does the library allow me to open multiple windows?

    * Similar to #6 - the library does not manage windows.  It opens dialogs - SURPRISE
    
8. I'd like to customize the dialogs

    * The library wasn't really designed or meant for this.  It was meant for using native dialogs.  That's it.  If you need something more customizable, this is not the library for it.

9. I'm getting a linker issue with System.Drawing.Color

    * This is due to a new BCL library being added in new versions of Xamarin.  You need to upgrade to the latest version of Xamarin (VS2019) to use anything beyond 7.0.4

## Contributors

* **[Martijn van Dijk](https://github.com/martijn00)** for tvOS and all of his contributions over the years!!
* **[Jelle Damen](https://twitter.com/JelleDamen)** for the wonderful icons
* **[Jong Heon Choi](https://github.com/JongHeonChoi)** for the Tizen implementation
* **[Federico Maccaroni](https://github.com/fedemkr)** for the macOS Implementation
