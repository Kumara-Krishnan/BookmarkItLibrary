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
            return new Uri(Constants.PocketAuthorizationUrl + $"?{Constants.ForceParamName}={Constants.LoginParamValue}" +
                $"&{Constants.WebAuthenticationBrokerParamName}={(isWebAuthenticationBroker ? 1 : 0)}&{Constants.RequestTokenParamName}={requestToken}" +
                $"&{Constants.RedirectUriParamName}={WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString()}");
        }

        public static Uri GetSignUpUrl(string requestToken, bool isWebAuthenticationBroker = true)
        {
            return new Uri(Constants.PocketAuthorizationUrl + $"?{Constants.ForceParamName}={Constants.SignUpParamValue}" +
                $"&{Constants.WebAuthenticationBrokerParamName}={(isWebAuthenticationBroker ? 1 : 0)}&{Constants.RequestTokenParamName}={requestToken}" +
                $"&{Constants.RedirectUriParamName}={WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString()}");
        }
    }
}
