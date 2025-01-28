namespace ChamadaApi.Web
{
    public class HttpContextAccessorHelper
    {

        private static IHttpContextAccessor _httpContextAccessor;

        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor != null ? _httpContextAccessor.HttpContext : null;
    }
}
