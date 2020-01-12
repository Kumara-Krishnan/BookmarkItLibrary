using BookmarkItLibrary.Data.Contract;
using BookmarkItLibrary.Data.Parser;
using BookmarkItLibrary.Domain;
using BookmarkItLibrary.Model;
using BookmarkItLibrary.Model.Entity;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.UseCase;
using UWPUtilities.Util;

namespace BookmarkItLibrary.Data
{
    internal class GetUserDetailsDataManager : DataManagerBase, IGetUserDetailsDataManager
    {
        public async Task GetUserDetailsAsync(GetUserDetailsRequest request, ICallback<GetUserDetailsResponse> callback)
        {
            UserDetails userDetailsFromDB = default;
            if (request.Type.HasLocalStorage())
            {
                userDetailsFromDB = GetUserDetailsFromDB(request);
                callback.OnSuccessOrFailed(ResponseType.LocalStorage, new GetUserDetailsResponse(userDetailsFromDB), IsValidUserDetailsResponse);
            }

            if (request.Type.HasNetwork())
            {
                var parsedUserDetails = await FetchUserDetailsFromServerAsync(request, userDetailsFromDB).ConfigureAwait(false);
                callback.OnSuccessOrFailed(ResponseType.Network, new GetUserDetailsResponse(parsedUserDetails?.User), IsValidUserDetailsResponse);
            }
        }

        public UserDetails GetUserDetailsFromDB(GetUserDetailsRequest request)
        {
            var user = DBHandler.GetUserDetails(request.UserId);
            if (request.SetAsCurrentUser) { SetAsCurrentUser(user); }
            return user;
        }

        public async Task<ParsedUserDetails> FetchUserDetailsFromServerAsync(GetUserDetailsRequest request, UserDetails userDetailsFromDB = default)
        {
            var userResponse = await NetHandler.GetAccessTokenAsync(request.RequestToken);
            var userDetailsFromServer = ResponseDataParser.ParseUserDetails(userResponse);
            if (userDetailsFromDB == default) { userDetailsFromDB = GetUserDetailsFromDB(request); }
            if (userDetailsFromDB != default)
            {
                userDetailsFromServer.User.LastSyncedTime = userDetailsFromDB.LastSyncedTime;
            }
            DBHandler.InsertUserDetails(userDetailsFromServer.User);
            CredentialLocker.StoreCredential(CredentialResourceKeys.AccessToken, userDetailsFromServer.User.Id, userDetailsFromServer.AccessToken);
            if (request.SetAsCurrentUser) { SetAsCurrentUser(userDetailsFromServer.User); }

            return userDetailsFromServer;
        }

        private void SetAsCurrentUser(UserDetails user)
        {
            if (user == default) { return; }
            DBHandler.SetSetting(new BookmarkItSettings(Constants.AppSetting, SettingKeys.CurrentUser, user.UserName));
        }

        private bool IsValidUserDetailsResponse(GetUserDetailsResponse response)
        {
            return response.User != default;
        }
    }
}
