using System.Data.SqlClient;

namespace DecisionEngine.Repositories.Interfaces
{
    public interface IConnectionFactory
    {
        SqlConnection OakbrookConnection();
    }
}
