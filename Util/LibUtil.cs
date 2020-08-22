using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace BookmarkItLibrary.Util
{
    public static class LibUtil
    {
        public static Uri GetSignInUrl(string requestToken, bool isWebAuthenticationBroker = true)
        {
            return new Uri(CommonConstants.PocketAuthorizationUrl + $"?{CommonConstants.ForceParamName}={CommonConstants.LoginParamValue}" +
                $"&{CommonConstants.WebAuthenticationBrokerParamName}={(isWebAuthenticationBroker ? 1 : 0)}&{CommonConstants.RequestTokenParamName}={requestToken}" +
                $"&{CommonConstants.RedirectUriParamName}={WebAuthenticationBroker.GetCurrentApplicationCallbackUri()}");
        }

        public static Uri GetSignUpUrl(string requestToken, bool isWebAuthenticationBroker = true)
        {
            return new Uri(CommonConstants.PocketAuthorizationUrl + $"?{CommonConstants.ForceParamName}={CommonConstants.SignUpParamValue}" +
                $"&{CommonConstants.WebAuthenticationBrokerParamName}={(isWebAuthenticationBroker ? 1 : 0)}&{CommonConstants.RequestTokenParamName}={requestToken}" +
                $"&{CommonConstants.RedirectUriParamName}={WebAuthenticationBroker.GetCurrentApplicationCallbackUri()}");
        }
    }
}
