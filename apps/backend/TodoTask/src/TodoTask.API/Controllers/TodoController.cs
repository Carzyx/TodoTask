using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TodoTask.API.Request;
using TodoTask.Application.Interfaces;
using TodoTask.Domain.Interfaces;

namespace TodoTask.API.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class TodoController : ControllerBase
{
    private readonly ITodoTaskService _todoTaskService;

    public TodoController(ITodoTaskService todoTaskService)
    {
        _todoTaskService = todoTaskService ?? throw new ArgumentNullException(nameof(todoTaskService));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Crea una nueva tarea",
        Description = "Crea un nuevo TodoItem con los datos proporcionados. Las categorías disponibles son: Entrantes, Platos principales, Postres, Bebidas, Menú del día.",
        OperationId = "CreateTodo",
        Tags = new[] { "TodoItems" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult CreateTodo([FromBody] CreateTodoRequest request)
    {
        try
        {
            _todoTaskService.CreateTodo(request.Title, request.Description, request.Category);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Actualiza la descripción de una tarea",
        Description = "Actualiza la descripción de un TodoItem existente. No se permite actualizar tareas con más del 50% de progreso.",
        OperationId = "UpdateTodo",
        Tags = new[] { "TodoItems" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult UpdateTodo(
        [SwaggerParameter("ID de la tarea a actualizar")] int id, 
        [FromBody] UpdateTodoRequest request)
    {
        try
        {
            _todoTaskService.UpdateTodo(id, request.Description);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Elimina una tarea",
        Description = "Elimina un TodoItem existente. No se permite eliminar tareas con más del 50% de progreso.",
        OperationId = "DeleteTodo",
        Tags = new[] { "TodoItems" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult DeleteTodo(
        [SwaggerParameter("ID de la tarea a eliminar")] int id)
    {
        try
        {
            _todoTaskService.DeleteTodo(id);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{id}/progression")]
    [SwaggerOperation(
        Summary = "Añade una progresión a una tarea",
        Description = "Añade una nueva progresión a un TodoItem existente. El porcentaje debe ser mayor que 0 y menor que 100. La fecha debe ser posterior a las progresiones existentes. La suma total de progresiones no puede superar el 100%.",
        OperationId = "AddProgression",
        Tags = new[] { "Progressions" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult AddProgression(
        [SwaggerParameter("ID de la tarea")] int id, 
        [FromBody] AddProgressionRequest request)
    {
        try
        {
            _todoTaskService.AddProgression(id, request.DateTime, request.Percent);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtiene todas las tareas",
        Description = "Devuelve una lista con todos los TodoItems y sus progresiones.",
        OperationId = "GetAllTodos",
        Tags = new[] { "TodoItems" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult GetAllTodos()
    {
        try
        {
            var todos = _todoTaskService.GetAllTodos();
            return Ok(todos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("categories")]
    [SwaggerOperation(
        Summary = "Obtiene todas las categorías disponibles",
        Description = "Devuelve la lista de categorías válidas que pueden usarse al crear un TodoItem.",
        OperationId = "GetCategories",
        Tags = new[] { "Categories" }
    )]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public IActionResult GetCategories()
    {
        try
        {
            var repository = HttpContext.RequestServices.GetRequiredService<ITodoListRepository>();
            var categories = repository.GetAllCategories();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}