using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using PlantiT.Service.MilkoScanCSVParser.Models;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class Repository : RepositoryBase
    {
        public Repository(IConfiguration configuration):base(configuration)
        {
            
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="fileName"></param>
       /// <param name="fileBody"></param>
       /// <returns></returns>
       public async Task<int> InsertMilkoScanData(MilkoScanData milkoScanData)
        {
            return await WithConnection( async conn =>
            {
                var p = new DynamicParameters();
                p.Add("szFileName", milkoScanData.FileName);
                p.Add("szFileBody", milkoScanData.FileBody);
                p.Add("tFileCreated", milkoScanData.FileCreated);
                p.Add("tFileModified", milkoScanData.FileModified);
                p.Add("nKey", dbType: DbType.Int32, direction: ParameterDirection.Output);
                
                await conn.QueryAsync<int>("sp_MS_MilkoScanDataInsert",
                    p,
                    commandType: CommandType.StoredProcedure
                );
                
                return p.Get<int>("nKey");
            });
        }
/// <summary>
/// 
/// </summary>
/// <param name="sample"></param>
/// <returns></returns>
        public async Task<int> InsertMilkoScanDataSample(int milkoScanDataId, MilkoScanSample sample)
        {
            var p = new DynamicParameters();
            p.Add("nMilkoscanDataLink", milkoScanDataId);
            p.Add("tAnalysisTime", sample.AnalysisTime);
            p.Add("szProductName", sample.ProductName);
            p.Add("szProductCode", sample.ProductCode);
            p.Add("szSampleType", sample.SampleType);
            p.Add("szSampleNumber", sample.SampleNumber);
            p.Add("szSampleComment", sample.SampleComment);
            p.Add("szInstrumentName", sample.InstrumentName);
            p.Add("szInstrumentSerialNumber", sample.InstrumentSerialNumber);
            p.Add("rFat", sample.Fat);
            p.Add("rRefFat", sample.RefFat);
            p.Add("rWhey", sample.Whey);
            p.Add("rRefWhey", sample.RefWhey);
            p.Add("rDryParticles", sample.DryParticles);
            p.Add("rRefDryParticles", sample.RefDryParticles);
            p.Add("rDryFatFreeParticles", sample.DryFatFreeParticles);
            p.Add("rRefDryFatFreeParticles", sample.RefDryFatFreeParticles);
            p.Add("rFreezingPoint", sample.FreezingPoint);
            p.Add("rRefFreezingPoint", sample.RefFreezingPoint);
            p.Add("rLactose", sample.Lactose);
            p.Add("rRefLactose", sample.RefLactose);
            
            
            p.Add("nKey", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            return await WithConnection(async conn =>
            {
                await conn.QueryAsync<int>("sp_MS_MilkoScanDataParametersInsert",
                   p,
                   commandType: CommandType.StoredProcedure
                );
                return p.Get<int>("nKey");
            });
        }
    }
}