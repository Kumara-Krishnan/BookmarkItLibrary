using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model.Contract
{
    public interface IAttachment
    {
        string Id { get; set; }

        string BookmarkId { get; set; }

        string Url { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        string LocalFileName { get; set; }
    }
}
