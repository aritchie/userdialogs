@echo off
copy *.nupkg C:\users\allan.ritchie\dropbox\nuget\ /y
nuget push *.nupkg
pause