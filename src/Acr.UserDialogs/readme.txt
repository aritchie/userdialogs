ACR UserDialogs

# Support Platforms
* NET Standard 2.0/NET6 - iOS, MacCatalyst, & Android

## Setup

To use, simply reference the nuget package in each of your platform projects. If you are getting issues with System.Drawing.Color, please make sure you are using the latest version of Xamarin

#### iOS and Windows

Nothing is necessary any longer as of v4.x.  There is an Init function for iOS but it is OPTIONAL and only required if you want/need to control
the top level viewcontroller for things like iOS extensions.  Progress prompts will not use this factory function though!

#### Android Initialization (In your main activity OnCreate function)

UserDialogs.Init(this);
OR UserDialogs.Init(() => provide your own top level activity provider)
