using System;
using System.Globalization;
using System.IO;
using PlantiT.Service.MilkoScanCSVParser.Helpers;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class LoggerService
    {
        private readonly ServiceSettings _serviceSettings;

        public LoggerService(ServiceSettings serviceSettings)
        {
            _serviceSettings = serviceSettings;
        }

        public void WriteLog(string taskName, string logText, int logRank = 0)
        {
            var logFilePath = _serviceSettings.LogPath;
            
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
    }
}