using BookmarkItLibrary.DI;
using BookmarkItLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Domain
{
    internal interface IGetCurrentUserDetailsDataManager
    {
        void GetCurrentUserDetails(GetCurrentUserDetailsRequest request, ICallback<GetCurrentUserDetailsResponse> callback = default);
    }

    public sealed class GetCurrentUserDetailsRequest : UseCaseRequest
    {
        public GetCurrentUserDetailsRequest() : base(RequestType.LocalStorage)
        {
        }
    }

    public sealed class GetCurrentUserDetailsResponse : GetUserDetailsResponse
    {
        public GetCurrentUserDetailsResponse(UserDetails user) : base(user)
        {

        }
    }

    public interface IGetCurrentUserDetailsPresenterCallback : ICallback<GetCurrentUserDetailsResponse> { }

    public sealed class GetCurrentUserDetails : UseCaseBase<GetCurrentUserDetailsRequest, GetCurrentUserDetailsResponse>
    {
        private readonly IGetCurrentUserDetailsDataManager DataManager;

        public GetCurrentUserDetails(GetCurrentUserDetailsRequest request, IGetCurrentUserDetailsPresenterCallback callback) : base(request, callback)
        {
            DataManager = DIServiceProvider.Instance.GetService<IGetCurrentUserDetailsDataManager>();
        }

        public override void Action()
        {
            DataManager.GetCurrentUserDetails(Request, new UseCaseCallback(this));
        }

        private class UseCaseCallback : CallbackBase<GetCurrentUserDetailsResponse>
        {
            private readonly GetCurrentUserDetails UseCase;

            public UseCaseCallback(GetCurrentUserDetails useCase)
            {
                UseCase = useCase;
            }

            public override void OnError(UseCaseError error)
            {
                UseCase.PresenterCallback?.OnError(error);
            }

            public override void OnFailed(IUseCaseResponse<GetCurrentUserDetailsResponse> response)
            {
                UseCase.PresenterCallback?.OnFailed(response);
            }

            public override void OnSuccess(IUseCaseResponse<GetCurrentUserDetailsResponse> response)
            {
                UseCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
