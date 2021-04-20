using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PlantiT.Service.MilkoScanCSVParser.Helpers;
using PlantiT.Service.MilkoScanCSVParser.Models;


namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class FileReader
    {
        private readonly ServiceSettings _serviceSettings;
        private readonly Repository _repository;
        private StreamReader _reader;

        public FileReader(ServiceSettings serviceSettings, Repository repository)
        {
            _serviceSettings = serviceSettings;
            _repository = repository;
        }

        public MilkoscanFile ReadFile()
        {
            string filePath = _serviceSettings.FilePath;

            if (!File.Exists(filePath))
            {
                if (Directory.Exists(filePath))
                {
                    var files = Directory.GetFiles(filePath);
                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file).ToLower() == ".csv")
                        {
                            filePath = file;
                            break;
                        }
                    }
                }
            }
            
            if (!File.Exists(filePath))
            {
                return null;
            }

            _reader = new StreamReader(filePath);

            MilkoscanFile milkoscanFile = new MilkoscanFile();
            MilkoscanFileData milkoscanFileData = new MilkoscanFileData();

            int linePointer = 0;
            while (!_reader.EndOfStream)
            {
                linePointer++;
                var line = _reader.ReadLine();
                var values = line?.Split(";");
                
                    if (linePointer == 1)
                    {
                        milkoscanFileData.Key = values;
                    }
                    else {
                        milkoscanFileData.Samples.Add(values);
                    }
            }

            milkoscanFile.FileName = Path.GetFileName(filePath);
            milkoscanFile.FileCreated = File.GetCreationTime(filePath);
            milkoscanFile.FileModified = File.GetLastWriteTime(filePath);
            milkoscanFile.FilePath = filePath;
            milkoscanFile.ReadingTime = DateTime.Now;
            milkoscanFile.HasWrongStructure = milkoscanFileData?.Key?.Length != 30;
            milkoscanFile.MilkoScanFileData = milkoscanFileData;

            _reader.Close();

            return milkoscanFile;
        }
        
    }
}