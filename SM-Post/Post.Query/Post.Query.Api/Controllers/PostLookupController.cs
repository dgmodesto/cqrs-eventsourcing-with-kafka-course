using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using System.Collections.Specialized;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger;
    private readonly IQueryDispatcher<PostEntity> _queryDispatcher;

    public PostLookupController(
        ILogger<PostLookupController> logger, 
        IQueryDispatcher<PostEntity> queryDispatcher)
    {
        _logger = logger;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllPostAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request retrieve all posts";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);

        }
    }

    [HttpGet("byId/{postId}")]
    public async Task<ActionResult> GetByPostIdAsync(Guid postId)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery { Id = postId });

            if (posts == null || !posts.Any()) return NoContent();

            return Ok(new PostLookupResponse
            {
                Posts = posts,
                Message = $"Successfully retuned post"
            });

        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request to post by id";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);

        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<ActionResult> GetPostByAuthorAsync(string author)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByAuthorQuery { Author = author });
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request posts by author";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);

        }
    }


    [HttpGet("withComments")]
    public async Task<ActionResult> GetPostWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request posts with comments.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);

        }
    }

    [HttpGet("withLikes/{numberOfLike}")]
    public async Task<ActionResult> GetPostLikesAsync(int numberOfLikes)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostWithLikesQuery {  NumberOfLikes = numberOfLikes});
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            const string SAFE_ERROR_MESSAGE = "Error while processing request posts with likes.";
            return ErrorResponse(ex, SAFE_ERROR_MESSAGE);

        }
    }

    private ActionResult NormalResponse(List<PostEntity> posts)
    {
        if (posts == null || !posts.Any()) return NoContent();

        var count = posts.Count();

        return Ok(new PostLookupResponse
        {
            Posts = posts,
            Message = $"Successfully retuned {count}  post{(count > 1 ? "s" : string.Empty)}"
        });
    }
    private ActionResult ErrorResponse(Exception ex, string safeErrorMessage)
    {
        _logger.LogError(ex, safeErrorMessage);

        return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
        {
            Message = safeErrorMessage,
        });
    }
}
