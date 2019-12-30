using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.Net.Contract;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;

namespace BookmarkItLibrary.Data.Handler
{
    public sealed class NetHandler : INetHandler
    {
        private readonly INetAdapter NetAdapter;

        public NetHandler(INetAdapter netAdapter)
        {
            NetAdapter = netAdapter;
        }

        public async Task<string> GetRequestTokenAsync()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.PocketRequestTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new HttpFormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(Constants.ConsumerKeyParamName,Constants.PocketConsumerKey),
                new KeyValuePair<string, string>(Constants.RedirectUriParamName,
                WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString())
            });
            return await NetAdapter.SendAsync(requestMessage);
        }

        public async Task<string> GetAccessTokenAsync(string requestToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.PocketAccessTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new HttpFormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>(Constants.ConsumerKeyParamName,Constants.PocketConsumerKey),
                new KeyValuePair<string, string>("code",requestToken)
            });
            return await NetAdapter.SendAsync(requestMessage);
        }
    }
}
