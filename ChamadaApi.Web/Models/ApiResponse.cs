namespace ChamadaApi.Web.Models
{
    public class ApiResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
