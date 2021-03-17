using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PlantiT.Service.MilkoScanCSVParser.Helpers
{
    public class ServiceSettings
    {
        public int FileReadingInterval { get; }

        public string FilePath => GetCSVFilePath();

        public string ArchivePath => GetArchivePath();

        public string LogPath => GetLogFilePath();


        private readonly string _filePath;
        private readonly string _fileArchivePath;
        private readonly string _logPath;

        public ServiceSettings(IConfiguration configuration)
        {
#if DEBUG
            _filePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFilePath"];
            _fileArchivePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFileArchivePath"];
            _logPath = Directory.GetCurrentDirectory() + configuration["Parameters:LogDirectoryPath"];
#else
            _filePath = configuration["Parameters:CSVFilePath"];
            _fileArchivePath = configuration["Parameters:CSVFileArchivePath"];
            _logPath = configuration["Parameters:LogDirectoryPath"];
#endif
            FileReadingInterval = Int32.Parse(configuration["Parameters:FileReadingInterval"]);
        }

        private string GetCSVFilePath()
        {
            return _filePath;
        }

        private string GetArchivePath()
        {
            return _fileArchivePath;
        }

        private string GetLogFilePath()
        {
            return  _logPath;
        }
    }
}