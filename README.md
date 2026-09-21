# RoyalVilla — ASP.NET Core 10 Starter Architecture

RoyalVilla is a **reusable starter / reference solution** for future ASP.NET Core applications.  
It demonstrates a three-project architecture with:

- a shared **DTO class library**,
- an **ASP.NET Core Web API** backend,
- an **ASP.NET Core MVC** frontend,
- Entity Framework Core + SQL Server,
- JWT authentication at the API,
- cookie/session authentication at the MVC application,
- typed application services using `IHttpClientFactory`,
- AutoMapper,
- OpenAPI + Scalar API documentation.

The project is intended to be copied and adapted when starting a new system so that the basic architecture does not need to be recreated each time.

> **Development environment used by this repository**
>
> - Visual Studio 2026
> - .NET 10 / ASP.NET Core 10
> - C#
> - SQL Server LocalDB for local development
> - Git / GitHub

---

## 1. Solution Overview

The solution contains three projects:

```text
RoyalVilla
│
├── RoyalVilla.DTO
│   └── Shared request/response DTO classes
│
├── RoyalVilla_API
│   └── REST API, EF Core, database models, JWT authentication
│
└── RoyalVillaWeb
    └── ASP.NET Core MVC frontend, API client services, cookies/session
```

Dependency direction:

```text
                 RoyalVilla.DTO
                  ▲          ▲
                  │          │
                  │          │
          RoyalVilla_API   RoyalVillaWeb
```

Both the API and MVC projects reference the DTO library.  
The DTO project must **not** reference the API or MVC projects.

---

# 2. Create the Blank Solution From Scratch

## Step 1 — Create the solution

In Visual Studio 2026:

```text
Create a new project
→ Blank Solution
```

Example:

```text
Solution Name: RoyalVilla
```

---

## Step 2 — Create the DTO project

Right-click the solution:

```text
Add
→ New Project
→ Class Library
```

Use:

```text
Project name: RoyalVilla.DTO
Framework: .NET 10
```

Delete the default `Class1.cs` file.

This project should contain shared DTOs only. It currently does not need extra NuGet packages.

---

## Step 3 — Create the backend API project

Right-click the solution:

```text
Add
→ New Project
→ ASP.NET Core Web API
```

Use:

```text
Project name: RoyalVilla_API
Framework: .NET 10
Authentication: None
HTTPS: Enabled
```

OpenAPI can be enabled because this solution uses OpenAPI + Scalar.

---

## Step 4 — Create the MVC frontend project

Right-click the solution:

```text
Add
→ New Project
→ ASP.NET Core Web App (Model-View-Controller)
```

Use:

```text
Project name: RoyalVillaWeb
Framework: .NET 10
Authentication: None
HTTPS: Enabled
```

Authentication is configured manually in `Program.cs` because this project uses its own API login + cookie/session flow.

---

# 3. Required Visual Studio Components

Install Visual Studio 2026 with at least:

```text
ASP.NET and web development workload
.NET 10 SDK / targeting pack
```

For this project's current database setup, SQL Server LocalDB is also required locally.

You can verify the SDK from a terminal:

```bash
dotnet --list-sdks
```

You should see a .NET 10 SDK installed.

> This solution mainly uses **NuGet packages**, not Visual Studio extensions/plugins.

---

# 4. NuGet Packages Used by the Current Source Code

## RoyalVilla.DTO

No third-party NuGet package is currently required.

---

## RoyalVilla_API

The uploaded project currently references:

```text
AutoMapper                              16.2.0
Microsoft.AspNetCore.Authentication.JwtBearer 10.0.12
Microsoft.AspNetCore.OpenApi           10.0.12
Microsoft.EntityFrameworkCore.SqlServer 10.0.12
Microsoft.EntityFrameworkCore.Tools    10.0.12
Scalar.AspNetCore                      2.17.3
```

Install from Visual Studio:

