using AutoMapper;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramDMSender.ApiWrapper;
using Moq;
using NUnit.Framework;

namespace InstagramDMSenderTest.ApiWrapper
{
    class ApiWrapperBuilderTest
    {
        private Mock<IInstaApiBuilder> instaApiBuilder;
        private Mock<IInstaApi> instaApi;
        private Mock<IMapper> mapper;

        private IApiWrapperBuilder subject;

        [SetUp]
        public void Setup()
        {
            instaApi = new Mock<IInstaApi>();
            instaApiBuilder = new Mock<IInstaApiBuilder>();
            mapper = new Mock<IMapper>();

            instaApiBuilder.Setup(b => b.Build()).Returns(instaApi.Object);

            subject = new ApiWrapperBuilder(instaApiBuilder.Object, mapper.Object);
        }

        [Test]
        public void ApiWrapperBuilder_CreateApi_returns_Instance_Of_ApiWrapper()
        {
            var result = subject.CreateApi();

            Assert.IsInstanceOf<IApiWrapper>(result);
        }
    }
}
