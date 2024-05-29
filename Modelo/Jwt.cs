using APICruzber.Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Linq;

namespace APICruzber.Modelo
{
    //Modelo del Jwt
    public class Jwt 
    {
        public  string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Subject { get; set; }

        public IConfiguration _configuration;

        public ConnectionBD _cnxdb;
        
        public string connectionString;

        public Jwt()
        {
        }

        public Jwt(IConfiguration configuration, ConnectionBD cnxdb)
        {
            _configuration = configuration;
            _cnxdb = cnxdb;
            connectionString = configuration.GetConnectionString("ConnectionStrings:ConexionBD");
            configuration.GetSection("Jwt").Bind(this);
        }

    }
}