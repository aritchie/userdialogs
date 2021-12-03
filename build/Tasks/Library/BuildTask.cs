using System;

using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.MSBuild;
using Cake.Frosting;


namespace ShinyBuild.Tasks.Library
{
    [TaskName("Build")]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        // needs to be windows build for UWP
        public override bool ShouldRun(BuildContext context)
        {
            if (!context.IsRunningOnWindows())
                return false;

            if (context.IsRunningInCI && context.BuildNumber == 0)
                throw new ArgumentException("BuildNumber argument is missing");

            return true;
        }


        public override void Run(BuildContext context)
        {
            try
            {
                context.CleanDirectories($"./src/**/obj/");
                context.CleanDirectories($"./src/**/bin/{context.MsBuildConfiguration}");
            }
            catch (Exception ex)
            {
                context.LogWarning("Failed to clear bin/obj");
            }

            try
            {
                context.MSBuild("../Build.slnf", x => x
                    //.WithRestore()
                    .WithTarget("Clean")
                    .WithTarget("Build")
                    .WithProperty("LibraryVersion", context.NugetVersion)
                    .WithProperty("CI", context.IsRunningInCI ? "true" : "")
                    .WithProperty("OS", context.OperatingSystemString)
                    .SetConfiguration(context.MsBuildConfiguration)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
