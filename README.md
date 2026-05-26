# API de Gestión de Inventarios

API REST desarrollada en ASP.NET Core 8 para administrar un inventario de productos organizado por categorías y estados. El sistema permite mantener los catálogos principales y consultar un resumen del inventario con métricas de valor, ocupación y stock crítico.

## Funcionalidades

- Gestión CRUD de categorías.
- Gestión CRUD de estados de producto.
- Gestión CRUD de productos.
- Consulta del resumen de inventario.
- Autenticación basada en JWT.
- Restricción de los endpoints de inventario al rol `Administrador`.
- Acceso a datos mediante Dapper y procedimientos almacenados.
- Pruebas unitarias con xUnit y Moq.

## Tecnologías

- .NET 8 / ASP.NET Core Web API
- Dapper
- SQL Server
- JWT Bearer Authentication
- Swagger / OpenAPI
- xUnit
- Moq
- coverlet.collector

## Estructura De La Solución

| Proyecto | Responsabilidad |
| --- | --- |
| `GestionProyectosApi.WebAPI` | Controladores HTTP, autenticación, configuración y Swagger. |
| `GestionProyectosApi.Application` | Servicios y lógica de negocio. |
| `GestionProyectosApi.Domain` | DTO y modelos utilizados entre capas. |
| `GestionProyectosApi.Infrastructure` | Repositorios, Dapper, conexión y unidad de trabajo. |
| `GestionProyectosApi.Utils` | Servicios auxiliares, incluyendo cifrado. |
| `GestionProyectosApi.Tests` | Pruebas unitarias con xUnit y Moq. |

> Aunque el nombre histórico de la solución es `GestionProyectosApi`, el módulo implementado corresponde a gestión de inventarios.

## Módulos Del Inventario

### Categorías

Permite consultar, crear, modificar y eliminar categorías. Durante una actualización parcial, valores vacíos o el valor de ejemplo `string` conservan los datos existentes.

### Estados De Producto

Permite mantener los estados asociados a un producto, por ejemplo disponible o agotado. La actualización también conserva el nombre actual cuando se recibe el valor de ejemplo `string`.

### Productos

Administra nombre, SKU, descripción, precio, existencias, stock mínimo, capacidad máxima, categoría y estado.

La lógica existente contempla:

- Validación de nombre y SKU al crear.
- Rechazo de stock inicial o stock mínimo negativos.
- Verificación de SKU duplicado en el repositorio.
- Actualizaciones parciales que conservan valores existentes cuando llegan campos vacíos, nulos o valores `0` interpretados como valores predeterminados.

### Resumen De Inventario

El endpoint de inventario expone:

- Valor total del inventario.
- Valor del inventario agrupado por categoría.
- Productos con stock crítico.
- Porcentaje de ocupación.

La composición del resumen se realiza en `InventoryService.GetSummary()`. `InventoryController` valida autorización y delega la operación al servicio.

## Endpoints Principales

Todos los endpoints listados, salvo autenticación, requieren un encabezado `Authorization: Bearer <token>` y rol `Administrador`.

| Método | Endpoint | Descripción |
| --- | --- | --- |
| `POST` | `/api/Token/Authentication` | Obtiene un token JWT. |
| `GET` | `/api/Categorias` | Lista categorías. |
| `GET` | `/api/Categorias/{id}` | Consulta una categoría. |
| `POST` | `/api/Categorias` | Crea una categoría. |
| `PATCH` | `/api/Categorias/{id}` | Actualiza una categoría. |
| `DELETE` | `/api/Categorias/{id}` | Elimina una categoría. |
| `GET` | `/api/EstadosProducto` | Lista estados de producto. |
| `GET` | `/api/EstadosProducto/{id}` | Consulta un estado. |
| `POST` | `/api/EstadosProducto` | Crea un estado. |
| `PATCH` | `/api/EstadosProducto/{id}` | Actualiza un estado. |
| `DELETE` | `/api/EstadosProducto/{id}` | Elimina un estado. |
| `GET` | `/api/Productos` | Lista productos. |
| `GET` | `/api/Productos/{id}` | Consulta un producto. |
| `POST` | `/api/Productos` | Crea un producto. |
| `PATCH` | `/api/Productos/{id}` | Actualiza un producto. |
| `DELETE` | `/api/Productos/{id}` | Elimina un producto. |
| `GET` | `/api/Inventory/summary` | Obtiene el resumen del inventario. |

