# 🌍 GloboClima Frontend

Uma aplicação web moderna e responsiva para consulta de informações climáticas e geográficas, desenvolvida em ASP.NET Core MVC com design futurista e interface intuitiva.

## 📋 Sobre o Projeto

O **GloboClima Frontend** é uma aplicação web que permite aos usuários consultar informações climáticas de cidades ao redor do mundo e obter dados detalhados sobre países. Com um design moderno inspirado em glass morphism e uma paleta de cores vibrante, a aplicação oferece uma experiência de usuário excepcional tanto em desktop quanto em dispositivos móveis.

### 🎯 Objetivo Principal

Fornecer uma interface intuitiva e moderna para:
- Consultar condições climáticas em tempo real de qualquer cidade
- Obter informações detalhadas sobre países
- Gerenciar listas de cidades e países favoritos
- Proporcionar uma experiência visual atrativa e responsiva

## ✨ Principais Funcionalidades

### 🏙️ **Consultar Clima por Cidade**
- Busca de informações climáticas em tempo real
- Dados detalhados de temperatura, umidade, condições atmosféricas
- Interface de busca intuitiva e responsiva

### 🗺️ **Consultar Informações por País**
- Informações geográficas e demográficas completas
- Dados sobre localização, população, idioma e moeda
- Interface otimizada para visualização de dados

### ⭐ **Gerenciar Cidades Favoritas**
- Adicionar e remover cidades da lista de favoritos
- Acesso rápido às cidades mais consultadas
- Interface modernizada com layout otimizado

### 🌟 **Gerenciar Países Favoritos**
- Sistema completo de favoritos para países
- Organização eficiente das consultas frequentes
- Design consistente com o resto da aplicação

