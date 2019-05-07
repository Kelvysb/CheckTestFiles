using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace CheckTestFiles
{
    class CheckTests
    {

        public static List<Matchs> checkDirectory(string p_base, string p_directory, string p_filter, string p_test, string p_excludeFiles, string p_includeFiles, string p_excludeDir, string p_includeDir)
        {
            List<Matchs> result = new List<Matchs>();
            List<string> files;
            List<string> codes;
            List<string> specs;
            string auxSpec;

            files = getAllFiles(p_directory, p_excludeFiles, p_includeFiles, p_excludeDir, p_includeDir);

            codes = files.FindAll(file => Regex.IsMatch(Path.GetFileName(file), p_filter));
            codes.RemoveAll(file => Regex.IsMatch(Path.GetFileName(file), p_test));
            specs = files.FindAll(file => Regex.IsMatch(Path.GetFileName(file), p_test));

            codes.ForEach(code =>
            {
                auxSpec = specs.Find(spec =>
                    Path.GetFileName(spec).StartsWith(Path.GetFileName(code).Split(".").FirstOrDefault()));
                if (auxSpec != null)
                {
                    result.Add(new Matchs(){Found = true, CodeFile = code.Replace(p_base, ""), TestFile = auxSpec.Replace(p_base, "") });
                }
                else
                {
                    result.Add(new Matchs() { Found = false, CodeFile = code.Replace(p_base, ""), TestFile = ""});
                }

            });

            return result;

        }

        private static List<string> getAllFiles(string p_directory, string p_excludeFiles, string p_includeFiles, string p_excludeDir, string p_includeDir)
        {
            try
            {
                List<string> result = new List<string>();
                List<string> directories = new List<string>();
                List<string> files = new List<string>();
                string[] auxDirectories;
                string[] auxFiles;

                auxDirectories = Directory.GetDirectories(p_directory);

                if (auxDirectories != null && auxDirectories.Length > 0)
                {
                    directories = auxDirectories.ToList();
                }

                if (!p_excludeDir.Equals(""))
                {
                    directories.RemoveAll(dir => Regex.IsMatch(dir, p_excludeDir));
                }

                if (!p_includeDir.Equals(""))
                {
                    directories.RemoveAll(dir => !Regex.IsMatch(dir, p_includeDir));
                }

                directories.ForEach(dir =>
                {
                    result.AddRange(getAllFiles(dir, p_excludeFiles, p_includeFiles, p_excludeDir, p_includeDir));
                });

                auxFiles = Directory.GetFiles(p_directory);

                if (auxFiles != null && auxFiles.Length > 0)
                {
                    files = auxFiles.ToList();
                }

                if (!p_excludeFiles.Equals(""))
                {
                    files.RemoveAll(file => Regex.IsMatch(Path.GetFileName(file), p_excludeFiles));
                }

                if (!p_includeFiles.Equals(""))
                {
                    files.RemoveAll(file => !Regex.IsMatch(Path.GetFileName(file), p_includeFiles));
                }

                result.AddRange(files);

                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

    }
}
