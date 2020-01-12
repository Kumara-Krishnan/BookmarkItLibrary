using BookmarkItLibrary.DI;
using BookmarkItLibrary.Model;
using BookmarkItLibrary.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Domain
{
    internal interface IGetUserDetailsDataManager
    {
        Task GetUserDetailsAsync(GetUserDetailsRequest request, ICallback<GetUserDetailsResponse> callback);

        UserDetails GetUserDetailsFromDB(GetUserDetailsRequest request);

        Task<ParsedUserDetails> FetchUserDetailsFromServerAsync(GetUserDetailsRequest request, UserDetails userDetailsFromDB = default);
    }

    public class GetUserDetailsRequest : UseCaseRequest
    {
        public readonly string UserId;
        public readonly string RequestToken;
        public readonly bool SetAsCurrentUser;

        public GetUserDetailsRequest(RequestType type, string userId, string requestToken = default, bool setAsCurrentUser = false,
            CancellationTokenSource cts = default) : base(type, cts)
        {
            UserId = userId;
            RequestToken = requestToken;
            SetAsCurrentUser = setAsCurrentUser;
        }
    }

    public class GetUserDetailsResponse
    {
        public readonly UserDetails User;

        public GetUserDetailsResponse(UserDetails user)
        {
            User = user;
        }
    }

    public interface IGetUserDetailsPresenterCallback : ICallback<GetUserDetailsResponse> { }

    public sealed class GetUserDetails : UseCaseBase<GetUserDetailsRequest, GetUserDetailsResponse>
    {
        private readonly IGetUserDetailsDataManager DataManager;

        public GetUserDetails(GetUserDetailsRequest request, IGetUserDetailsPresenterCallback callback) : base(request, callback)
        {
            DataManager = DIServiceProvider.Instance.GetService<IGetUserDetailsDataManager>();
        }

        public override void Action()
        {
            DataManager.GetUserDetailsAsync(Request, new UseCaseCallback(this));
        }

        class UseCaseCallback : CallbackBase<GetUserDetailsResponse>
        {
            private readonly GetUserDetails UseCase;

            public UseCaseCallback(GetUserDetails useCase)
            {
                UseCase = useCase;
            }

            public override void OnError(UseCaseError error)
            {
                UseCase.PresenterCallback?.OnError(error);
            }

            public override void OnSuccess(IUseCaseResponse<GetUserDetailsResponse> response)
            {
                UseCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
