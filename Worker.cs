using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly LoggerService _loggerService;


        public Worker(ILogger<Worker> logger, ServiceSettings serviceSettings,
            FileReader fileReader, Repository repository, FileArchive fileArchive, LoggerService loggerService)
        {
            _logger = logger;
            _serviceSettings = serviceSettings;
            _fileReader = fileReader;
            _repository = repository;
            _fileArchive = fileArchive;
            _loggerService = loggerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Service started");
            }
            else
            {
                _logger.LogInformation("Service stoped");
            }
            
            _loggerService.WriteLog("Service"
                , $"started by {Environment.UserName} with interval - {_serviceSettings.FileReadingInterval}ms"
                , 0);

            while (!stoppingToken.IsCancellationRequested)
            {
                

                if (File.Exists(_serviceSettings.FilePath))
                {
                    _loggerService.WriteLog($"({Path.GetFileName(_serviceSettings.FilePath)}) file observed"
                        , $"start reading {DateTime.Now.ToString(new CultureInfo("es-ES"))}"
                        , 1);
                    // Read CSV file
                    MilkoScanData milkoScanData = _fileReader.ReadFile();

                    _loggerService.WriteLog($"File path"
                        , $"{_serviceSettings.FilePath}"
                        , 3);
                    _loggerService.WriteLog($"File created"
                        , $"{milkoScanData.FileCreated}"
                        , 3);

                    _loggerService.WriteLog($"File modified"
                        , $"{milkoScanData.FileModified}"
                        , 3);

                    _loggerService.WriteLog($"File body"
                        , $"({milkoScanData.FileBody})"
                        , 3);

                    // Write Milkoscan data/sample to DB
                    try
                    {
                        var milkoScanDataId = await _repository.InsertMilkoScanData(milkoScanData);

                        _loggerService.WriteLog($"New row added to table tbl_MS_MilkoScanData"
                            , $"id = {milkoScanDataId}"
                            , 2);

                        var milkoScanDataSampleId =
                            await _repository.InsertMilkoScanDataSample(milkoScanDataId, milkoScanData.MilkoScanSample);

                        _loggerService.WriteLog($"New row added to table tbl_MS_MilkoScanDataSample"
                            , $"id = {milkoScanDataSampleId}"
                            , 2);
                    }
                    catch (Exception e)
                    {
                        _loggerService.WriteLog($"ERROR - Insert to DB"
                            , $"{e.Message}"
                            , 0);
                        
                        _logger.LogError("Error Insert data to DB");
                    }

                    // Archive file
                    var result = _fileArchive.Execute();

                    if (result != null)
                    {
                        _loggerService.WriteLog($"File archived"
                            , $"path = {result}"
                            , 2);
                    }
                    else
                    {
                        _loggerService.WriteLog($"ERROR File archiving failed"
                            , $"archive path: {_serviceSettings.ArchivePath}"
                            , 0);
                    }
                }

                await Task.Delay(_serviceSettings.FileReadingInterval, stoppingToken);
            }
        }
    }
}