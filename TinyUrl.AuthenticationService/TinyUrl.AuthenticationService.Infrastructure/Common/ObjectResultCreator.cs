using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Responses;

namespace TinyUrl.AuthenticationService.Infrastructure.Common
{
    public static class ObjectResultCreator
    {
        public static ObjectResult To401UnauthorizedResult(string errorMessage, string errorTitle)
        {
            var error = new ErrorContract(StatusCodes.Status401Unauthorized, errorMessage, errorTitle);

            return new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }

        public static ObjectResult To404NotFoundResult(string errorMessage, string errorTitle)
        {
            var error = new ErrorContract(StatusCodes.Status404NotFound, errorMessage, errorTitle);

            return new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }

        public static ObjectResult To409ConflictResult(string errorMessage, string errorTitle)
        {
            var error = new ErrorContract(StatusCodes.Status409Conflict, errorMessage, errorTitle);

            return new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }

        public static ObjectResult To500InternalServerErrorResult(string errorMessage, string errorTitle)
        {
            var error = new ErrorContract(StatusCodes.Status500InternalServerError, errorMessage, errorTitle);

            return new ObjectResult(error)
            {
                StatusCode = error.StatusCode
            };
        }
    }
}
