using BlackoutGuardianConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackoutGuardianConsole.Services
{
    public class LoginService
    {
        private readonly List<Usuario> _usuarios = new()
        {
            new Usuario {
                Id = 1,
                Nome = "Administrador",
                Login = "RM550360",
                Senha = "090603",
                Tipo = "Admin"
            },
            new Usuario {
                Id = 2,
                Nome = "Usuário Comum",
                Login = "user",
                Senha = "user123",
                Tipo = "UsuarioComum"
            }
        };

        public Usuario FazerLogin(string login, string senha)
        {
            var usuario = _usuarios.FirstOrDefault(u =>
                u.Login.Equals(login, StringComparison.Ordinal) &&
                u.Senha == senha);

            return usuario ?? throw new UnauthorizedAccessException("Credenciais inválidas");
        }
    }
}