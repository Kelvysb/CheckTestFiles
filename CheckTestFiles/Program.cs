using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using CheckTestFiles.Model;

namespace CheckTestFiles
{
    class Program
    {

        private static string workDirectory = "";
        private static string currentDirectory = "";
        static void Main(string[] args)
        {

            try
            {

                initializeDirs();
                List<string> argsList = args.ToList();                
                Dictionary<String, String> inputArgs = new Dictionary<string, string>();

                foreach (var arg in args)
                {
                    if (arg.StartsWith("-"))
                    {
                        if (arg.Equals("--check", StringComparison.InvariantCultureIgnoreCase) ||
                            arg.Equals("-c", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("CHECK", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                inputArgs.Add("CHECK", currentDirectory);
                            }
                        }
                        else if (arg.Equals("--generate", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-g", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 &&
                                !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("GENERATE", argsList[argsList.IndexOf(arg) + 1]);                               
                            }
                            else
                            {
                                inputArgs.Add("GENERATE", currentDirectory);
                            }

                            if (argsList.Count > argsList.IndexOf(arg) + 2 &&
                                !argsList[argsList.IndexOf(arg) + 2].StartsWith("-"))
                            {
                                inputArgs.Add("GENERATEPATERN", argsList[argsList.IndexOf(arg) + 2]);
                            }
       
                        }              
                        else if (arg.Equals("--filter", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-f", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("FILTER", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --filter.");
                            }
                        }
                        else if (arg.Equals("--test", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-t", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("TEST", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --spec.");
                            }
                        }
                        else if (arg.Equals("--exclude-dir", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-ed", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("EXCLUDEDIR", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --exclude-dir.");
                            }
                        }
                        else if (arg.Equals("--include-dir", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-id", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("INCLUDEDIR", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --include-dir.");
                            }
                        }
                        else if (arg.Equals("--exclude-files", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-ef", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("EXCLUDEFILES", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --exclude-files.");
                            }
                        }
                        else if (arg.Equals("--include-files", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-if", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("INCLUDEFILES", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("Invalid filter on --include-files.");
                            }
                        }
                        else if (arg.Equals("--ok-only", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-ok", StringComparison.InvariantCultureIgnoreCase))
                        {
                            inputArgs.Add("OKONLY", "");
                        }
                        else if (arg.Equals("--not-found-only", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-nf", StringComparison.InvariantCultureIgnoreCase))
                        {
                            inputArgs.Add("NOTFOUNDONLY", "");
                        }
                        else if (arg.Equals("-csv", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("--csv", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("CSV", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                inputArgs.Add("CSV", "output.csv");
                            }
                        }
                        else if (arg.Equals("--help", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-h", StringComparison.InvariantCultureIgnoreCase))
                        {
                            inputArgs.Add("HELP", "");
                        }
                        else if (arg.Equals("--version", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-v", StringComparison.InvariantCultureIgnoreCase))
                        {
                            inputArgs.Add("VERSION", "");
                        }
                        else if (arg.Equals("--env", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("-env", StringComparison.InvariantCultureIgnoreCase))
                        {
                            inputArgs.Add("ENV", "");
                        }
                        else if (arg.Equals("-cf", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("--config", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("CONFIG", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("The configuration must have an name.");
                            }
                        }
                        else if (arg.Equals("-sc", StringComparison.InvariantCultureIgnoreCase) ||
                                 arg.Equals("--save-config", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (argsList.Count > argsList.IndexOf(arg) + 1 && !argsList[argsList.IndexOf(arg) + 1].StartsWith("-"))
                            {
                                inputArgs.Add("SAVECONFIG", argsList[argsList.IndexOf(arg) + 1]);
                            }
                            else
                            {
                                throw new Exception("The configuration must have an name.");
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid command use -h or --help to a list of valid commands.");
                        }
                    }
                }

                if (inputArgs.ContainsKey("CONFIG"))
                {
                    inputArgs = loadConfiguration(inputArgs.GetValueOrDefault("CONFIG"));
                }

                if (inputArgs.Count > 0)
                {

                    if (!inputArgs.ContainsKey("FILTER"))
                    {
                        inputArgs.Add("FILTER", "\\.ts*$|\\.js*$");
                    }

                    if (!inputArgs.ContainsKey("TEST"))
                    {
                        inputArgs.Add("TEST", "\\.spec\\.*");
                    }

                    if (!inputArgs.ContainsKey("EXCLUDEDIR"))
                    {
                        inputArgs.Add("EXCLUDEDIR", "node_modules|^\\.");
                    }

                    if (!inputArgs.ContainsKey("INCLUDEDIR"))
                    {
                        inputArgs.Add("INCLUDEDIR", "");
                    }

                    if (!inputArgs.ContainsKey("EXCLUDEFILES"))
                    {
                        inputArgs.Add("EXCLUDEFILES", "\\.conf\\.*");
                    }

                    if (!inputArgs.ContainsKey("INCLUDEFILES"))
                    {
                        inputArgs.Add("INCLUDEFILES", "");
                    }

                    if (inputArgs.ContainsKey("OKONLY") && inputArgs.ContainsKey("NOTFOUNDONLY"))
                    {
                        inputArgs.Remove("OKONLY");
                        inputArgs.Remove("NOTFOUNDONLY");
                    }


                    if (inputArgs.ContainsKey("SAVECONFIG"))
                    {
                        saveConfig(inputArgs, inputArgs.GetValueOrDefault("SAVECONFIG"));
                    }
                    else if (inputArgs.ContainsKey("CHECK"))
                    {
                        executeCheck(inputArgs);
                    }
                    //else if (inputArgs.ContainsKey("GENERATE"))
                    //{
                    //   generateTestFiles(inputArgs);
                    //}
                    else if (inputArgs.ContainsKey("VERSION"))
                    {
                        Console.WriteLine("CheckTestFiles version: " + Assembly.GetEntryAssembly().GetName().Version);
                    }
                    else if (inputArgs.ContainsKey("ENV"))
                    {
                        Console.WriteLine("Current working directory: " + currentDirectory);
                    }
                    else if (inputArgs.ContainsKey("HELP"))
                    {
                        help();
                    }
                    else
                    {
                        Console.WriteLine("Invalid command, use -h or --help to see a list of valid commands.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command, use -h or --help to see a list of valid commands.");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //private static void generateTestFiles(Dictionary<string, string> inputArgs)
        //{
        //}

        private static void executeCheck(Dictionary<string, string> inputArgs)
        {

            List<Matchs> result;
            StreamWriter file;
            string auxLine;


            result = CheckTests.checkDirectory(inputArgs.GetValueOrDefault("CHECK"),
                               inputArgs.GetValueOrDefault("CHECK"),
                               inputArgs.GetValueOrDefault("FILTER"),
                               inputArgs.GetValueOrDefault("TEST"),
                               inputArgs.GetValueOrDefault("EXCLUDEFILES"),
                               inputArgs.GetValueOrDefault("INCLUDEFILES"),
                               inputArgs.GetValueOrDefault("EXCLUDEDIR"),
                               inputArgs.GetValueOrDefault("INCLUDEDIR"));

            Console.WriteLine("Results:");

            foreach (Matchs res in result)
            {
                if (res.Found)
                {
                    if (inputArgs.GetValueOrDefault("NOTFOUNDONLY") == null)
                    {
                        auxLine = "OK: " + res.CodeFile + " test: " + res.TestFile;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(auxLine);
                        Console.ResetColor();
                    }
                }
                else
                {
                    if (inputArgs.GetValueOrDefault("OKONLY") == null)
                    {
                        auxLine = "NOT FOUND: " + res.CodeFile + " test: " + res.TestFile;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(auxLine);
                        Console.ResetColor();
                    }
                }
            }
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Total code files: " + result.Count);
            Console.WriteLine("Code files with test: " + result.FindAll(matchs => matchs.Found).Count);
            Console.WriteLine("Code files without test: " + result.FindAll(matchs => !matchs.Found).Count);
            Console.WriteLine("");

            if (inputArgs.ContainsKey("CSV"))
            {
                file = new StreamWriter(inputArgs.GetValueOrDefault("CSV"), false);
                foreach (Matchs res in result)
                {
                    auxLine = (res.Found ? "OK;" : "NOT FOUND;") + res.CodeFile + ";" + res.TestFile;
                    file.WriteLine(auxLine);
                }
                file.Close();
                file.Dispose();
                Console.WriteLine("CSV file saved: " + inputArgs.GetValueOrDefault("CSV"));
            }
        }

        private static Dictionary<string, string> loadConfiguration(string name)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Config config;

            if (File.Exists(Path.Combine(workDirectory, name + ".json")))
            {
                config = Config.load(Path.Combine(workDirectory, name + ".json"));
                foreach (ConfigKey configConfigKey in config.ConfigKeys)
                {
                    result.Add(configConfigKey.key, configConfigKey.value);
                }
            }
            else
            {
                throw new Exception("Configuration " + name + "not found.");
            }

            return result;
        }

        private static void saveConfig(Dictionary<string, string> inputArgs, string name)
        {
            Config config = new Config() {Name = name};

            foreach (KeyValuePair<string, string> keyValuePair in inputArgs)
            {
                config.ConfigKeys.Add(new ConfigKey()
                {
                    key = keyValuePair.Key,
                    value = keyValuePair.Value
                });
                config.ConfigKeys.RemoveAll(key => key.key.Equals("SAVECONFIG"));
            }
            config.save(Path.Combine(workDirectory, name + ".json"));            
        }

        private static void help()
        {
            Console.WriteLine("Check on folders:");
            Console.WriteLine(" -c or --check [optional directory path]");
            //Console.WriteLine("Generate missing test files:");
            //Console.WriteLine(" -g or --generate [optional directory path] [pattern file name (if omitted will use default)]");
            Console.WriteLine("Use saved configuration:");
            Console.WriteLine(" -cf or --config <configuration name>");
            Console.WriteLine("");

            Console.WriteLine("Optional args:");
            Console.WriteLine(" Filter code file types:");
            Console.WriteLine("     -f or --filter [files filter]");
            Console.WriteLine("     Default: \\.ts*$|\\.js*$");
            Console.WriteLine("");

            Console.WriteLine(" Test files name filter:");
            Console.WriteLine("     -t or --test [test filter]");
            Console.WriteLine("     Default: \\.spec\\.*|\\.test\\.*");
            Console.WriteLine("");

            Console.WriteLine(" Exclude directories:");
            Console.WriteLine("     -ed or --exclude-dir [exclude filter]");
            Console.WriteLine("     Default: 'node_modules|^\\.'");
            Console.WriteLine("");

            Console.WriteLine(" Include directories:");
            Console.WriteLine("     -id or --include-dir [include filter]");
            Console.WriteLine("     Default: '[.]*");
            Console.WriteLine("");

            Console.WriteLine(" Exclude files:");
            Console.WriteLine("     -ed or --exclude-files [exclude filter]");
            Console.WriteLine("     Default: '\\.conf\\.*'");
            Console.WriteLine("");

            Console.WriteLine(" Include files:");
            Console.WriteLine("     -if or --include-files [include filter]");
            Console.WriteLine("     Default: '[.]*");
            Console.WriteLine("");

            Console.WriteLine(" Show Only Ok:");
            Console.WriteLine("     -ok or --ok-only");
            Console.WriteLine("");

            Console.WriteLine(" Show Only Not found:");
            Console.WriteLine("     -nf or --not-found-only");
            Console.WriteLine("");

            Console.WriteLine(" Generate csv file:");
            Console.WriteLine("     -csv or --csv [output file name: default 'output.csv']");
            Console.WriteLine("     Default: ''");
            Console.WriteLine("");
           
            Console.WriteLine(" Save configuration:");
            Console.WriteLine("     -sc or --save-config <configuration name>");
            Console.WriteLine("");

            Console.WriteLine(" Get current dir:");
            Console.WriteLine("     -env or --env");
            Console.WriteLine("");
            Console.WriteLine(" Program Version:");
            Console.WriteLine("     -v or --version");
        }

        private static void initializeDirs()
        {
            string auxPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            auxPath = Path.Combine(auxPath, "checktestfiles");
            if (!Directory.Exists(auxPath))
            {
                Directory.CreateDirectory(auxPath);
            }            
            currentDirectory = Environment.CurrentDirectory;
            workDirectory = auxPath;
        }
    }
}
