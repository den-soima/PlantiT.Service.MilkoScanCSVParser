using System;
using System.Collections.Generic;
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
        private const int _maxLinesToRead = 2;

        public FileReader(ServiceSettings serviceSettings, Repository repository)
        {
            _serviceSettings = serviceSettings;
            _repository = repository;
        }

        public MilkoScanData ReadFile()
        {
            int linePointer = 0;
            var fileBody = "file body";

            string filePath = _serviceSettings.FilePath;

            if (!File.Exists(filePath))
            {
                if (Directory.Exists(filePath))
                {
                    var files = Directory.GetFiles(filePath);
                    foreach (var file in files)
                    {
                        if (Path.GetExtension(file) == ".csv")
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

            MilkoScanData milkoScanData = new MilkoScanData();
            List<MilkoScanParameter> parameters = new List<MilkoScanParameter>();

            while (!_reader.EndOfStream && linePointer < _maxLinesToRead)
            {
                linePointer++;

                var line = _reader.ReadLine();
                var values = line?.Split(",");

                fileBody= linePointer == 2 ? line : "Error reading";

                for (int i = 0; i < values?.Length; i++)
                {
                    if (linePointer == 1)
                    {
                        parameters.Add(new MilkoScanParameter
                        {
                            Id = i,
                            Key = values[i],
                            Value = ""
                        });
                    }
                    else if (linePointer == 2)
                    {
                        parameters[i].Value = values[i];
                    }
                }
            }

            milkoScanData.FileName = Path.GetFileName(filePath);
            milkoScanData.FileCreated = File.GetCreationTime(filePath);
            milkoScanData.FileModified = File.GetLastWriteTime(filePath);
            milkoScanData.FilePath = filePath;
            milkoScanData.ReadingTime = DateTime.Now;
            milkoScanData.FileBody = fileBody;
            milkoScanData.Parameters = parameters;

            // sample data
            milkoScanData.MilkoScanSample = SetValues(parameters);
            
            // wrong structure
            milkoScanData.HasWrongStructure = milkoScanData.MilkoScanSample == null;

            // duplicate 
            if (milkoScanData.MilkoScanSample != null)
                milkoScanData.IsDuplicate = _repository.MilkoScanDataDuplicateCheck(milkoScanData.MilkoScanSample.AnalysisTime);

            _reader.Close();

            return milkoScanData;
        }

        private MilkoScanSample SetValues(List<MilkoScanParameter> parameters)
        {
            MilkoScanSample milkoScanSample = null;

            try
            {
                var analysisTime = DateTime.Parse(parameters[0].Value);
                var productName = parameters[1].Value;
                var productCode = parameters[2].Value;
                var sampleType = parameters[3].Value;
                var sampleNumber = parameters[4].Value;
                var sampleComment = parameters[5].Value;
                var instrumentName = parameters[6].Value;
                var instrumentSerialNumber = parameters[7].Value;
                var fat = decimal.Parse(parameters[8].Value);
                decimal? refFat = string.IsNullOrWhiteSpace(parameters[9].Value) ? null : decimal.Parse(parameters[9].Value);
                var whey = decimal.Parse(parameters[10].Value);
                decimal? refWhey = string.IsNullOrWhiteSpace(parameters[11].Value)
                    ? null
                    : decimal.Parse(parameters[11].Value);
                var dryParticles = decimal.Parse(parameters[12].Value);
                decimal? refDryParticles = string.IsNullOrWhiteSpace(parameters[13].Value)
                    ? null
                    : decimal.Parse(parameters[13].Value);
                var dryFatFreeParticles = decimal.Parse(parameters[14].Value);
                decimal? refDryFatFreeParticles = string.IsNullOrWhiteSpace(parameters[15].Value)
                    ? null
                    : decimal.Parse(parameters[15].Value);
                var freezingPoint = decimal.Parse(parameters[16].Value);
                decimal? refFreezingPoint = string.IsNullOrWhiteSpace(parameters[17].Value)
                    ? null
                    : decimal.Parse(parameters[17].Value);
                var lactose = decimal.Parse(parameters[18].Value);
                decimal? RefLactose = string.IsNullOrWhiteSpace(parameters[19].Value)
                    ? null
                    : decimal.Parse(parameters[19].Value);
                
                milkoScanSample = new MilkoScanSample
                {
                    AnalysisTime = analysisTime,
                    ProductName = productName,
                    ProductCode = productCode,
                    SampleType = sampleType,
                    SampleNumber = sampleNumber,
                    SampleComment = sampleComment,
                    InstrumentName = instrumentName,
                    InstrumentSerialNumber = instrumentSerialNumber,
                    Fat = fat,
                    RefFat = refFat,
                    Whey = whey,
                    RefWhey = refWhey,
                    DryParticles = dryParticles,
                    RefDryParticles = refDryParticles,
                    DryFatFreeParticles = dryFatFreeParticles,
                    RefDryFatFreeParticles = refDryFatFreeParticles,
                    FreezingPoint = freezingPoint,
                    RefFreezingPoint = refFreezingPoint,
                    Lactose = lactose,
                    RefLactose = RefLactose
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                milkoScanSample = null;
            }

            return milkoScanSample;
        }
    }
}