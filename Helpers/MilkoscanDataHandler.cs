using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using PlantiT.Service.MilkoScanCSVParser.Helpers;
using PlantiT.Service.MilkoScanCSVParser.Models;


namespace PlantiT.Service.MilkoScanCSVParser.Helpers
{
    public class MilkoscanDataHandler
    {
        public List<MilkoscanSample> HandleData(MilkoscanFileData milkoscanFileData)
        {
            List<MilkoscanSample> samples = new List<MilkoscanSample>();

            foreach (var sampleRow in milkoscanFileData.Samples)
            {
                if (sampleRow.Length == milkoscanFileData.Key.Length && !String.IsNullOrWhiteSpace(sampleRow[1]))
                {
                    MilkoscanSampleParameters sample = ParseSample(sampleRow);

                    if (sample != null)
                    {
                        var analysisTime = GetAnalysisTime(sample.Date, sample.Time);
                        samples.Add(new MilkoscanSample
                        {
                            Parameters = sample,
                            AnalysisTime = analysisTime,
                            SampleRow = String.Join(";",sampleRow)
                            
                        });
                    }
                }
            }

            return samples;
        }
        
        private MilkoscanSampleParameters ParseSample(string[] parameters)
        {
            MilkoscanSampleParameters milkoscanSampleParameters = null;

            try
            {
                var productName = parameters[0];
                var sampleId = parameters[1];
                var date = parameters[2];
                var time = parameters[3];
                var sampleStatus = parameters[4];
                var sampleNumber = Int32.Parse(parameters[5]);
                var whey = decimal.Parse(parameters[11]);
                var fat = decimal.Parse(parameters[12]);
                var lactose = decimal.Parse(parameters[13]);

                var dryParticles = decimal.Parse(parameters[14]);
                var dryFatFreeParticles = decimal.Parse(parameters[16]);
                var freezingPoint = decimal.Parse(parameters[17]);
                var instrumentStatus = parameters[29];

                milkoscanSampleParameters = new MilkoscanSampleParameters()
                {
                    ProductName = productName,
                    SampleId = sampleId,
                    Date = date,
                    Time = time,
                    SampleStatus = sampleStatus,
                    SampleNumber = sampleNumber,
                    Whey = whey,
                    Fat = fat,
                    Lactose = lactose,
                    DryParticles = dryParticles,
                    DryParticlesFatFree = dryFatFreeParticles,
                    FreezingPoint = freezingPoint,
                    InstrumentStatus = instrumentStatus
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                milkoscanSampleParameters = null;
            }

            return milkoscanSampleParameters;
        }

        private DateTime GetAnalysisTime(string dateParameter, string timeParameter)
        {
            var date = DateTime.ParseExact(dateParameter,"dd-MM-yyyy", CultureInfo.InvariantCulture);
            var time = TimeSpan.Parse(timeParameter);

            return date.Add(time);
        }
    }
}