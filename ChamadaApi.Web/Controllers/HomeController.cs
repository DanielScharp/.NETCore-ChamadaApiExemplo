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
                    Body = new Leilao { Codigo = 123, Data = DateTime.Now, Descricao = "Leilão de carros" }
                    //QueryParams = new { categoriaId = 1, valor = 2.5, vdd = true }, Exemplo um ou mais parametros
                    //Body = new Leilao { Codigo = 123, Data = DateTime.Now, Descricao = "Leilão de carros" } Exemplo de uma class
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
        public async Task<IActionResult> RetornarLeilao(int codigo)
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

                return View("Leilao", leilao);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> InserirAlterarLeilao(Leilao leilao)
        {
            try
            {
                var request = new ApiRequest();

                if(leilao.Codigo == 0)
                {
                    request.Route = "/Leiloes/Inserir";
                    request.Method = HttpMethod.Post;
                } 
                else
                {
                    request.Route = "/Leiloes/Alterar";
                    request.Method = HttpMethod.Put;
                }

                request.Body = leilao;

                var result = await _apiService.ExecuteRequestAsync(request);

                if(result.Success)
                {
                    leilao = JsonConvert.DeserializeObject<Leilao>(result.Data.ToString());
                }

                return Json(
                    new ApiResponse
                    {
                        Success = result.Success,
                        Message = result.Message,
                        Data = leilao
                    }
                );
            }
            catch(Exception ex)
            {
                return Json(
                    new ApiResponse
                    {
                        Success = false,
                        Message = "Erro ao alterar o leilão. Tente novamente mais tarde.",
                        Data = ex.Data
                    }
                );
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirLeilao(int codigo)
        {
            try
            {
                var request = new ApiRequest
                {
                    Route = "/Leiloes/Excluir",
                    Method = HttpMethod.Delete,
                    QueryParams = new { id = codigo },
                };

                var result = await _apiService.ExecuteRequestAsync(request);

                return Json(result);
            }
            catch(Exception ex)
            {
                return Json(
                    new ApiResponse { 
                        Success = false, 
                        Message = "Erro ao excluir o leilão. Tente novamente mais tarde.",
                        Data = ex.Data
                    }
                );
            }
        }

        public IActionResult Error(int id = 0, string? message = null)
        {
            var modelError = new ErrorViewModel
            {
                ErrorCode = id,
                Message = message ?? "Houve um erro ao processar a rotina! Tente novamente mais tarde ou contate nosso suporte.",
                Title = "Algo deu errado!"
            };

            switch(id)
            {
                case 500:
                    modelError.Title = "Ocorreu um erro!";
                    modelError.Message = message ?? "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                break;
                case 400:
                    modelError.Title = "Algo deu errado!";
                    modelError.Message = message ?? "Houve um erro ao processar a rotina! Tente novamente mais tarde ou contate nosso suporte.";
                break;
                case 404:
                    modelError.Title = "Ops! Página não encontrada.";
                    modelError.Message = message ?? "A página que está procurando não existe! <br/>Em caso de dúvidas entre em contato com nosso suporte.";
                break;
                case 503:
                    modelError.Title = "Serviço Indisponível";
                    modelError.Message = message ?? "O serviço solicitado está indisponível no momento. <br/>Por favor, tente novamente mais tarde.";
                break;
            }
            return View(modelError);

        }

    }
}
