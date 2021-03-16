using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using PlantiT.Service.MilkoScanCSVParser.Helpers;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class FileArchive
    {
        private readonly ServiceSettings _serviceSettings;

        public FileArchive(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public string Execute()
        {
            string filePath = _serviceSettings.FilePath;
            string archivePath = _serviceSettings.ArchivePath;
            
            if (File.Exists(filePath))
            {
                File.Move(filePath, archivePath);
            }

            return File.Exists(archivePath) ? archivePath : null;
        }
    }
}