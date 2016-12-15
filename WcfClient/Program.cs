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
        static GenericXmlSecurityToken WrapJwt(string jwt)
        {
            var subject = new ClaimsIdentity("saml");
            subject.AddClaim(new Claim("jwt", jwt));

            var descriptor = new SecurityTokenDescriptor
            {
                TokenType = TokenTypes.Saml2TokenProfile11,
                TokenIssuerName = "urn:wrappedjwt",
                Subject = subject
            };

            var handler = new Saml2SecurityTokenHandler();
            var token = handler.CreateToken(descriptor);

            var xmlToken = new GenericXmlSecurityToken(
                XElement.Parse(token.ToTokenXmlString()).ToXmlElement(),
                null,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                null,
                null,
                null);

            return xmlToken;
        }

        static string GetToken()
        {
            var oauth2Client = new TokenClient(
                "http://localhost:444/connect/token",
                "client_rsw",
                "secret");

            var tokenResponse =
                oauth2Client.RequestResourceOwnerPasswordAsync("admin", "admin", "openid").Result;

            return tokenResponse.AccessToken;
        }


        private static IClaimService CreateClient()
        {
            var jwt = GetToken();
            var xmlToken = WrapJwt(jwt);

            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
            //var binding = new WS2007HttpBinding(SecurityMode.TransportWithMessageCredential);
            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;

            var factory = new ChannelFactory<IClaimService>(
                binding,
                new EndpointAddress("https://localhost:44335/token"));

            var channel = factory.CreateChannelWithIssuedToken(xmlToken);

            return channel;
        }

        static void Main(string[] args)
        {
            var client = CreateClient();

            try
            {
                var x = client.GetAllClaims();
                Console.WriteLine(x);
            }
            catch (Exception fEx)
            {
                throw;
            }
            

            Console.ReadKey();
        }
    }
}
