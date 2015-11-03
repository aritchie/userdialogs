@echo off
del *.nupkg
rem stupid wp8 doesn't like 64bit and 32bit doesn't like c#6... my machine?
rem %WinDir%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe ../src/Acr.UserDialogs.sln /p:Platform="Any CPU" /property:Configuration=Release /t:Clean,Build
nuget pack Acr.UserDialogs.nuspec
nuget pack Acr.UserDialogs.Android.AppCompat.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.nuspec
rem nuget pack Acr.MvvmCross.Plugins.UserDialogs.AppCompat.nuspec
rem nuget pack Acr.XamForms.UserDialogs.nuspec
pause