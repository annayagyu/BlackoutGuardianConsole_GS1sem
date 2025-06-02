using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackoutGuardianConsole.Models
{
    public class Mensagem
    {
        public int Id { get; set; }
        public int RemetenteId { get; set; }
        public required string RemetenteNome { get; set; }
        public required string Destinatario { get; set; }
        public required string Conteudo { get; set; }
        public DateTime DataHora { get; set; }
        public bool Offline { get; set; }
    }
}