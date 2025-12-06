using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoPOOMaterialDeConstrucaoGustavoSavio.Modelos
{
    public class Endereco
    {
        public int ID_endereco { get; set; }
        public string estado { get; set; }
        public string rua { get; set; }
        public string referencia { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public string logradouro { get; set; }
    }
}