```text
Tools
→ NuGet Package Manager
→ Manage NuGet Packages for Solution
```

Or with the CLI from the API project folder:

```bash
dotnet add package AutoMapper --version 16.2.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 10.0.12
dotnet add package Microsoft.AspNetCore.OpenApi --version 10.0.12
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 10.0.12
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 10.0.12
dotnet add package Scalar.AspNetCore --version 2.17.3
```

---

## RoyalVillaWeb

The uploaded project currently references:

```text
AutoMapper                              16.2.0
JWT                                     11.1.0
Microsoft.AspNetCore.Authentication.JwtBearer 10.0.12
System.IdentityModel.Tokens.Jwt         8.23.0
```

Install from Visual Studio NuGet Package Manager or use:

```bash
dotnet add package AutoMapper --version 16.2.0
dotnet add package JWT --version 11.1.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 10.0.12
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.23.0
```

> After cloning this repository, Visual Studio normally restores packages automatically. You can also run `dotnet restore`.

---

# 5. Project References

Both application projects must reference `RoyalVilla.DTO`.

In Visual Studio:

```text
Right-click RoyalVilla_API
→ Add
→ Project Reference
→ RoyalVilla.DTO
```

Repeat for `RoyalVillaWeb`.

Expected reference:

```xml
<ProjectReference Include="..\RoyalVilla.DTO\RoyalVilla.DTO.csproj" />
```

## Important cleanup note

The uploaded `RoyalVilla_API.csproj` and `RoyalVillaWeb.csproj` currently contain **two DTO references**:

```xml
<ProjectReference Include="..\..\RoyalVilla.DTO\RoyalVilla.DTO.csproj" />
<ProjectReference Include="..\RoyalVilla.DTO\RoyalVilla.DTO.csproj" />
```

For the folder layout shown in this repository, normally only this reference is required:

```xml
<ProjectReference Include="..\RoyalVilla.DTO\RoyalVilla.DTO.csproj" />
```

Remove the duplicate/wrong path after confirming the solution builds correctly.

---

# 6. Current File Architecture

## 6.1 RoyalVilla.DTO

```text
RoyalVilla.DTO
│
├── ApiResponse.cs
├── LoginRequestDTO.cs
├── LoginResponseDTO.cs
├── RegisterationRequestDTO.cs
├── UserDTO.cs
├── VillaAmentiesCreateDTO.cs
├── VillaAmentiesDTO.cs
├── VillaAmentiesUpdateDTO.cs
├── VillaCreateDTO.cs
├── VillaDTO.cs
├── VillaUpdateDTO.cs
└── RoyalVilla.DTO.csproj
```

### Manually added files

All business DTO files above were manually added.

### Purpose

DTOs are contracts used to transfer data between the Web application and API without exposing EF Core database entities directly.

Example:

```text
Database Entity
     ↓
API Controller / Service
     ↓
DTO
     ↓
JSON
     ↓
MVC Application
```

### Important DTOs

| File | Purpose |
|---|---|
| `ApiResponse.cs` | Standard API response wrapper containing success, status code, message, data, errors and timestamp |
| `LoginRequestDTO.cs` | Login email/password request |
| `LoginResponseDTO.cs` | JWT token + logged-in user information |
| `RegisterationRequestDTO.cs` | User registration data |
| `UserDTO.cs` | Safe user data returned to clients |
| `VillaDTO.cs` | Villa read/display model |
| `VillaCreateDTO.cs` | Villa create request |
| `VillaUpdateDTO.cs` | Villa update request |
| `VillaAmentiesDTO.cs` | Villa amenity read model |
| `VillaAmentiesCreateDTO.cs` | Amenity create request |
| `VillaAmentiesUpdateDTO.cs` | Amenity update request |

---

## 6.2 RoyalVilla_API

