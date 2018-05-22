using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using TodoCoreWebApi.Models;

namespace TodoCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            var items = _context.TodoItems.ToList();
            return items.Select(WithUrl);
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(WithSameUrl(item));
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, WithUrl(item));
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Completed = item.Completed;
            todo.Title = item.Title;
            todo.Order = item.Order;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            return new ObjectResult(WithSameUrl(todo));
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(long id, [FromBody] TodoItem item)
        {
            if (item == null )
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            if (item.Order.HasValue)
				todo.Order = item.Order;

			if (item.Title != null)
				todo.Title = item.Title;

			if (item.Url != null)
				todo.Url = item.Url;

			if (item.Completed != todo.Completed)
				todo.Completed = item.Completed;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();

            return new ObjectResult(WithSameUrl(todo));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete]
		public IActionResult Clear()
		{
			var todos = _context.TodoItems.ToList();
            _context.TodoItems.RemoveRange(todos);
            _context.SaveChanges();

            return new NoContentResult();
		}

        private TodoItem WithSameUrl(TodoItem todo)
        {
			todo.Url = Request.GetEncodedUrl();
			return todo;
		}

        private TodoItem WithUrl(TodoItem todo) 
        {
			todo.Url = $"{Request.GetEncodedUrl()}/{todo.Id.ToString()}";
			return todo;
		}
    }
}