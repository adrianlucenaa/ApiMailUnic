using APICruzber.Connection;
using APICruzber.Interfaces;
using APICruzber.Modelo;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace APICruzber.Datos
{
    public class DatosMail : IEmail
    {
        //Declaro la variable de conexion a la BBDD
        private readonly ConnectionBD _cnxdb;
        //Declaro la variable de logger
        private readonly ILogger<DatosMail> _logger;

        //Constructor de datos cliente
        public DatosMail(ConnectionBD cnxdb, ILogger<DatosMail> logger)
        {
            _cnxdb = cnxdb;
            _logger = logger;
        }

        //metodo para traerte todos los mails
        public async Task<IActionResult> GetMails(string token, string lang)
        {
            try
            {
                // Obtener la lista de mails utilizando el método apropiado
                var mails = await GetMailByLang(token, lang);

                // Devolver la lista de mails como un OkObjectResult
                return new OkObjectResult(mails);
            }
            catch (Exception ex)
            {
                // Manejar la excepción y devolver un código de estado 500 Internal Server Error con un mensaje de error
                Console.WriteLine($"Error al mostrar mails: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //Logica con la que obtienes todos los mails y lo filtras por paraetro
        private async Task<List<EmailModelo>> GetMailByLang(string token, string lang)
        {
            var lista = new List<EmailModelo>();
            try
            {
                _logger.LogInformation("Starting database operation for lang: {Lang}", lang);
                //Usso de la cadena de conexion a la base de datos
                using (var sql = new SqlConnection(_cnxdb.cadenaSQL()))
                {
                    //Decalo el procedimiento almacenado y lo ejecuto
                    var cmd = new SqlCommand("OooobtenerDatosMailchimp", sql);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Añade el parámetro @Lang al comando
                    cmd.Parameters.AddWithValue("@Lang", lang);

                    _logger.LogInformation("Opening SQL connection.");
                    //Abro la base de datos de forma asincrona
                    await sql.OpenAsync();

                    _logger.LogInformation("Executing command: {CommandText}", cmd.CommandText);
                    //Ejecuto el procedimiento almacenado de manera asincrona
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var emailModelo = new EmailModelo
                            {
                                Email = reader["Email"] as string
                            };
                            lista.Add(emailModelo);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error while fetching emails for lang: {Lang}", lang);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General Error while fetching emails for lang: {Lang}", lang);
                throw;
            }
            return lista;
        }
    }
}