```text
RoyalVilla_API
│
├── Controllers
│   ├── AuthController.cs
│   ├── VillaController.cs
│   └── VillaAmentiesController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Migrations
│   └── EF Core generated migration files
│
├── Models
│   ├── User.cs
│   ├── Villa.cs
│   └── VillaAmenities.cs
│
├── Services
│   ├── IAuthService.cs
│   └── AuthService.cs
│
├── Properties
│   └── launchSettings.json
│
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── libman.json
├── RoyalVilla_API.http
└── RoyalVilla_API.csproj
```

### Files/folders manually added

```text
Controllers/AuthController.cs
Controllers/VillaController.cs
Controllers/VillaAmentiesController.cs
Data/ApplicationDbContext.cs
Models/User.cs
Models/Villa.cs
Models/VillaAmenities.cs
Services/IAuthService.cs
Services/AuthService.cs
```

The `Program.cs` and `appsettings.json` files came from the project template but contain substantial manual configuration described below.

### Files generated automatically

```text
Migrations/*
obj/*
bin/*
```

`Migrations` are generated by Entity Framework commands.  
`bin` and `obj` are build output and should not be committed to Git.

---

## 6.3 RoyalVillaWeb

```text
RoyalVillaWeb
│
├── Controllers
│   ├── AuthController.cs
│   ├── HomeController.cs
│   └── VillaController.cs
│
├── Models
│   ├── ApiRequest.cs
│   └── ErrorViewModel.cs
│
├── Services
│   ├── IServices
│   │   ├── IAuthService.cs
│   │   ├── IBaseService.cs
│   │   └── IVillaService.cs
│   │
│   ├── AuthService.cs
│   ├── BaseService.cs
│   └── VillaService.cs
│
├── Views
│   ├── Auth
│   │   ├── Login.cshtml
│   │   ├── Register.cshtml
│   │   └── AccessDenied.cshtml
│   │
│   ├── Home
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   │
│   ├── Villa
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Delete.cshtml
│   │
│   └── Shared
│
├── wwwroot
├── Properties
│   └── launchSettings.json
│
├── SD.cs
├── Program.cs
├── appsettings.json
└── RoyalVillaWeb.csproj
```

### Manually added application files

```text
Models/ApiRequest.cs
SD.cs
Services/IServices/IBaseService.cs
Services/IServices/IVillaService.cs
Services/IServices/IAuthService.cs
Services/BaseService.cs
Services/VillaService.cs
Services/AuthService.cs
Controllers/AuthController.cs
Controllers/VillaController.cs
Views/Auth/*
Views/Villa/*
```

`HomeController`, standard shared MVC views, `wwwroot`, `Program.cs`, configuration files and `ErrorViewModel` originate from the MVC template, although some are later edited.

---

# 7. Where Configuration Is Done

## 7.1 API database configuration

File:

```text
RoyalVilla_API/appsettings.json
```

Current configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RoyalVilla;TrustServerCertificate=True;Trusted_Connection=True;"
}
```

Registered in:

```text
RoyalVilla_API/Program.cs
```

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

Database context:

```text
RoyalVilla_API/Data/ApplicationDbContext.cs
```

Current tables:

```text
Villa
Users
VillaAmenities
```

---

## 7.2 JWT configuration

File:

```text
RoyalVilla_API/appsettings.json
```

Current setting:

```json
"JwtSettings": {
  "Secret": "..."
}
```

JWT validation is registered in:

```text
RoyalVilla_API/Program.cs
```

JWT generation is implemented in:

```text
RoyalVilla_API/Services/AuthService.cs
```

Current JWT claims include:

```text
NameIdentifier
Email
Name
Role
```

Token expiration is currently seven days.

### Security warning

The current source stores the JWT signing key directly in `appsettings.json`. That is acceptable only for a disposable learning environment.

For a real project, use one of:

```text
.NET User Secrets for local development
Environment variables
Azure Key Vault / another secret manager
```

Do **not** commit production secrets to GitHub.

Example local setup:

```bash
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:Secret" "YOUR-LONG-SECRET"
```

---

## 7.3 API URL used by MVC

File:

```text
RoyalVillaWeb/appsettings.json
```

Current setting:

```json
"ServiceUrls": {
  "VillaAPI": "https://localhost:7123"
}
```

This matches the API HTTPS URL defined in:

```text
RoyalVilla_API/Properties/launchSettings.json
```

Current API HTTPS address:

```text
https://localhost:7123
```

Current MVC HTTPS address:

```text
https://localhost:7088
```

If the API port changes, update:

```text
RoyalVillaWeb/appsettings.json
→ ServiceUrls:VillaAPI
```

---

# 8. Important Manual Configuration in API Program.cs

The API `Program.cs` contains the following manual application setup.

## JWT authentication

```text
AddAuthentication()
AddJwtBearer()
TokenValidationParameters
```

Purpose:

```text
Receive Bearer token
→ Validate signature
→ Validate expiry
→ Build authenticated ClaimsPrincipal
```

---

## CORS

Registered with:

```csharp
builder.Services.AddCors();
```

Current middleware allows any origin/header/method:

```csharp
app.UseCors(o =>
    o.AllowAnyOrigin()
     .AllowAnyHeader()
     .AllowAnyMethod()
     .WithExposedHeaders("*"));
