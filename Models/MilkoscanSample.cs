using System;
using System.Collections.Generic;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoscanSample
    {
        public MilkoscanSampleParameters Parameters { get; set; }

        public DateTime AnalysisTime { get; set; }
        
        public string SampleRow { get; set; }
        
    }
}