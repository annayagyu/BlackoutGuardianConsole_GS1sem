using BlackoutGuardianConsole.Models;
using System;
using System.Collections.Generic;

namespace BlackoutGuardianConsole.Services
{
    public class RelatorioService
    {
        private readonly List<FalhaEnergia> _falhas;

        public RelatorioService(List<FalhaEnergia> falhas)
        {
            _falhas = falhas ?? throw new ArgumentNullException(nameof(falhas));
        }

        public void GerarRelatorio()
        {
            Console.WriteLine("\n----- RELATÓRIO DE FALHAS -----");
            Console.WriteLine($"Total de falhas: {_falhas.Count}");
            Console.WriteLine("--------------------------------");

            foreach (var falha in _falhas)
            {
                Console.WriteLine($"ID: {falha.Id}");
                Console.WriteLine($"Local: {falha.Localizacao}");
                Console.WriteLine($"Início: {falha.DataInicio:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Severidade: {falha.Severidade}");
                Console.WriteLine("--------------------------------");
            }
        }
    }
}