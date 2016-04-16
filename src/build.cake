#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers

var TARGET = Argument("target", Argument("t", "nuget"));

Setup(() => {
    DeleteFiles("./output/*.*");

	if (!DirectoryExists("./output"))
		CreateDirectory("./output");
});

Task("libs")
	.Does (() =>
{
	NuGetRestore("./Acr.UserDialogs.sln");
	DotNetBuild("./Acr.UserDialogs.sln", c => c.Configuration = "Release");
});

Task("nuget")
	.IsDependentOn("libs")
	.Does(() =>
{
	// NuGet on mac trims out the first ./ so adding it twice works around
	var basePath = IsRunningOnUnix () ? (System.IO.Directory.GetCurrentDirectory().ToString() + @"/.") : "./";

	NuGetPack("./nuspec/Acr.UserDialogs.nuspec", new NuGetPackSettings {
		BasePath = basePath,
		Verbosity = NuGetVerbosity.Detailed
	});
	MoveFiles("./nuspec/*.nupkg", "./output");
});

Task("publish")
    .IsDependentOn("nuget")
    .Does(() =>
{

    // publish to nuget.org
    NuGetPush("./output/*.nupkg");

    // local nuget packages - overwrite
    CopyFiles("./ouput/*.nupkg", "c:\\users\\allan.ritchie\\dropbox\\nuget");
});

RunTarget(target)