using BookmarkItLibrary.Data.Contract;
using BookmarkItLibrary.DI;
using BookmarkItLibrary.Domain;
using BookmarkItLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Error;
using UWPUtilities.Extension;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Data
{
    internal sealed class GetCurrentUserDetailsDataManager : DataManagerBase, IGetCurrentUserDetailsDataManager
    {
        public void GetCurrentUserDetails(GetCurrentUserDetailsRequest request, ICallback<GetCurrentUserDetailsResponse> callback)
        {
            DBHandler.Initialize();
            var settingsDM = DIServiceProvider.Instance.GetService<ISettingsDataManager<Model.Entity.BookmarkItSettings>>();
            var currentUserId = settingsDM.GetSetting(Constants.AppSetting, SettingKeys.CurrentUser).OptString();
            if (string.IsNullOrEmpty(currentUserId))
            {
                callback?.OnFailed(ResponseType.LocalStorage, ResponseStatus.Failed);
            }
            else
            {
                var currentUser = DBHandler.GetUserDetails(currentUserId);
                callback?.OnSuccessOrFailed(ResponseType.LocalStorage, new GetCurrentUserDetailsResponse(currentUser), IsValidResponse);
            }
        }

        private bool IsValidResponse(GetCurrentUserDetailsResponse response)
        {
            return response.User != default;
        }
    }
}
