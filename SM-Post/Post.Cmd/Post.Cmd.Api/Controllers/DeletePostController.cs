using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public  class DeleteuPostController : ControllerBase
{
    private readonly ILogger<DeleteuPostController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public DeleteuPostController(ILogger<DeleteuPostController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePostAsync(Guid id, DeletePostCommand command)
    {
        try
        {
            command.Id = id;

            await _commandDispatcher.SendAsync(command);

            return Ok(new BaseResponse
            {
                Message = "Delete post request completed sucessfully"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client made a bad request");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Could not retrieve aggregate, client passed an incorrect post ID tagetting the aggregate");
            return BadRequest(new BaseResponse
            {
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {

            const string SAFE_ERROR_MESSAGE = "Error while processing request to delete a post!";
            _logger.Log(LogLevel.Error, ex, SAFE_ERROR_MESSAGE);

            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                Message = SAFE_ERROR_MESSAGE
            });
        }
    }
}
