using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlantiT.Service.MilkoScanCSVParser.Helpers;
using PlantiT.Service.MilkoScanCSVParser.Services;
using PlantiT.Service.MilkoScanCSVParser.Models;

namespace PlantiT.Service.MilkoScanCSVParser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceSettings _serviceSettings;
        private readonly FileReader _fileReader;
        private readonly Repository _repository;
        private readonly FileArchive _fileArchive;


        public Worker(ILogger<Worker> logger,ServiceSettings serviceSettings,  FileReader fileReader, Repository repository, FileArchive fileArchive)
        {
            _logger = logger;
            _serviceSettings = serviceSettings;
            _fileReader = fileReader;
            _repository = repository;
            _fileArchive = fileArchive;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                if (File.Exists(_serviceSettings.FilePath))
                {
                    // Read CSV file
                    MilkoScanData milkoScanData = _fileReader.ReadFile();
 
                    //TODO: add try catch
                    // Write Milkoscan reading data to DB
                    var milkoScanDataId = await _repository.InsertMilkoScanData(milkoScanData);
                
                    // Write Milkoscan parameters to DB

                    if (milkoScanDataId > 0)
                    {
                        var milkoScanDataSampleId = await _repository.InsertMilkoScanDataSample(milkoScanDataId, milkoScanData.MilkoScanSample);
                    }
                
                    // Archive file
                    var result =_fileArchive.Execute();
                }

                await Task.Delay(_serviceSettings.FileReadingInterval, stoppingToken);
            }
        }
    }
}