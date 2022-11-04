using System.IO;
using System.Text.RegularExpressions;

namespace Pars
{
    internal class Program
    {
        const string resultFile = "ololog.txt";
        const string regexMask = @"threw an exception during launch";
        const string fileRegex = @".*\.log";
        static void Main(string[] args)
        {
            if (args.Length == 0) return;

            if (!File.Exists(resultFile)) File.Create(resultFile);
            File.WriteAllText(resultFile, "");

            List<string> files = new List<string>();

            if (File.Exists(args[0]))
            {
                files.Add(args[0]);
            }
            if (Directory.Exists(args[0]))
            {
                files.AddRange(FileNamesFromDirectory(args[0]));
            }
            foreach (string file in files)
            {
                ProcessSingleFile(file);
            }
        }
        public static List<string> FileNamesFromDirectory(string dir)
        {
            return Directory
                        .EnumerateFiles(dir)
                        .Where(x => new Regex(fileRegex)//коряво(лишний энтер) но красиво
                        .IsMatch(x))
                        .ToList();
        }

        public static void ProcessSingleFile(string fileName)
        {
            using (StreamWriter sw = File.CreateText(resultFile))
            {
                foreach (string processedLine in File
                .ReadAllLines(fileName)
                .Select((line, i) => (String//коряво(лишний энтер) но красиво
                .Format("[Файл {0}][Строка {1}] ", fileName, i), line))
                .Where(prefixAndLine => new Regex(regexMask)//коряво(лишний энтер) но красиво
                .IsMatch(prefixAndLine.line))
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