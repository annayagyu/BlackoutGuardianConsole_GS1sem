using BlackoutGuardianConsole.Models;
using BlackoutGuardianConsole.Services;
using BlackoutGuardianConsole.Utils;
using System.Text.Json;

namespace BlackoutGuardianConsole
{
    class Program
    {
        private static readonly LoginService _loginService = new();
        private static readonly FalhaService _falhaService = new();
        private static readonly AlertaService _alertaService = new();
        private static readonly RedeOfflineService _redeOffline = new();
        private static readonly MensagemService _mensagemService = new();
        private static Usuario? _usuarioAtual;

        static void Main(string[] args)
        {
            try
            {
                ConfigurarConsole();
                CarregarDados();
                RealizarLogin();
                ExecutarMenuPrincipal();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nERRO CRÍTICO: {ex.Message}");
                Console.ResetColor();
                Logger.Log($"Erro fatal: {ex.Message}");
            }
            finally
            {
                SalvarDados();
                Console.WriteLine("\nSistema encerrado. Pressione qualquer tecla para sair...");
                Console.ReadKey();
            }
        }

        #region Configuração
        static void ConfigurarConsole()
        {
            Console.Title = "Blackout Guardian - Sistema de Gerenciamento de Falhas";
            Console.WriteLine("=== BLACKOUT GUARDIAN ===");
        }

        static void CarregarDados()
        {
            try
            {
                _falhaService.CarregarDados();
                _alertaService.CarregarDados();
                _redeOffline.CarregarDados();
                _mensagemService.CarregarDados();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso: Erro ao carregar dados. {ex.Message}");
            }
        }

        static void SalvarDados()
        {
            try
            {
                _falhaService.SalvarDados();
                _alertaService.SalvarDados();
                _redeOffline.SalvarDados();
                _mensagemService.SalvarDados();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Aviso: Erro ao salvar dados. {ex.Message}");
            }
        }
        #endregion

        #region Autenticação
        static void RealizarLogin()
        {
            while (_usuarioAtual == null)
            {
                try
                {
                    Console.WriteLine("\n--- LOGIN ---");
                    string login = ObterInput("Usuário: ", obrigatorio: true)!;
                    string senha = ObterInput("Senha: ", obrigatorio: true)!;

                    _usuarioAtual = _loginService.FazerLogin(login, senha);
                    _redeOffline.ConfigurarUsuario(_usuarioAtual.Nome);
                    Console.WriteLine($"\nBem-vindo, {_usuarioAtual.Nome}!");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nERRO: {ex.Message}");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nERRO: {ex.Message}");
                }
            }
        }
        #endregion

