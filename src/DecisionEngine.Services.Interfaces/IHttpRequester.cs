using DecisionEngine.Models;
using System.Threading.Tasks;

namespace DecisionEngine.Services.Interfaces
{
    public interface IHttpRequester
    {
        Task<Response> PostAsync(User model);
    }
}