```

For production, replace this with named and restricted origins.

---

## EF Core

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(...);
```

This registers the database context in dependency injection.

---

## Controllers

```csharp
builder.Services.AddControllers();
```

and:

```csharp
app.MapControllers();
```

---

## OpenAPI + Scalar

OpenAPI registration:

```csharp
builder.Services.AddOpenApi(...);
```

Development endpoints:

```csharp
app.MapOpenApi();
app.MapScalarApiReference();
```

The API launch profile currently opens Scalar.

Typical URL:

```text
https://localhost:7123/scalar/v1
```

OpenAPI JSON:

```text
https://localhost:7123/openapi/v1.json
```

The code also adds a Bearer security scheme so a JWT can be supplied when testing protected endpoints.

---

## AutoMapper

AutoMapper mappings are currently declared directly in `Program.cs`.

Examples:

```text
Villa ↔ VillaDTO
Villa ↔ VillaCreateDTO
Villa ↔ VillaUpdateDTO
User ↔ UserDTO
VillaAmenities ↔ VillaAmentiesDTO
```

For a larger project, move these mappings into dedicated AutoMapper profile classes such as:

```text
Mapping/VillaProfile.cs
Mapping/UserProfile.cs
```

---

## Auth service registration

```csharp
builder.Services.AddScoped<IAuthService, AuthService>();
```

This means one `AuthService` instance is created per web request scope.

---

## Automatic migration on startup

The project calls:

```csharp
await SeedDataAsync(app);
```

which runs:

```csharp
await context.Database.MigrateAsync();
```

Therefore pending EF Core migrations are automatically applied when the API starts.

This is useful for development, but production teams often run migrations as a controlled deployment step instead.

---

# 9. Important Manual Configuration in MVC Program.cs

The MVC `Program.cs` configures several parts manually.

## MVC

```csharp
builder.Services.AddControllersWithViews();
```

---

## HttpContext access

```csharp
builder.Services.AddHttpContextAccessor();
```

Required because `BaseService` reads the current JWT token from session.

---

## Session

```csharp
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(...);
```

Middleware:

```csharp
app.UseSession();
```

Current session timeout:

```text
60 minutes
```

The session key is defined in:

```text
RoyalVillaWeb/SD.cs
```

```csharp
public const string SessionToken = "JWTToken";
```

---

## Cookie authentication

Configured with:

```csharp
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(...);
```

Important paths:

```text
LoginPath        /auth/login
AccessDeniedPath /auth/accessdenied
```

Cookie expiration:

```text
60 minutes
```

with sliding expiration enabled.

---

## HttpClient

A named HttpClient is registered:

