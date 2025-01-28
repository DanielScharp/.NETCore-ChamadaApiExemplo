using ChamadaApi.Domain;
using ChamadaApi.Web.Models;
using System.Text;
using System.Text.Json;

namespace ChamadaApi.Web.services
{
    public class MyApiService : IMyApiService
    {
        private readonly HttpClient _httpClient;

        public MyApiService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("MyApiClient");
        }

        public async Task<ApiResponse> ExecuteRequestAsync(ApiRequest request)
        {
            // Monta a URL com os parâmetros de query string (se existirem)
            var url = request.Route;

            if(request.QueryParams != null)
            {
                var queryString = BuildQueryString(request.QueryParams);
                if(!string.IsNullOrEmpty(queryString))
                {
                    url += "?" + queryString;
                }
            }

            // Cria a requisição HTTP
            var httpRequest = new HttpRequestMessage(request.Method, url);

            var token = ClienteSessao.Logado.Token; // Obtém o token do cliente logado
            if(!string.IsNullOrEmpty(token))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Adiciona o corpo, se houver
            if(request.Body != null)
            {
                var json = JsonSerializer.Serialize(request.Body);
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            // Envia a requisição
            var response = await _httpClient.SendAsync(httpRequest);

            if(!response.IsSuccessStatusCode)
            {
                // Log e tratamento de erro
                throw new HttpRequestException($"Erro ao consumir a API: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if(apiResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize the API response.");
            }

            return apiResponse;



        }

        public static string BuildQueryString(object queryParams)
        {
            if(queryParams == null)
                return string.Empty;

            var properties = queryParams.GetType().GetProperties()
                .Where(p => p.GetValue(queryParams) != null)
                .Select(p => $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(queryParams)!.ToString()!)}");

            return string.Join("&", properties);
        }


    }

}
