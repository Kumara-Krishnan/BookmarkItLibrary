using BookmarkItLibrary.Data.Contract;
using BookmarkItLibrary.Data.Parser;
using BookmarkItLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Data
{
    internal sealed class GetRequestTokenDataManager : DataManagerBase, IGetRequestTokenDataManager
    {
        public async Task GetRequestTokenAsync(GetRequestTokenRequest request, ICallback<GetRequestTokenResponse> callback)
        {
            var requestTokenResponse = await NetHandler.GetRequestTokenAsync().ConfigureAwait(false);
            var requestToken = ResponseDataParser.ParseRequestToken(requestTokenResponse);
            callback.OnSuccess(ResponseType.Network, ResponseStatus.Success, new GetRequestTokenResponse(requestToken));
        }
    }
}
