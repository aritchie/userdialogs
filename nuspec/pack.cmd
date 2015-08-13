@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
nuget pack Acr.UserDialogs.Android.AppCompat.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.AppCompat.nuspec
pause