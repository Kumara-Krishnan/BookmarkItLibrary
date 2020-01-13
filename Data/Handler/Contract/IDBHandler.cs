using BookmarkItLibrary.Model;
using BookmarkItLibrary.Model.Entity;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Model;

namespace BookmarkItLibrary.Data.Handler.Contract
{
    public interface IDBHandler
    {
        void Initialize();

        UserDetails GetUserDetails(string userId);

        int InsertUserDetails(UserDetails user);

        T GetSetting<T>(string userId, string key) where T : SettingsBase, new();

        int SetSetting<T>(T setting) where T : SettingsBase, new();

        int AddOrReplaceParsedBookmarksResponse(ParsedBookmarksResponse bookmarksResponse);

        IEnumerable<BookmarkBObj> GetBookmarks(string userId, BookmarkFilter filter, bool? isFavorite, string tag, BookmarkType bookmarkType,
            SortBy sortBy, int? count, int? offset);

        IEnumerable<ImageBObj> GetImagesByBookmarkIds(params string[] bookmarkIds);

        IEnumerable<VideoBObj> GetVideosByBookmarkIds(params string[] bookmarkIds);
    }
}
