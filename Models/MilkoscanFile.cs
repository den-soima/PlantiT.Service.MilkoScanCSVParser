using System;
using System.Collections.Generic;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoscanFile
    { 
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime FileCreated { get; set; }
        public DateTime FileModified { get; set; }
        public DateTime ReadingTime { get; set; }
        public bool? HasWrongStructure { get; set; }
        
        public bool IsDuplicate { get; set; }
        public MilkoscanFileData MilkoScanFileData { get; set; }
    }
}