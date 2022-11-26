using System.Security.Claims;

namespace smartshop.Api.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static int GetBusinessId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return Convert.ToInt32(principal.FindFirst("businessId")?.Value);
        }

        public static int GetStoreId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return Convert.ToInt32(principal.FindFirst("storeId")?.Value);
        }

        public static bool IsInRoles(this ClaimsPrincipal principal, string role)
        {
            return GetUserRoles(principal).Any(x => x == role);
        }

        public static bool IsInRoles(this ClaimsPrincipal principal, List<string> roles)
        {
            var userRoles = GetUserRoles(principal);
            foreach (var role in roles)
                return GetUserRoles(principal).Any(x => x == role);

            return false;
        }

        private static List<string> GetUserRoles(ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            var rolesClaims = principal.FindAll(ClaimTypes.Role);
            var roles = new List<string>();
            foreach (var item in rolesClaims)
                roles.Add(item.Value);

            return roles;
        }
    }
}
