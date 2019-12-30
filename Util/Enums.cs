using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Util
{
    public enum BookmarkType
    {
        Unknown,
        Article,
        Image,
        Video
    }

    public enum BookmarkStatus
    {
        Available,
        Archived,
        Deleted
    }
}
