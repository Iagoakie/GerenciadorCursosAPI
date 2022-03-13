using GerenciadorCursos.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GerenciadorCursos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {

        private readonly ConfiguracoesJWT ConfiguracoesJWT;
        public AutenticacaoController(IOptions<ConfiguracoesJWT>opcoes)
        {
            ConfiguracoesJWT =  opcoes.Value; 
        }

        [HttpGet]
        public IActionResult ObterToken()
        {

            var token = GerarToken();
            var retorno = new
            {

                token = token
            };

            return Ok(retorno);

        }

        private string  GerarToken()
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role,"Gerente"));
            claims.Add(new Claim(ClaimTypes.Role, "Secretaria"));



            var handler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfiguracoesJWT.Segredo)), SecurityAlgorithms.HmacSha256Signature),
                Audience = "https://localhost:44398",
                Issuer = "CursosValid",
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(5),
            };

            SecurityToken token = handler.CreateToken(tokenDescriptor);

             return handler.WriteToken(token);
          
        }





    }
}
