using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PlantiT.Service.MilkoScanCSVParser.Services
{
    public class RepositoryBase
    {
        private readonly string _connectionString;

        public RepositoryBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                await using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return await getData(connection);
            }
            catch (TimeoutException e)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout",
                    e);
            }
            catch (SqlException e)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", e);
            }
        }
        
        protected T WithConnection<T>(Func<IDbConnection,T> getData)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                connection.Open();
                return getData(connection);
            }
            catch (TimeoutException e)
            {
                throw new Exception($"{GetType().FullName}.WithConnection() experienced a SQL timeout",
                    e);
            }
            catch (SqlException e)
            {
                throw new Exception(
                    $"{GetType().FullName}.WithConnection() experienced a SQL exception (not a timeout)", e);
            }
        }
    }
}