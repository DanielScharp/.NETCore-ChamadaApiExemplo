using ChamadaApi.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChamadaApi.Api.Services
{
    public static class TokenService
    {

        /// Aula do balta sobre JWT
        /// Ref:
        /// https://www.youtube.com/watch?v=vAUXU0YIWlU 


        //Verificar a possibilidade de usar refresh token, mas dificilmente o usuário passará 12 horas no site.
        //Provavelmente a sessão do cookie cairá ao fechar o navegador e ele terá que fazer um novo login

        public static string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Environment.GetEnvironmentVariable("Dashboard_TokenSecret");

            if(string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("A chave secreta do token não foi configurada.");
            }

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim ("ClienteId", usuario.UsuarioId.ToString()),
                        new Claim ("Apelido", usuario.Apelido),
                        //new Claim (ClaimTypes.Role, usuario.Role), //Cliente Ou Admin
                    }),

                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

    }

}
