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
        public async Task<Leilao> GetAuctionAsync(int id)
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
        public async Task<List<Leilao>> GetAuctionsAsync()
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" SELECT codigo, data, descricao FROM leilao.leiloes limit 10; ");

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

        public async Task<Leilao> AlterAuctionAsync(Leilao leilao)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.Append(" UPDATE leiloes set ");
                query.AppendFormat(" descricao = '{0}', ", leilao.Descricao);
                query.AppendFormat(" data = '{0}' ", leilao.Data.DateMySql());
                query.AppendFormat(" where ( codigo = '{0}' ) ", leilao.Codigo);

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

        public async Task<Leilao> InsertAuctionAsync(Leilao leilao)
        {

            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();
                query.AppendFormat(" insert leiloes (`data`, `descricao`) values ('{0}', '{1}') ", leilao.Data.ToString("yyyy-MM-dd"), leilao.Descricao);

                using MySqlCommand command = new(query.ToString(), connection);

                await command.ExecuteReaderAsync();
                leilao.Codigo = (int)command.LastInsertedId;

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

        public async Task<bool> DeleteAuctionAsync(int id)
        {
            using var connection = new MySqlConnection(_connMySql);

            try
            {
                await connection.OpenAsync();

                var query = new StringBuilder();

                query.AppendFormat(" DELETE FROM `leilao`.`leiloes` WHERE (`codigo` = '{0}'); ", id);

                using MySqlCommand command = new(query.ToString(), connection);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0; // Retorna true se pelo menos uma linha foi afetada
            }
            catch
            {
                throw;
            }
            finally
            {
                if(connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

    }
}
