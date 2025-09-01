# Academic Events - Sistema de Gerenciamento de Eventos Acadêmicos

## Sobre o Projeto

Este projeto foi desenvolvido como parte da disciplina de Programação Web II do curso de Análise e Desenvolvimento de Sistemas. Trata-se de uma aplicação web completa para gerenciamento de eventos acadêmicos, permitindo que usuários se inscrevam em eventos e administradores gerenciem o sistema.

## Funcionalidades Principais

### Para Usuários Comuns
- Cadastro e login de usuários
- Visualização de eventos disponíveis
- Inscrição em eventos acadêmicos
- Consulta de inscrições realizadas
- Sistema de "lembrar-me" para login automático

### Para Administradores
- Gerenciamento completo de eventos (criar, editar, excluir)
- Visualização de lista de inscritos por evento
- Controle de acesso administrativo
- Dashboard para administração do sistema

## Tecnologias Utilizadas

- **Backend**: ASP.NET Core 8.0
- **Banco de Dados**: SQL Server
- **ORM**: Entity Framework Core 9.0.8
- **Frontend**: HTML, CSS, JavaScript, Bootstrap 5.3.3
- **Autenticação**: Sistema customizado com sessões e cookies
- **Arquitetura**: MVC (Model-View-Controller)

## Estrutura do Projeto

```
ProgWebII/
├── Controllers/           # Controladores MVC
│   ├── AccountController.cs    # Autenticação e registro
│   ├── AdminController.cs      # Área administrativa
│   ├── EventsController.cs     # Gestão de eventos
│   └── HomeController.cs       # Página inicial
├── Data/                  # Contexto do banco de dados
│   ├── ApplicationDbContext.cs # Configuração do EF Core
│   └── DataSeeder.cs          # Dados iniciais
├── Models/               # Modelos de dados
│   ├── Event.cs         # Modelo de eventos
│   ├── Registration.cs   # Modelo de inscrições
│   └── User.cs          # Modelo de usuários
├── ViewModels/          # ViewModels para formulários
│   ├── LoginViewModel.cs
│   └── RegisterViewModel.cs
├── Views/               # Interface do usuário
│   ├── Account/         # Telas de login e registro
│   ├── Admin/           # Área administrativa
│   ├── Events/          # Listagem e inscrições
│   └── Shared/          # Layout compartilhado
├── Utils/               # Utilitários
│   └── PasswordHasher.cs # Criptografia de senhas
└── Migrations/          # Migrações do banco de dados
```

## Modelos de Dados

### User (Usuário)
- Id: Identificador único
- Name: Nome completo
- Email: E-mail (único)
- PasswordHash: Senha criptografada
- Salt: Salt para criptografia
- IsAdmin: Indica se é administrador
- CreatedAt: Data de criação

### Event (Evento)
- Id: Identificador único
- Name: Nome do evento
- Date: Data e hora do evento
- Description: Descrição detalhada
- Location: Local de realização

### Registration (Inscrição)
- Id: Identificador único
- UserId: Referência ao usuário
- EventId: Referência ao evento
- RegisteredAt: Data da inscrição

## Pré-requisitos

- .NET 8.0 SDK
- SQL Server (local ou Docker)
- Visual Studio 2022 ou VS Code

## Configuração e Instalação

### 1. Clone o repositório
```bash
git clone https://github.com/danielp2904/ProgWebII.git
cd ProgWebII
```

### 2. Configure o banco de dados
Edite o arquivo `appsettings.json` e ajuste a string de conexão conforme seu ambiente:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,7000;Database=ProgWebII;User Id=sa;Password=abc123@;Encrypt=True;TrustServerCertificate=True;"
  }
}
```

### 3. Execute as migrações
```bash
dotnet ef database update
```

### 4. Execute a aplicação
```bash
dotnet run
```

A aplicação estará disponível em:
- HTTP: http://localhost:5006
- HTTPS: https://localhost:7003

## Usuários Padrão

O sistema cria automaticamente dois usuários para teste:

### Administrador
- **Email**: admin@ifc.local
- **Senha**: Admin@123
- **Permissões**: Acesso total ao sistema

### Usuário Comum
- **Email**: user@ifc.local
- **Senha**: User@123
- **Permissões**: Inscrição em eventos

## Funcionalidades Técnicas Implementadas

### Autenticação e Autorização
- Sistema customizado de hash de senhas com salt
- Controle de sessão para manter usuário logado
- Sistema de cookies para "lembrar-me"
- Middleware para restauração automática de sessão
- Controle de acesso baseado em roles (admin/usuário)

### Banco de Dados
- Relacionamentos bem definidos entre entidades
- Índices únicos para email de usuário
- Índice composto para evitar inscrições duplicadas
- Migrations para versionamento do schema
- Seed de dados iniciais

### Interface de Usuário
- Design responsivo com Bootstrap 5
- Validação client-side e server-side
- Mensagens de feedback para o usuário
- Layout consistente com navegação dinâmica
- Confirmação para ações críticas

### Segurança
- Proteção contra inscrições duplicadas
- Validação de dados de entrada
- Hash seguro de senhas
- Controle de acesso a áreas administrativas

## Estrutura de Navegação

### Usuário Não Logado
- Visualização limitada
- Acesso apenas a login e registro

### Usuário Logado
- Lista de eventos disponíveis
- Inscrição em eventos
- Consulta de suas inscrições

### Administrador
- Todas as funcionalidades de usuário comum
- Criação, edição e exclusão de eventos
- Visualização de lista de inscritos por evento

## Conceitos de Programação Web Aplicados

- **MVC Pattern**: Separação clara entre Model, View e Controller
- **Entity Framework**: ORM para mapeamento objeto-relacional
- **Dependency Injection**: Injeção de dependências do ASP.NET Core
- **Session Management**: Gerenciamento de estado da aplicação
- **Form Validation**: Validação tanto client quanto server-side
- **Responsive Design**: Interface adaptável para diferentes dispositivos
- **Database Migrations**: Controle de versão do banco de dados

## Como Testar

1. Execute a aplicação
2. Acesse http://localhost:5006
3. Faça login com um dos usuários padrão
4. Teste as funcionalidades de inscrição (usuário comum)
5. Teste as funcionalidades administrativas (usuário admin)

## Próximas Melhorias

Este projeto pode ser expandido com:
- Sistema de notificações por email
- Upload de imagens para eventos
- Relatórios de participação
- API REST para integração mobile
- Sistema de avaliação de eventos
- Integração com calendários externos

## Observações Acadêmicas

Este projeto demonstra a aplicação prática dos conceitos estudados em Programação Web II, incluindo desenvolvimento full-stack, persistência de dados, autenticação, autorização e criação de interfaces responsivas. A arquitetura seguiu as melhores práticas do ASP.NET Core, proporcionando uma base sólida para projetos profissionais.

## Autor

Desenvolvido como projeto acadêmico para a disciplina de Programação Web II.

## Licença

Este projeto é destinado exclusivamente para fins educacionais.
