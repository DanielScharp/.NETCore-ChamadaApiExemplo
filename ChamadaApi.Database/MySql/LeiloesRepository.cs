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
    public class LeiloesRepository
    {
        private readonly string _connMySql;

        public LeiloesRepository(string connMySql)
        {
            _connMySql = connMySql;
        }
        public async Task<Leilao> GetLeilaoAsync(int id)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT codigo, data, descricao FROM leilao.leiloes ");
                query.AppendFormat(" where codigo = {0}", id);

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                var leilao = new Leilao();

                while(reader.Read())
                {

                    leilao.Codigo = reader.GetInt32(reader.GetOrdinal("codigo"));
                    leilao.Data = reader[reader.GetOrdinal("data")] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("data")) : new DateTime();
                    leilao.Descricao = reader.GetString(reader.GetOrdinal("descricao"));

                }

                return leilao;
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
        public async Task<List<Leilao>> GetLeiloesAsync()
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT codigo, data, descricao FROM leilao.leiloes limit 2; ");

                using MySqlCommand command = new(query.ToString(), connection);

                using MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync();

                var listaLeiloes = new List<Leilao>();

                while(reader.Read())
                {
                    var leilao = new Leilao();

                    leilao.Codigo = reader.GetInt32(reader.GetOrdinal("codigo"));
                    leilao.Data = reader[reader.GetOrdinal("data")] != DBNull.Value ? reader.GetDateTime(reader.GetOrdinal("data")) : new DateTime();
                    leilao.Descricao = reader.GetString(reader.GetOrdinal("descricao"));

                    listaLeiloes.Add(leilao);
                }

                return listaLeiloes;
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

        public async Task<Leilao> AlterDescricaoLeilaoAsync(Leilao leilao)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.AppendFormat(" UPDATE leiloes set descricao = '{0}' where codigo = '{1}' ", leilao.Descricao, leilao.Codigo);

                using MySqlCommand command = new(query.ToString(), connection);

                await command.ExecuteReaderAsync();

                return leilao;
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

        public async Task<Leilao> InsertLeilaoAsync(Leilao leilao)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.AppendFormat(" insert leiloes (`data`, `descricao`) values ('{0}', '{1}') ", leilao.Data.ToString("yyyy-MM-dd"), leilao.Descricao);

                using MySqlCommand command = new(query.ToString(), connection);

                await command.ExecuteReaderAsync();

                return leilao;
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
