@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.nuspec
pause