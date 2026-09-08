# CompanySys API

A .NET Core Web API for managing the workflow of a software development company — departments, teams, employees, projects, and tasks — with JWT authentication and a fine-grained, permission-based authorization system.

Built with **Clean Architecture** and **CQRS** (via MediatR).

---

## ✨ Features

- **Authentication** — JWT access tokens + refresh tokens, customer self-registration
- **Authorization** — Role-based (Owner, TeamLead, Engineer, Customer) with granular permission claims
- **Company / Department / Team management** — full CRUD, team membership management
- **Employee management** — Owner can create and manage TeamLead/Engineer accounts
- **Project workflow** — customer requests → owner approval → team assignment
- **Task management** — creation, multi-engineer assignment, progress tracking, submission & review cycle, task dependencies
- **Audit logging** — every command is automatically tracked (who did what, and when)
- **Structured logging** — Serilog writing to Console, rolling file, and [Seq](https://datalust.co/seq)

---

## 🏗️ Architecture

The solution follows Clean Architecture, split into four projects with a strict, one-directional dependency rule:

```
CompanySys.API            → depends on Infrastructure, Application, Domain
CompanySys.Infrastructure → depends on Application, Domain
CompanySys.Application    → depends on Domain
CompanySys.Domain         → depends on nothing
```

| Project | Responsibility |
|---|---|
| **CompanySys.Domain** | Entities, enums, domain errors, `Result`/`Error` abstractions. No dependencies on anything else. |
| **CompanySys.Application** | CQRS commands/queries, handlers, validators, and the interfaces (`IApplicationDbContext`, `IIdentityService`, `ICurrentUserService`, etc.) that Infrastructure implements. |
| **CompanySys.Infrastructure** | EF Core `DbContext`, ASP.NET Identity, JWT generation, permission policy provider, and implementations of the Application-layer interfaces. |
| **CompanySys.API** | Controllers, `Program.cs` composition root, and anything tied directly to ASP.NET Core (e.g. `HttpContext`-based services). |

Each feature (e.g. `Departments`, `Teams`, `Tasks`) lives under `Application/Features/<FeatureName>/Commands` and `/Queries`, with one folder per operation containing its `Command`/`Query`, `Validator`, and `Handler`.

### Cross-cutting concerns

Two MediatR pipeline behaviors wrap every request:

1. **`ValidationBehavior`** — runs the FluentValidation validator (if one exists) before the handler executes.
2. **`AuditLoggingBehavior`** — automatically records every successful/failed command to the `AuditLogs` table and to Serilog, with no per-handler boilerplate.

---

## 🧩 Domain Model

```
Company
 └── Department
      └── Team ── has one TeamLead, many Engineers (many-to-many)
           ├── Project (via ProjectTeam)
           └── Task
                ├── TaskAssignment (many-to-many with Engineers)
                └── TaskDependency (task depends on another task)

Customer ── has many Projects
```

---

## 👥 Roles & Permissions

| Role | Can do |
|---|---|
| **Owner** | Full control — departments, teams, employees, projects, all permissions |
| **TeamLead** | Manage own teams, create/assign/review tasks, update project status |
| **Engineer** | View assigned tasks, update progress, submit work |
| **Customer** | Submit project requests, track their own projects |

Authorization is permission-based (`Departments.Create`, `Tasks.Assign`, `Projects.Update`, etc.) rather than role-based at the endpoint level — roles are just a convenient bundle of permissions assigned at seed time. See `CompanySys.Infrastructure/Identity/PermissionsList.cs` and `RolePermissions.cs`.

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB is fine for local development)
- (Optional) [Seq](https://datalust.co/seq) for structured log viewing

### 1. Clone the repository

```bash
git clone https://github.com/Ahmeddkhalill/CompanySys.git
cd CompanySys
```

### 2. Configure secrets

The repo ships with placeholder values in `appsettings.json`. Set your own values using the .NET user-secrets manager (never commit real secrets):

```bash
dotnet user-secrets set "Jwt:Key" "your-own-secret-key-at-least-32-characters" --project CompanySys.API
```

You'll also need an `InitialOwner` section (used to seed the first Owner account on startup) — set these as user secrets too:

```bash
dotnet user-secrets set "InitialOwner:Email" "owner@yourcompany.com" --project CompanySys.API
dotnet user-secrets set "InitialOwner:Password" "YourStrongPassword123!" --project CompanySys.API
dotnet user-secrets set "InitialOwner:FirstName" "System" --project CompanySys.API
dotnet user-secrets set "InitialOwner:LastName" "Owner" --project CompanySys.API
```

### 3. Update the connection string

Check `CompanySys.API/appsettings.json` and adjust `ConnectionStrings:DefaultConnection` if your SQL Server instance differs from the default LocalDB setup.

### 4. Apply migrations

```bash
dotnet ef database update --project CompanySys.Infrastructure --startup-project CompanySys.API
```

### 5. Run the API

```bash
dotnet run --project CompanySys.API
```

On first run, the app automatically seeds:
- All roles and their permission claims
- The initial Owner account (from the secrets above)
- A default Company record

### 6. Explore the API

With the app running in Development mode, open the Scalar UI:
```
https://localhost:<port>/scalar/v1
```

---

## 📝 Logging

Serilog is configured entirely through the `Serilog` section in `appsettings.json` and writes to three sinks simultaneously:

| Sink | Where |
|---|---|
| **Console** | Visible while the app is running |
| **File** | `CompanySys.API/Logs/log-{date}.txt` (rolls daily, retains 14 days) |
| **Seq** | `http://localhost:5341` — structured, queryable log viewer |

Minimum level is `Information`, with framework noise (EF Core, ASP.NET Core internals) suppressed to `Warning`.

---

## 🔍 Audit Trail

Every command (create/update/delete/assign/etc.) is automatically recorded in the `AuditLogs` table via `AuditLoggingBehavior`, capturing the acting user, the action, the affected entity, and a timestamp — no manual logging code required in any handler. Owners can review this history via:

```
GET /api/AuditLogs
```

---

## 🛠️ Tech Stack

- **ASP.NET Core Web API** (.NET 10)
- **Entity Framework Core** + SQL Server
- **MediatR** (CQRS)
- **FluentValidation**
- **ASP.NET Core Identity** + JWT Bearer authentication
- **Serilog** (Console, File, Seq sinks)
- **Scalar** for API documentation/testing

---

## 📄 License

This project currently has no license specified.
