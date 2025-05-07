using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TodoTask.API.Request;
using TodoTask.Application.Interfaces;

namespace TodoTask.API.Controllers
{
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
        public IActionResult UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
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
        public IActionResult DeleteTodo(int id)
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
        public IActionResult AddProgression(int id, [FromBody] AddProgressionRequest request)
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
        public IActionResult GetAllTodos()
        {
            try
            {
                _todoTaskService.ShowAllTodos();
                return Ok("Todos printed to console");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}