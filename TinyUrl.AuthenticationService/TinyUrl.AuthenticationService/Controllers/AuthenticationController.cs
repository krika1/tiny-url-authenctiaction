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
        private readonly ILogger<AuthenticationController> _logger;
        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserRequest request)
        {
            try
            {
                await _authenticationService.RegisterUserAsync(request).ConfigureAwait(false);

                return Ok();
            }
            catch (ConflictException ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To409ConflictResult(ex.Message, ErrorTitles.RegisterUserErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.RegisterUserErrorTitle);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To401UnauthorizedResult(ex.Message, ErrorTitles.UnauthorizedErrorTitle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return ObjectResultCreator.To500InternalServerErrorResult(ex.Message, ErrorTitles.LoginFailedErrorTitle);
            }
        }
    }
}
