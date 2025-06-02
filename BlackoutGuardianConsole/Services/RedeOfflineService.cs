using BlackoutGuardianConsole.Models;
using BlackoutGuardianConsole.Utils;
using System.Text.Json;

namespace BlackoutGuardianConsole.Services
{
    public class RedeOfflineService
    {
        private const string ArquivoDados = "mensagens_offline.json";
        private List<Mensagem> _mensagens = new();
        private string? _nomeUsuario;

        public void ConfigurarUsuario(string nome)
        {
            _nomeUsuario = nome ?? throw new ArgumentNullException(nameof(nome));
        }

        public void EnviarMensagem(string conteudo, string destinatario = "Todos")
        {
            if (_nomeUsuario == null)
                throw new InvalidOperationException("Usuário não configurado");

            _mensagens.Add(new Mensagem
            {
                Id = _mensagens.Count + 1,
                RemetenteId = -1,
                RemetenteNome = _nomeUsuario,
                Destinatario = destinatario,
                Conteudo = conteudo,
                DataHora = DateTime.Now,
                Offline = true
            });
            SalvarDados();
        }

        public void ListarMensagens()
        {
            Console.WriteLine("\n=== MENSAGENS OFFLINE ===");
            foreach (var msg in _mensagens.OrderBy(m => m.DataHora))
            {
                Console.WriteLine($"[{msg.DataHora:HH:mm:ss}] {msg.RemetenteNome} > {msg.Destinatario}: {msg.Conteudo}");
            }
        }

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
                Logger.Log($"Erro ao carregar mensagens offline: {ex.Message}");
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
                Logger.Log($"Erro ao salvar mensagens offline: {ex.Message}");
            }
        }
    }
}