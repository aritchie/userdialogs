@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
rem nuget pack Acr.UserDialogs.WindowsForms.nuspec
pause