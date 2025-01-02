using MeuTodo.Data;
using MeuTodo.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var todos = _context.Todos.ToList();
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var todo = _context.Todos.FirstOrDefault(x => x.Id == id);
            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Todo todo)
        {
            try
            {
                _context.Todos.Add(todo);
                _context.SaveChanges();
                return Created($"/v1/todos/{todo.Id}", todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Todo todo)
        {
            if (todo == null || string.IsNullOrEmpty(todo.Title))
                return BadRequest(new { message = "Tarefa ou título vazio." });

            // Busca o item pelo ID no banco de dados
            var existingTodo = _context.Todos.FirstOrDefault(x => x.Id == id);

            if (existingTodo == null)
                return NotFound(); // Retorna 404 se não encontrar o item

            // Atualiza as propriedades do item existente
            existingTodo.Title = todo.Title;
            existingTodo.Done = todo.Done;

            // Salva as alterações no banco
            _context.SaveChanges();

            return Ok(existingTodo); // Retorna o item atualizado
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todoToDelete = _context.Todos.FirstOrDefault(x => x.Id == id);

            if (todoToDelete == null)
                return NotFound();

            try
            {
                _context.Todos.Remove(todoToDelete);
                _context.SaveChanges();

                return NoContent(); // Status 204, indicando exclusão bem-sucedida sem conteúdo no corpo da resposta
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}