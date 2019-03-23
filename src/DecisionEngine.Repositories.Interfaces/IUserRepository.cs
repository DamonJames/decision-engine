using DecisionEngine.Models;
using System.Threading.Tasks;

namespace DecisionEngine.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task PostAsync(User user);
        Task PutAsync(User user);
    }
}
