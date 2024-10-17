using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Services;

namespace TinyUrl.AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CounterContract>> GetUserByIdAsync()
        {
            try
            {
                var currentUser = User.GetUserId();
                var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");

                var user = await _userService.GetUserByIdAsync(int.Parse(currentUser), token).ConfigureAwait(false);

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To404NotFoundResult(ex.Message, ErrorTitles.GetUserFailedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.GetUserFailedErrorTitle);
            }
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var currentUser = User.GetUserId();

                await _userService.ChangePasswordAsync(request, int.Parse(currentUser)).ConfigureAwait(false);

                return Ok();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To404NotFoundResult(ex.Message, ErrorTitles.ChangeUserPasswordFailedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.ChangeUserPasswordFailedErrorTitle);
            }
        }
    }
}
