using System.Collections.Generic;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoscanFileData
    {
        public string[] Key { get; set; }
        public List<string[]> Samples { get; set; }

        public MilkoscanFileData()
        {
            Samples = new List<string[]>();
        }
    }
}