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

            List<string> files = new List<string>();
            if (File.Exists(args[0]))
            {
                files.Add(args[0]);
            }
            if (Directory.Exists(args[0]))
            {
                files.AddRange(FileNamesFromDirectory(args[0]));
            }
            from file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.log", SearchOption.AllDirectories)
            from line in File.ReadLines(file)
            where new Regex(@"threw an exception during launch").IsMatch(line)
            //foreach (string line in File
            //    .ReadAllLines("log.log")
            //    .Select((x, i) => (String
            //    .Format("[Строка {0}] {1}", i, x)))
            //    .Where(x => new Regex(@"threw an exception during launch")
            //    .IsMatch(x))
            //    .ToArray())
            //    Console.WriteLine(line);
            //foreach (
            //    var match in from file in Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.log", SearchOption.AllDirectories)
            //                 from line in File.ReadLines(file)
            //                 select (x, i)
            //                where new Regex(@"threw an exception during launch").IsMatch(line)
            //    select new
            //    {
            //        File = file,
            //        Line = line,
            //        index = index
            //    }) Console.WriteLine(String.Format("[file]", match.File, match.Line));

        }
        public static List<string> FileNamesFromDirectory(string dir)
        {
            return Directory
                        .EnumerateFiles(dir)
                        .Where(x => new Regex(fileRegex)
                        .IsMatch(x))
                        .ToList();
        }

        public void ProcessSingleFile()
        {

        }
    }
}