### 🎨 **Layout Responsivo e Design Moderno**
- **Menu Lateral Inteligente**: Navegação fluida com animações suaves
- **Paleta de Cores Consistente**: Roxo vibrante (#a020f0) como cor principal
- **Glass Morphism**: Efeitos visuais modernos com blur e transparência
- **Animações CSS3**: Transições suaves e interações dinâmicas
- **Responsividade Total**: Adaptação perfeita para todos os dispositivos
- **Tipografia Moderna**: Orbitron para títulos e Inter para textos

## 🚀 Deploy

A aplicação está disponível online através do Railway:

**🔗 [GloboClima Frontend - Deploy](https://globoclimafrontend-production-c8e1.up.railway.app/)**

## 💻 Como Rodar o Projeto Localmente

### 📋 Pré-requisitos

- **.NET 6.0 SDK** ou superior
- **Visual Studio Code** (recomendado) ou Visual Studio
- **Git** para clonagem do repositório

### 🏃‍♂️ Executando o Projeto

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/mthz99/GloboClimaFrontend.git
   cd GloboClimaFrontend
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

4. **Acesse no navegador:**
   ```
   http://localhost:5000
   ```

## ⚙️ Configuração do Backend

### 🌐 URL da API

A aplicação consome dados da seguinte API:
```
https://c6ks654l19.execute-api.us-east-1.amazonaws.com/prod
```

### 🔐 Configuração JWT

As credenciais de autenticação estão configuradas no `appsettings.json`:

```json
{
  "Jwt": {
    "Username": "mtt",
    "Password": "rogeriocenimaiordetodos"
  }
}
```

## 🏗️ Estrutura do Projeto

```
GloboClimaFrontend/
├── Controllers/           # Controladores MVC
│   ├── HomeController.cs          # Dashboard e navegação principal
│   ├── LoginController.cs         # Autenticação e sessões
│   ├── ConsultController.cs       # Consultas de clima e países
│   ├── FavoriteCitysController.cs # Gerenciamento de cidades favoritas
│   └── FavoriteCountryController.cs # Gerenciamento de países favoritos
├── Views/                # Views Razor
│   ├── Home/                      # Páginas principais
│   │   ├── Index.cshtml          # Dashboard modernizado
│   │   ├── ConsultarClimaCidade.cshtml # Consulta de clima
│   │   ├── ConsultarPais.cshtml  # Consulta de países
│   │   ├── FavoritesCidades.cshtml # Lista de cidades favoritas
│   │   └── FavoritesPaises.cshtml # Lista de países favoritos
│   ├── Login/                     # Sistema de autenticação
│   │   ├── Index.cshtml          # Página inicial de login
│   │   └── LoginUser.cshtml      # Formulário de login moderno
│   ├── _Layout.cshtml            # Layout base da aplicação
│   └── _ViewImports.cshtml       # Importações globais
├── wwwroot/              # Recursos estáticos
│   ├── css/                      # Estilos customizados
│   ├── font/                     # Fontes e ícones (Bootstrap Icons)
│   └── favicon.ico               # Ícone da aplicação
├── Services/             # Serviços da aplicação
│   └── JwtService.cs             # Serviço de autenticação JWT
├── Properties/           # Configurações do projeto
│   └── launchSettings.json       # Configurações de execução
├── appsettings.json      # Configurações da aplicação
└── Program.cs            # Ponto de entrada da aplicação
```

## 🛠️ Tecnologias Utilizadas

### Frontend
- **HTML5 & CSS3** - Estrutura e estilização
- **JavaScript ES6+** - Interatividade e animações
- **Bootstrap Icons** - Iconografia moderna
- **Google Fonts** - Tipografia (Orbitron + Inter)
- **Font Awesome** - Ícones complementares

### Design & UX
- **Glass Morphism** - Efeitos visuais modernos
- **CSS3 Animations** - Animações e transições
- **Responsive Design** - Adaptação para todos os dispositivos
- **Dark Theme** - Paleta de cores escura e vibrante

### Deploy & DevOps
- **Railway** - Plataforma de deploy
- **Git** - Controle de versão
- **GitHub** - Repositório remoto

## 🎨 Boas Práticas Adotadas

### 📝 **Organização de Código**
- **Separação de Responsabilidades**: Controllers focados em lógica de negócio, Views para apresentação
- **Nomenclatura Consistente**: Padrão de nomenclatura claro em classes, métodos e variáveis
- **Comentários Úteis**: Documentação focada na explicação de funcionalidades complexas
- **Estrutura de Pastas Lógica**: Organização que facilita manutenção e escalabilidade

### 📱 **Responsividade**
- **Mobile-First Design**: Desenvolvimento priorizando dispositivos móveis
- **Breakpoints Estratégicos**: Sistema de breakpoints para diferentes tamanhos de tela
- **Layout Flexível**: Uso de CSS Grid e Flexbox para layouts adaptativos
- **Otimização de Performance**: CSS otimizado para carregamento rápido

### 🔄 **Reaproveitamento de Componentes**
- **Classes CSS Reutilizáveis**: Sistema de classes CSS modulares (prefixo `gc-`)
- **Layouts Consistentes**: Template base para manter consistência visual
- **Partial Views**: Componentização para reutilização de elementos

### 🎨 **Padrão de Design**
- **Sistema de Design Coeso**: Paleta de cores, tipografia e espaçamentos consistentes
- **Feedback Visual**: Estados de hover, loading e erro bem definidos
- **Acessibilidade**: Contraste adequado e navegação por teclado
- **Experiência do Usuário**: Fluxos intuitivos e feedback claro

### 🔒 **Segurança**
- **Autenticação JWT**: Sistema seguro de autenticação
- **Validação de Sessão**: Verificação de sessões ativas
- **Tratamento de Erros**: Gestão adequada de erros e exceções

### ⚡ **Performance**
- **Otimização de Assets**: CSS e JavaScript minificados
- **Carregamento Assíncrono**: Requisições HTTP não-bloqueantes
- **Cache Estratégico**: Uso eficiente de cache do navegador

## 👥 Contribuição

Este projeto foi desenvolvido com foco na qualidade, usabilidade e manutenibilidade. Sugestões e melhorias são sempre bem-vindas!

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

**Desenvolvido com 💜 utilizando tecnologias modernas e boas práticas de desenvolvimento**
