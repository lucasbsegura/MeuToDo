using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route("v1/todo/")]
    public class TodoController : ControllerBase
    {
        private readonly AppDbContext _context2;
        public TodoController(AppDbContext contex2) 
        { 
            _context2 = context2;
        }

        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> GetAsync()
        {
            var todos = await _context2.Todos
                                        .AsNoTracking()
                                        .ToListAsync();
            
            return Ok(todos);
        }

        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context,
                                                      [FromRoute]int id)
        {
            var todo = await context.Todos
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id == id);

            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost]
        [Route("todos")]
        public async Task<IActionResult> PostAsync([FromServices]AppDbContext context, 
                                                   [FromBody]CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = new Todo
            {
                Date = System.DateTime.Now,
                Done = false,
                Type = model.Title
            };

            try
            {

                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();

        }
    }
}
