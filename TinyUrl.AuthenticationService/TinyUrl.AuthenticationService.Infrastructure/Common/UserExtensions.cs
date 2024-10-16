﻿using System.Security.Claims;

namespace TinyUrl.AuthenticationService.Infrastructure.Common
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claims)
        {
            var claim = claims.Claims.FirstOrDefault(x => x.Type == "UserId");
            return claim!.Value;
        }
    }
}
