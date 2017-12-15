@echo off
copy *.nupkg %HOMEPATH%\dropbox\nuget\ /y
nuget push *.nupkg -Source https://www.nuget.org/api/v2/package
nuget push .\src\Acr.UserDialogs\bin\Release\*.nupkg -Source https://www.nuget.org/api/v2/package
pause