using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.Model;
using BookmarkItLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.DB.Contract;
using UWPUtilities.Extension;
using UWPUtilities.Model;
using Windows.ApplicationModel;

namespace BookmarkItLibrary.Data.Handler
{
    public sealed class DBHandler : IDBHandler
    {
        private readonly IDBAdapter DBAdapter;

        public DBHandler(IDBAdapter dbAdapter)
        {
            DBAdapter = dbAdapter;
        }

        public void Initialize()
        {
            DBAdapter.Initialize($"{Package.Current.DisplayName}.db");
            DBAdapter.CreateTables(types: new Type[]
            {
                typeof(Author),
                typeof(Bookmark),
                typeof(BookmarkAuthorMapper),
                typeof(BookmarkDomainMapper),
                typeof(BookmarkItSettings),
                typeof(DomainMetaData),
                typeof(Image),
                typeof(Tag),
                typeof(UserDetails),
                typeof(Video)
            });
        }

        public UserDetails GetUserDetails(string userId)
        {
            return DBAdapter.Find<UserDetails>(userId);
        }

        public int InsertUserDetails(UserDetails user)
        {
            return DBAdapter.InsertOrReplace<UserDetails>(user);
        }

        public int UpdateLastSyncedTime(string userId, long lastSyncedTime)
        {
            var query = $@"UPDATE {nameof(UserDetails)} 
                           SET {nameof(UserDetails.LastSyncedTime)} = ? 
                           WHERE {nameof(UserDetails.Id)} = ? ";
            return DBAdapter.Execute(query, lastSyncedTime, userId);
        }

        public T GetSetting<T>(string userId, string key) where T : SettingsBase, new()
        {
            return DBAdapter.Find<T>($"{userId}_{key}");
        }

        public int SetSetting<T>(T setting) where T : SettingsBase, new()
        {
            return DBAdapter.InsertOrReplace<T>(setting);
        }

        public int AddOrReplaceParsedBookmarksResponse(ParsedBookmarksResponse bookmarksResponse)
        {
            int count = 0;
            try
            {
                DBAdapter.RunInTransaction(() =>
                {
                    if (bookmarksResponse.Since > 0)
                    {
                        count += UpdateLastSyncedTime(bookmarksResponse.UserId, bookmarksResponse.Since);
                    }
                    if (bookmarksResponse.Bookmarks.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll<Bookmark>(bookmarksResponse.Bookmarks);
                    }
                    if (bookmarksResponse.Images.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.Images);
                    }
                    if (bookmarksResponse.Videos.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.Videos);
                    }
                    if (bookmarksResponse.Tags.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.Tags);
                    }
                    if (bookmarksResponse.Authors.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.Authors);
                    }
                    if (bookmarksResponse.AuthorMapper.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.AuthorMapper);
                    }
                    if (bookmarksResponse.Domains.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.Domains);
                    }
                    if (bookmarksResponse.DomainMapper.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll(bookmarksResponse.DomainMapper);
                    }
                }, reThrow: true);
            }
            catch
            {
                count = 0;
            }
            return count;
        }
    }
}
