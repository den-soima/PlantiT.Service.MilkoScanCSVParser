using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
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
                int milkoScanDataId = 0;
                
                //TODO: Exclude scan files in folder from FileReader
                
                // Read CSV file
                MilkoscanFile milkoscanFile = _fileReader.ReadFile();

                if (milkoscanFile != null)
                {
                    #region Log file header

                    _workerLogger.WriteLog($"({milkoscanFile.FileName}) file observed"
                        , $"start reading {DateTime.Now.ToString(new CultureInfo("es-ES"))}"
                        , 1);

                    _workerLogger.WriteLog($"File path"
                        , $"{_serviceSettings.FilePath}"
                        , 3);
                    _workerLogger.WriteLog($"File created"
                        , $"{milkoscanFile.FileCreated}"
                        , 3);

                    _workerLogger.WriteLog($"File modified"
                        , $"{milkoscanFile.FileModified}"
                        , 3);

                    #endregion

                    try
                    {
                        milkoScanDataId = await _repository.InsertMilkoScanFileData(milkoscanFile);

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
                        _logger.LogError(e.Message);
                    }

                    // Wrong structure
                    if (milkoscanFile.HasWrongStructure ?? true)
                    {
                        _workerLogger.WriteLog($"File has wrong structure"
                            , $"will be moved to trash folder"
                            , 2);

                        var result = _fileHandler.Trash(milkoscanFile.FilePath);

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
                    else
                    {
                        MilkoscanDataHandler milkoscanDataHandler = new MilkoscanDataHandler();

                        List<MilkoscanSample> samples =
                            milkoscanDataHandler.HandleData(milkoscanFile.MilkoScanFileData);

                        if (samples.Count > 0)
                        {
                            // get last sample date time

                            var lastAnalysisDate = await _repository.GetMilkoscanLastSampleDate();

                            var filteredSamples = lastAnalysisDate == null? samples
                                : samples.Where(sample => sample.AnalysisTime > lastAnalysisDate);

                            _workerLogger.WriteLog($"New samples"
                                , $"Count = {filteredSamples.Count()}"
                                , 2);
                            
                            foreach (var sample in filteredSamples)
                            {
                                // Write Milkoscan sample to DB
                                try
                                {
                                    var milkoScanDataSampleId =
                                        await _repository.InsertMilkoScanDataSample(milkoScanDataId,
                                            sample);

                                    _workerLogger.WriteLog($"New row added to table tbl_MS_MilkoScanDataSample"
                                        , $"id = {milkoScanDataSampleId}"
                                        , 2);
                                    _workerLogger.WriteLog($"Sample"
                                        , $"data = {sample.SampleRow}"
                                        , 3);
                                }
                                catch (Exception e)
                                {
                                    _workerLogger.WriteLog($"ERROR - Insert to DB"
                                        , $"{e.Message}"
                                        , 0);

                                    _logger.LogError("Error Insert data to DB");
                                }
                            }
                        }

                        // Archive file
                        var result = _fileHandler.Archive(milkoscanFile.FilePath);

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
}