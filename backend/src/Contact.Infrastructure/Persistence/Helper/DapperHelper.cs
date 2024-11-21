using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace Contact.Infrastructure.Persistence.Helper
{
    public class DapperHelper : IDapperHelper
    {
        private readonly AppSettings myConfig;
        private readonly ILogger _logger;

        public DapperHelper(IOptions<AppSettings> myConfigValue, ILogger<DapperHelper> logger)
        {
            myConfig = myConfigValue.Value;
            _logger = logger;
        }

        public IDbConnection GetConnection()
        {
            _logger.LogInformation("Connection String: {connectionString}", myConfig.ConnectionStrings.DefaultConnection);  
            return new SqlConnection(myConfig.ConnectionStrings.DefaultConnection);
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public async Task<int> Execute(string sp, object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null)
        { 
            var db = transaction?.Connection ?? GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var resultObj = await db.ExecuteAsync(sp, parms, commandType: commandType, transaction: transaction);
                if (transaction == null)
                    db.Close();
                return resultObj;
            }
            catch (Exception exception)
            {
                if (transaction == null && db?.State == ConnectionState.Open)
                    db.Close();
                _logger.LogInformation("SQL DB error exception: {error}", exception.Message);
                throw exception;
            }
        }

        public async Task<T> Get<T>(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            T result;
            var db = transaction?.Connection ?? GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var resultObj = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, transaction: transaction);
                result = resultObj;
                if (transaction == null)
                    db.Close();
                return result;
            }
            catch (Exception exception)
            {
                if (transaction == null && db?.State == ConnectionState.Open)
                    db.Close();
                _logger.LogInformation("SQL DB error exception: {error}", exception.Message);
                throw exception;
            }
        }

        public async Task<IEnumerable<T>> GetAll<T>(string sp, Object parms, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (IDbConnection db = GetConnection())
                {
                    var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SQL DB error exception: {error}", ex.Message);
               
                throw ex;
            }
        }

        public async Task<T> Insert<T>(string sp, Object parms, CommandType commandType = CommandType.Text, IDbTransaction transaction = null)
        {
            T result;
            var db = transaction?.Connection ?? GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var resultObj = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, transaction: transaction);
                result = resultObj;
                if (transaction == null)
                    db.Close();
                return result;
            }
            catch (Exception exception)
            {
                if (transaction == null && db?.State == ConnectionState.Open)
                    db.Close();
                _logger.LogInformation("SQL DB error exception: {error}", exception.Message);
                throw exception;
            }
        }

        public async Task<T> Update<T>(string sp, object parms, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null)
        {
            T result;
            var db = transaction?.Connection ?? GetConnection();
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var resultObj = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, transaction: transaction);
                result = resultObj;
                if (transaction == null)
                    db.Close();
                return result;
            }
            catch (Exception ex)
            {
                if (transaction == null && db?.State == ConnectionState.Open)
                    db.Close();
                _logger.LogInformation("SQL DB error exception: {error}", ex.Message);
                throw ex;
            }

        }
    }
}
