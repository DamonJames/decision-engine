using DecisionEngine.Models;
using System.Threading.Tasks;

namespace DecisionEngine.Services.Interfaces
{
    public interface IUserService
    {
        Task<Status> SubmitAsync(User user);
    }
}
