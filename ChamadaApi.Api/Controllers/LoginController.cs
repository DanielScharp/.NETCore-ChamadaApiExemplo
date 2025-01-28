using ChamadaApi.Api.Services;
using ChamadaApi.Application;
using ChamadaApi.Database;
using ChamadaApi.Database.MySql;
using ChamadaApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ChamadaApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController :ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UsuarioApplication _usuarioApplication;


        public LoginController(IHttpContextAccessor httpContextAccessor, UsuarioRepository usuarioRepository)
        {
            _httpContextAccessor = httpContextAccessor;

            _usuarioApplication = new UsuarioApplication(usuarioRepository);

        }

        [Route("Acessar")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> In(Login login)
        {
            try
            {

                //Desencripta a senha
                string keyStoreDecoded = Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(login.Password));
                var password = AESEncrytDecry.DecryptStringAES(keyStoreDecoded);

                //Validações
                var apelidoValidado = login.Apelido.SafeSql();
                var pwdValidado = password.SafeSql();

                //Verifica Apelido
                if(string.IsNullOrEmpty(apelidoValidado))
                {
                    return BadRequest(ResultMessage.Erro("O parâmetro [Login] é obrigatório."));
                }


                //Verifica Senha
                if(string.IsNullOrEmpty(pwdValidado))
                {
                    return BadRequest(ResultMessage.Erro("O parâmetro [Senha] é obrigatório."));
                }

                //Atribui o login e validado ao modelo
                login.Apelido = apelidoValidado;

                //Atribui a senha descriptada e validada ao modelo
                login.Password = pwdValidado;

                var result = await _usuarioApplication.GetAsync(login);
                if(result.UsuarioId == 0)
                {
                    return NotFound(ResultMessage.Erro("Cliente não localizado com os dados informados!"));
                }


                //Gerar Token 
                var token = TokenService.GenerateToken(result);


                //Notifica email de login efetuado na aplicação


                //Achou o usuário, retorna um loginResult
                var loginResult = new LoginResult
                {
                    UsuarioId = result.UsuarioId,
                    Apelido = result.Apelido,
                    AppOrigem = login.AppOrigem,
                    IpOrigem = login.IpOrigem,
                    //Role = result.Role,
                    Token = token

                };

                return Ok(ResultMessage.Sucesso(0, loginResult));


            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }


        [Route("check-token-validation")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CheckTokenValidation()
        {
            try
            {
                return Ok(ResultMessage.Sucesso(0, "TokenValido"));
            }
            catch(Exception ex)
            {
                return new StatusCodeResult(500);
            }
        }

    }
}
