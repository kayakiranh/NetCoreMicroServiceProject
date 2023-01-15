using Microsoft.Extensions.Configuration;
using MP.Core.Application.DataTransferObjects;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Enums;
using System;
using System.Data.Common;

namespace MP.Infrastructure.Persistance.PostgreSql
{
    /// <summary>
    /// PostgreSql repository
    /// </summary>
    public class PostgreSqlRepository : IPostgreSqlRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerRepository _loggerRepository;

        public PostgreSqlRepository(IConfiguration configuration, ILoggerRepository loggerRepository)
        {
            _configuration = configuration;
            _loggerRepository = loggerRepository;
        }

        private DbConnection Connect()
        {
            DbProviderFactories.RegisterFactory(_configuration.GetSection("PostgreSqlSettings:ProviderName").Value, Npgsql.NpgsqlFactory.Instance);
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(_configuration.GetSection("PostgreSqlSettings:ProviderName").Value);

            DbConnectionStringBuilder dbConnectionStringBuilder = dbProviderFactory.CreateConnectionStringBuilder();
            dbConnectionStringBuilder.Add("Host", _configuration.GetSection("PostgreSqlSettings:Host").Value);
            dbConnectionStringBuilder.Add("Username", _configuration.GetSection("PostgreSqlSettings:Username").Value);
            dbConnectionStringBuilder.Add("Password", _configuration.GetSection("PostgreSqlSettings:Password").Value);
            dbConnectionStringBuilder.Add("Database", _configuration.GetSection("PostgreSqlSettings:Database").Value);

            DbConnection dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = dbConnectionStringBuilder.ConnectionString;

            try
            {
                using (dbConnection)
                {
                    return dbConnection;
                }
            }
            catch (Exception ex)
            {
                _loggerRepository.Insert(LogTypes.Critical, "PostgreSqlRepository Connect Error", ex);
            }

            return null;
        }

        public int Insert(CreditCardApplicationDto model)
        {
            try
            {
                DbConnection dbConnection = Connect();
                DbCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = @"INSERT INTO public.creditcardapplications(id, customerid, creditcardid, filepath, created) VALUES (:id, :customerid, :creditcardid, :score, :filepath, :created)";

                DbParameter paramId = dbCommand.CreateParameter();
                paramId.ParameterName = "id";
                paramId.Value = model.Id;
                dbCommand.Parameters.Add(paramId);

                DbParameter paramCustomerId = dbCommand.CreateParameter();
                paramCustomerId.ParameterName = "customerid";
                paramCustomerId.Value = model.CustomerId;
                dbCommand.Parameters.Add(paramCustomerId);

                DbParameter paramCreditCardId = dbCommand.CreateParameter();
                paramCreditCardId.ParameterName = "creditcardid";
                paramCreditCardId.Value = model.CreditCardId;
                dbCommand.Parameters.Add(paramCreditCardId);

                DbParameter paramFilePath = dbCommand.CreateParameter();
                paramFilePath.ParameterName = "filepath";
                paramFilePath.Value = model.FilePath;
                dbCommand.Parameters.Add(paramFilePath);

                DbParameter paramCreated = dbCommand.CreateParameter();
                paramCreated.ParameterName = "created";
                paramCreated.Value = model.Created;
                dbCommand.Parameters.Add(paramCreated);

                int effectedRowCount = dbCommand.ExecuteNonQuery();
                if (effectedRowCount > 0) _loggerRepository.Insert(LogTypes.Information, "postgresql success", null, model.Id, model.CustomerId, model.CreditCardId, model.FilePath, model.Created);
                else _loggerRepository.Insert(LogTypes.Warning, "postgresql error", null, model.Id, model.CustomerId, model.CreditCardId, model.FilePath, model.Created);
                return effectedRowCount;
            }
            catch (Exception ex)
            {
                _loggerRepository.Insert(LogTypes.Information, "postgresql error", ex, model.Id, model.CustomerId, model.CreditCardId, model.FilePath, model.Created);
                return 0;
            }
        }
    }
}
