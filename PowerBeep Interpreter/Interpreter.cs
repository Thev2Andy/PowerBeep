using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using PowerLog;

namespace PowerBeep
{
    public static class Interpreter
    {
        // Execution settings..
        public static bool ExecuteProgram = true;

        // Surpression settings..
        public static bool SurpressExecutionCrashes = false;
        public static bool SurpressFeedback = false;
        public static bool SurpressLogs = false;
        public static bool SurpressExecutionLogs = false;

        // Private / Internal variables..
        public static bool IsInterpreting { get; internal set; }

        public static async Task<bool> Interpret(string Target)
        {
            if (!LogSession.Initialized) LogSession.Initialize(false);

            if (!IsInterpreting)
            {
                string[] Lines = Target.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                List<string> Keywords = new List<string>();
                List<int> Values = new List<int>();
                bool HasThrownError = false;

                bool CompilationStatus = true;
                IsInterpreting = true;

                Stopwatch TotalCompilationTime = new Stopwatch();
                TotalCompilationTime.Start();
                if (!SurpressLogs) Log.Info("Started compilation..");

                for (int i = 0; i < Lines.Length; i++)
                {
                    string Keyword = Lines[i].Split((" ").ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    int Value = 0;

                    // Check for compile errors..
                    switch (Keyword.ToUpper())
                    {
                        case "BEEP":
                            break;

                        case "WAIT":
                            break;


                        default:
                            if (!SurpressFeedback) Log.Error($"Unknown keyword '{Keyword}'.");
                            HasThrownError = true;
                            break;
                    }


                    try {
                        Value = Convert.ToInt32(Lines[i].Split((" ").ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1].Trim());
                    }

                    catch (FormatException) {
                        if (!SurpressFeedback) Log.Warning($"Invalid value '{Lines[i].Split((" ").ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1].Trim()}', defaulted to 1.");
                        Value = 1;
                    }

                    catch (IndexOutOfRangeException) {
                        if (!SurpressFeedback) Log.Warning("Line found with no value, defaulted to 1.");
                        Value = 1;
                    }



                    if (!HasThrownError) {
                        Keywords.Add(Keyword);
                        Values.Add(Value);
                    }
                }

                TotalCompilationTime.Stop();
                if(HasThrownError) {
                    if (!SurpressFeedback) {
                        Log.Error("Interpreting failed.");
                        Log.Null("", (LogMode.InvokeEvent | LogMode.Save));
                        Log.Error($"Compilation failed after {TotalCompilationTime.ElapsedMilliseconds} ms.");
                        Log.Null("", (LogMode.InvokeEvent | LogMode.Save));
                    }

                    CompilationStatus = false;
                }


                bool HasApplicationCrashed = false;
                if (!HasThrownError) {

                    if (!SurpressLogs) Log.Info($"Successfully compiled after {TotalCompilationTime.ElapsedMilliseconds} ms.");
                    if (!SurpressExecutionLogs && ExecuteProgram) Log.Info("Starting the program..");
                    if (!SurpressExecutionLogs || !SurpressLogs) Log.Null("", (LogMode.InvokeEvent | LogMode.Save));



                    // From this point on we execute the program.
                    if (ExecuteProgram)
                    {
                        await Task.Delay(1500);


                        if (!SurpressExecutionLogs) Log.Info("Program started.");
                        Stopwatch ExecutionTime = new Stopwatch();
                        ExecutionTime.Start();

                        for (int i = 0; i < Keywords.Count; i++)
                        {
                            if (Keywords.Count == Values.Count)
                            {
                                switch (Keywords[i].ToUpper())
                                {
                                    case "BEEP":
                                        for (int BeepCount = 0; BeepCount < Values[i]; BeepCount++) {
                                            if (Values[i] > 0) {
                                                Console.Beep();
                                            }
                                        }
                                        break;

                                    case "WAIT":
                                        if (Values[i] > 0) {
                                            await Task.Delay(Values[i]);
                                        }
                                        break;


                                    default:
                                        if (!SurpressExecutionCrashes) Log.Fatal($"Keyword '{Keywords[i]}' not defined in Execution.");
                                        HasApplicationCrashed = true;
                                        goto EndCodeInterpretingProcess;
                                }
                            }
                            
                            else {
                                if (!SurpressExecutionCrashes) Log.Fatal("Keywords and values don't match.");
                                HasApplicationCrashed = true;
                                goto EndCodeInterpretingProcess;
                            }
                        }

                        if (!SurpressExecutionLogs && ExecuteProgram) Log.Info($"Execution finished in {(ExecutionTime.ElapsedMilliseconds / 1000f)} second{(((ExecutionTime.ElapsedMilliseconds / 1000f) != 1f) ? "s" : "")}.");
                        ExecutionTime.Stop();
                    }
                }


                EndCodeInterpretingProcess:
                if(!HasThrownError) {
                    if (!SurpressLogs && ExecuteProgram) Log.Info($"Program exited with exit code {Convert.ToInt32(HasApplicationCrashed)}.");
                }

                HasApplicationCrashed = false;
                IsInterpreting = false;
                return CompilationStatus;
            }
            
            else {
                if (!SurpressLogs) Log.Error("The interpreter is currently busy..");
                return false;
            }
        }
    }
}
