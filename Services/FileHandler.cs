using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlantiT.Service.MilkoScanCSVParser.Helpers;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class FileHandler
    {
        private readonly ServiceSettings _serviceSettings;

        public FileHandler(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public string Archive(string csvFilePath)
        {
            string archiveFilePath = GetArchiveFilePath(csvFilePath);

            return MoveFile(csvFilePath , archiveFilePath);
        }

        private string GetArchiveFilePath(string csvFilePath)
        {
            var dir = _serviceSettings.ArchivePath;;

            var file = Path.GetFileNameWithoutExtension(csvFilePath)
                       + "_archive_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            return dir + file;
        }

        public string Duplicate(string csvFilePath)
        {
            string duplicateFilePath = GetDuplicateFilePath(csvFilePath);

            return MoveFile(csvFilePath , duplicateFilePath);
        }

        private string GetDuplicateFilePath(string csvFilePath)
        {
            var dir = _serviceSettings.DuplicatePath;;

            var file = Path.GetFileNameWithoutExtension(csvFilePath)
                       + "_duplicate_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            return dir + file;
        }
        
        public string Trash(string csvFilePath)
        {
            string trashFilePath = GetTrashFilePath(csvFilePath);

            return MoveFile(csvFilePath , trashFilePath);
        }
        
        private string GetTrashFilePath(string csvFilePath)
        {
            var dir = _serviceSettings.TrashPath;;

            var file = Path.GetFileNameWithoutExtension(csvFilePath)
                       + "_trash_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

            return dir + file;
        }

        private string MoveFile(string sourcePath, string targetPath)
        {
            string targetDirectory = Path.GetDirectoryName(targetPath);
            
            if (!Directory.Exists(Path.GetDirectoryName(targetDirectory)))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, targetPath);
            }

            return File.Exists(targetPath) ? targetPath : null;
        }
    }
}