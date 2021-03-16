using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using PlantiT.Service.MilkoScanCSVParser.Helpers;
using PlantiT.Service.MilkoScanCSVParser.Models;


namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class FileReader
    {
        private readonly ServiceSettings _serviceSettings;
        private StreamReader _reader;
        private const int _maxLinesToRead = 2;

        public FileReader(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public MilkoScanData ReadFile()
        {
            int linePointer = 0;

            string filePath = _serviceSettings.FilePath;
            
            _reader = new StreamReader(File.OpenRead(filePath));

            MilkoScanData milkoScanData = new MilkoScanData();
            List<MilkoScanParameter> parameters = new List<MilkoScanParameter>();

            while (!_reader.EndOfStream && linePointer < _maxLinesToRead)
            {
                linePointer++;

                var line = _reader.ReadLine();
                var values = line?.Split(",");

                milkoScanData.FileBody = linePointer == 2 ? line : String.Empty;

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
            milkoScanData.ReadingTime = DateTime.Now;
            milkoScanData.Parameters = parameters;
            milkoScanData.MilkoScanSample = SetValues(parameters);

            return milkoScanData;
        }

        private MilkoScanSample SetValues(List<MilkoScanParameter> parameters)
        {
            return new MilkoScanSample
            {
                AnalysisTime = DateTime.Parse(parameters[0].Value),
                ProductName = parameters[1].Value,
                ProductCode = parameters[2].Value,
                SampleType = parameters[3].Value,
                SampleNumber = parameters[4].Value,
                SampleComment = parameters[5].Value,
                InstrumentName = parameters[6].Value,
                InstrumentSerialNumber = parameters[7].Value,
                Fat = decimal.Parse(parameters[8].Value),
                RefFat = string.IsNullOrWhiteSpace(parameters[9].Value) ? null : decimal.Parse(parameters[9].Value),
                Whey = decimal.Parse(parameters[10].Value),
                RefWhey = string.IsNullOrWhiteSpace(parameters[11].Value) ? null : decimal.Parse(parameters[11].Value),
                DryParticles = decimal.Parse(parameters[12].Value),
                RefDryParticles = string.IsNullOrWhiteSpace(parameters[13].Value)
                    ? null
                    : decimal.Parse(parameters[13].Value),
                DryFatFreeParticles = decimal.Parse(parameters[14].Value),
                RefDryFatFreeParticles = string.IsNullOrWhiteSpace(parameters[15].Value)
                    ? null
                    : decimal.Parse(parameters[15].Value),
                FreezingPoint = decimal.Parse(parameters[16].Value),
                RefFreezingPoint = string.IsNullOrWhiteSpace(parameters[17].Value)
                    ? null
                    : decimal.Parse(parameters[17].Value),
                Lactose = decimal.Parse(parameters[18].Value),
                RefLactose = string.IsNullOrWhiteSpace(parameters[19].Value)
                    ? null
                    : decimal.Parse(parameters[19].Value)
            };
        }
    }
}