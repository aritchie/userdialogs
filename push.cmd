@echo off
copy *.nupkg %HOMEPATH%\dropbox\nuget\ /y
nuget push .\src\Acr.UserDialogs\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
del .\src\Acr.UserDialogs\bin\Release\*.nupkg
pause