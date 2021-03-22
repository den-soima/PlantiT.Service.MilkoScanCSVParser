using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceSettings _serviceSettings;
        private readonly FileReader _fileReader;
        private readonly Repository _repository;
        private readonly FileHandler _fileHandler;
        private readonly WorkerLogger _workerLogger;


        public Worker(ILogger<Worker> logger, ServiceSettings serviceSettings,
            FileReader fileReader, Repository repository, FileHandler fileHandler, WorkerLogger workerLogger)
        {
            _logger = logger;
            _serviceSettings = serviceSettings;
            _fileReader = fileReader;
            _repository = repository;
            _fileHandler = fileHandler;
            _workerLogger = workerLogger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string filePath = _serviceSettings.FilePath;
            string archivePath = _serviceSettings.ArchivePath;
            string duplicatePath = _serviceSettings.DuplicatePath;
            string trashPath = _serviceSettings.TrashPath;
            string logPath = _serviceSettings.LogPath;

            int milkoScanDataId = 0;
#if DEBUG
            _logger.LogInformation("Service settings:"
                                   + $"\r\n filePath - {filePath}"
                                   + $"\r\n archivePath - {archivePath}"
                                   + $"\r\n duplicatePath - {duplicatePath}"
                                   + $"\r\n trashPath - {trashPath}"
                                   + $"\r\n logPath - {logPath}"
                , filePath, archivePath, logPath);
#endif
            _workerLogger.WriteLog("Service"
                , $"started by {Environment.UserName} with interval - {_serviceSettings.FileReadingInterval}ms"
                , 0);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Read CSV file
                MilkoScanData milkoScanData = _fileReader.ReadFile();

                if (milkoScanData == null)
                {
                    continue;
                }
                
                _workerLogger.WriteLog($"({milkoScanData.FileName}) file observed"
                    , $"start reading {DateTime.Now.ToString(new CultureInfo("es-ES"))}"
                    , 1);
                
                _workerLogger.WriteLog($"File path"
                    , $"{_serviceSettings.FilePath}"
                    , 3);
                _workerLogger.WriteLog($"File created"
                    , $"{milkoScanData.FileCreated}"
                    , 3);

                _workerLogger.WriteLog($"File modified"
                    , $"{milkoScanData.FileModified}"
                    , 3);

                _workerLogger.WriteLog($"File body"
                    , $"({milkoScanData.FileBody})"
                    , 3);
                
               // Write Milkoscan data to DB
                try
                {
                    milkoScanDataId = await _repository.InsertMilkoScanData(milkoScanData);

                    _workerLogger.WriteLog($"New row added to table tbl_MS_MilkoScanData"
                        , $"id = {milkoScanDataId}"
                        , 2);
                }
                catch (Exception e)
                {
                    _workerLogger.WriteLog($"ERROR - Insert to DB"
                        , $"{e.Message}"
                        , 0);

                    _logger.LogError("Error Insert data to DB");
                }
            
                // Wrong structure
                if (milkoScanData.HasWrongStructure)
                {
                    _workerLogger.WriteLog($"File has wrong structure"
                        , $"will be moved to trash folder"
                        , 2);
                    
                    var result = _fileHandler.Trash(milkoScanData.FilePath);

                    if (result != null)
                    {
                        _workerLogger.WriteLog($"File moved to trash folder"
                            , $"path = {result}"
                            , 2);
                    }
                    else
                    {
                        _workerLogger.WriteLog($"ERROR File moving failed"
                            , $"path: {_serviceSettings.TrashPath}"
                            , 0);
                    }
                }
                
                
                // Duplicate
                if (milkoScanData.IsDuplicate)
                {
                    _workerLogger.WriteLog($"File is a duplicate"
                        , $"will be moved to duplicate folder"
                        , 2);
                    
                    var result = _fileHandler.Duplicate(milkoScanData.FilePath);

                    if (result != null)
                    {
                        _workerLogger.WriteLog($"File moved to duplicate folder"
                            , $"path = {result}"
                            , 2);
                    }
                    else
                    {
                        _workerLogger.WriteLog($"ERROR File moving failed"
                            , $"path: {_serviceSettings.DuplicatePath}"
                            , 0);
                    }
                }

    
                if (milkoScanDataId > 0 && !milkoScanData.HasWrongStructure && !milkoScanData.IsDuplicate)
                {
                    // Write Milkoscan sample to DB
                    try
                    {
                        var milkoScanDataSampleId = await _repository.InsertMilkoScanDataSample(milkoScanDataId, milkoScanData.MilkoScanSample);

                        _workerLogger.WriteLog($"New row added to table tbl_MS_MilkoScanDataSample"
                            , $"id = {milkoScanDataSampleId}"
                            , 2);
                    }
                    catch (Exception e)
                    {
                        _workerLogger.WriteLog($"ERROR - Insert to DB"
                            , $"{e.Message}"
                            , 0);

                        _logger.LogError("Error Insert data to DB");
                    }

                    // Archive file
                    var result = _fileHandler.Archive(milkoScanData.FilePath);

                    if (result != null)
                    {
                        _workerLogger.WriteLog($"File archived"
                            , $"path = {result}"
                            , 2);
                    }
                    else
                    {
                        _workerLogger.WriteLog($"ERROR File archiving failed"
                            , $"archive path: {_serviceSettings.ArchivePath}"
                            , 0);
                    }
                }

                await Task.Delay(_serviceSettings.FileReadingInterval, stoppingToken);
            }
        }
    }
}