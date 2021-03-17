using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlantiT.Service.MilkoScanCSVParser.Helpers;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class FileArchiver
    {
        private readonly ServiceSettings _serviceSettings;

        public FileArchiver(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public string Execute(string filePath)
        {
            string archivePath = _serviceSettings.ArchivePath;
            string archiveFilePath = GetArchiveFilePath(filePath, archivePath);

            if (!Directory.Exists(Path.GetDirectoryName(archivePath)))
            {
                Directory.CreateDirectory(archivePath);
            }
            
            if (File.Exists(filePath))
            {
                File.Move(filePath, archiveFilePath);
            }

            return File.Exists(archiveFilePath) ? archiveFilePath : null;
        }

        private string GetArchiveFilePath(string filePath, string archivePath)
        {
            var dir = archivePath;

            var file = Path.GetFileNameWithoutExtension(filePath)
                       + "_archive_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            return dir + file;
        }
    }
}