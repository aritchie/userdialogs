@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
nuget pack Acr.MvvmCross.Plugins.UserDialogs.nuspec
pause