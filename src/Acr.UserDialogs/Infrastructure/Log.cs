using System;


namespace Acr.UserDialogs.Infrastructure
{

    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3
    }


    public static class Log
    {
        static Log()
        {
            Out = (cat, msg, level) => System.Diagnostics.Debug.WriteLine($"[{level}][{cat}] {msg}");
        }


        public static LogLevel MinLogLevel { get; set; } = LogLevel.Info;
        public static Action<string, string, LogLevel> Out { get; set; }


        public static void Debug(string category, string msg) => Write(category, msg, LogLevel.Debug);
        public static void Info(string category, string msg) => Write(category, msg, LogLevel.Info);
        public static void Warn(string category, string msg) => Write(category, msg, LogLevel.Warn);
        public static void Error(string category, string msg) => Write(category, msg, LogLevel.Error);


        public static void Write(string category, string msg, LogLevel level = LogLevel.Debug)
        {
            if (level >= MinLogLevel)
                Out?.Invoke(category, msg, level);
        }
    }
}
