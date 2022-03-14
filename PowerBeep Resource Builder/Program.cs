using System;
using System.Threading.Tasks;
using PowerBeep;
using PowerLog;
using System.IO;

namespace PowerBeep.ResourceBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            LogImplementation.Initialize();
            Build();

            while (true) ;
        }

        public static async void Build()
        {
            Console.Write("Enter script path: ");
            string ScriptFileContent;

            try {
                ScriptFileContent = File.ReadAllText(Console.ReadLine());
            }

            catch (FileNotFoundException) {
                Log.Error("Script file doesn't exist.");
                return;
            }

            catch (DirectoryNotFoundException) {
                Log.Error("Script file doesn't exist.");
                return;
            }

            catch (UnauthorizedAccessException) {
                Log.Error("Cannot access script file.");
                return;
            }

            catch (PathTooLongException) {
                Log.Error("Script file path is too long.");
                return;
            }

            Console.Write("Enter application name: ");
            string ApplicationName = Console.ReadLine();
            Console.Write("Enter output path: ");
            string OutputPath = Console.ReadLine();

            Interpreter.SurpressLogs = false;
            Interpreter.SurpressExecutionCrashes = false;
            Interpreter.SurpressExecutionLogs = false;

            Compiler.GenerateData(ApplicationName, OutputPath);
            await Compiler.Compile(ScriptFileContent, OutputPath);

            await Task.Delay(4500);
            Environment.Exit(0);
        }
    }
}
