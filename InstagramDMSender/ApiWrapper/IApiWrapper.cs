using InstagramDMSender.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramDMSender.ApiWrapper
{
    public interface IApiWrapper
    {
        Task<bool> LoginAsync(string userName, string password);
        User GetUser(long userId);
        User GetLoggedInUser();
        IEnumerable<User> GetFollowers(long userId);
        IEnumerable<User> GetFollowersForLoggedInUser();
    }
}