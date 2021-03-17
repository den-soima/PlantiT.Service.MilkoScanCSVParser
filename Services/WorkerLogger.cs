using System;
using System.Globalization;
using System.IO;
using PlantiT.Service.MilkoScanCSVParser.Helpers;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class WorkerLogger
    {
        private readonly ServiceSettings _serviceSettings;

        public WorkerLogger(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public void WriteLog(string taskName, string logText, int logRank = 0)
        {
            var logFilePath = GetLogFilePath(_serviceSettings.LogPath);
            
            if (!Directory.Exists(Path.GetDirectoryName(logFilePath)))
            {
                Directory.CreateDirectory(logFilePath);
            }
            
            if (!File.Exists(logFilePath))
            {
                string header = "           MilkoScan CSV Parser LOG: " + "Created - "
                                                                        + DateTime.Now.ToString(
                                                                            new CultureInfo("es-ES"))
                                                                        + " ( - ProLeiT UA)" + Environment.NewLine;
                File.WriteAllText(logFilePath, header);
            }

            string body = new String(' ', 5 * logRank)
                          + (logRank == 0 ? DateTime.Now.ToString(new CultureInfo("es-ES")) : "")
                          + " - " + taskName + " : " + logText + Environment.NewLine;

            File.AppendAllText(logFilePath, body);
        }
        
        private string GetLogFilePath(string logPath)
        {
            var dir = logPath;
            var file = "Log_MilkoScanCSVParser_" + DateTime.Now.ToString("yyyyMM") + ".log";
            
            return  dir + file;
        }
    }
}