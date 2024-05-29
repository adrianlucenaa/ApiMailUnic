/*
using APICruzber.Modelo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICruzber.Controllers
{
    public class ValidateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly string _connectionStrings;

        public ValidateController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionStrings = configuration.GetConnectionString("ConnectionStrings:ConnectionBD");
        }

        public IActionResult Validar()
        {
            //Obtenemos el token
            var authHeader = HttpContext.Request.Headers["Authorization"];

            //Comprobamos si el token existe
            if (authHeader != "null" && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                // Aquí validas el token JWT
                var esValido = ValidarToken(token);
                if (esValido)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Token válido" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token inválido" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token no proporcionado" });
            }
        }

        public bool ValidarToken(string token)
        {
            //Instancia de JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
           
            //Instanciamos  la clave secreta
            var authHeader = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConnectionStrings:Logging:AllowedHosts:Jwt:Key").Value));

            //var authHeader = HttpContext.Request.Headers["Authorization"];

            //Validamos los parametros del token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = authHeader,
                ValidateIssuer = false,
                ValidateAudience = false,
                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConnectionStrings:Logging:AllowedHosts:Jwt:Key").Value))
            };

            //Validamos el token y devolvemos true o false
            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
*/

using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APICruzber.Controllers
{
    public class ValidateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ValidateController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("ConexionBD");
        }
        /*
        public IActionResult Validar()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authHeader) && authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);

                var esValido = ValidarToken(token);
                if (esValido)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "Token válido" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token inválido" });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { mensaje = "Token no proporcionado" });
            }
        }
        */
        public bool ValidarToken(string token)
        {
            //Instancia de JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Obetenemos la configuración del token
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            //Instanciamos  la clave secreta

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("estosoloesunaclavedepruebaadmincruzber08"));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConnectionStrings:Logging:AllowedHosts:Jwt:Key").Value));

            //Validamos los parametros del token
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            //Validamos el token y devolvemos true o false
            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

