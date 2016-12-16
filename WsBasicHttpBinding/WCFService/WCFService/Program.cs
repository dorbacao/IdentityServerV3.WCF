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
            var host = new ServiceHost(typeof(ClaimService), new Uri("http://localhost:2222"));

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
            var host = "https://identityserver.desenvolvimento.vvssistemas.com.br:1443/";
            var handler = new IdentityServerWrappedJwtHandler(host, "openid");
            identityConfiguration.SecurityTokenHandlers.Add(handler);
            identityConfiguration.ClaimsAuthorizationManager = new RequireAuthenticationAuthorizationManager();

            return identityConfiguration;
        }

        static Binding CreateBinding()
        {
            var binding = new BasicHttpBinding();

            //binding.HostNameComparisonMode = HostNameComparisonMode.Exact;

            return binding;
        }
    }
}
