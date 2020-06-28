using CSharpUsersAPI.Utils.Excecoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUsersAPI.Utils.JWT
{
    public class ValidJWTHandler : AuthorizationHandler<ValidJWTRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        public ValidJWTHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidJWTRequirement requirement)
        {
//#if DEBUG
//            context.Succeed(requirement);
//            return Task.CompletedTask;
//#else
            string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (authHeader == null || !authHeader.StartsWith("Bearer"))
            {
                throw new UnauthorizedException("O Cabecalho de autorizacao esta vazio ou nao contem 'Bearer' ");
            }
            string jwt = authHeader.Substring("Bearer ".Length).Trim();
            if (ValidateToken(jwt))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            context.Fail();
            return Task.CompletedTask;
//#endif
        }

        private bool ValidateToken(string authToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = GetValidationParameters();
            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                ////Podemos verificar as Claims a partir do principal
            }
            catch (Exception e)
            {
                throw new UnauthorizedException("Seu token de acesso nao pode ser validado", false, e);
            }
            return validatedToken != null;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            const string sec = "S9M83hX6KDcXopKN4rRW0Bc9G6W5dlTtJw6qmxhxRyE";
            byte[] keyByteArray = TextEncodings.Base64Url.Decode(sec);

            var securityKey = new SymmetricSecurityKey(keyByteArray);
            //var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = "https://api.sysdam.com.br/",
                ValidAudience = "fa2ecac2857245b1b9c4792941f1eb6e",
                IssuerSigningKey = securityKey
            };
        }
    }
}
