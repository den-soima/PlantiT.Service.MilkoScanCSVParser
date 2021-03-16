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

        public string LogPath => GetLogFilePath();
        

        private readonly string _filePath;
        private readonly string _logPath;

        public ServiceSettings(IConfiguration configuration)
        {
#if DEBUG
            _filePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFilePath"];
            _logPath = Directory.GetCurrentDirectory() + configuration["Parameters:LogDirectoryPath"];
#else
            _filePath = configuration["Parameters:CSVFilePath"];
            _logPath = configuration["Parameters:LogDirectoryPath"];
#endif
            FileReadingInterval = Int32.Parse(configuration["Parameters:FileReadingInterval"]);
        }

        private string GetCSVFilePath()
        {
            if (File.Exists(_filePath))
            {
                return _filePath;
            }
            else if (Directory.Exists(_filePath))
            {
                var files = Directory.GetFiles(_filePath);
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

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var name = Path.GetFileNameWithoutExtension(FilePath)
                       + "_archive_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            return dir + name;
        }
        
        private string GetLogFilePath()
        {
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }
            
            return _logPath + "Log_MilkoScanCSVParser_" + DateTime.Now.ToString("yyyyMM") + ".log";
        }
    }
}