```csharp
builder.Services.AddHttpClient("RoyalVillaAPI", client =>
{
    var villaAPIUrl = builder.Configuration
        .GetValue<string>("ServiceUrls:VillaAPI");

    client.BaseAddress = new Uri(villaAPIUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
```

This is the HTTP client used by `BaseService` to communicate with the API.

---

## MVC services

```csharp
builder.Services.AddScoped<IVillaService, VillaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
```

These allow controllers to use interfaces rather than instantiate services directly.

---

# 10. API Request Architecture

The main MVC-to-API flow is:

```text
Browser
   │
   ▼
MVC Controller
   │
   ▼
Application Service
(VillaService / AuthService)
   │
   ▼
BaseService
   │
   ▼
IHttpClientFactory
Named client: RoyalVillaAPI
   │
   ▼
HTTP / JSON
   │
   ▼
ASP.NET Core API Controller
   │
   ├──────────────► API Service (authentication)
   │
   ▼
ApplicationDbContext
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

---

# 11. BaseService — Why It Exists

File:

```text
RoyalVillaWeb/Services/BaseService.cs
```

`BaseService` centralizes HTTP communication so every feature service does not duplicate the same code.

It handles:

```text
GET / POST / PUT / DELETE selection
JSON request serialization
JSON response deserialization
Named HttpClient creation
JWT Bearer header injection
Error handling
```

`ApiRequest.cs` carries the request information:

```text
ApiType
Url
Data
Token
```

`SD.cs` defines the supported HTTP operation enum:

```text
GET
POST
PUT
DELETE
```

Example Villa flow:

```text
VillaController
    ↓
IVillaService
    ↓
VillaService
    ↓
BaseService.SendAsync()
    ↓
/api/villa
```

This is preferable to putting raw `HttpClient` code in every MVC controller.

---

# 12. Villa CRUD Flow

## List villas

```text
Browser
→ HomeController / VillaController
→ VillaService.GetAllAsync()
→ BaseService
→ GET /api/villa
→ API VillaController.GetVillas()
→ ApplicationDbContext.Villa
→ SQL Server
→ ApiResponse<List<VillaDTO>>
→ Razor View
```

## Create villa

```text
Villa/Create.cshtml
→ MVC VillaController.Create()
→ VillaService.CreateAsync()
→ POST /api/villa
→ API VillaController.CreateVilla()
→ duplicate-name validation
→ AutoMapper: VillaCreateDTO → Villa
→ EF Core AddAsync()
→ SaveChangesAsync()
→ ApiResponse<VillaDTO>
→ MVC redirect to Index
```

## Update villa

Current implementation uses:

```text
POST /api/villa/{id}
```

Flow:

```text
Villa/Edit.cshtml
→ MVC VillaController.Edit()
→ VillaService.UpdateAsync()
→ API VillaController.UpdateVilla()
→ verify ID
→ check existing villa
→ duplicate-name validation
→ AutoMapper into entity
→ SaveChangesAsync()
```

> REST convention normally uses `PUT /api/villa/{id}` or `PATCH`. The current project uses POST for update, so this is an area you may want to standardize later.

## Delete villa

```text
Villa/Delete.cshtml
→ MVC VillaController.Delete()
→ VillaService.DeleteAsync()
→ DELETE /api/villa/{id}
→ API VillaController.DeleteVilla()
→ EF Core Remove()
→ SaveChangesAsync()
```

---

# 13. Login / JWT / Cookie Authentication Flow

Authentication is intentionally split between the API and MVC application.

```text
Login Form
   │
   ▼
RoyalVillaWeb.AuthController
   │
   ▼
RoyalVillaWeb.AuthService
   │
   ▼
POST /api/auth/login
   │
   ▼
RoyalVilla_API.AuthController
   │
   ▼
RoyalVilla_API.AuthService
   │
   ▼
Users table
   │
   ▼
Generate JWT
   │
   ▼
LoginResponseDTO
(Token + UserDTO)
   │
   ▼
