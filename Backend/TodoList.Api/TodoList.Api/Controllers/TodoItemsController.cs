using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Model;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemsController(ILogger<TodoItemsController> logger, ITodoItemRepository todoItemRepository)
        {
            _logger = logger;
            _todoItemRepository = todoItemRepository;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            try
            {
                var results = await _todoItemRepository.GetTodoItems();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem([FromHeader] Guid id)
        {
            try
            {
                var results = await _todoItemRepository.GetTodoItemById(id);
                if (results == null)
                {
                    return NotFound();
                }
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            try
            {
                await _todoItemRepository.UpdateTodoItem(id, todoItem); 
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, ex.Message);
                if (!(await _todoItemRepository.TodoItemIdExists(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return NoContent();
        } 

        // POST: api/TodoItems 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            try
            {
                var results = await _todoItemRepository.AddTodoItem(todoItem);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        } 
    }
}
