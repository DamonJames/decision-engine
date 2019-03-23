using DecisionEngine.Models;
using DecisionEngine.Repositories.Interfaces;
using DecisionEngine.Services.Interfaces;
using System.Threading.Tasks;

namespace DecisionEngine.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpRequester _httpRequester;

        public UserService(IUserRepository userRepository, IHttpRequester httpRequester)
        {
            _userRepository = userRepository;
            _httpRequester = httpRequester;
        }

        public async Task<Status> SubmitAsync(User user)
        {
            await _userRepository.PostAsync(user);

            var apiResult = await _httpRequester.PostAsync(user);

            SetRequestStatus(user, apiResult);

            await _userRepository.PutAsync(user);

            return (Status)user.Result;
        }

        private User SetRequestStatus(User user, Response response)
        {
            switch (response.DecisionResult)
            {
                case "Accepted":
                    user.Result = (int)Status.Accepted;
                    return user;
                case "Declined":
                    user.Result = (int)Status.Declined;
                    return user;
                case "Errored":
                    user.Result = (int)Status.Errored;
                    return user;
                default:
                    user.Result = (int)Status.Errored;
                    return user;
            }
        }
    }
}
