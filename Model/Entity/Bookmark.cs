using BookmarkItLibrary.Model.Contract;
using BookmarkItLibrary.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model.Entity
{
    public class Bookmark : IBookmark
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string ResolvedId { get; set; }

        public string Url { get; set; }

        public string ResolvedUrl { get; set; }

        public string Title { get; set; }

        public string ResolvedTitle { get; set; }

        public bool IsFavorite { get; set; }

        public string Summary { get; set; }

        public BookmarkType Type { get; set; }

        public BookmarkStatus Status { get; set; }

        public bool HasImage { get; set; }

        public bool HasVideo { get; set; }

        public long WordCount { get; set; }
    }
}
