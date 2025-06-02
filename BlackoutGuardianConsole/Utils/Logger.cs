using System;
using System.IO;
using System.Threading;

namespace BlackoutGuardianConsole.Utils
{
    public static class Logger
    {
        private static readonly string CaminhoLog = "logs.txt";
        private static readonly object _lock = new object();

        public static void Log(string mensagem)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(CaminhoLog, $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}: {mensagem}\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Falha ao registrar log: {ex.Message}");
                }
            }
        }
    }
}