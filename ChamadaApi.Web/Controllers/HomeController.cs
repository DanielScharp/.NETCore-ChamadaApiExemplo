using ChamadaApi.Domain;
using ChamadaApi.Web.Models;
using ChamadaApi.Web.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChamadaApi.Web.Controllers
{
    [Authorize]
    public class HomeController :Controller
    {
        private readonly IMyApiService _apiService;

        public HomeController(IMyApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var request = new ApiRequest
                {
                    Route = "/Leiloes/Listar",
                    Method = HttpMethod.Get, 
                    QueryParams = new { categoriaId = 1, valor = 2.5, vdd = true },
                    Body = new Leilao { Codigo = 123, Data = DateTime.Now, Descricao = "Leilão de carros" }
                };

                var result = await _apiService.ExecuteRequestAsync(request);

                List<Leilao> leiloesCounters = new List<Leilao>();
                if(result.Success)
                {
                    leiloesCounters = JsonConvert.DeserializeObject<List<Leilao>>(result.Data.ToString());
                }

                return View(leiloesCounters);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Leilao(int codigo)
        {
            try
            {
                var request = new ApiRequest
                {
                    Route = "/Leiloes/Retornar",
                    Method = HttpMethod.Get,
                    QueryParams = new { id = codigo },
                };

                var result = await _apiService.ExecuteRequestAsync(request);

                Leilao leilao = new Leilao();

                if(result.Success)
                {
                    leilao = JsonConvert.DeserializeObject<Leilao>(result.Data.ToString());
                }

                return View(leilao);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AlterarLeilao(Leilao leilao)
        {
            try
            {
                var request = new ApiRequest
                {
                    Route = "/Leiloes/Alterar-descricao",
                    Method = HttpMethod.Put,
                    Body = new Leilao { Codigo = leilao.Codigo, Descricao = leilao.Descricao },
                };

                var result = await _apiService.ExecuteRequestAsync(request);

                if(result.Success)
                {
                    leilao = JsonConvert.DeserializeObject<Leilao>(result.Data.ToString());
                }

                // Retorne algum resultado após a operação
                return RedirectToAction("Leilao", leilao);
            }
            catch(Exception ex)
            {
                // Log a exceção e retorne uma resposta adequada
                return StatusCode(500, $"Erro: {ex.Message}");
            }
        }
    }
}
