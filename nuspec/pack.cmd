@echo off
del *.nupkg
nuget pack Acr.UserDialogs.nuspec
nuget pack Acr.UserDialogs.WindowsForms.nuspec
pause