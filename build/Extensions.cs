using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;


namespace ShinyBuild
{
    public static class Extensions
    {
        public static IProcess Execute(this ICakeContext context, FilePath exe, string args) =>
            context.ProcessRunner.Start(
                exe,
                new ProcessSettings
                {
                    Arguments = ProcessArgumentBuilder.FromString(args)
                }
            );

        public static void LogWarning(this ICakeContext context, string message)
            => context.Log.Write(Verbosity.Normal, LogLevel.Warning, message);
    }
}
