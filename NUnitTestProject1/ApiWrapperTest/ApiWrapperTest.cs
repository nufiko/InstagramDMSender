using AutoMapper;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests.ApiWrapperTest
{
    public class ApiWrapperTest
    {
        private string userName = "userName";
        private string password = "password";
        private long userId = 1111111111111111;

        private Mock<IInstaApi> instaApi;
        private Mock<IMapper> mapper;

        private IApiWrapper subject;

        [SetUp]
        public void Setup()
        {
            instaApi = new Mock<IInstaApi>();
            mapper = new Mock<IMapper>();

            subject = new ApiWrapper(instaApi.Object, mapper.Object);
        }

        [Test]
        public async Task Login_Returns_True_If_Succesfull()
        {
            instaApi.Setup(a => a.LoginAsync(true)).ReturnsAsync(Result.Success(InstaLoginResult.Success));

            var result = await subject.LoginAsync(userName, password);
            Assert.Multiple(() =>
            {
                instaApi.Verify(x => x.SetUser(userName, password));
                Assert.IsTrue(result);
            });
        }

        [Test]
        public async Task Login_Returns_False_If_Unsuccesfull()
        {
            instaApi.Setup(a => a.LoginAsync(true)).ReturnsAsync(Result.Fail("some text", InstaLoginResult.Exception));

            var result = await subject.LoginAsync(userName, password);
            Assert.Multiple(() =>
            {
                instaApi.Verify(x => x.SetUser(userName, password));
                Assert.IsFalse(result);
                
            });
        }

        [Test]
        public async Task GetLoogedInUserAsync_Returns_Valid_User()
        {
            var currentUser = CreateReturnResultCurrentUser();

            instaApi.Setup(x => x.GetCurrentUserAsync()).ReturnsAsync(currentUser);
            mapper.Setup(m => m.Map<User>(currentUser.Value)).Returns(new User
                {
                    UserId = userId,
                    UserName = userName,
                    Private = false
                }
            );

            var result = await subject.GetLoggedInUserAsync();
            AssertUserAreExpected(result);
        }

        [Test]
        public async Task GetUserByIdAsync_Returns_Valid_User()
        {
            var user = CreateReturnResultUserInfo();
            var userProcessor = new Mock<IUserProcessor>();

            userProcessor.Setup(p => p.GetUserInfoByIdAsync(userId)).ReturnsAsync(user);
            instaApi.Setup(x => x.UserProcessor).Returns(userProcessor.Object);
            mapper.Setup(m => m.Map<User>(user.Value)).Returns(new User
                {
                    UserId = userId,
                    UserName = userName,
                    Private = false
                }
            );

            var result = await subject.GetUserByIdAsync(userId);

            AssertUserAreExpected(result);
        }

        [Test]
        public async Task GetUserByNameAsync_Returns_Valid_User()
        {
            var user = CreateReturnResultInstaUser();
            var userProcessor = new Mock<IUserProcessor>();

            userProcessor.Setup(p => p.GetUserAsync(userName)).ReturnsAsync(user);
            instaApi.Setup(x => x.UserProcessor).Returns(userProcessor.Object);

            var result = await subject.GetUserByNameAsync(userName);

            AssertUserAreExpected(result);
        }

        private Result<InstaCurrentUser> CreateReturnResultCurrentUser()
        {
            var instaUser = new InstaUserShort
            {
                UserName = userName,
                Pk = userId,
                IsPrivate = false
            };
            var returnUser = new InstaCurrentUser(instaUser);
            return new Result<InstaCurrentUser>(true, returnUser);            
        }

        private Result<InstaUserInfo> CreateReturnResultUserInfo()
        {
            var returnUser = new InstaUserInfo
            {
                Username = userName,
                Pk = userId,
                IsPrivate = false
            };
            return new Result<InstaUserInfo>(true, returnUser);
        }

        private Result<InstaUser> CreateReturnResultInstaUser()
        {
            var instaUserShort = new InstaUserShort
            {
                UserName = userName,
                Pk = userId,
                IsPrivate = false
            };
            var instaUser = new InstaUser(instaUserShort);
            return new Result<InstaUser>(true, instaUser);
        }

        private void AssertUserAreExpected(User user)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(userName, user.UserName);
                Assert.AreEqual(userId, user.UserId);
                Assert.AreEqual(false, user.Private);
            });
        }
    }
}