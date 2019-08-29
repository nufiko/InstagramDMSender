using InstagramApiSharp.API;

namespace InstagramDMSender.ApiWrapper
{
    public interface IApiWrapperBuilder
    {
        IApiWrapper CreateApi();
    }
}