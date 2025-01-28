using ChamadaApi.Domain;

namespace ChamadaApi.Web.Models
{
    public class ClienteSessao
    {
        //Inicializa o HttpContextAccessorHelper ao efetuar o login

        public static LoginResult Logado
        {
            get
            {
                return GetCookie();
            }
        }

        private static LoginResult GetCookie()
        {
            var clienteLogin = new LoginResult();

            try
            {
                if(HttpContextAccessorHelper.Current != null && HttpContextAccessorHelper.Current.User.Identity.IsAuthenticated)
                {
                    clienteLogin.UsuarioId = Convert.ToInt32(HttpContextAccessorHelper.Current.User.FindFirst("UsuarioId").Value);
                    clienteLogin.Apelido = HttpContextAccessorHelper.Current.User.FindFirst("Apelido").Value;
                    clienteLogin.Token = HttpContextAccessorHelper.Current.User.FindFirst("TokenApi").Value;

                }
            }
            catch
            {
                //Não faz nada, só vai retornar um objeto vazio
            }

            return clienteLogin;

        }


    }
}
