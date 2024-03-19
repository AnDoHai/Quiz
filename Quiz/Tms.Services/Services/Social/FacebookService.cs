using Facebook;
using Tms.Services.DataContract;

namespace Tms.Services
{
    public interface IFacebookService
    {
        FacebookAccountInfos RetrieveAccount(string path, string accessToken);
    }

    public class FacebookService : IFacebookService
    {
        public FacebookAccountInfos RetrieveAccount(string path, string accessToken)
        {
            var client = new FacebookClient(accessToken);
            var fbAccount = client.Get<FacebookAccountInfos>(path);
            return fbAccount;
        }

    }
}
