using System;
using Cake.Frosting;

namespace ShinyBuild
{
    class Program
    {
        //https://cakebuild.net/docs/running-builds/runners/cake-frosting#bootstrapping-for-cake-frosting
        public static int Main(string[] args)
            => new CakeHost()
                .UseContext<BuildContext>()
                .Run(args);
    }
}