MVC AuthController
   │
   ├── Decode claims
   ├── Create authentication cookie
   └── Store JWT in Session
              │
              ▼
      Session["JWTToken"]
              │
              ▼
      Future BaseService calls
              │
              ▼
Authorization: Bearer <token>
              │
              ▼
             API
```

This allows MVC authorization such as:

```csharp
[Authorize(Roles = "Admin")]
```

while the JWT is also available to authenticate API requests.

---

# 14. Registration Flow

```text
Register.cshtml
→ MVC AuthController.Register()
→ MVC AuthService.RegisterAsync()
→ POST /api/auth/register
→ API AuthController.Register()
→ API AuthService.RegisterAsync()
→ check duplicate email
→ create User
→ SaveChangesAsync()
→ UserDTO
→ ApiResponse<UserDTO>
→ redirect to Login
```

---

# 15. Entity Framework Core Setup

## DbContext

File:

```text
RoyalVilla_API/Data/ApplicationDbContext.cs
```

Current DbSets:

```csharp
public DbSet<Villa> Villa { get; set; }
public DbSet<User> Users { get; set; }
public DbSet<VillaAmenities> VillaAmenities { get; set; }
```

The context also contains initial Villa seed data using `HasData()`.

---

## Create a migration

Visual Studio Package Manager Console:

```powershell
Add-Migration MigrationName
```

Then:

```powershell
Update-Database
```

CLI alternative:

```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

If `dotnet ef` is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Use a version compatible with EF Core 10.

---

# 16. OpenAPI / Scalar Testing

Run `RoyalVilla_API` with the HTTPS profile.

The current API profile launches Scalar.

Expected local API URL:

```text
https://localhost:7123
```

Scalar:

```text
https://localhost:7123/scalar/v1
```

Use Scalar to test:

```text
GET    /api/villa
GET    /api/villa/{id}
POST   /api/villa
POST   /api/villa/{id}
DELETE /api/villa/{id}

POST   /api/auth/register
POST   /api/auth/login
```

Villa amenities are exposed under the current route:

```text
/api/villa-amenitie
```

---

# 17. Running API and MVC Together

For normal development, both startup projects should run.

In Visual Studio, configure multiple startup projects for:

```text
RoyalVilla_API    Start
RoyalVillaWeb     Start
```

Current development ports:

```text
API HTTPS:  https://localhost:7123
API HTTP:   http://localhost:5174

Web HTTPS:  https://localhost:7088
Web HTTP:   http://localhost:5296
```

The Web project must point to the same API HTTPS URL in:

```text
RoyalVillaWeb/appsettings.json
```

---

# 18. Adding a New Feature Correctly

Example: add a `Customer` feature.

## DTO project

Create manually:

```text
CustomerDTO.cs
CustomerCreateDTO.cs
CustomerUpdateDTO.cs
```

## API project

Create:

```text
Models/Customer.cs
Services/ICustomerService.cs        (if service layer is used)
Services/CustomerService.cs
Controllers/CustomerController.cs
```

Add to DbContext:

```csharp
public DbSet<Customer> Customers { get; set; }
```

Add AutoMapper mappings.

Create migration:

```powershell
Add-Migration AddCustomer
Update-Database
```

## MVC project

Create:

```text
Services/IServices/ICustomerService.cs
Services/CustomerService.cs
Controllers/CustomerController.cs
Views/Customer/Index.cshtml
Views/Customer/Create.cshtml
Views/Customer/Edit.cshtml
Views/Customer/Delete.cshtml
```

Register the service in:

```text
RoyalVillaWeb/Program.cs
```

Example:

```csharp
builder.Services.AddScoped<ICustomerService, CustomerService>();
```

Recommended feature flow:

```text
Database model
    ↓
DTOs
    ↓
DbContext
    ↓
Migration
    ↓
API service/business logic
    ↓
API controller
    ↓
Test API in Scalar
    ↓
MVC service
    ↓
MVC controller
    ↓
Razor views
    ↓
End-to-end test
```

