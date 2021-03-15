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
        public decimal? RefFat { get; set; }
        public decimal Whey { get; set; }
        public decimal? RefWhey { get; set; }
        public decimal DryParticles { get; set; }
        public decimal? RefDryParticles { get; set; }
        public decimal DryFatFreeParticles { get; set; }
        public decimal? RefDryFatFreeParticles { get; set; }
        public decimal FreezingPoint { get; set; }
        public decimal? RefFreezingPoint { get; set; }
        public decimal Lactose { get; set; }
        public decimal? RefLactose { get; set; }
        
    }
}