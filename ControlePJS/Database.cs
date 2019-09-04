using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePJS
{
    public class Database
    {
        public  class Projeto
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public string Cliente { get; set; }
            public int Total { get; set; }
            public int Recebido { get; set; }
            public string Descricao { get; set; }
            public int Status { get; set; }
            public DateTime Entrega { get; set; }
            public DateTime DataInicio { get; set; }
        }
    }
}
