using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace PlantiT.Service.MilkoScanCSVParser.Helpers
{
    public class ServiceSettings
    {
        public int FileReadingInterval { get; }

        public string FilePath { get; }

        public string ArchivePath { get; }

        public string DuplicatePath { get; }

        public string TrashPath { get; }

        public string LogPath { get; }


        public ServiceSettings(IConfiguration configuration)
        {
#if DEBUG
            FilePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFilePath"];
            ArchivePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFileArchivePath"];
            DuplicatePath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFileDuplicatePath"];
            TrashPath = Directory.GetCurrentDirectory() + configuration["Parameters:CSVFileTrashPath"];
            LogPath = Directory.GetCurrentDirectory() + configuration["Parameters:LogDirectoryPath"];
#else
            FilePath = configuration["Parameters:CSVFilePath"];
            ArchivePath = configuration["Parameters:CSVFileArchivePath"];
            DuplicatePath = configuration["Parameters:CSVFileDuplicatePath"];
            TrashPath = configuration["Parameters:CSVFileTrashPath"];
            LogPath = configuration["Parameters:LogDirectoryPath"];
#endif
            FileReadingInterval = Int32.Parse(configuration["Parameters:FileReadingInterval"]);
        }
    }
}