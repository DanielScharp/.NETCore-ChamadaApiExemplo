using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Domain
{
    public class Leilao
    {
        public int Codigo { get; set; }
        public DateTime Data { get; set; }
        public string? Descricao { get; set; }
    }
}
