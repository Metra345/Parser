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
            if (args.Length == 0) return;//без аргументов выходим из программы
            if (args.Length > 1) regexMask = args[1];//первое, что перезаписываем - фильтр строк, ключевая переменная в программе
            if (args.Length > 2) resultFile = args[2];//второе, что перезаписываем - при необходимости использовать другой выходной файл
            if (args.Length > 3) fileRegex = args[3];//последнее, что перезаписываем - если вдруг хотим работать не с .log файлами

            if (!File.Exists(resultFile)) File.Create(resultFile);

            File.WriteAllText(resultFile, "");

            List<string> files = new List<string>();

            if (File.Exists(args[0]) && new Regex(fileRegex).IsMatch(args[0])) files.Add(args[0]);

            if (Directory.Exists(args[0])) files.AddRange(FileNamesFromDirectory(args[0]));

            //if (files.Count == 0) return;

            foreach (string file in files) Process2_0(file, new Regex(regexMask));
        }
        public static List<string> FileNamesFromDirectory(string dir)
        {
            return Directory
                        .EnumerateFiles(dir)
                        .Where(x => new Regex(fileRegex).IsMatch(x))
                        .ToList();
        }
        public static void Process2_0(string fileName, Regex regex)
        {
            string[] lines = File.ReadAllLines(fileName);
            List<string> result = new List<string>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (regex.IsMatch(lines[i]))
                {
                    result.Add(String.Format("[Файл {0}][Строка {1}] ", fileName, i) + lines[i]);
                }
                Console.Write("\rФайл {0}, прогресс: {1}%", fileName, Math.Floor((i+1) * 100.0 / lines.Length));
                if (i % 10 == 0)Thread.Sleep(1);//без задержки незаметно
            }
            Console.WriteLine();
            File.AppendAllLines(resultFile, result);
        }
    }
}