using System;
using System.Threading.Tasks;
using System.IO;
using PowerBeep;
using PowerLog;

namespace PowerBeep
{
    public static class Compiler
    {
        public static async Task Compile(string ScriptContent, string OutputPath)
        {
            if (!LogSession.Initialized) LogSession.Initialize();
            Interpreter.ExecuteProgram = false;

            char[] InvalidChars = Path.GetInvalidPathChars();
            for (int i = 0; i < InvalidChars.Length; i++) {
                if (OutputPath.Contains(InvalidChars[i])) {
                    Log.Error("Invalid output path!");
                    return;
                }
            }


            if (!File.GetAttributes(OutputPath).HasFlag(FileAttributes.Directory)) {
                Log.Error("Output path is not a directory.");
                return;
            }

            bool CompilationResult = await Interpreter.Interpret(ScriptContent);
            if(CompilationResult != true) {
                Log.Error("Building failed.");
                await Task.Delay(5000);
                return;
            }


            if (!Directory.Exists(OutputPath)) {
                Directory.CreateDirectory(OutputPath);
            }


            try {
                File.WriteAllText(Path.Combine(OutputPath, "Resources.bin"), Obfuscator.ToHexadecimal(ScriptContent));
            }

            catch (DirectoryNotFoundException) {
                Log.Error("Output path directory doesn't exist.");
                return;
            }

            catch (UnauthorizedAccessException) {
                Log.Error("Cannot access output location.");
                return;
            }

            catch (PathTooLongException) {
                Log.Error("Output path is too long.");
                return;
            }

            Log.Info($"Script file compiled at '{OutputPath}' as a hexadecimal 'bin' file.");
        }



        public static void GenerateData(string ApplicationName, string OutputPath)
        {
            if (!LogSession.Initialized) LogSession.Initialize();

            char[] InvalidChars = Path.GetInvalidPathChars();
            for (int i = 0; i < InvalidChars.Length; i++) {
                if (OutputPath.Contains(InvalidChars[i])) {
                    Log.Error("Invalid output path!");
                    return;
                }
            }


            if (!File.GetAttributes(OutputPath).HasFlag(FileAttributes.Directory)) {
                Log.Error("Output path is not a directory.");
                return;
            }

            try {
                File.WriteAllText(Path.Combine(OutputPath, "ApplicationData.bin"), Obfuscator.ToHexadecimal(ApplicationName));
            }

            catch (DirectoryNotFoundException) {
                Log.Error("Output path directory doesn't exist.");
                return;
            }

            catch (UnauthorizedAccessException) {
                Log.Error("Cannot access output location.");
                return;
            }

            catch (PathTooLongException) {
                Log.Error("Output path is too long.");
                return;
            }

            Log.Info($"ApplicationData file generated at '{OutputPath}' as a hexadecimal 'bin' file.");
        }
    }
}
