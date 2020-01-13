using BookmarkItLibrary.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPUtilities.UseCase;

namespace BookmarkItLibrary.Domain
{
    internal interface IGetRequestTokenDataManager
    {
        Task GetRequestTokenAsync(GetRequestTokenRequest request, ICallback<GetRequestTokenResponse> callback);
    }

    public sealed class GetRequestTokenRequest : UseCaseRequest
    {
        public GetRequestTokenRequest(CancellationTokenSource cts = null) : base(RequestType.Network, cts) { }
    }

    public sealed class GetRequestTokenResponse
    {
        public readonly string RequestToken;

        public GetRequestTokenResponse(string requestToken)
        {
            RequestToken = requestToken;
        }
    }

    public interface IGetRequestTokenPresenterCallback : ICallback<GetRequestTokenResponse> { }

    public sealed class GetRequestToken : UseCaseBase<GetRequestTokenRequest, GetRequestTokenResponse>
    {
        private readonly IGetRequestTokenDataManager DataManager;

        public GetRequestToken(GetRequestTokenRequest request, IGetRequestTokenPresenterCallback callback) : base(request, callback)
        {
            DataManager = DIServiceProvider.Instance.GetService<IGetRequestTokenDataManager>();
        }

        protected override void Action()
        {
            DataManager.GetRequestTokenAsync(Request, new UseCaseCallback(this));
        }

        class UseCaseCallback : CallbackBase<GetRequestTokenResponse>
        {
            private readonly GetRequestToken UseCase;

            public UseCaseCallback(GetRequestToken useCase)
            {
                UseCase = useCase;
            }

            public override void OnError(UseCaseError error)
            {
                UseCase.PresenterCallback?.OnError(error);
            }

            public override void OnFailed(IUseCaseResponse<GetRequestTokenResponse> response)
            {
                UseCase.PresenterCallback?.OnFailed(response);
            }

            public override void OnSuccess(IUseCaseResponse<GetRequestTokenResponse> response)
            {
                UseCase.PresenterCallback?.OnSuccess(response);
            }
        }
    }
}
