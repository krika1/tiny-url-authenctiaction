using Microsoft.AspNetCore.Mvc;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Requests;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using TinyUrl.AuthenticationService.Infrastructure.Services;

namespace TinyUrl.AuthenticationService.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            try
            {
                await _authenticationService.RegisterUserAsync(request).ConfigureAwait(false);

                return Created();
            }
            catch (ConflictException ex)
            {
                var error = new ErrorContract(StatusCodes.Status409Conflict, ex.Message, ErrorTitles.RegisterUserErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status409Conflict,
                };
            }
            catch (Exception ex)
            {
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.RegisterUserErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenContract>> LoginUserAsync([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authenticationService.LoginAsync(request).ConfigureAwait(false);

                return Ok(token);
            }
            catch (UnauthorizedException ex)
            {
                var error = new ErrorContract(StatusCodes.Status401Unauthorized, ex.Message, ErrorTitles.UnauthorizedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                };
            }
            catch (Exception ex)
            {
                var error = new ErrorContract(StatusCodes.Status500InternalServerError, ex.Message, ErrorTitles.LoginFailedErrorTitle);

                return new ObjectResult(error)
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }
    }
}
