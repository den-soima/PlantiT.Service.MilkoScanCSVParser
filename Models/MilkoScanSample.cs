using System;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoScanSample
    {
        public DateTime AnalysisTime { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string SampleType { get; set; }
        public string SampleNumber { get; set; }
        public string SampleComment { get; set; }
        public string InstrumentName { get; set; }
        public string InstrumentSerialNumber { get; set; }
        public decimal Fat { get; set; }
        public string RefFat { get; set; }
        public decimal Whey { get; set; }
        public string RefWhey { get; set; }
        public decimal DryParticles { get; set; }
        public string RefDryParticles { get; set; }
        public decimal DryFatFreeParticles { get; set; }
        public string RefDryFatFreeParticles { get; set; }
        public decimal FreezingPoint { get; set; }
        public string RefFreezingPoint { get; set; }
        public decimal Lactose { get; set; }
        public string RefLactose { get; set; }
        
    }
}