        #region Menu Principal
        static void ExecutarMenuPrincipal()
        {
            bool executando = true;
            while (executando)
            {
                try
                {
                    Console.WriteLine("\n--- MENU PRINCIPAL ---");
                    Console.WriteLine("1. Registrar falha de energia");
                    Console.WriteLine("2. Gerar alerta");
                    Console.WriteLine("3. Visualizar falhas");
                    Console.WriteLine("4. Visualizar alertas");
                    Console.WriteLine("5. Enviar mensagem");
                    Console.WriteLine("6. Ver minhas mensagens");
                    Console.WriteLine("7. Modo Offline");
                    Console.WriteLine("8. Sair");
                    Console.Write("Opção: ");

                    switch (Console.ReadLine()?.Trim())
                    {
                        case "1": RegistrarFalha(); break;
                        case "2": GerarAlertaPersonalizado(); break;
                        case "3": VisualizarFalhas(); break;
                        case "4": VisualizarAlertas(); break;
                        case "5": EnviarMensagem(); break;
                        case "6": VisualizarMinhasMensagens(); break;
                        case "7": IniciarModoOffline(); break;
                        case "8": executando = false; break;
                        default: throw new ArgumentException("Opção inválida! Use 1-8.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nERRO: {ex.Message}");
                    Console.ResetColor();
                    Logger.Log($"Erro no menu: {ex.Message}");
                }
            }
        }
        #endregion

        #region Funcionalidades
        static void RegistrarFalha()
        {
            if (_usuarioAtual == null) return;

            Console.WriteLine("\n--- REGISTRO DE FALHA DE ENERGIA ---");

            string local = ObterInputValidado<string>(
                prompt: "Localização (sem números): ",
                validacao: input =>
                {
                    if (ValidacaoUtils.ValidarLocalizacao(input, out string? erro))
                        return (input, null);
                    return (null, erro ?? "Erro desconhecido");
                },
                mensagemErroPadrao: "Use apenas letras, espaços e hífens."
            )!;

            DateTime data = ObterInputValidado<DateTime>(
                prompt: "Data (dd/MM/yyyy HH:mm:ss): ",
                validacao: input =>
                {
                    if (ValidacaoUtils.ValidarDataHora(input, out DateTime dt, out string? erro))
                        return (dt, null);
                    return (default, erro ?? "Erro desconhecido");
                },
                mensagemErroPadrao: "Formato inválido. Exemplo: 01/01/2023 14:30:00"
            );

            string severidade = ObterInputValidado<string>(
                prompt: "Severidade (Leve/Moderada/Informativo): ",
                validacao: input =>
                {
                    if (ValidacaoUtils.ValidarSeveridade(input, out string? erro))
                        return (input, null);
                    return (null, erro ?? "Erro desconhecido");
                },
                mensagemErroPadrao: "Opções válidas: Leve, Moderada ou Informativo"
            )!;

            _falhaService.RegistrarFalha(local, data, severidade, _usuarioAtual.Id);
        }

        static void GerarAlertaPersonalizado()
        {
            Console.WriteLine("\n--- GERAR ALERTA ---");
            string mensagem = ObterInput("Mensagem: ", obrigatorio: true)!;
            string tipo = ObterInput("Tipo (Urgente/Importante/Informativo): ", obrigatorio: true)!;

            _alertaService.GerarAlerta(mensagem, tipo);
        }

        static void VisualizarFalhas()
        {
            if (_usuarioAtual == null) return;

            var falhas = _usuarioAtual.Tipo == "Admin"
                ? _falhaService.GetFalhas()
                : _falhaService.GetFalhasPorUsuario(_usuarioAtual.Id);

            Console.WriteLine("\n=== FALHAS DE ENERGIA ===");
            foreach (var falha in falhas.OrderBy(f => f.DataInicio))
            {
                Console.WriteLine($"[{falha.DataInicio:dd/MM/yyyy HH:mm}] {falha.Localizacao} - {falha.Severidade}");
            }
        }

        static void VisualizarAlertas()
        {
            var alertas = _alertaService.GetAlertas();
            Console.WriteLine("\n=== HISTÓRICO DE ALERTAS ===");
            foreach (var alerta in alertas.OrderBy(a => a.DataHora))
            {
                Console.WriteLine($"[{alerta.DataHora:dd/MM/yyyy HH:mm:ss}] {alerta.Tipo}: {alerta.Mensagem}");
            }
        }

        static void EnviarMensagem()
        {
            if (_usuarioAtual == null) return;

            Console.WriteLine("\n--- ENVIAR MENSAGEM ---");
            string destinatario = ObterInput("Destinatário: ", obrigatorio: true)!;
            string conteudo = ObterInput("Mensagem: ", obrigatorio: true)!;

            _mensagemService.EnviarMensagem(
                _usuarioAtual.Id,
                _usuarioAtual.Nome,
                destinatario,
                conteudo);

            Console.WriteLine("Mensagem enviada com sucesso!");
        }

        static void VisualizarMinhasMensagens()
        {
            if (_usuarioAtual == null) return;

            var mensagens = _mensagemService.GetMensagensPorUsuario(_usuarioAtual.Id);
            Console.WriteLine("\n=== MINHAS MENSAGENS ===");
            foreach (var msg in mensagens.OrderBy(m => m.DataHora))
            {
                Console.WriteLine($"[{msg.DataHora:dd/MM HH:mm}] Para {msg.Destinatario}: {msg.Conteudo}");
            }
        }

        static void IniciarModoOffline()
        {
            Console.WriteLine("\n=== MODO OFFLINE ===");
            Console.WriteLine("Simulando rede mesh via Bluetooth/Wi-Fi Direct...");

            while (true)
            {
                Console.WriteLine("\n1. Enviar mensagem offline");
                Console.WriteLine("2. Ver mensagens offline");
                Console.WriteLine("3. Voltar");
                Console.Write("Opção: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1":
                        string msg = ObterInput("Digite sua mensagem: ", obrigatorio: true)!;
                        string destinatario = ObterInput("Destinatário (ou 'Todos'): ") ?? "Todos";
                        _redeOffline.EnviarMensagem(msg, destinatario);
                        break;
                    case "2":
                        _redeOffline.ListarMensagens();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            }
        }
        #endregion

        #region Helpers
        static T? ObterInputValidado<T>(string prompt, Func<string, (T? resultado, string? erro)> validacao, string mensagemErroPadrao)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Entrada não pode ser vazia");
                    continue;
                }

                var (resultado, erro) = validacao(input);
                if (erro == null && resultado != null)
                {
                    return resultado;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERRO: {erro ?? mensagemErroPadrao}");
                Console.ResetColor();
            }
        }

        static string? ObterInput(string prompt, bool obrigatorio = false)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();

                if (!obrigatorio || !string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Este campo é obrigatório!");
            }
        }
        #endregion
    }
}