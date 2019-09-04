using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InstagramApiSharp.API;
using InstagramDMSender.Models;

namespace InstagramDMSender.ApiWrapper
{
    public class ApiWrapper : IApiWrapper
    {
        private const string SESSION_FILE_NAME = "InstaSession.tmp";
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

        public async Task SaveLoginData()
        {
            var fileStream = File.Create(Path.GetFullPath(SESSION_FILE_NAME));
            var sessiondata = instaApi.GetStateDataAsString();
            var bytesToSave = UTF8Encoding.UTF8.GetBytes(sessiondata);
            await fileStream.WriteAsync(bytesToSave, 0, bytesToSave.Length);
            fileStream.Close();
        }

        public async Task<bool> LoadLoginData()
        {
            if (!File.Exists(Path.GetFullPath(SESSION_FILE_NAME)))
                return false;
            var fileStream = File.OpenText(Path.GetFullPath(SESSION_FILE_NAME));
            var sessionData = await fileStream.ReadToEndAsync();
            fileStream.Close();
            instaApi.LoadStateDataFromString(sessionData);
            if(instaApi.IsUserAuthenticated)
            {
                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await instaApi.LogoutAsync();
            DeleteSessionData();
        }

        private void DeleteSessionData()
        {
            File.Delete(Path.GetFullPath(SESSION_FILE_NAME));
        }

        public Task SendMessage(IEnumerable<long> recieversIds, string message, long senderId)
        {
            throw new NotImplementedException();
        }
    }
}
