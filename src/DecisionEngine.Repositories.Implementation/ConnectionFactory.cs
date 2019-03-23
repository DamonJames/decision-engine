using DecisionEngine.Models;
using DecisionEngine.Repositories.Interfaces;
using System.Data.SqlClient;

namespace DecisionEngine.Repositories.Implementation
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionStrings _connectionStrings;

        public ConnectionFactory(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public SqlConnection OakbrookConnection()
        {
            return new SqlConnection(_connectionStrings.Oakbrook);
        }
    }
}
