using BookmarkItLibrary.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Extension;

namespace BookmarkItLibrary.Data.Parser
{
    public static class ResponseDataParser
    {
        public static string ParseRequestToken(string reqTokenResponse)
        {
            var jReqTokenResponse = JObject.Parse(reqTokenResponse);
            return jReqTokenResponse.GetString("code");
        }

        public static ParsedUserDetails ParseUserDetails(string userDetailsResponse)
        {
            var jUserDetails = JObject.Parse(userDetailsResponse);
            var userName = jUserDetails.GetString("username");
            var accessToken = jUserDetails.GetString("access_token");
            return new ParsedUserDetails(userName, accessToken);
        }
    }
}
