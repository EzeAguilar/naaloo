# 🧾 ClientesApi — REST API en .NET Core

API RESTful desarrollada en C# con ASP.NET Core como parte de una evaluación técnica. Permite gestionar una lista de clientes mediante operaciones CRUD estándar, con almacenamiento en memoria, middleware de logging y despliegue automático en Azure mediante un pipeline CI/CD.

---

## 🌐 URL en Producción

```
https://naaloo-b4c0ckc9h2eeb8ge.brazilsouth-01.azurewebsites.net
```

Recurso disponible:

```
https://naaloo-b4c0ckc9h2eeb8ge.brazilsouth-01.azurewebsites.net/clientes
```

---

## 📌 Descripción General

La API expone el recurso `/clientes` y permite:

- Listar todos los clientes
- Obtener un cliente por ID
- Crear un nuevo cliente
- Actualizar un cliente existente
- Eliminar un cliente

Los datos se almacenan en un **arreglo en memoria**, sin base de datos externa.

---

## 🛠️ Tecnologías Utilizadas

| Tecnología | Descripción |
|---|---|
| .NET Core (ASP.NET Core) | Framework principal para la Web API |
| C# | Lenguaje de programación |
| Visual Studio 2026 | Entorno de desarrollo |
| Azure App Service | Hosting en la nube |
| Azure DevOps | Pipeline CI/CD para despliegue automático |

---

## 📁 Estructura del Proyecto

```
ClientesApi/
├── Controllers/
│   └── ClientesController.cs     # Endpoints REST
├── Models/
│   ├── Cliente.cs                # Entidad principal
│   └── ClienteDto.cs             # DTO para creación/edición
├── Repositories/
│   ├── IClienteRepository.cs     # Interfaz del repositorio
│   └── ClienteRepository.cs      # Implementación en memoria
├── Middleware/
│   └── LoggingMiddleware.cs      # Middleware de logging personalizado
├── Program.cs                    # Configuración de la aplicación
└── README.md
```

---

## 🧩 Modelo de Datos

### `Cliente`

```csharp
public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
}
```

### `ClienteDto` (para creación y edición)

```csharp
public class ClienteDto
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
}
```

> Se usa un **DTO (Data Transfer Object)** para separar la entidad interna del modelo expuesto al cliente HTTP, evitando exponer campos como el `Id` en las operaciones de creación/edición.

---

## 🔌 Endpoints

### `GET /clientes`
Devuelve todos los clientes.

**Response `200 OK`:**
```json
[
  {
    "id": 1,
    "nombre": "Juan",
    "apellido": "Pérez",
    "direccion": "Av. Siempre Viva 123"
  }
]
```

---

### `GET /clientes/{id}`
Devuelve un cliente por su ID.

**Response `200 OK`:**
```json
{
  "id": 1,
  "nombre": "Juan",
  "apellido": "Pérez",
  "direccion": "Av. Siempre Viva 123"
}
```

**Response `404 Not Found`** si no existe el ID.

---

### `POST /clientes`
Crea un nuevo cliente.

**Request body:**
```json
{
  "nombre": "María",
  "apellido": "Gómez",
  "direccion": "Calle Falsa 456"
}
```

**Response `201 Created`** con el cliente creado (incluyendo su `Id` asignado).

---

### `PUT /clientes/{id}`
Actualiza un cliente existente.

**Request body:**
```json
{
  "nombre": "María",
  "apellido": "Gómez",
  "direccion": "Nueva Dirección 789"
}
```

**Response `200 OK`** con el cliente actualizado.  
**Response `404 Not Found`** si no existe el ID.

---

### `DELETE /clientes/{id}`
Elimina un cliente por su ID.

**Response `204 No Content`** si fue eliminado correctamente.  
**Response `404 Not Found`** si no existe el ID.

---

## 🧠 Conceptos Clave Implementados

### 🔁 API REST
La API sigue los principios REST: uso semántico de los verbos HTTP (`GET`, `POST`, `PUT`, `DELETE`), recursos identificables por URL y respuestas en formato **JSON**.

---

### 💉 Inyección de Dependencias (IoC)

Se utiliza el contenedor de IoC nativo de ASP.NET Core. El repositorio se registra como `Scoped` en `Program.cs`:

```csharp
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
```

El controlador recibe la dependencia por constructor sin instanciarla manualmente, siguiendo el principio de **Inversión de Control**:

```csharp
public ClientesController(IClienteRepository repository)
{
    _repository = repository;
}
```

Esto desacopla la lógica del controlador de la implementación concreta del repositorio, facilitando el testing y el mantenimiento.

---

### 🗃️ Repositorio en Memoria

El `ClienteRepository` implementa la interfaz `IClienteRepository` y almacena los clientes en una `List<Cliente>` en memoria. El ID se autoincrementa con cada inserción.

---

### ⚙️ Middleware de Logging Personalizado

Se implementó un **middleware custom** que intercepta cada request en el pipeline de ASP.NET Core y registra en consola el tiempo de procesamiento en milisegundos:

```csharp
public async Task InvokeAsync(HttpContext context)
{
    var stopwatch = Stopwatch.StartNew();
    await _next(context);
    stopwatch.Stop();
    _logger.LogInformation(
    "[RequestTiming] {Method} {Path} -> {StatusCode} | {ElapsedMs} ms",
    context.Request.Method,
    context.Request.Path,
    context.Response.StatusCode,
    stopwatch.ElapsedMilliseconds);
}
```

Se registra en el pipeline en `Program.cs`:

```csharp
app.UseMiddleware<LoggingMiddleware>();
```

Ejemplo de salida en consola:
```
[GET] /clientes => 200 | 4ms
[POST] /clientes => 201 | 2ms
[DELETE] /clientes/3 => 204 | 1ms
```

---

### 🔄 Pipeline de ASP.NET Core

El pipeline de middlewares define el orden en que se procesan los requests. Cada middleware puede ejecutar lógica antes y después de llamar al siguiente (`_next`), lo que permite logging, autenticación, manejo de errores, etc., de forma modular y reutilizable.

---

## 🚀 CI/CD con Azure DevOps

El proyecto cuenta con un pipeline configurado en **Azure DevOps** que automatiza el proceso de integración y despliegue continuo, organizado en dos stages bien diferenciados: **Build** y **Deploy**.

### Flujo general

1. **Trigger:** Cualquier `push` a la rama `master` dispara el pipeline automáticamente.
2. **Build Stage:** Se instala el SDK de .NET 10, se compila y publica el proyecto generando un `.zip` como artefacto.
3. **Deploy Stage:** El artefacto generado se despliega al **Azure App Service** (Linux) usando la conexión de servicio configurada en Azure DevOps.

### Decisiones del pipeline

| Decisión | Motivo |
|---|---|
| `ubuntu-latest` como agente | El App Service corre en Linux; consistencia entre build y runtime |
| `.NET 10.x` | Versión utilizada en el proyecto |
| `zipAfterPublish: true` | Genera un `.zip` listo para desplegar directamente al App Service |
| `environment: 'Produccion'` | Permite trazabilidad y aprobaciones manuales en Azure DevOps si se configuran |
| `condition: succeeded()` | El deploy solo ocurre si el build fue exitoso, evitando deploys rotos |
