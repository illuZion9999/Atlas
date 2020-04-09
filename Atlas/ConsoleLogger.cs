using System.Diagnostics;
using System.Drawing;
using Atlas.Core;
using Console = Colorful.Console;

namespace Atlas
{
    public class ConsoleLogger : ILogger
    {
        static string GetModule()
        {
            var trace = new StackTrace();
            var frame = trace.GetFrame(3);
            var method = frame.GetMethod();
            
            // ReSharper disable once PossibleNullReferenceException
            #if DEBUG
            return $"{method.DeclaringType.Name}::{method.Name}";
            #else
            return $"{method.DeclaringType.Name}";
            #endif
        }
        
        static void Log(string message, Color color)
        {
            Console.WriteLine($"[{GetModule()}] {message}", color);
        }
        
        public void Debug(string message) => Log(message, Color.LightSlateGray);
        public void Info(string message) => Log(message, Color.White);
        public void Warning(string message) => Log(message, Color.DarkOrange);
        public void Success(string message) => Log(message, Color.Lime);
        public void Error(string message) => Log(message, Color.Red);
    }
}