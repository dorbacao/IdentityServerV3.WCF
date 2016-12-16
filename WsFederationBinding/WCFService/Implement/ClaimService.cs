using System;
using System.Security.Claims;
using System.ServiceModel;
using System.Text;

namespace WCFService.Implement
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ClaimService : IClaimService
    {
        public string GetAllClaims()
        {
            return "olá";
            var sb = new StringBuilder();

            foreach (var claim in ClaimsPrincipal.Current.Claims)
            {
                sb.AppendFormat("{0} :: {1}\n", claim.Type, claim.Value);
            }

            return sb.ToString();
        }
    }
}