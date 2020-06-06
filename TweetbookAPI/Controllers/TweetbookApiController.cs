using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TweetbookAPI.Controllers
{
    [ApiController]
    //[RequiresApiKey] //This is a simplistic way to require from clients an API key so the request can or not be validated
    [Produces(MediaTypeNames.Application.Json)] //Controller-level Swagger documentation increment
    public abstract class TweetbookApiController : ControllerBase
    {
    }
}
