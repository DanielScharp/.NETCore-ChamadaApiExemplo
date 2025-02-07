using ChamadaApi.Application;
using ChamadaApi.Domain;
using ChamadaApi.Database.MySql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MySqlX.XDevAPI.Common;

namespace ChamadaApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LeiloesController :ControllerBase
    {
        private readonly LeiloesApplication _leiloesApplication;

        public LeiloesController(LeiloesRepository leiloesRepository)
        {
            _leiloesApplication = new LeiloesApplication(leiloesRepository);
        }

        [Route("Retornar")]
        [HttpGet]
        public async Task<IActionResult> GetAuctionAsync(int id)
        {
            try
            {
                var result = await _leiloesApplication.GetAuctionAsync(id);


                return Ok(ResultMessage.Sucesso(id, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Listar")]
        [HttpGet]
        public async Task<IActionResult> GetAuctionsAsync(int categoriaId, double valor, bool vdd, Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.GetAuctionsAsync();

                return Ok(ResultMessage.Sucesso(0, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Inserir")]
        [HttpPost]
        public async Task<IActionResult> InsertAuctionAsync(Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.InsertAuctionAsync(leilao);

                return Ok(ResultMessage.Sucesso(result.Codigo, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Alterar")]
        [HttpPut]
        public async Task<IActionResult> AlterAuctionAsync(Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.AlterAuctionAsync(leilao);

                return Ok(ResultMessage.Sucesso(leilao.Codigo, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpDelete("Excluir")]
        public async Task<IActionResult> DeleteAuctionAsync(int id)
        {
            try
            {
                var leilao = await _leiloesApplication.GetAuctionAsync(id);

                if(leilao.Codigo == 0)
                    throw new KeyNotFoundException(id.ToString()); // Agora a exceção é lançada antes da exclusão

                var deletado = await _leiloesApplication.DeleteAuctionAsync(id);

                if(!deletado)
                    return StatusCode(500, "Erro ao deletar o leilão.");


                return Ok(ResultMessage.Sucesso(leilao.Codigo, $"O leilão {id} foi deletado com sucesso!"));
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound($"O Leilão {ex.Message} não foi encontrado. "); // 404 - Recurso não existe
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
