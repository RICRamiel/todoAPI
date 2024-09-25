using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using TodoApi.Models;
using todoAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace todoAPI.Controllers
{
    [Route("api/todoItems")]
    [ApiController]
    public class todoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public todoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<todoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/todoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<todoItem>> GettodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/todoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttodoItem(long id, todoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!todoItemExists(id))
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

        // POST: api/todoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<todoItem>> PosttodoItem(todoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GettodoItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<todoItem>> editToDoItem([FromBody] JsonPatchDocument todoItem,[FromRoute]long id)
        {
            var toItem = await _context.TodoItems.FindAsync(id);
            if (toItem != null)
            {
                todoItem.ApplyTo(toItem);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<todoItem>> changeStatus(long id)
        {

            var toItem = await _context.TodoItems.FindAsync(id);
            if (toItem != null)
            {
                toItem.IsComplete = !toItem.IsComplete;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }


        // DELETE: api/todoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("upload")]
        public async Task<ActionResult<List<todoItem>>> uploadList(List<todoItem> toUploadItems)
        {
            foreach (var toItem in toUploadItems)
            {
                toItem.Id = 0;
            }
            _context.TodoItems.RemoveRange(_context.TodoItems);
            _context.TodoItems.AddRange(toUploadItems);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool todoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
