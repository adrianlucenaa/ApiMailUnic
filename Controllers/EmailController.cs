
using APICruzber.Connection;
using APICruzber.Interfaces;
using APICruzber.Modelo;
using APICruzber.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace APICruzber.Controllers
{
    [ApiController]
    [Route("api/emails")]
    
    public class EmailController: ControllerBase , IEmail
    {
        public readonly ConnectionBD _cnxdb;

        public readonly IConfiguration _configuration;

        public readonly IEmail _email;

        private readonly ILogger<EmailController> _logger;

        private string key = string.Empty;
        public EmailController(IConfiguration configuration, ConnectionBD cnxdb, IEmail email, ILogger<EmailController> logger)
        {
            _configuration = configuration;
            _cnxdb = cnxdb;
            _email = email;
            _logger = logger;
        }

        //Http por los parametros token y lang
        [HttpGet("GetMails/{token}/{lang}")]
        public async Task<IActionResult> GetMails( string token, string lang)
        {
            //var authHeader = HttpContext.Request.Headers["Authorization"];

            //var tokenHandler = new JwtSecurityTokenHandler();
            var builderjwt = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            key = builderjwt.GetSection("Jwt:Key").Value;

            //var jwtConfig = _configuration.GetSection("Jwt");
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("ConnectionStrings:Logging:AllowedHosts:Jwt:Key").Value));
            
            //Cogemos el token desde la cabecera 


            // Comprobamos si el token existe
            if (token != null && (token == key))
            {
                try
                {
                    // Obtener la lista de correos electrónicos
                    var mails = await _email.GetMails(token, lang);
                    //Si mail es igual a null impreme un no content , si no es igual te vuelve un 200 y la lista de emails
                    if (mails == null )
                    {
                        return NoContent(); // Devuelve un 204 si no hay correos electrónicos
                    }
                    return Ok(mails); // Devuelve un 200 con la lista de correos electrónicos
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener los correos electrónicos para el idioma {lang}", lang);
                    return StatusCode(500, $"Error interno del servidor: {ex.Message}");

                }
            }

            else { return StatusCode(401, "Acceso no autorizado"); } // No hay token o el token no coincide con la clave(); }
        }
        
    }
}