using ChamadaApi.Database.MySql;
using ChamadaApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Application
{
    public class UsuarioApplication
    {
        private readonly UsuarioRepository _usuarioRepository;


        //Contrutor com injeção de dependência
        public UsuarioApplication(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        //Retorna o usuário por login e senha
        public async Task<Usuario> GetAsync(Login login)
        {
            try
            {
                return await _usuarioRepository.GetAsync(login);
            }
            catch
            {
                throw;
            }
        }

    }
}
