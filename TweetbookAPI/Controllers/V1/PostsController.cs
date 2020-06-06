using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.Domain;
using TweetbookAPI.Infrastructure;
using TweetbookAPI.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace TweetbookAPI.Controllers.V1
{
    [ApiVersion("1")]
    [ApiVersion("2")]
    [Route("posts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = ApplicationRoles.Administrators + "," + ApplicationRoles.Publishers)]
    public class PostsController : TweetbookApiController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //The comments below are consumed and publiched by Wagger documentation! (Refer to SwaggerInstaller.cs file)
        /// <summary>
        /// Retrieves all application existing posts
        /// </summary>
        /// <response code="200">Retrieves all application existing posts</response>
        [MapToApiVersion("1")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get() => Ok(await _postService.GetAllAsync());

        [MapToApiVersion("1")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            var post = await _postService.GetByIdAsync(id);

            if (post != null)
                return Ok(post);

            return NotFound();
        }

        //The comments below are consumed and publiched by Wagger documentation! (Refer to SwaggerInstaller.cs file)
        /// <summary>
        /// Inserts a new post into the application database
        /// </summary>
        /// <response code="201">Inserts a new post into the application database</response>
        /// <response code="400">Returns RuleViolation errors collection</response>
        [ProducesResponseType(typeof(MaintainPostResponse), (int)HttpStatusCode.Created)] //Action-level Swagger documentation increment
        [ProducesErrorResponseType(typeof(MaintainPostResponse))] //Action-level Swagger documentation increment
        [MapToApiVersion("1")]
        [HttpPost]
        public async Task<ActionResult> Post(MaintainPostRequest createPostRequest) //Automatic model validation was set with a combination of Action Filters and the FluentValidation library
        {
            var post = createPostRequest.CastTo<Post>();
            post.UserId = HttpContext.GetCurrentUserId();
            
            if (await _postService.CreateAsync(post))
            {
                var response = post.CastTo<MaintainPostResponse>();

                return CreatedAtRoute(new { id = response.Id }, response);
            }

            return BadRequest();
        }

        [MapToApiVersion("1")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MaintainPostRequest updatePostRequest)
        {
            var post = updatePostRequest.CastTo<Post>();
            post.Id = id;

            if (await _postService.UpdateAsync(post))
            {
                var response = post.CastTo<MaintainPostResponse>();

                return Ok(response);
            }

            return NotFound();
        }

        [MapToApiVersion("1")]
        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRoles.Administrators)]
        public async Task<ActionResult> Delete(int id)
        {
            if (await _postService.RemoveAsync(id))
                return NoContent();

            return NotFound();
        }
    }
}
