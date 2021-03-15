using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PlantiT.Service.MilkoScanCSVParser.Services;
using PlantiT.Service.MilkoScanCSVParser.Models;

namespace PlantiT.Service.MilkoScanCSVParser
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly FileReader _fileReader;
        private readonly Repository _repository;


        public Worker(ILogger<Worker> logger, FileReader fileReader, Repository repository)
        {
            _logger = logger;
            _fileReader = fileReader;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                
                // Read CSV file
                MilkoScanData milkoScanData = _fileReader.ReadFile();
                
                // Write Milkoscan reading data to DB
                var milkoScanDataId = await _repository.InsertMilkoScanData(milkoScanData);
                
                // Write Milkoscan parameters to DB

                if (milkoScanDataId > 0)
                {
                    var milkoScanParametersId = await _repository.InsertMilkoScanDataSample(milkoScanDataId, milkoScanData.MilkoScanSample);
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}