using ChamadaApi.Application;
using ChamadaApi.Domain;
using ChamadaApi.Database.MySql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [Route("Inserir")]
        [HttpPost]
        public async Task<IActionResult> InsertLeilaoAsync(Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.InsertLeilaoAsync(leilao);

                return Ok(ResultMessage.Sucesso(0, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Retornar")]
        [HttpGet]
        public async Task<IActionResult> GetLeilaoAsync(int id)
        {
            try
            {
                var result = await _leiloesApplication.GetLeilaoAsync(id);

                return Ok(ResultMessage.Sucesso(0, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Listar")]
        [HttpGet]
        public async Task<IActionResult> GetLeiloesAsync(int categoriaId, double valor, bool vdd, Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.GetLeiloesAsync();

                return Ok(ResultMessage.Sucesso(0, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        [Route("Alterar-descricao")]
        [HttpPut]
        public async Task<IActionResult> AlterDescricaoLeilaoAsync(Leilao leilao)
        {
            try
            {
                var result = await _leiloesApplication.AlterDescricaoLeilaoAsync(leilao);

                return Ok(ResultMessage.Sucesso(0, result));
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
