using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using Refit;
using System.Threading.Tasks;

namespace TweetbookAPI.Sdk
{
    //https://reactiveui.github.io/refit/ (see 'Redefining headers' section - I was in trouble when trying to pass two headers in the same request)
    [Headers("Authorization: Bearer")]
    public interface ITweetbookClientApi
    {
        [Post("/api/v1/posts")]
        Task<MaintainPostResponse> Post([Body]MaintainPostRequest request, [Header("X-Api-Key")] string apiKey = "45buyng498hkunrjhtowe8");

        [Put("/api/v1/posts/{id}")]
        Task<MaintainPostResponse> Post(int id, [Body]MaintainPostRequest request, [Header("X-Api-Key")] string apiKey = "45buyng498hkunrjhtowe8");

        [Delete("/api/v1/posts/{id}")]
        Task<string> Delete(int id, [Header("X-Api-Key")] string apiKey = "45buyng498hkunrjhtowe8");
    }
}
