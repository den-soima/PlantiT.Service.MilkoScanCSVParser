using System;
using System.Collections.Generic;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoScanData
    {
        public string FileName { get; set; }
        public string FileBody { get; set; }
        public DateTime FileCreated { get; set; }
        public DateTime FileModified { get; set; }
        public DateTime ReadingTime { get; set; }
        public List<MilkoScanParameter> Parameters { get; set; }
        public MilkoScanSample MilkoScanSample { get; set; }
    }
}