---

# 19. Which Files Should Be Created Manually for a New Project?

A useful checklist:

## DTO project

```text
[MANUAL] ApiResponse.cs
[MANUAL] FeatureDTO.cs
[MANUAL] FeatureCreateDTO.cs
[MANUAL] FeatureUpdateDTO.cs
[MANUAL] LoginRequestDTO.cs
[MANUAL] LoginResponseDTO.cs
[MANUAL] UserDTO.cs
```

## API project

```text
[TEMPLATE] Program.cs
[TEMPLATE] appsettings.json
[TEMPLATE] Properties/launchSettings.json

[MANUAL] Data/ApplicationDbContext.cs
[MANUAL] Models/*.cs
[MANUAL] Controllers/*.cs
[MANUAL] Services/*.cs
[MANUAL] JWT configuration
[MANUAL] AutoMapper mappings
[MANUAL] EF Core registration
[MANUAL] OpenAPI/Scalar configuration

[GENERATED] Migrations/*
[GENERATED] bin/*
[GENERATED] obj/*
```

## MVC project

```text
[TEMPLATE] Program.cs
[TEMPLATE] appsettings.json
[TEMPLATE] Properties/launchSettings.json
[TEMPLATE] HomeController.cs
[TEMPLATE] ErrorViewModel.cs
[TEMPLATE] Shared views
[TEMPLATE] wwwroot structure

[MANUAL] Models/ApiRequest.cs
[MANUAL] SD.cs
[MANUAL] Services/IServices/*.cs
[MANUAL] Services/BaseService.cs
[MANUAL] Feature services
[MANUAL] AuthController.cs
[MANUAL] Feature controllers
[MANUAL] Feature Razor views
[MANUAL] Session configuration
[MANUAL] Cookie authentication
[MANUAL] named HttpClient
[MANUAL] DI service registrations
```

---

# 20. Files That Should Not Be Committed to GitHub

Your `.gitignore` should exclude at least:

```gitignore
.vs/
bin/
obj/

*.user
*.suo
*.userosscache
*.sln.docstates
```

The uploaded ZIP files contain `obj` and `.csproj.user` files. These are local/generated artifacts and normally should not be part of the Git repository.

Also do not commit:

```text
Real passwords
Production connection strings
JWT secrets
API keys
OAuth client secrets
Access tokens
Private certificates
```

---

# 21. Important Security Improvements Before Production

This repository is suitable as a learning/starter architecture, but several items should be changed before using it in a real production application.

## Hash passwords

The current API compares/stores passwords directly. Production systems must store a salted password hash rather than plaintext passwords.

Possible approaches:

```text
ASP.NET Core Identity
PasswordHasher<TUser>
A reviewed password-hashing library/algorithm such as bcrypt or Argon2
```

Never store the original password.

---

## Move JWT secret out of appsettings.json

Use User Secrets locally and environment/secret management in deployed environments.

---

## Restrict CORS

Replace `AllowAnyOrigin()` with explicitly allowed frontend origins for production.

---

## Use HTTPS

Keep HTTPS enabled and do not disable HTTPS metadata requirements in production unless there is a specific infrastructure reason.

---

## Standardize REST update methods

The current villa update endpoint uses POST. Prefer PUT/PATCH when establishing a production API convention.

---

## Add global exception handling

Instead of repeating `try/catch` in every controller, consider centralized exception handling with ASP.NET Core exception handlers / Problem Details.

---

# 22. Recommended Architecture Improvement for Future Versions

The existing starter architecture works, but as it grows, consider this API structure:

```text
RoyalVilla_API
│
├── Controllers
├── Data
├── Models
├── Services
│   ├── Interfaces
│   └── Implementations
├── Mapping
├── Middleware
├── Exceptions
└── Configuration
```

Then keep controllers thin:

```text
Controller
    ↓
Service
    ↓
DbContext / Repository
    ↓
Database
```