## Configuración

La aplicación utiliza `GestionProyectosApi.WebAPI/appsettings.json` y su variante de desarrollo. Deben estar configuradas estas secciones:

```json
{
  "ConnectionStrings": {
    "ConnetionToken": "<cadena cifrada>",
    "ConnetionGestionInventario": "<cadena cifrada>"
  },
  "Authentication": {
    "SecretKey": "<clave JWT>",
    "Issuer": "<emisor>",
    "Audience": "<audiencia>"
  },
  "SectionConfiguration": {
    "SecureDomains": [],
    "DuracionEnMinutosToken": 1440
  }
}
```

Las cadenas de conexión son descifradas por la aplicación al iniciar. Para entornos reales se recomienda no almacenar secretos reales en archivos versionados y utilizar variables de entorno o un administrador de secretos.

## Ejecución Local

Requisitos:

- .NET SDK 8.
- SQL Server con los procedimientos almacenados esperados por los repositorios.
- Configuración válida de conexión y autenticación.

Restaurar y compilar:

```powershell
dotnet restore .\GestionProyectosApi.sln
dotnet build .\GestionProyectosApi.sln --configuration Debug
```

Ejecutar la Web API:

```powershell
dotnet run --project .\GestionProyectosApi.WebAPI\GestionInventariosApi.WebAPI.csproj
```

En el perfil HTTPS de desarrollo, Swagger se encuentra normalmente en:

```text
https://localhost:7147/swagger
```

## Pruebas Unitarias

El proyecto `GestionProyectosApi.Tests` cumple el requisito de utilizar xUnit y Moq. Los servicios fueron preparados para recibir repositorios e `IUnitOfWork` simulados, de manera que la lógica de negocio pueda probarse sin conexión a SQL Server.

Cobertura implementada:

| Componente | Escenarios Cubiertos |
| --- | --- |
| `CategoriaService` | Consulta, creación con error, actualización parcial, categoría inexistente, eliminación y rollback. |
| `EstadoProductoService` | Consulta, creación, actualización parcial, eliminación con error y rollback. |
| `ProductoService` | Consulta, creación, actualización parcial, DTO nulo, eliminación y rollback. |
| `InventoryService` | Construcción del resumen, listas nulas convertidas a vacías, error y rollback. |
| Repositorios | Validaciones de entrada de categoría, estado y producto sin ejecutar base de datos. |
| `UnitOfWork` | Rollback sin transacción iniciada y rollback sobre una transacción activa. |

Ejecutar pruebas:

```powershell
dotnet test .\GestionProyectosApi.Tests\GestionInventariosApi.Tests.csproj --configuration Debug
```

Resultado verificado:

```text
Total: 28, Superado: 28, Con error: 0, Omitido: 0
```

## Diseño Para Testabilidad

Originalmente los servicios creaban repositorios concretos dentro del flujo de ejecución, lo que impedía simular dependencias con Moq. Se incorporaron constructores alternativos en los servicios de categorías, estados, productos e inventario para inyectar:

- La interfaz del repositorio correspondiente.
- `IUnitOfWork`.

El constructor usado en producción, basado en `IOptions<ConnectionStrings>`, se conserva, por lo que la API mantiene su comportamiento normal y las pruebas pueden ejecutar únicamente lógica de negocio.

## Observaciones Técnicas

- La lógica del resumen del inventario está en la capa de aplicación (`InventoryService`), no en el controlador.
- `UnitOfWork.Rollback()` y `Commit()` toleran que no exista una transacción activa. Esto evita una excepción secundaria cuando una operación falla antes de iniciar transacción.
- Los repositorios utilizan procedimientos almacenados; sus pruebas completas de persistencia corresponden a pruebas de integración, no a las pruebas unitarias con Moq solicitadas.
- La compilación actual presenta advertencias `NU1701` por paquetes heredados restaurados para plataformas .NET Framework mientras los proyectos apuntan a `net8.0`.
- También existen advertencias de nulabilidad y de APIs criptográficas obsoletas en código previo. No bloquean la compilación ni las pruebas unitarias, pero conviene tratarlas como deuda técnica.


