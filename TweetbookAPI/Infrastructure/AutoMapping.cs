using AutoMapper;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.Domain;
using TweetbookAPI.DTO;

namespace TweetbookAPI.Infrastructure
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<MaintainPostRequest, Post>();
            CreateMap<Post, MaintainPostResponse>();
            CreateMap<AuthenticationResult, AuthenticateUserResponse>();
            CreateMap<AuthenticationResult, RegisterUserResponse>();
            CreateMap<MaintainTagRequest, Tag>();
            CreateMap<Tag, MaintainTagResponse>();
        }
    }
}
