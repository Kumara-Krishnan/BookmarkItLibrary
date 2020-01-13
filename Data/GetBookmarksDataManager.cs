using BookmarkItLibrary.Data.Contract;
using BookmarkItLibrary.Data.Parser;
using BookmarkItLibrary.Domain;
using BookmarkItLibrary.Model;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Extension;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Data
{
    internal sealed class GetBookmarksDataManager : DataManagerBase, IGetBookmarksDataManager
    {
        public async Task GetBookmarksAsync(GetBookmarksRequest request, ICallback<GetBookmarksResponse> callback = null)
        {
            if (request.Type.HasLocalStorage())
            {
                var bookmarksFromDB = GetBookmarksFromDB(request);
                callback.OnSuccessOrFailed(ResponseType.LocalStorage, new GetBookmarksResponse(bookmarksFromDB), IsValidResponse);
            }

            if (request.Type.HasNetwork())
            {
                var bookmarksFromServer = await FetchBookmarksFromServerAsync(request).ConfigureAwait(false);
                callback.OnSuccessOrFailed(ResponseType.Network, new GetBookmarksResponse(bookmarksFromServer), IsValidResponse);
            }
        }

        private IEnumerable<BookmarkBObj> GetBookmarksFromDB(GetBookmarksRequest request)
        {
            return DBHandler.GetBookmarks(request.UserId, request.Filter, request.IsFavorite, request.Tag, request.BookmarkType, request.SortBy,
               request.Count, request.Offset);
        }

        private async Task<IEnumerable<BookmarkBObj>> FetchBookmarksFromServerAsync(GetBookmarksRequest request)
        {
            var response = await NetHandler.GetBookmarksAsync(request.UserId, request.Since, request.SearchKey,
                request.Count, request.Offset, request.Filter, request.IsFavorite, request.Tag, request.BookmarkType,
                request.SortBy, domain: request.Domain).ConfigureAwait(false);
            var parsedBookmarksResponse = ResponseDataParser.ParseBookmarks(request.UserId, response);
            DBHandler.AddOrReplaceParsedBookmarksResponse(parsedBookmarksResponse);

            return parsedBookmarksResponse.Bookmarks.Where(b => b.Status != BookmarkStatus.Deleted);
        }

        private bool IsValidResponse(GetBookmarksResponse response)
        {
            return response.Bookmarks.IsNonEmpty();
        }
    }
}
