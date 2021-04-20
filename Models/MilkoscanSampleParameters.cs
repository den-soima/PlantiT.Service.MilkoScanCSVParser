using System;

namespace PlantiT.Service.MilkoScanCSVParser.Models
{
    public class MilkoscanSampleParameters
    {
        public string ProductName { get; set; }

        public string SampleId { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string SampleStatus { get; set; }
        public int SampleNumber { get; set; }
        public decimal Whey { get; set; }
        public decimal Fat { get; set; }
        public decimal Lactose { get; set; }
        public decimal DryParticles { get; set; }
        
        public decimal DryParticlesFatFree { get; set; }
        public decimal FreezingPoint { get; set; }

        public string InstrumentStatus { get; set; }

    }
}