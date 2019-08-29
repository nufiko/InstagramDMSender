using AutoMapper;
using InstagramApiSharp;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.ApiWrapperTest
{
    class ApiWrapperFollowersTest
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
        public async Task GetFollowers_Returns_List_Of_Followers()
        {
            var returnedFollowers = new Result<InstaUserShortList>(true, new InstaUserShortList());
            var expectedFollowers = new List<User>();
            var userProcessor = new Mock<IUserProcessor>();

            userProcessor.Setup(p => p.GetUserFollowersByIdAsync(userId, It.IsAny<PaginationParameters>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(returnedFollowers);
            instaApi.Setup(a => a.UserProcessor).Returns(userProcessor.Object);

            var result = await subject.GetFollowersAsync(userId);

            Assert.AreEqual(expectedFollowers, result);
        }
    }
}
