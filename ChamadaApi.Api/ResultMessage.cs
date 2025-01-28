namespace ChamadaApi.Api
{
    public class ResultMessage
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ResultMessage Sucesso(int id, object data) => new() { Id = id, Success = true, Message = "Operação realizada com sucesso.", Data = data };
        public static ResultMessage Erro(object data) => new() { Id = 0, Success = false, Message = "Erro ao tentar realizar a operação.", Data = new { error = data } };


    }
}
