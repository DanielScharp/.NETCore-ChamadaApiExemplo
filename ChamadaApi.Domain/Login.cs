using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Domain
{
    public class Login
    {
        public string Apelido { get; set; }
        public string Password { get; set; }
        public string IpOrigem { get; set; }
        public string AppOrigem { get; set; }

    }
}
