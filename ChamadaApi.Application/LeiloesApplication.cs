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

        public async Task<Leilao> GetAuctionAsync(int id)
        {
            try
            {
                return await _leiloesRepository.GetAuctionAsync(id);
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Leilao>> GetAuctionsAsync()
        {
            try
            {
                return await _leiloesRepository.GetAuctionsAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Leilao> AlterAuctionAsync(Leilao leilao)
        {
            try
            {
                return await _leiloesRepository.AlterAuctionAsync(leilao);
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Leilao> InsertAuctionAsync(Leilao leilao)
        {
            try
            {
                return await _leiloesRepository.InsertAuctionAsync(leilao);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteAuctionAsync(int id)
        {
            try
            {
                return await _leiloesRepository.DeleteAuctionAsync(id);
            }
            catch
            {
                throw;
            }
        }

    }
}
