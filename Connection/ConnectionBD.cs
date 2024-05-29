
using APICruzber.Modelo;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace APICruzber.Connection
{
    //Clase para la configuracion de la base de datos
    public class ConnectionBD
    {
        private string ConnectionString = string.Empty;

        public ConnectionBD()
        {
            var mybuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            ConnectionString = mybuilder.GetSection("ConnectionStrings:ConexionBD").Value;
            //Console.WriteLine(ConnectionString.ToString());                 //Obtiene la cadena de la base de datos, mediante el appsetting , que dentro lleva el connection string que ese connection string,
        }                                                                   //lleva la cadena de conexion a la base de datos.
        public string cadenaSQL()
        {
            return ConnectionString;
        }

    }
}
