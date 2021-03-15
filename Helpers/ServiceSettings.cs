using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace PlantiT.Service.MilkoScanCSVParser.Helpers
{
    public class ServiceSettings
    {
        public int FileReadingInterval { get; }

        public string FilePath => GetCSVFilePath();

        public string ArchivePath => GetArchivePath();
        

        private readonly string _filePath;

        public ServiceSettings(IConfiguration configuration)
        {
#if DEBUG
            _filePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFilePath"];
#else
            _filePath = configuration["Parameters:CSVFilePath"];
#endif

            FileReadingInterval = Int32.Parse(configuration["Parameters:FileReadingInterval"]);
        }

        private string GetCSVFilePath()
        {
            string filePath = _filePath.EndsWith("/") ? _filePath.Remove(_filePath.Length - 1) : _filePath;

            if (File.Exists(filePath))
            {
                return filePath;
            }
            else if (Directory.Exists(filePath))
            {
                var files = Directory.GetFiles(filePath);
                foreach (var file in files)
                {
                    if (Path.GetExtension(file) == ".csv")
                    {
                        return file;
                    }
                }
            }

            return null;
        }

        private string GetArchivePath()
        {
            var dir = Path.GetDirectoryName(FilePath) + @"/" + "Archive" + @"/";
            var name = Path.GetFileNameWithoutExtension(FilePath)
                       + "_archive_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            return dir + name;
        }
    }
}