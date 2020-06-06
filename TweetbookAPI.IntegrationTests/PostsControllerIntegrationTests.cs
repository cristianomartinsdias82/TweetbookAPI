using FluentAssertions;
using TweetbookAPI.Contracts.V1.Requests;
using TweetbookAPI.Contracts.V1.Responses;
using TweetbookAPI.Domain;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TweetbookAPI.IntegrationTests
{
    public class PostsControllerIntegrationTests : IntegrationTest
    {
        [Fact]
        public async Task Get_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            //Arrange
            await SetupAuthenticationRequestAsync();

            //Act
            var response = await TestClient.GetAsync("api/v1/posts");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadAsAsync<IEnumerable<Post>>()).Should().NotBeEmpty();
        }

        [Fact]
        public async Task PostedPost_ReturnsNewlyCreatedPost()
        {
            //Arrange
            await SetupAuthenticationRequestAsync();

            var now = DateTime.Now;
            var author = $"Integ. Test User ({now})";
            var post = new MaintainPostRequest()
            {
                Author = author,
                Content = "Integ. test - Post data",
                CreationDate = now,
                DisplayUntil = now.AddDays(7),
                Title = "This is an integration test"
            };

            //Act
            var response = await TestClient.PostAsJsonAsync("api/v1/posts", post); //https://stackoverflow.com/questions/19158378/httpclient-not-supporting-postasjsonasync-method-c-sharp

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var newlyCreatedPost = await response.Content.ReadAsAsync<MaintainPostResponse>();
            newlyCreatedPost?.Should().NotBeNull();
            newlyCreatedPost.Author.Should().NotBeEmpty().And.Be(author);
        }
    }
}