using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Util
{
    public static class Constants
    {
        public const string PocketConsumerKey = "89128-4d07a487ef25fea6e30be6e2";
        public const string ConsumerKeyParamName = "consumer_key";
        public const string RedirectUriParamName = "redirect_uri";
        public const string RequestTokenParamName = "request_token";
        public const string PocketBaseUrl = "https://getpocket.com";
        public const string PocketV3BaseUrl = PocketBaseUrl + "/v3";
        public const string PocketRequestTokenUrl = PocketV3BaseUrl + "/oauth/request";
        public const string PocketAuthorizationUrl = PocketBaseUrl + "/auth/authorize";
        public const string PocketAccessTokenUrl = PocketV3BaseUrl + "/oauth/authorize";
    }
}
