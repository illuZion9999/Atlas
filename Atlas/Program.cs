using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Atlas.Core;
using Console = Colorful.Console;

namespace Atlas
{
    static class Program 
    {
        static void Main(string[] args)
        {
            Console.Title = "Atlas by xsilent007";
            Console.WriteLine("Welcome.", Color.Aqua);
            Console.WriteLine($"Version: {Version.Get()}\n", Color.Chocolate);
            
            //Get a file from args
            if (args.Length < 1)
                ErrorThenExit($"Usage: Atlas.exe <path to file|project>");

            var file = args[0];
            if (!File.Exists(file))
                ErrorThenExit($"'{file}' doesn't exist or it is not a file");

            #if !DEBUG
            try
            {
            #endif
                //Load or make a new project
                var ext = Path.GetExtension(file);
                var proj = string.Equals(ext, ".json", StringComparison.OrdinalIgnoreCase)
                    ? Project.FromFile(file)
                    : new Project
                    {
                        Assemblies = new List<string>
                        {
                            file
                        },
                        BaseDirectory = GetBaseDirectory(file),
                        OutputDirectory = Path.Combine(GetBaseDirectory(file), "Protected")
                    };

                //Execute project
                proj.Run(new ConsoleLogger());
            #if !DEBUG    
            }
            catch (Exception ex)
            {
                ErrorThenExit("An exception occured:\n" + ex, Color.Red);
            }
            finally
            {
            #endif
                Console.ReadLine();
            #if !DEBUG
            }
            #endif
        }

        static void ErrorThenExit(string message)
        {
            ErrorThenExit(message, Color.Orange);
        }

        static void ErrorThenExit(string message, Color color)
        {
            Console.WriteLine(message, color);
            Console.ReadLine();
            Environment.Exit(-1);   
        }

        static string GetBaseDirectory(string file) => Path.GetDirectoryName(file);
    }
}