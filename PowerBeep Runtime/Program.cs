using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text;
using System.Linq;
using PowerLog;
using PowerBeep;
using System.IO;

namespace PowerBeep.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            LogSession.Initialize();
            Log.OnLog += OnLog;

            string ResourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(ResourcePath)) Directory.CreateDirectory(ResourcePath);
            StartExecution(ResourcePath);
            string WindowName = String.Empty;

            try {
                WindowName = Obfuscator.ToString(File.ReadAllText(Path.Combine(ResourcePath, "ApplicationData.bin")));
            }

            catch (FileNotFoundException) {
                Log.Warning("Data file missing.");
            }

            finally {
                Console.Title = ((!string.IsNullOrEmpty(WindowName)) ? WindowName : "PowerBeep Runtime");
            }


            while (true) ;
        }

        private static void OnLog(object Sender, LogArgs LogArgs)
        {
            ConsoleColor BackupColor = Console.ForegroundColor;
            ConsoleColor TargetColor;

            switch (LogArgs.LogLevel)
            {
                case LogType.Trace:
                    TargetColor = ConsoleColor.Gray;
                    break;

                case LogType.Debug:
                    TargetColor = ConsoleColor.DarkGray;
                    break;

                case LogType.Info:
                    TargetColor = ConsoleColor.White;
                    break;

                case LogType.Warning:
                    TargetColor = ConsoleColor.Yellow;
                    break;

                case LogType.Error:
                    TargetColor = ConsoleColor.Red;
                    break;

                case LogType.Network:
                    TargetColor = ConsoleColor.Blue;
                    break;

                case LogType.Fatal:
                    TargetColor = ConsoleColor.DarkRed;
                    break;

                case LogType.Null:
                    TargetColor = BackupColor;
                    break;


                default:
                    TargetColor = BackupColor;
                    break;
            }

            Console.ForegroundColor = TargetColor;
            Console.WriteLine($"{((LogArgs.LoggingMode.HasFlag(LogMode.Timestamp)) ? $"[{DateTime.Now.ToString("HH:mm:ss")}] " : String.Empty)}" +
                $"{((LogArgs.LogLevel != LogType.Null) ? $"{LogArgs.LogLevel.ToString()}: " : String.Empty)}" +
                $"{LogArgs.LogMessage}");

            Console.ForegroundColor = BackupColor;
        }

        private static async void StartExecution(string ResourcePath)
        {
            string Code = string.Empty;
            Interpreter.SurpressFeedback = true;
            Interpreter.SurpressLogs = true;

            try {
                Code = Obfuscator.ToString(File.ReadAllText(Path.Combine(ResourcePath, "Resources.bin")));
            }

            catch (FileNotFoundException) {
                Log.Fatal("Resources file missing. Cannot execute the program.");
                await Task.Delay(1950);
                Environment.Exit(1);
            }


            bool RuntimeResult = await Interpreter.Interpret(Code);
            if (RuntimeResult == false) {
                Log.Fatal("Couldn't recompile source.");
            }

            await Task.Delay(850);
            Log.Info($"Program exited with exit code {Convert.ToInt32(!RuntimeResult)}.");

            Environment.Exit(Convert.ToInt32(!RuntimeResult));
        }
    }
}
