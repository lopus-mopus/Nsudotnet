using System;
using System.Collections.Generic;
using System.Text;

namespace Eliseev.Nsudotnet.LinesCounter
{
    class Program
    {
        private const int MinProgramParameters = 1;
        private const int IndexOfDirectoryParamater = 1;
        private const int IndexOfFilesMaskParameter = 0;
        private const string UsageMessage = "program file_mask [directory]";

        static void Main(string[] args)
        {
            string fileFormat;
            string directory;

            try
            {
                FetchParameters(args, out fileFormat, out directory);
                Console.WriteLine($"Searching files {fileFormat} in {directory} directory:");
                int lines = LinesCounter.CountLinesInFilesInDirectoryAndSubDirectoryes(directory, fileFormat);
                Console.WriteLine($"Total {lines} lines.");
            }
            catch (Exception e)
            {
                Console.WriteLine("There were errors:");
                Console.WriteLine(e.ToString());
            }
        }

        private static void FetchParameters(string[] args, out string fileFormat, out string directory)
        {
            if (args.Length < MinProgramParameters)
            {
                throw new ArgumentException($"Not enought arguments. Usage: {UsageMessage}");
            }

            fileFormat = args[IndexOfFilesMaskParameter];
            directory = args.Length > IndexOfDirectoryParamater
                ? args[IndexOfDirectoryParamater]
                : Directory.GetCurrentDirectory();
        }

    }
}
