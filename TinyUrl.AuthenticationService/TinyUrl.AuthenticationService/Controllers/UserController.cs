using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Services;

namespace TinyUrl.AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserContract>> GetUserByIdAsync([FromRoute] int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId).ConfigureAwait(false);

                return Ok(user);
            }
            catch (NotFoundException ex)
            {
                var error = new ErrorContract(StatusCodes.Status404NotFound, ex.Message, ErrorTitles.GetUserFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }
            catch (Exception ex)
            {
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.GetUserFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
