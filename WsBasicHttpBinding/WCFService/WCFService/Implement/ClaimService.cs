using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml.Linq;
using IdentityModel.Constants;
using IdentityModel.Extensions;

namespace WCFService.Implement
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class ClaimService : IClaimService
    {

        static GenericXmlSecurityToken WrapJwt(SecurityToken securityToken)
        {
            var xmlToken = new GenericXmlSecurityToken(
                XElement.Parse(securityToken.ToTokenXmlString()).ToXmlElement(),
                null,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                null,
                null,
                null);

            return xmlToken;
        }

        static SecurityToken CreateSecurityToken(string jwt)
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

            return token;
        }

        public IEnumerable<Claim> GetAllClaims()
        {
            var request = (HttpRequestMessageProperty)OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name];
            var token = request.Headers["account-token"];
            var wraped = CreateSecurityToken(token);

            var identityHelper = new IdentityServerWrappedJwtHandler("http://localhost:444", "openid", "profile", "grupoempresa", "usuario", "offline_access", "permissoes", "licenciamento");
            var claims = identityHelper.ValidateToken(wraped);

            return claims[0].Claims;
        }
    }
}