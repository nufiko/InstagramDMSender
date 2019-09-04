using InstagramDMSender.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramDMSender.ApiWrapper
{
    public interface IApiWrapper
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<User> GetUserByIdAsync(long userId);
        Task<User> GetUserByNameAsync(string userName);
        Task<User> GetLoggedInUserAsync();
        Task<IEnumerable<User>> GetFollowersAsync(long userId);
        Task<IEnumerable<User>> GetFollowersForLoggedInUserAsync();
        Task SaveLoginData();
        Task<bool> LoadLoginData();
        Task Logout();
        Task SendMessage(IEnumerable<long> recieversIds, string message, long senderId);
    }
}