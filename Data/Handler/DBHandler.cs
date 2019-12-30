using BookmarkItLibrary.Data.Handler.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.DB.Contract;
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
            //DBAdapter.CreateTables(types: new Type[]
            //{

            //});
        }
    }
}
