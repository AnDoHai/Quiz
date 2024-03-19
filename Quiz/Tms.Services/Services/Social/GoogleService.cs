using Tms.Services.DataContract;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceSystem.Core.Restful;

namespace Tms.Services
{
    public interface IGoogleService
    {
        Task<GoogleAccountInfos> RetrieveAccountInfo(string endpoint, string accessToken);
    }
    public class GoogleService : RestProviderBase, IGoogleService
    {
        public async Task<GoogleAccountInfos> RetrieveAccountInfo(string endpoint, string accessToken)
        {
            var dicParams = new Dictionary<string, string>();
            dicParams.Add("access_token", accessToken);
            var response =  await Task.Run(() => CreateInstance(endpoint).GetMany<GoogleAccountInfos>(null, dicParams)).ConfigureAwait(false);
            return response.Payload != null ? response.Payload : null;
        }
    }
}
