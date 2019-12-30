using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model
{
    public sealed class ParsedUserDetails
    {
        public string UserName { get; set; }

        public string AccessToken { get; set; }

        public ParsedUserDetails(string userName, string accessToken)
        {
            UserName = userName;
            AccessToken = accessToken;
        }
    }
}
