ACR UserDialogs

# Support Platforms
8.x - .NET 6 targets only
    * .NET for iOS
    * .NET for MacCatalyst
    * .NET for Android
    * .NET 6

7.x
    * Xamarin Android (Major Target 12)
    * Xamarin iOS
    * Xamarin MacOS
    * Xamarin tvOS
    * Universal Windows Platform (UWP)
    * NET Standard 2.0

## Setup

To use, simply reference the nuget package in each of your platform projects.

#### iOS and Windows

Nothing is necessary any longer as of v4.x.  There is an Init function for iOS but it is OPTIONAL and only required if you want/need to control
the top level viewcontroller for things like iOS extensions.  Progress prompts will not use this factory function though!

#### Android Initialization (In your main activity OnCreate function)

UserDialogs.Init(this);
OR UserDialogs.Init(() => provide your own top level activity provider)
