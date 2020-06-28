//using AdminWebAPI.Utils.Excecoes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CSharpUsersAPI.Utils
{
    public class UserUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public UserUtils(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static IHttpContextAccessor GetHttpContextAccessor()
        {
            return _httpContextAccessor;
        }

        public int ObterIdUserPeloContexto()
        {
            string StrIdUser = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(StrIdUser))
                throw new Exception("Não foi possível obter o Id do Usuário pelo Contexto.");
            //throw new ParametroNaoEncontradoException("Não foi possível obter o Id do Usuário pelo Contexto.");

            return Convert.ToInt32(StrIdUser);
        }

        public static int ObterIdUserPeloToken()
        {
            try
            {
                string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                string tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadToken(tokenStr) as JwtSecurityToken;
                string StrIdUser = token.Claims.First(claim => claim.Type == "nameid").Value;

                if (string.IsNullOrEmpty(StrIdUser))
                    throw new Exception("Não foi possível obter o Id do Usuário pelo Token.");
                //throw new ParametroNaoEncontradoException("Não foi possível obter o Id do Usuário pelo Token.");

                return Convert.ToInt32(StrIdUser);
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível obter o Id do Usuário pelo Token.");
                //throw new ParametroNaoEncontradoException("Não foi possível obter o Id do Usuário pelo Token.");
            }
        }

        public string ObterNomeUserPeloToken()
        {
            try
            {
                string authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                string tokenStr = authHeader.Substring("Bearer ".Length).Trim();
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.ReadToken(tokenStr) as JwtSecurityToken;
                string NomeUser = token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

                if (string.IsNullOrEmpty(NomeUser))
                    throw new Exception("Não foi possível obter o Nome do Usuário pelo Token.");
                //throw new ParametroNaoEncontradoException("Não foi possível obter o Nome do Usuário pelo Token.");

                return NomeUser;
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível obter o Nome do Usuário pelo Token.");
                //throw new ParametroNaoEncontradoException("Não foi possível obter o Nome do Usuário pelo Token.");
            }
        }

        public static string ObterToken()
        {
            try
            {
                return _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível obter o Token.");
                //throw new UnauthorizedException("Não foi possível obter o Token.");
            }
        }

    }
}
