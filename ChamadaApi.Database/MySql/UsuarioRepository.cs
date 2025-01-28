using ChamadaApi.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Database.MySql
{
    public class UsuarioRepository
    {
        private readonly string _connMySql;

        public UsuarioRepository(string connMySql)
        {
            _connMySql = connMySql;
        }

        //Retornar usuário por apelido e password || READ_WRITE
        public async Task<Usuario> GetAsync(Login login)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT * ");
                query.Append(" FROM usuarios.tbl_usuarios ");
                query.AppendFormat(" WHERE Login = '{0}' AND Senha = MD5('{1}'); ", login.Apelido, login.Password);

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                if(reader.Read())
                {
                    var cliente = new Usuario();

                    cliente.UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId"));
                    cliente.Apelido = reader.GetString(reader.GetOrdinal("Login"));

                    return cliente;
                }

                return new Usuario();
            }
            catch
            {
                throw;
            }
            finally
            {
                if(connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


    }
}
