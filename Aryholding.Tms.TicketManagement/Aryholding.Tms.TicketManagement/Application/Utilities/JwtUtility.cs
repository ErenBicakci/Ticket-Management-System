using Aryholding.Tms.TicketManagement.Application.Common.Exceptions;
using System.Security.Claims;

namespace Aryholding.Tms.TicketManagement.Application.Utilities
{
    public static class JwtUtility
    {
        public static string GetUserIdFromJwt(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedException("Kullanıcı giriş yapmamış.");

            var userIdClaim = user.FindFirst("uid") ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                throw new UnauthorizedException("Kullanıcı ID'si JWT token'da bulunamadı.");
            
            return userIdClaim.Value;
        }

        public static string GetUsernameFromJwt(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                throw new UnauthorizedException("Kullanıcı giriş yapmamış.");

            var usernameClaim = user.FindFirst("username") ?? user.FindFirst(ClaimTypes.Name);
            if (usernameClaim == null || string.IsNullOrEmpty(usernameClaim.Value))
                throw new UnauthorizedException("Kullanıcı adı JWT token'da bulunamadı.");
            
            return usernameClaim.Value;
        }
    }
}
