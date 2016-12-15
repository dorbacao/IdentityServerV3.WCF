using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using IdentityModel.Client;
using IdentityModel.Extensions;
using WCFService.Implement;
using IdentityModel.Constants;

namespace WCFService
{
    class Program
    {
      
        static void CreateService()
        {
            var host = new ServiceHost(
               typeof(ClaimService),
               new Uri("https://localhost:44335"));


            host.Credentials.IdentityConfiguration = CreateIdentityConfiguration();
            host.Credentials.UseIdentityConfiguration = true;

            var authz = host.Description.Behaviors.Find<ServiceAuthorizationBehavior>();
            authz.PrincipalPermissionMode = PrincipalPermissionMode.Always;

            //host.Description.Behaviors.Add(new ErrorServiceBehavior());

            var endpoint = host.AddServiceEndpoint(typeof(IClaimService), CreateBinding(), "token");

            host.Open();
        }
        static void Main(string[] args)
        {
            CreateService();

            Console.WriteLine("server running...");
            Console.ReadLine();

        }

        static IdentityConfiguration CreateIdentityConfiguration()
        {
            var identityConfiguration = new IdentityConfiguration();

            identityConfiguration.SecurityTokenHandlers.Clear();
            var host= "http://localhost:444/";
            var handler = new IdentityServerWrappedJwtHandler(host, "openid");
            identityConfiguration.SecurityTokenHandlers.Add(handler);
            identityConfiguration.ClaimsAuthorizationManager = new RequireAuthenticationAuthorizationManager();

            return identityConfiguration;
        }

        static Binding CreateBinding()
        {
            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
            // only for testing on localhost
            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;
            
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;

            return binding;
        }
    }
}
