Blackout Guardian - Sistema de Gerenciamento de Falhas de Energia

Finalidade do Sistema
O Blackout Guardian é uma solução completa para monitoramento e gestão de falhas de energia, desenvolvido em C# como aplicação de console. O sistema oferece:

Registro e acompanhamento de falhas de energia

Sistema de alertas prioritários

Relatórios personalizados por tipo de usuário

Comunicação offline entre usuários

Autenticação segura com dois níveis de acesso (Admin/Usuário Comum)

Estrutura do Projeto
![{155975F8-420C-4C80-A365-7027B4B98EAE}](https://github.com/user-attachments/assets/42a6ed4d-abf9-41a9-91e1-5c8d3fb5fea5)

Instruções de Execução
Pré-requisitos
.NET 6.0 SDK ou superior

Visual Studio 2022 (recomendado) ou VS Code
------------------------------

Como executar
Clone o repositório:

bash
git clone [URL_DO_REPOSITORIO]
Navegue até a pasta do projeto:

bash
cd BlackoutGuardianConsole
Execute o projeto:

bash
dotnet run
Ou abra o arquivo .sln no Visual Studio e pressione F5.
------------------------------

Credenciais de Acesso
Administrador:

Usuário:  RM550360

Senha: 090603
------------------------------

Usuário Comum:

Usuário: user

Senha: user123
------------------------------

Dependências
Principais Pacotes
System.Text.Json - Para serialização dos dados

Microsoft.NET.Sdk - SDK base do projeto

-------------------------------

Armazenamento de Dados
O sistema utiliza arquivos JSON para persistência:

falhas.json - Registro de todas as falhas de energia

alertas.json - Histórico de alertas gerados

mensagens.json - Mensagens entre usuários

mensagens_offline.json - Comunicação em modo offline

logs.txt - Registro de atividades do sistema

-------------------------------

Recursos Avançados
Modo Offline:

Simula rede mesh via Bluetooth/Wi-Fi Direct

Mensagens armazenadas localmente até reconexão

Validações Robustas:

Formato estrito de datas (dd/MM/yyyy HH:mm:ss)

Tipos de severidade pré-definidos

Localização sem caracteres numéricos

-------------------------------

Segurança:

Separação de acessos por tipo de usuário

Dados sensíveis nunca armazenados em texto puro

------------------------------

Integrantes:

RM 550360  | Anna heloisa Soto Yagyu
RM 99275    | Breno da Silva Santos
RM 99679    | Gustavo Kawamura Christofani
------------------------------ 

Licença
Este projeto está licenciado sob a MIT License.
