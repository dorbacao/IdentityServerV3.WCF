using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IdentityModel.Client;
using IdentityModel.Constants;
using IdentityModel.Extensions;
using WCFService.Implement;

namespace WcfClient
{
    class Program
    {
       

        static string GetToken()
        {
            var oauth2Client = new TokenClient(
                "http://localhost:444/connect/token",
                "client_rsw",
                "secret");

            var tokenResponse =
                oauth2Client.RequestResourceOwnerPasswordAsync("admin", "admin", "openid profile grupoempresa usuario offline_access permissoes licenciamento").Result;

            return tokenResponse.AccessToken;
        }


        private static IClaimService CreateClient()
        {
            var token = GetToken();

            //var wrap = WrapJwt(token);

            var binding = new BasicHttpBinding();
            //var binding = new WS2007HttpBinding(SecurityMode.TransportWithMessageCredential);
            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;

            var endpoint = new EndpointAddress("http://localhost:2222/token");

            var factory = new ChannelFactory<IClaimService>(binding, endpoint);
            factory.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior(token));

            var channel = factory.CreateChannel();

            return channel;
        }

        static void Main(string[] args)
        {
            var client = CreateClient();

            try
            {
                var x = client.GetAllClaims();

                foreach (var claim in x)
                    Console.WriteLine(claim.Type + ":" + claim.Value);
                
            }
            catch (Exception fEx)
            {
                throw;
            }


            //Console.ReadKey();
        }
    }
}
