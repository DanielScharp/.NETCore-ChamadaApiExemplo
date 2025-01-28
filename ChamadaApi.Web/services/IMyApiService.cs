using ChamadaApi.Domain;
using ChamadaApi.Web.Models;

namespace ChamadaApi.Web.services
{
    public interface IMyApiService
    {
        Task<ApiResponse> ExecuteRequestAsync(ApiRequest request);
    }
}