Business rules should live in services rather than controllers.

---

# 23. Current Naming Notes

The project currently uses names such as:

```text
RegisterationRequestDTO
VillaAmentiesDTO
VillaAmentiesController
```

For future development, the conventional spellings would be:

```text
RegistrationRequestDTO
VillaAmenitiesDTO
VillaAmenitiesController
```

Renaming is optional for the current learning project, but standardizing names early will make future code easier to maintain.

---

# 24. Build and Restore

Restore packages:

```bash
dotnet restore
```

Build the complete solution:

```bash
dotnet build
```

Or in Visual Studio:

```text
Build
→ Build Solution
```

Shortcut:

```text
Ctrl + Shift + B
```

---

# 25. Git Workflow

Before starting work:

```bash
git pull
```

After completing a logical change:

```bash
git status
git add -A
git commit -m "Add customer CRUD module"
git push
```

Recommended team branches:

```text
master/main
│
├── feature/authentication
├── feature/customer-crud
├── feature/villa-amenities
└── bugfix/login-validation
```

---

# 26. Quick Setup Checklist After Cloning

```text
□ Install Visual Studio ASP.NET/web workload
□ Install/verify .NET 10 SDK
□ Open solution in Visual Studio 2026
□ Restore NuGet packages
□ Verify API/Web → DTO project references
□ Remove duplicate DTO ProjectReference entries if present
□ Verify SQL Server LocalDB is available
□ Check RoyalVilla_API/appsettings.json connection string
□ Move JWT secret to User Secrets for non-disposable work
□ Check RoyalVillaWeb/appsettings.json ServiceUrls:VillaAPI
□ Build solution
□ Apply/create EF migrations if required
□ Configure API + Web as multiple startup projects
□ Run API
□ Test API using Scalar
□ Run MVC frontend
□ Test registration/login
□ Test Villa CRUD
```

---

# 27. Full Application Flow Summary

```text
                         ┌─────────────────────┐
                         │       Browser       │
                         └──────────┬──────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │    RoyalVillaWeb    │
                         │ ASP.NET Core MVC 10 │
                         └──────────┬──────────┘
                                    │
                         Controller / Service
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │     BaseService     │
                         │ IHttpClientFactory  │
                         └──────────┬──────────┘
                                    │
                       HTTPS + JSON + JWT Bearer
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │   RoyalVilla_API    │
                         │ ASP.NET Core API 10 │
                         └──────────┬──────────┘
                                    │
                    Controller / Service / Mapper
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │ ApplicationDbContext│
                         │ Entity Framework 10 │
                         └──────────┬──────────┘
                                    │
                                    ▼
                         ┌─────────────────────┐
                         │ SQL Server / LocalDB│
                         └─────────────────────┘

              ┌────────────────────────────────────┐
              │          RoyalVilla.DTO            │
              │ Shared request/response contracts  │
              └──────────────▲──────────▲──────────┘
                             │          │
                            API        Web
```

---

# 28. Purpose of This Repository

This repository should be treated as a **starter architecture and learning reference**, not merely a Royal Villa sample application.

When beginning a future project:

```text
1. Clone/copy this repository
2. Create a new Git repository
3. Rename the solution
4. Rename the DTO/API/Web projects
5. Rename namespaces
6. Verify project references
7. Replace database configuration
8. Replace JWT/authentication configuration as required
9. Remove RoyalVilla-specific models, DTOs and views
10. Keep the reusable infrastructure
11. Build and test
12. Begin the new application's business modules
```

Reusable infrastructure includes:

```text
DTO separation
ApiResponse pattern
HttpClient/BaseService pattern
Dependency injection
Session handling
Cookie authentication
JWT API authentication
AutoMapper
EF Core configuration
OpenAPI + Scalar
MVC → API separation
Git project structure
```

This provides a consistent starting point for future ASP.NET Core 10 enterprise, CRUD, internal business, REST API and MVC applications.
