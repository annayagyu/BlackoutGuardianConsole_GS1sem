using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackoutGuardianConsole.Models
{
    public class FalhaEnergia
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }  // Novo campo
        public required string Localizacao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public required string Severidade { get; set; }
    }
}