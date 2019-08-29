using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramDMSender.Models;

namespace InstagramDMSender.ApiWrapper
{
    public class ApiWrapper : IApiWrapper
    {
        private IInstaApi instaApi;
        private IMapper mapper;

        public ApiWrapper(IInstaApi instaApi, IMapper mapper)
        {
            this.instaApi = instaApi;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetFollowersAsync(long userId)
        {
            var followers = await instaApi.UserProcessor.GetUserFollowersByIdAsync(userId, InstagramApiSharp.PaginationParameters.Empty);
            if (followers.Succeeded)
                return followers.Value.Select(a => new User
                {
                    UserId = a.Pk,
                    UserName = a.UserName,
                    Private = a.IsPrivate
                });
            return new List<User>();
        }

        public async Task<IEnumerable<User>> GetFollowersForLoggedInUserAsync()
        {
            var followers = await instaApi.UserProcessor.GetCurrentUserFollowersAsync(InstagramApiSharp.PaginationParameters.Empty);
            if (followers.Succeeded)
                return followers.Value.Select(a => mapper.Map<User>(a));
            return new List<User>();
        }

        public async Task<User> GetLoggedInUserAsync()
        {
            var currentUser = await instaApi.GetCurrentUserAsync();

            return mapper.Map<User>(currentUser.Value);
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            var user = await instaApi.UserProcessor.GetUserInfoByIdAsync(userId);

            return mapper.Map<User>(user.Value);
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            var user = await instaApi.UserProcessor.GetUserAsync(userName);

            return new User
            {
                UserName = user.Value.UserName,
                UserId = user.Value.Pk,
                Private = user.Value.IsPrivate
            };
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            instaApi.SetUser(userName, password);

            var loginResponse = await instaApi.LoginAsync();

            return loginResponse.Succeeded;
        }
    }
}
