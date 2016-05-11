#addin "Cake.Xamarin"
#addin "Cake.FileHelpers"

var target = Argument("target", Argument("t", "nuget"));

Setup(() => {
    DeleteFiles("./output/*.*");

	if (!DirectoryExists("./output"))
		CreateDirectory("./output");
});

Task("build")
	.Does (() =>
{
	NuGetRestore("./src/lib.sln");
	DotNetBuild("./src/lib.sln", c => c
        //.SetVerbosity(Verbosity.Minimal)
        .SetConfiguration("Release")
        .WithTarget("Any CPU")
        //.WithProperty("TreatWarningsAsErrors","true")
    );
});

Task("nuget")
	.IsDependentOn("build")
	.Does(() =>
{
	// NuGet on mac trims out the first ./ so adding it twice works around
	var basePath = IsRunningOnUnix () ? (System.IO.Directory.GetCurrentDirectory().ToString() + @"/.") : "./";

	NuGetPack("./nuspec/Acr.UserDialogs.nuspec", new NuGetPackSettings
    {
		BasePath = basePath,
		Verbosity = NuGetVerbosity.Detailed
	});
	NuGetPack("./nuspec/Acr.UserDialogs.WindowsForms.nuspec", new NuGetPackSettings
    {
		BasePath = basePath,
		Verbosity = NuGetVerbosity.Detailed
	});
	MoveFiles("./nuspec/*.nupkg", "./output");
    CopyFiles("./ouput/*.nupkg", "c:\\users\\allan.ritchie\\dropbox\\nuget");
});

Task("publish")
    .IsDependentOn("nuget")
    .Does(() =>
{
    NuGetPush("./output/*.nupkg", new NuGetPushSettings
    {
        Verbosity = NuGetVerbosity.Detailed
    });
});

RunTarget(target)