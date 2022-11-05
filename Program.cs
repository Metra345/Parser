using System.IO;
using System.Text.RegularExpressions;

namespace Pars
{
    internal class Program
    {
        static string resultFile = "ololog.txt";
        static string regexMask = @"threw an exception during launch";
        static string fileRegex = @".*\.log";
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            if (!File.Exists(resultFile)) File.Create(resultFile);

            File.WriteAllText(resultFile, "");

            List<string> files = new List<string>();

            if (File.Exists(args[0]) && new Regex(fileRegex).IsMatch(args[0])) files.Add(args[0]);

            if (Directory.Exists(args[0])) files.AddRange(FileNamesFromDirectory(args[0]));

            //if (files.Count == 0) return;

            foreach (string file in files) ProcessSingleFile(file, new Regex(regexMask));
        }
        public static List<string> FileNamesFromDirectory(string dir)
        {
            return Directory
                        .EnumerateFiles(dir)
                        .Where(x => new Regex(fileRegex).IsMatch(x))
                        .ToList();
        }

        public static void ProcessSingleFile(string fileName, Regex regex)
        {
            using (StreamWriter sw = File.CreateText(resultFile))
            {
                foreach (string processedLine in File
                .ReadAllLines(fileName)
                .Select((line, i) => (String.Format("[Файл {0}][Строка {1}] ", fileName, i), line))
                .Where(prefixAndLine => regex.IsMatch(prefixAndLine.line))
                .Select(prefixAndLine => prefixAndLine.Item1 + prefixAndLine.line)
                .ToArray())
                {
                    Console.WriteLine(processedLine);
                    sw.WriteLine(processedLine);
                }
            }
        }
    }
}