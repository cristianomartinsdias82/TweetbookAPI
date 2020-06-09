using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.Domain;
using TweetbookAPI.Infrastructure;
using TweetbookAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace TweetbookAPI.Controllers.V1
{
    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("tags")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagsController : TweetbookApiController
    {
        private readonly IPostService _postService;

        public TagsController(IPostService postService)
        {
            _postService = postService;
        }

        //The comments below are consumed and published by Swagger documentation! (Refer to SwaggerInstaller.cs file)
        /// <summary>
        /// Retrieves all tags for an existing post
        /// </summary>
        /// <response code="200">Retrieves all post tags</response>
        //[Authorize(Policy = ResourcePolicies.ViewTagPermissionPolicy)]//(Pre-configured in SecurityConcernsInstaller class) It checks the requesting user data against the policy and hits or not this endpoint
        [Authorize(Policy = ResourcePolicies.InternalEmployeePermissionPolicy)]
        [HttpGet("GetTagsByPost/{postId}")]
        public async Task<IEnumerable<MaintainTagResponse>> Get(int postId)
        {
            var tags = await _postService.GetTagsByPostAsync(postId);

            return tags.BatchCastTo<MaintainTagResponse>();
        }

        //The comments below are consumed and published by Swagger documentation! (Refer to SwaggerInstaller.cs file)
        /// <summary>
        /// Created a new tag, assigned to an existing post
        /// </summary>
        /// <response code="201">Retrieves the created tag</response>
        /// <response code="500">Returns internal server error</response>
        [Authorize(Policy = ResourcePolicies.InternalEmployeePermissionPolicy)]
        [HttpPost("{postId}")]
        [ProducesResponseType(typeof(MaintainTagResponse), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(MaintainTagResponse))]
        public async Task<IActionResult> Post(MaintainTagRequest request, int postId)
        {
            var newTag = request.CastTo<Tag>();
            newTag.PostId = postId;

            var newlyCreatedTag = await _postService.CreatePostTagAsync(newTag);
            if (newlyCreatedTag != null)
            {
                var response = newlyCreatedTag.CastTo<MaintainTagResponse>();

                return CreatedAtRoute(new { id = response.Id }, response);
            }

            return Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
