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


        public bool Execute()
        {
            if (File.Exists(_serviceSettings.FilePath))
            {
                File.Move(_serviceSettings.FilePath, _serviceSettings.ArchivePath);
            }

            return File.Exists(_serviceSettings.ArchivePath);
        }
    }
}