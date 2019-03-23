using System.Data;
using Dapper;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using DecisionEngine.Models;
using DecisionEngine.Repositories.Interfaces;

namespace DecisionEngine.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger _logger;

        public UserRepository(IConnectionFactory connectionFactory, ILogger<UserRepository> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task PostAsync(User user)
        {
            const string sql =
                @"INSERT INTO Requests (Id, FirstName, LastName, DateOfBirth, Result)
                  VALUES (@Id, @FirstName, @LastName, @DateOfBirth, @Result)";

            try
            {
                using (IDbConnection connection = _connectionFactory.OakbrookConnection())
                {
                    connection.Open();
                    await connection.ExecuteAsync(sql, user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
        }

        public async Task PutAsync(User user)
        {
            const string sql =
                @"UPDATE Requests SET Result = @Result
                  WHERE Id = @Id";
            try
            {
                using (IDbConnection connection = _connectionFactory.OakbrookConnection())
                {
                    connection.Open();
                    await connection.ExecuteAsync(sql, user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
}
    }
}
