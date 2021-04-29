using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PlantiT.Service.MilkoScanCSVParser.Models;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class Repository : RepositoryBase
    {
        public Repository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> InsertMilkoScanFileData(MilkoscanFile milkoscanFile)
        {
            return await WithConnection(async conn =>
            {
                var p = new DynamicParameters();
                p.Add("szFileName", milkoscanFile.FileName);
                p.Add("szFileBody", String.Empty);
                p.Add("tFileCreated", milkoscanFile.FileCreated);
                p.Add("tFileModified", milkoscanFile.FileModified);
                p.Add("bHasWrongStructure", milkoscanFile.HasWrongStructure);
                p.Add("bIsDuplicate", false);
                p.Add("nKey", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.QueryAsync<int>("sp_MS_MilkoScanDataInsert",
                    p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: conn.ConnectionTimeout
                );

                return p.Get<int>("nKey");
            });
        }
        
        public async Task<int> UpdateMilkoScanFileData(int milkoScanDataId, bool bIsDuplicate)
        {
            return await WithConnection(async conn =>
            {
                var p = new DynamicParameters();
                p.Add("nKey", milkoScanDataId);
                p.Add("bIsDuplicate", bIsDuplicate);

                var affectedRows =  await conn.ExecuteAsync("sp_MS_MilkoScanDataUpdate",
                    p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: conn.ConnectionTimeout
                );

                return affectedRows;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public async Task<int> InsertMilkoScanDataSample(int milkoScanDataId, MilkoscanSample sample)
        {
            var p = new DynamicParameters();
            p.Add("nMilkoScanDataLink", milkoScanDataId);
            p.Add("tAnalysisTime", sample.AnalysisTime);
            p.Add("szProductName", sample.Parameters.ProductName);
            p.Add("szSampleId", sample.Parameters.SampleId);
            p.Add("szDate", sample.Parameters.Date);
            p.Add("szTime", sample.Parameters.Time);
            p.Add("szSampleStatus", sample.Parameters.SampleStatus);
            p.Add("nSampleNumber", sample.Parameters.SampleNumber);
            p.Add("rWhey", sample.Parameters.Whey);
            p.Add("rFat", sample.Parameters.Fat);
            p.Add("rLactose", sample.Parameters.Lactose);
            p.Add("rDryParticles", sample.Parameters.DryParticles);
            p.Add("rDryParticlesFatFree", sample.Parameters.DryParticlesFatFree);
            p.Add("rFreezingPoint", sample.Parameters.FreezingPoint);
            p.Add("szInstrumentStatus", sample.Parameters.InstrumentStatus);

            p.Add("nKey", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return await WithConnection(async conn =>
            {
                await conn.QueryAsync<int>("sp_MS_MilkoScanDataSampleInsert",
                    p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: conn.ConnectionTimeout
                );
                return p.Get<int>("nKey");
            });
        }

        public async Task<IEnumerable<int>> GetMilkoscanLastSamples()
        {
           return await WithConnection(async conn=>
            {
                var p = new DynamicParameters();
                // p.Add("tAnalysisTime", dbType: DbType.DateTime, direction: ParameterDirection.Output);
                var result = await conn.QueryAsync<int>("sp_MS_MilkoScanLastSamplesGet",
                    p,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: conn.ConnectionTimeout
                );

                return result;
            });
        }
    }
}