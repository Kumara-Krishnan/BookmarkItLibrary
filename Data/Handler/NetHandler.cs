using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.Net.Contract;
using UWPUtilities.Error;
using UWPUtilities.Extension;
using UWPUtilities.Util;
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

        public Task<string> GetRequestTokenAsync()
        {
            ThrowIfNetworkUnavailable();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.PocketRequestTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new HttpFormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(Constants.ConsumerKeyParamName,Constants.PocketConsumerKey),
                new KeyValuePair<string, string>(Constants.RedirectUriParamName,
                WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString())
            });
            return NetAdapter.SendAsync(requestMessage);
        }

        public Task<string> GetAccessTokenAsync(string requestToken)
        {
            ThrowIfNetworkUnavailable();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(Constants.PocketAccessTokenUrl));
            requestMessage.Headers.Add("X-Accept", "application/json");
            requestMessage.Content = new HttpFormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>(Constants.ConsumerKeyParamName,Constants.PocketConsumerKey),
                new KeyValuePair<string, string>("code", requestToken),
                new KeyValuePair<string, string>("account", "1")
            });

            return NetAdapter.SendAsync(requestMessage);
        }

        public Task<string> GetBookmarksAsync(string userName, long since = 0, string searchKey = default, int? count = default, int? offset = default,
            BookmarkFilter filter = BookmarkFilter.All, bool? isFavorite = default, string tag = default,
            BookmarkType bookmarkType = BookmarkType.Unknown, SortBy sortBy = SortBy.Newest,
            BookmarkDetailDepth detailDepth = BookmarkDetailDepth.Complete, string domain = default)
        {
            ThrowIfNetworkUnavailable();
            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("state", filter.ToStringLower()),
                new KeyValuePair<string, string>("sort", sortBy.ToStringLower()),
                new KeyValuePair<string, string>("detailType", detailDepth.ToStringLower()),
            };
            if (isFavorite != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("favorite", isFavorite.GetInt().ToString()));
            }
            if (!string.IsNullOrEmpty(tag))
            {
                requestParams.Add(new KeyValuePair<string, string>("tag", tag));
            }
            if (bookmarkType != BookmarkType.Unknown)
            {
                requestParams.Add(new KeyValuePair<string, string>("contentType", bookmarkType.ToStringLower()));
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                requestParams.Add(new KeyValuePair<string, string>("search", searchKey));
            }
            if (!string.IsNullOrEmpty(domain))
            {
                requestParams.Add(new KeyValuePair<string, string>("domain", domain));
            }
            if (since > 0)
            {
                requestParams.Add(new KeyValuePair<string, string>("since", since.ToString()));
            }
            if (count != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("count", count.ToString()));
            }
            if (offset != default)
            {
                requestParams.Add(new KeyValuePair<string, string>("offset", offset.ToString()));
            }

            AddAuthorizationParameters(userName, requestParams);
            return NetAdapter.PostAsync(GetAbsoluteRequestUrl("get"), requestParams);
        }

        private string GetAbsoluteRequestUrl(string method)
        {
            return Constants.PocketV3BaseUrl + method;
        }

        private void AddAuthorizationParameters(string userName, List<KeyValuePair<string, string>> requestParamters)
        {
            requestParamters.Add(GetAccessTokenParameter(userName));
            requestParamters.Add(GetConsumerKeyParamter());
        }

        private KeyValuePair<string, string> GetAccessTokenParameter(string userName)
        {
            return new KeyValuePair<string, string>("access_token", CredentialLocker.RetrievePassword(CredentialResourceKeys.AccessToken, userName));
        }

        private KeyValuePair<string, string> GetConsumerKeyParamter()
        {
            return new KeyValuePair<string, string>(Constants.ConsumerKeyParamName, Constants.PocketConsumerKey);
        }

        private void ThrowIfNetworkUnavailable()
        {
            if (!NetworkConnectivity.IsInternetAvailable)
            {
                throw new NoInternetAccessException();
            }
        }
    }
}
