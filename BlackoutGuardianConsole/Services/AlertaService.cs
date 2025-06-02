using BlackoutGuardianConsole.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BlackoutGuardianConsole.Services
{
    public class AlertaService
    {
        private const string ArquivoDados = "alertas.json";
        private List<Alerta> _alertas;

        public AlertaService()
        {
            CarregarDados();
        }

        public void GerarAlerta(string mensagem, string tipo)
        {
            _alertas.Add(new Alerta
            {
                Id = _alertas.Count + 1,
                Mensagem = mensagem,
                Tipo = tipo,
                DataHora = DateTime.Now
            });
            SalvarDados();
            Console.WriteLine($"ALERTA ({tipo.ToUpper()}): {mensagem}");
        }

        public List<Alerta> GetAlertas() => _alertas;

        public void CarregarDados()
        {
            try
            {
                if (File.Exists(ArquivoDados))
                {
                    string json = File.ReadAllText(ArquivoDados);
                    _alertas = JsonSerializer.Deserialize<List<Alerta>>(json) ?? new List<Alerta>();
                }
                else
                {
                    _alertas = new List<Alerta>();
                }
            }
            catch
            {
                _alertas = new List<Alerta>();
            }
        }

        public void SalvarDados()
        {
            try
            {
                string json = JsonSerializer.Serialize(_alertas);
                File.WriteAllText(ArquivoDados, json);
            }
            catch
            {
                // Logar erro se necessário
            }
        }
    }
}