# 📋 Documentação Técnica - GloboClima Frontend

## 🏗️ Arquitetura da Aplicação

### Padrão MVC (Model-View-Controller)
- **Models**: Representação de dados via classes C# e DTOs
- **Views**: Interface de usuário em Razor Pages (.cshtml)
- **Controllers**: Lógica de negócio e roteamento de requisições

### Fluxo de Autenticação
1. **Login** → `LoginController.LoginUser()`
2. **Validação JWT** → `JwtService.cs`
3. **Criação de Sessão** → `HttpContext.Session`
4. **Middleware de Autorização** → Verificação em cada controller

## 🎨 Sistema de Design

### Paleta de Cores
```css
--primary-color: #a020f0;     /* Roxo vibrante */
--secondary-color: #7c3aed;   /* Roxo escuro */
--background: #0f0f0f;        /* Preto profundo */
--surface: rgba(255,255,255,0.1); /* Glass effect */
--text-primary: #ffffff;      /* Branco */
--text-secondary: #b3b3b3;    /* Cinza claro */
```

### Tipografia
- **Display/Títulos**: Orbitron (400, 700, 900)
- **Corpo/Textos**: Inter (400, 500, 600)

### Prefixo CSS
Todas as classes customizadas utilizam o prefixo `gc-` (GloboClima):
- `gc-container`, `gc-btn`, `gc-form`, `gc-card`, etc.

## 🔧 Configurações Técnicas

### Dependências Principais
```xml
<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="6.0.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
```

### Configuração de Sessão
```csharp
// Startup.cs
services.AddSession(options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
```

### API Integration
- **Base URL**: `https://c6ks654l19.execute-api.us-east-1.amazonaws.com/prod`
- **Autenticação**: HTTP Basic Auth via JWT
- **Content-Type**: `application/json`

## 📱 Responsividade

### Breakpoints Sistema
```css
/* Mobile */
@media (max-width: 768px) { width: 98%; }

/* Tablet */
@media (min-width: 768px) and (max-width: 1024px) { width: 95%; }

/* Desktop */
@media (min-width: 1024px) { width: 90%; }

/* Large Desktop */
@media (min-width: 1440px) { width: 85%; }
```

## 🛣️ Roteamento

### Principais Rotas
```
GET  /                    → Login.Index (redirect to /login)
GET  /login              → Login.LoginUserGet
POST /login              → Login.LoginUser
GET  /Home               → Home.Index
GET  /Consult/Cidade     → Consult.ConsultarClimaCidade
GET  /Consult/Pais       → Consult.ConsultarPais
GET  /Favorites/Cidades  → FavoriteCitys.GetFavoriteCitysList
GET  /Favorites/Paises   → FavoriteCountry.GetFavoriteCountryList
POST /Login/Logout       → Login.Logout
```

## 🔐 Segurança

### Validação de Sessão
```csharp
if (HttpContext.Session.GetString("Username") == null)
{
    return RedirectToAction("Index", "Login");
}
```

### Criptografia de Senha
```csharp
public static string EncryptSha256(string password)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
```

## ⚡ Performance

### Otimizações CSS
- Uso de `transform` e `opacity` para animações (hardware acceleration)
- `backdrop-filter: blur()` para glass morphism
- Transições CSS em vez de JavaScript quando possível

### Carregamento de Assets
- Google Fonts com `display=swap`
- Font Awesome via CDN
- CSS interno otimizado

## 🧪 Debugging

### Configuração VS Code
```json
// .vscode/launch.json
{
    "name": ".NET Core Launch (web)",
    "type": "coreclr",
    "request": "launch",
    "program": "${workspaceFolder}/bin/Debug/net6.0/GloboClimaFrontend.dll",
    "args": [],
    "cwd": "${workspaceFolder}",
    "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
    },
    "sourceFileMap": {
        "/Views": "${workspaceFolder}/Views"
    }
}
```

## 🚀 Deploy Railway

### Configuração Automática
- **Build Command**: `dotnet build --configuration Release`
- **Start Command**: `dotnet GloboClimaFrontend.dll`
- **Port**: Automático (via `$PORT`)

### Variáveis de Ambiente
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```
