using ChamadaApi.Domain;
using ChamadaApi.Database.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Application
{
    public class LeiloesApplication
    {
        private readonly LeiloesRepository _leiloesRepository;


        //Contrutor com injeção de dependência
        public LeiloesApplication(LeiloesRepository leiloesRepository)
        {
            _leiloesRepository = leiloesRepository;
        }

        public async Task<Leilao> GetLeilaoAsync(int id)
        {
            try
            {
                return await _leiloesRepository.GetLeilaoAsync(id);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Leilao>> GetLeiloesAsync()
        {
            try
            {
                return await _leiloesRepository.GetLeiloesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Leilao> AlterDescricaoLeilaoAsync(Leilao leilao)
        {
            try
            {
                return await _leiloesRepository.AlterDescricaoLeilaoAsync(leilao);
            }
            catch
            {
                throw;
            }
        }
        

        public async Task<Leilao> InsertLeilaoAsync(Leilao leilao)
        {
            try
            {
                return await _leiloesRepository.InsertLeilaoAsync(leilao);
            }
            catch
            {
                throw;
            }
        }

    }
}
