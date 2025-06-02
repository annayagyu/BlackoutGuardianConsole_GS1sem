using BlackoutGuardianConsole.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BlackoutGuardianConsole.Services
{
    public class FalhaService
    {
        private const string ArquivoDados = "falhas.json";
        private List<FalhaEnergia> _falhas;

        public FalhaService()
        {
            CarregarDados();
        }

        public void RegistrarFalha(string localizacao, DateTime data, string severidade, int usuarioId)
        {
            _falhas.Add(new FalhaEnergia
            {
                Id = _falhas.Count + 1,
                UsuarioId = usuarioId,
                Localizacao = localizacao,
                DataInicio = data,
                Severidade = severidade
            });
            SalvarDados();
        }

        public List<FalhaEnergia> GetFalhas() => _falhas;

        public List<FalhaEnergia> GetFalhasPorUsuario(int usuarioId) =>
            _falhas.Where(f => f.UsuarioId == usuarioId).ToList();

        public void CarregarDados()
        {
            try
            {
                if (File.Exists(ArquivoDados))
                {
                    string json = File.ReadAllText(ArquivoDados);
                    _falhas = JsonSerializer.Deserialize<List<FalhaEnergia>>(json) ?? new List<FalhaEnergia>();
                }
                else
                {
                    _falhas = new List<FalhaEnergia>();
                }
            }
            catch
            {
                _falhas = new List<FalhaEnergia>();
            }
        }

        public void SalvarDados()
        {
            try
            {
                string json = JsonSerializer.Serialize(_falhas);
                File.WriteAllText(ArquivoDados, json);
            }
            catch
            {
                // Logar erro se necessário
            }
        }
    }
}