# Sistema de Gestión de Turnos (API REST + GraphQL)

API para gestionar turnos de un consultorio, taller o servicio. Desarrollada con ASP.NET Core, Entity Framework Core (Code First, SQLite), HotChocolate (GraphQL), xUnit y Moq.

## Requisitos

- .NET SDK 10.0
- Git
- `dotnet-ef` (herramienta de migraciones): `dotnet tool install --global dotnet-ef`

## Instalación y ejecución

Desde la raíz del repositorio:

```bash
dotnet restore
dotnet ef database update --project Turnos.API
dotnet run --project Turnos.API
```

La base de datos SQLite (`turnos.db`) se crea automáticamente al aplicar las migraciones.

| Recurso | URL |
|---|---|
| Swagger (API REST) | http://localhost:5288/swagger |
| GraphQL | http://localhost:5288/graphql |

## Tests

```bash
dotnet test Turnos.slnx
```

Los tests unitarios de `TurnoService` verifican las reglas principales: turno en horario libre, horario ocupado con el mismo profesional (rechazado), mismo horario con otro profesional (permitido), profesional o cliente inexistente, y cancelación de turnos.

## Estructura de carpetas

```
ProyectoTurnosAPI/
├── Turnos.slnx
├── Turnos.API/
│   ├── Controllers/     Endpoints REST (Turnos, Profesionales, Clientes)
│   ├── GraphQL/         Query y Mutation de HotChocolate
│   ├── Services/        Lógica de negocio (ITurnoService, TurnoService)
│   ├── Repositories/    Acceso a datos (interfaces e implementaciones)
│   ├── Models/          Entidades: Profesional, Cliente, Turno, EstadoTurno
│   ├── Data/            AppDbContext (EF Core)
│   ├── Migrations/      Migraciones Code First
│   └── Program.cs       Configuración y registro de dependencias
└── Turnos.Tests/        Tests unitarios (xUnit + Moq)
```

## Funcionalidades

- ABM de Profesionales y Clientes.
- Sacar un turno (fecha, hora, profesional, cliente).
- Cancelar un turno.
- Listar turnos por día y por cliente.
- Regla de negocio: no se permiten dos turnos en el mismo horario con el mismo profesional. Un turno cancelado libera el horario. La API devuelve `409 Conflict` ante un horario ocupado.
- Los mismos casos de uso están disponibles por REST y por GraphQL.

## Cómo se aplicaron los principios

### Arquitectura en capas

`Controllers / GraphQL` → `Services` → `Repositories` → `AppDbContext`. Cada capa solo conoce a la siguiente a través de interfaces.

### SOLID

- **S (Responsabilidad única):** cada clase tiene un solo motivo para cambiar. `TurnoService` contiene solo reglas de negocio, los repositorios solo acceden a datos y los controllers solo traducen HTTP.
- **O (Abierto/cerrado):** se puede cambiar el almacenamiento (por ejemplo, de SQLite a SQL Server) o agregar nuevos repositorios sin modificar `TurnoService`.
- **L (Sustitución de Liskov):** cualquier implementación de `ITurnoRepository` puede reemplazar a otra. Los tests usan un mock del repositorio en lugar del real.
- **I (Segregación de interfaces):** hay interfaces pequeñas y específicas (`ITurnoRepository`, `IClienteRepository`, `IProfesionalRepository`, `ITurnoService`), en lugar de una interfaz genérica gigante.
- **D (Inversión de dependencias):** `TurnoService` depende de interfaces, no de clases concretas, y recibe sus dependencias por constructor.

### Inyección de dependencias

Todos los servicios se registran en el contenedor de .NET en `Program.cs`:

```csharp
builder.Services.AddScoped<IProfesionalRepository, ProfesionalRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
```

Los controllers y las clases de GraphQL reciben `ITurnoService` por inyección. Gracias a esto, REST y GraphQL comparten la misma regla de negocio sin duplicar código.

### Clean Code

- Nombres descriptivos (`ExisteTurnoEnHorarioAsync`, `CancelarTurnoAsync`).
- Métodos cortos y con un único propósito.
- Validaciones en orden y con mensajes claros (profesional existe, cliente existe, horario libre).
- Sin duplicación: la lógica de turnos vive solo en `TurnoService`.

## Ejemplo de uso en GraphQL

```graphql
mutation {
  sacarTurno(profesionalId: 1, clienteId: 1, fechaHora: "2026-11-02T10:00:00") {
    exito
    mensaje
    turno { id estado }
  }
}

query {
  turnos {
    id
    fechaHora
    estado
    profesional { nombre especialidad }
    cliente { nombre }
  }
}