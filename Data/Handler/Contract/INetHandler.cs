using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Data.Handler.Contract
{
    public interface INetHandler
    {
        Task<string> GetRequestTokenAsync();

        Task<string> GetAccessTokenAsync(string requestToken);
    }
}
