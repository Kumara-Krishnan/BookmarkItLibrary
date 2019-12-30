using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model.Contract
{
    public interface IBookmark
    {
        string Id { get; set; }

        string Url { get; set; }

        string Title { get; set; }

        string Summary { get; set; }
    }
}
