﻿using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.Model;
using BookmarkItLibrary.Model.Entity;
using BookmarkItLibrary.Util;
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
                typeof(DownloadsMapper),
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
                        count += DBAdapter.InsertOrReplaceAll<Image>(bookmarksResponse.Images);
                    }
                    if (bookmarksResponse.Videos.IsNonEmpty())
                    {
                        count += DBAdapter.InsertOrReplaceAll<Video>(bookmarksResponse.Videos);
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
                        count += DBAdapter.InsertOrReplaceAll<DomainMetaData>(bookmarksResponse.Domains);
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

        public IEnumerable<BookmarkBObj> GetBookmarks(string userId, BookmarkFilter filter, bool? isFavorite, string tag, BookmarkType bookmarkType,
            SortBy sortBy, int? count, int? offset)
        {
            var filterClause = string.Empty;
            var filterQueryParam = string.Empty;
            switch (filter)
            {
                case BookmarkFilter.Unread:
                    filterClause = $@" AND {nameof(Bookmark.TimeMarkedAsRead)} = ? ";
                    filterQueryParam = "0";
                    break;
                case BookmarkFilter.Archived:
                    filterClause = $@" AND {nameof(Bookmark.Status)} = ? ";
                    filterQueryParam = ((int)filter).ToString();
                    break;
            }

            var favoriteClause = isFavorite == default ? string.Empty : $@" AND {nameof(Bookmark.IsFavorite)} = ? ";
            var favoriteQueryParam = isFavorite == default ? string.Empty : isFavorite.GetInt().ToString();

            var bookmarkTypeClause = string.Empty;
            var bookmarkTypeQueryParam = string.Empty;
            switch (bookmarkType)
            {
                case BookmarkType.Article:
                case BookmarkType.Image:
                case BookmarkType.Video:
                    bookmarkTypeClause = $@" AND {nameof(Bookmark.Type)} = ? ";
                    bookmarkTypeQueryParam = ((int)bookmarkType).ToString();
                    break;
            }

            var orderByClause = string.Empty;
            switch (sortBy)
            {
                case SortBy.Newest:
                    orderByClause = $@" ORDER BY {nameof(Bookmark.CreatedTime)} DESC ";
                    break;
                case SortBy.Oldest:
                    orderByClause = $@" ORDER BY {nameof(Bookmark.CreatedTime)} ";
                    break;
                case SortBy.Title:
                    orderByClause = $@" ORDER BY {nameof(Bookmark.Title)}, {nameof(Bookmark.ResolvedTitle)} ";
                    break;
                case SortBy.Site:
                    orderByClause = $@" ORDER BY {nameof(Bookmark.Url)}, {nameof(Bookmark.ResolvedUrl)} ";
                    break;
            }

            var limitClause = count == default ? string.Empty : " LIMIT ? ";
            var limitQueryParam = count?.ToString() ?? string.Empty;

            var offsetClause = offset == default ? string.Empty : " OFFSET ? ";
            var offsetQueryParam = offset?.ToString() ?? string.Empty;

            var query = $@"SELECT * FROM {nameof(Bookmark)} 
                           WHERE {nameof(Bookmark.UserId)} = ? 
                           {filterClause}
                           {favoriteClause}
                           {bookmarkTypeClause}
                           {orderByClause}
                           {limitClause}
                           {offsetClause}";

            var queryParams = new List<string>() { userId };
            if (!string.IsNullOrEmpty(filterQueryParam)) { queryParams.Add(filterQueryParam); }
            if (!string.IsNullOrEmpty(favoriteQueryParam)) { queryParams.Add(favoriteQueryParam); }
            if (!string.IsNullOrEmpty(bookmarkTypeQueryParam)) { queryParams.Add(bookmarkTypeQueryParam); }
            if (!string.IsNullOrEmpty(limitQueryParam)) { queryParams.Add(limitQueryParam); }
            if (!string.IsNullOrEmpty(offsetQueryParam)) { queryParams.Add(offsetQueryParam); }

            var bookmarks = DBAdapter.Query<BookmarkBObj>(query, queryParams.ToArray());
            if (bookmarks.IsNullOrEmpty()) { return bookmarks; }

            var bookmarkIds = bookmarks.Select(b => b.EntityId).ToArray();

            var images = GetImagesByBookmarkIds(bookmarkIds);
            var videos = GetVideosByBookmarkIds(bookmarkIds);

            //TODO: Populate Tags, Domain and Authors.
            foreach (var bookmark in bookmarks)
            {
                var bookmarkImages = images.Where(img => img.BookmarkId == bookmark.EntityId);
                bookmark.SetImages(bookmarkImages);

                var bookmarkVideos = videos.Where(vid => vid.BookmarkId == bookmark.EntityId);
                bookmark.SetVideos(bookmarkVideos);
            }
            return bookmarks;
        }

        public IEnumerable<ImageBObj> GetImagesByBookmarkIds(params string[] bookmarkIds)
        {
            var query = $@"SELECT * FROM {nameof(Image)} 
                           WHERE {nameof(Image.BookmarkId)} 
                           IN ({DBAdapter.GetQueryParamPlaceholders(bookmarkIds.Length)})";
            return DBAdapter.Query<ImageBObj>(query, bookmarkIds);
        }

        public IEnumerable<VideoBObj> GetVideosByBookmarkIds(params string[] bookmarkIds)
        {
            var query = $@"SELECT * FROM {nameof(Video)} 
                           WHERE {nameof(Video.BookmarkId)} 
                           IN ({DBAdapter.GetQueryParamPlaceholders(bookmarkIds.Length)})";
            return DBAdapter.Query<VideoBObj>(query, bookmarkIds);
        }
    }
}
