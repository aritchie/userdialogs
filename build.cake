#addin "Cake.Xamarin"
#addin "Cake.FileHelpers"
var target = Argument("target", Argument("t", "publish"));

Setup(() =>
{
    DeleteFiles("./output/*.*");
	if (!DirectoryExists("./output"))
		CreateDirectory("./output");
});

Task("build")
	.Does (() =>
{
	NuGetRestore("./src/lib.sln");
	MSBuild("./src/lib.sln", x => x
        .SetConfiguration("Release")
    );
});

Task("package")
	.IsDependentOn("build")
	.Does(() =>
{
	NuGetPack(new FilePath("./nuspec/Acr.UserDialogs.nuspec"), new NuGetPackSettings());
	MoveFiles("./*.nupkg", "./output");
});


Task("publish")
    .IsDependentOn("package")
    .Does(() =>
{
/*
    NuGetPush("./output/*.nupkg", new NuGetPushSettings
    {
        Verbosity = NuGetVerbosity.Detailed
    });
*/
    CopyFiles("./ouput/*.nupkg", "c:\\users\\allan.ritchie\\dropbox\\nuget");
});

RunTarget(target)