using System;
using Cake.Frosting;
using ShinyBuild.Tasks.Library;


namespace ShinyBuild.Tasks
{
    [TaskName("Default")]
    [IsDependentOn(typeof(CopyArtifactsTask))]
    [IsDependentOn(typeof(NugetDeployTask))]
    public sealed class DefaultTarget : FrostingTask<BuildContext> { }
}
