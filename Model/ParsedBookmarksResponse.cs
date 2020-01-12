using BookmarkItLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model
{
    public sealed class ParsedBookmarksResponse
    {
        public readonly List<BookmarkBObj> Bookmarks = new List<BookmarkBObj>();

        public readonly List<Tag> Tags = new List<Tag>();

        public readonly List<Image> Images = new List<Image>();

        public readonly List<Video> Videos = new List<Video>();

        public readonly List<Author> Authors = new List<Author>();

        public readonly List<BookmarkAuthorMapper> AuthorMapper = new List<BookmarkAuthorMapper>();

        public readonly List<DomainMetaData> Domains = new List<DomainMetaData>();

        public readonly List<BookmarkDomainMapper> DomainMapper = new List<BookmarkDomainMapper>();

        public readonly string UserId;

        public int Status { get; set; }

        public string Error { get; set; }

        public string SearchType { get; set; }

        public long Since { get; set; }

        public ParsedBookmarksResponse(string userId)
        {
            UserId = userId;
        }

        public void Add(BookmarkBObj bookmark)
        {
            Bookmarks.Add(bookmark);
            Images.AddRange(bookmark.Images);
            Videos.AddRange(bookmark.Videos);
            Tags.AddRange(bookmark.Tags);
            AddAuthors(bookmark.EntityId, bookmark.Authors);
            AddDomain(bookmark.EntityId, bookmark.Domain);
        }

        private void AddAuthors(string bookmarkId, List<Author> bookmarkAuthors)
        {
            foreach (var author in bookmarkAuthors)
            {
                Authors.Add(author);
                AuthorMapper.Add(new BookmarkAuthorMapper(bookmarkId, author.Id));
            }
        }

        private void AddDomain(string bookmarkId, DomainMetaData domain)
        {
            if (domain != null)
            {
                Domains.Add(domain);
                DomainMapper.Add(new BookmarkDomainMapper(bookmarkId, domain.Id));
            }
        }
    }
}
