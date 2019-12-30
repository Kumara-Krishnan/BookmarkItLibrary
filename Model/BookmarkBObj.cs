using BookmarkItLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model
{
    public sealed class BookmarkBObj : Bookmark
    {
        public ObservableCollection<string> Tags = new ObservableCollection<string>();

        public readonly List<Image> Images = new List<Image>();

        public readonly List<Video> Videos = new List<Video>();

        public readonly List<string> Authors = new List<string>();
    }
}
