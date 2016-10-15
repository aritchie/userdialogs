@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
nuget pack Plugin.UserDialogs.nuspec
rem nuget pack Acr.UserDialogs.WindowsForms.nuspec
pause