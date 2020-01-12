using BookmarkItLibrary.Util;
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

        Task<string> GetBookmarksAsync(string userName, long since = 0, string searchKey = default, int? count = default, int? offset = default,
            BookmarkFilter filter = BookmarkFilter.All, bool? isFavorite = default, string tag = default,
            BookmarkType bookmarkType = BookmarkType.Unknown, SortBy sortBy = SortBy.Newest,
            BookmarkDetailDepth detailDepth = BookmarkDetailDepth.Complete, string domain = default);
    }
}
