using AutoMapper;
using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;

namespace InstagramDMSender.ApiWrapper
{
    public class ApiWrapperBuilder : IApiWrapperBuilder
    {
        private IInstaApiBuilder instaApiBuilder;
        private IMapper mapper;

        public ApiWrapperBuilder(IInstaApiBuilder instaApiBuilder, IMapper mapper)
        {
            this.instaApiBuilder = instaApiBuilder;
            this.mapper = mapper;
        }

        public IApiWrapper CreateApi()
        {
            var instagramApi = instaApiBuilder.Build();

            return new ApiWrapper(instagramApi, mapper);
        }
    }
}
