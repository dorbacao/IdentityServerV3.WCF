using System.Security.Claims;

namespace WCFService.Implement
{
    public class RequireAuthenticationAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            return context.Principal.Identity.IsAuthenticated;
        }
    }
}