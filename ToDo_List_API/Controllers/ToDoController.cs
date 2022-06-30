using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo_List_API.Models;

namespace ToDo_List_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly DataContext _context;
        public ToDoController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound("Ticket Not Found");

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<ToDo>> AddToDo(ToDo todo, int? personId)
        {
            if (personId != null)
            {
                var person = await _context.Persons.FindAsync(personId);
                if (person == null)
                    return NotFound("Person Not Found");
            }

            var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Tickets.Add(todo);
                await _context.SaveChangesAsync();

                if (personId != null)
                {
                    int relation = _context.Database.ExecuteSqlRaw("INSERT INTO PersonToDo (personsId, ticketsId) VALUES({0}, {1})", personId, todo.Id);
                }

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, e.Message);
            }

            return Created("/api/ToDo/" + todo.Id, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ToDo>> UpdateToDo(ToDo todo, int id)
        {
            if(todo.Id != 0 && todo.Id != id) return Conflict("The Id in the path is not the same Id in the body");

            var currentToDo = await _context.Tickets.FindAsync(id);

            if(currentToDo == null) return NotFound("Ticket Not Found");

            if(todo.Status != null)
            {
                var parsed = Enum.TryParse("New", out ToDo.ItemStatus myStatus);
                if(!parsed)
                    return BadRequest("Not Valid Status");
                currentToDo.Status = myStatus;
            }

            if (todo.Task != null) currentToDo.Task = todo.Task;
            if (todo.Description != null) currentToDo.Description = todo.Description;
            if (todo.DueDateTime != null) currentToDo.DueDateTime = todo.DueDateTime;

            await _context.SaveChangesAsync();
            return Ok(currentToDo);
        }

        [HttpPut("{todoId}/assign/{personId}")]
        public async Task<ActionResult<ToDo>> UpdateToDo(int todoId, int personId)
        {
            try
            {
                int relation = _context.Database.ExecuteSqlRaw("INSERT INTO PersonToDo (personsId, ticketsId) VALUES({0}, {1})", personId, todoId);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> DeleteToDo(int id)
        {

            var currentToDo = await _context.Tickets.FindAsync(id);

            if (currentToDo == null)
                return NotFound("Ticket Not Found");

            _context.Tickets.Remove(currentToDo);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
