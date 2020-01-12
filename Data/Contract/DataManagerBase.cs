using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Model;

namespace BookmarkItLibrary.Data.Contract
{
    internal abstract class DataManagerBase
    {
        protected readonly IDBHandler DBHandler;
        protected readonly INetHandler NetHandler;

        protected DataManagerBase()
        {
            DBHandler = DIServiceProvider.Instance.GetService<IDBHandler>();
            NetHandler = DIServiceProvider.Instance.GetService<INetHandler>();
        }
    }
}
