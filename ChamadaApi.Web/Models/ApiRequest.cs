namespace ChamadaApi.Web.Models
{
    public class ApiRequest
    {
        public string Route { get; set; } = string.Empty; // Rota do endpoint
        public object? QueryParams { get; set; } // Parâmetros de query string como um objeto
        public HttpMethod Method { get; set; } = HttpMethod.Get; // Método HTTP
        public object? Body { get; set; } // Corpo da requisição (se aplicável)
    }


}
