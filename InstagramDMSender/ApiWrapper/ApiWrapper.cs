using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramDMSender.Models;

namespace InstagramDMSender.ApiWrapper
{
    public class ApiWrapper : IApiWrapper
    {
        private IInstaApi instaApi;

        public IEnumerable<User> GetFollowers(long userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetFollowersForLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public User GetLoggedInUser()
        {
            throw new NotImplementedException();
        }

        public User GetUser(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            instaApi = CreateApiHandler(userName, password);

            var loginResponse = await instaApi.LoginAsync();

            return loginResponse.Succeeded;
        }

        private IInstaApi CreateApiHandler(string userName, string password)
        {
            var loginCredentials = new UserSessionData
            {
                UserName = userName,
                Password = password
            };

            var instagramApi = InstaApiBuilder.CreateBuilder().SetUser(loginCredentials).Build();

            return instagramApi;
        }
    }
}
