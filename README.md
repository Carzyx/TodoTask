# TodoTask API

TodoTask es una API RESTful para la gestión de tareas con seguimiento de progresión, desarrollada como prueba técnica siguiendo una arquitectura limpia y principios de Domain-Driven Design.

## 🚀 Características

- Gestión completa de tareas (TodoItems)
- Seguimiento del progreso de cada tarea
- Validaciones de dominio para garantizar la integridad de los datos
- Arquitectura limpia con separación de responsabilidades
- API versionada y documentada con Swagger
- Pruebas unitarias completas para todos los componentes

## 📋 Requisitos Previos

- .NET 9.0
- Un IDE compatible como Visual Studio 2022, VS Code, o Rider

## 🔧 Instalación y Ejecución

1. Clona el repositorio:
```bash
git clone https://github.com/Carzyx/TodoTask.git
cd todo-task
```

2. Restaura las dependencias y compila:
```bash
cd apps/backend/TodoTask
dotnet restore
dotnet build
```

3. Ejecuta la aplicación:
```bash
cd src/TodoTask.API
dotnet run
```

4. Abre un navegador y navega a:
```
http://localhost:5261
```

## 📚 Estructura del Proyecto

El proyecto sigue una arquitectura limpia con las siguientes capas:

- **TodoTask.Domain**: Contiene las entidades de dominio, objetos de valor, agregados e interfaces del repositorio.
- **TodoTask.Application**: Contiene los servicios de aplicación y lógica de negocio.
- **TodoTask.Infrastructure**: Implementa las interfaces del repositorio con una base de datos en memoria.
- **TodoTask.API**: Expone la API REST y configura los servicios.
- **TodoTask.*.UnitTests**: Pruebas unitarias para cada capa del proyecto.

## 🗃️ Categorías Predefinidas

El sistema incluye las siguientes categorías predefinidas que se pueden utilizar al crear un TodoItem:

- Entrantes
- Platos principales
- Postres
- Bebidas
- Menú del día

## 📝 Endpoints de la API

| Método HTTP | Ruta | Descripción |
|-------------|------|-------------|
| GET | `/v1/Todo` | Obtiene todos los TodoItems |
| POST | `/v1/Todo` | Crea un nuevo TodoItem |
| PUT | `/v1/Todo/{id}` | Actualiza la descripción de un TodoItem |
| DELETE | `/v1/Todo/{id}` | Elimina un TodoItem |
| POST | `/v1/Todo/{id}/progression` | Añade una progresión a un TodoItem |

## 📋 Ejemplos de Uso

### Crear un TodoItem

```http
POST /v1/Todo
Content-Type: application/json

{
  "title": "Paella Valenciana",
  "description": "Preparar paella valenciana para 4 personas",
  "category": "Platos principales"
}
```

### Añadir una Progresión

```http
POST /v1/Todo/1/progression
Content-Type: application/json

{
  "dateTime": "2025-05-07T14:30:00",
  "percent": 25.5
}
```

### Obtener Todos los TodoItems

```http
GET /v1/Todo
```

### Actualizar la Descripción de un TodoItem

```http
PUT /v1/Todo/1
Content-Type: application/json

{
  "description": "Preparar paella valenciana para 6 personas con conejo y pollo"
}
```

### Eliminar un TodoItem

```http
DELETE /v1/Todo/1
```

## 🧪 Pruebas

Para ejecutar las pruebas unitarias:

```bash
cd apps/backend/TodoTask
dotnet test
```

## ⚠️ Reglas de Negocio

- No se permite actualizar ni eliminar un TodoItem que tenga más del 50% de progreso completado.
- Las progresiones deben tener una fecha mayor que las progresiones anteriores.
- El porcentaje de progresión debe ser mayor que 0 y menor que 100.
- La suma total de los porcentajes de progresión no puede superar el 100%.
- Un TodoItem se considera completado cuando la suma de sus progresiones alcanza o supera el 100%.

## 🛠️ Herramientas y Tecnologías

- .NET 9.0
- ASP.NET Core
- Swagger/OpenAPI
- xUnit para pruebas unitarias
- Moq para mocking en pruebas
- Husky para hooks de git
- Commitlint para validación de mensajes de commit

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 👥 Contacto

Para cualquier pregunta o sugerencia, por favor abre un issue en este repositorio.

