@echo off
del *.nupkg
nuget pack *.nuspec
pause