using BookmarkItLibrary.Data.Contract;
using BookmarkItLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Data
{
    internal sealed class GetBookmarksDataManager : DataManagerBase, IGetBookmarksDataManager
    {
        public Task GetBookmarksAsync(GetBookmarksRequest request, ICallback<GetBookmarksResponse> callback = null)
        {
            return Task.CompletedTask;
        }
    }
}
