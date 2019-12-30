using BookmarkItLibrary.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookmarkItLibrary.Model.Entity
{
    public class Video : IAttachment
    {
        public string Id { get; set; }

        public string BookmarkId { get; set; }

        public string Url { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string LocalFileName { get; set; }

        public string VId { get; set; }
    }
}
