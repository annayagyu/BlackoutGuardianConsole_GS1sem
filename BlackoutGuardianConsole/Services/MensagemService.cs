using BlackoutGuardianConsole.Models;
using BlackoutGuardianConsole.Utils;
using System.Text.Json;

namespace BlackoutGuardianConsole.Services
{
    public class MensagemService
    {
        private const string ArquivoDados = "mensagens.json";
        private List<Mensagem> _mensagens = new();

        public void EnviarMensagem(int remetenteId, string remetenteNome, string destinatario, string conteudo)
        {
            _mensagens.Add(new Mensagem
            {
                Id = _mensagens.Count + 1,
                RemetenteId = remetenteId,
                RemetenteNome = remetenteNome,
                Destinatario = destinatario,
                Conteudo = conteudo,
                DataHora = DateTime.Now,
                Offline = false
            });
            SalvarDados();
        }

        public List<Mensagem> GetMensagensPorUsuario(int usuarioId) =>
            _mensagens.Where(m => m.RemetenteId == usuarioId && !m.Offline).ToList();

        public void CarregarDados()
        {
            try
            {
                if (File.Exists(ArquivoDados))
                {
                    string json = File.ReadAllText(ArquivoDados);
                    _mensagens = JsonSerializer.Deserialize<List<Mensagem>>(json) ?? new();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao carregar mensagens: {ex.Message}");
            }
        }

        public void SalvarDados()
        {
            try
            {
                string json = JsonSerializer.Serialize(_mensagens);
                File.WriteAllText(ArquivoDados, json);
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao salvar mensagens: {ex.Message}");
            }
        }
    }
}