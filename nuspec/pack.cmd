@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
nuget pack Acr.UserDialogs.Android.AppCompat.nuspec
nuget pack Acr.MvvmCross.Plugins.UserDialogs.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.AppCompat.nuspec
nuget pack Acr.XamForms.UserDialogs.nuspec
pause