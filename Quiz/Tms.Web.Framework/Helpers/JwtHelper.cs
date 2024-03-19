using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace Tms.Web.Framework
{
    public class JwtUtil
    {
        public static string GenerateToken(string userName)
        {

            var clientID = ConfigurationManager.AppSettings["Jwt_Client_Id"];
            var symmetricKeyAsBase64 = ConfigurationManager.AppSettings["Jwt_Secret_Key"];
            var issuer = "SHOWDC";
            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("sub", userName));
            identity.AddClaim(new Claim("aud", clientID));
      

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var token = new JwtSecurityToken(issuer, clientID, identity.Claims, DateTime.UtcNow, DateTime.UtcNow.AddDays(30), signingKey);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        public static bool IsValidToken(string token,out ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                claimsPrincipal = null;
                var client_id = ConfigurationManager.AppSettings["Jwt_Client_Id"];
                string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["Jwt_Secret_Key"];

                byte[] keyBytes = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new List<string>() { "SHOWDC" },
                    ValidAudiences = new List<string>() { client_id },
                    IssuerSigningToken = new BinarySecretSecurityToken(keyBytes),
                };

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken resultTokent = null;
                claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out resultTokent);
                if (claimsPrincipal == null || resultTokent == null || resultTokent.ValidFrom > DateTime.UtcNow || resultTokent.ValidTo < DateTime.UtcNow)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                claimsPrincipal = null;
                return false;
            }

        }
    }
}