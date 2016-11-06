# Android Specifics

AndroidStyleId
Init
AppCompat vs Standard
Android Prompt - MaxLength and Numeric Input types

## Init
You should pick the mechanism that best fits your tech stack.  

UserDialogs.Init(this);
    OR UserDialogs.Init(() => provide your own top level activity provider)
    OR MvvmCross - UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity)
    OR Xamarin.Forms - UserDialogs.Init(() => (Activity)Forms.Context)