using System.IO;
using System.Text.RegularExpressions;

namespace Pars
{
    internal class Program
    {
        static string regexMask = "threw an exception during launch";
        static string resultFile = "ololog.txt";
        static string fileRegex = ".*\\.log";
        static void Main(string[] args)
        {
            if (args.Length == 0) return;


            if (args.Length > 1) regexMask = args[1];//первое, что перезаписываем - фильтр строк, ключевая переменная в программе
            if (args.Length > 2) resultFile = args[2];//второе, что перезаписываем - при необходимости использовать другой выходной файл
            if (args.Length > 3) fileRegex = args[3];//последнее, что перезаписываем - если вдруг хотим работать не с .log файлами







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
            var result = File
                .ReadAllLines(fileName)
                .Select((line, i) => (String.Format("[Файл {0}][Строка {1}] ", fileName, i), line))
                .Where(prefixAndLine => regex.IsMatch(prefixAndLine.line))
                .Select(prefixAndLine => prefixAndLine.Item1 + prefixAndLine.line)
                .ToArray();
            foreach (string processedLine in result) Console.WriteLine(processedLine);
            File.AppendAllLines(resultFile, result);
        }
    }
}