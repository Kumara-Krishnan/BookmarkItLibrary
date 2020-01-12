using BookmarkItLibrary.Model;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Domain
{
    internal interface IGetBookmarksDataManager
    {
        Task GetBookmarksAsync(GetBookmarksRequest request, ICallback<GetBookmarksResponse> callback = default);
    }

    public class GetBookmarksRequest : AuthenticatedUseCaseRequest
    {
        public BookmarkFilter Filter { get; set; } = BookmarkFilter.All;
        public bool? IsFavorite { get; set; }
        public string Tag { get; set; }
        public BookmarkType BookmarkType { get; set; }
        public SortBy SortBy { get; set; }
        public string SearchKey { get; set; }
        public string Domain { get; set; }
        public DateTime Since { get; set; }
        public int? Count { get; set; }
        public int? Offset { get; set; }

        public GetBookmarksRequest(RequestType type, string userName, CancellationTokenSource cts = default) : base(type, userName, cts) { }

        public GetBookmarksRequest(RequestType type, DateTime since, string userName, CancellationTokenSource cts = default) : this(type, userName, cts)
        {
            Since = since;
        }
    }

    public class GetBookmarksResponse
    {
        public readonly List<BookmarkBObj> Bookmarks = new List<BookmarkBObj>();

        public GetBookmarksResponse(IEnumerable<BookmarkBObj> bookmarks)
        {
            Bookmarks.AddRange(bookmarks);
        }
    }

    public interface IGetBookmarksPresenterCallback : ICallback<GetBookmarksResponse> { }

    public sealed class GetBookmarks
    {

    }
}
