using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackoutGuardianConsole.Models
{
    public class Alerta
    {
        public int Id { get; set; }
        public required string Mensagem { get; set; }
        public required string Tipo { get; set; }
        public DateTime DataHora { get; set; }
    }
}