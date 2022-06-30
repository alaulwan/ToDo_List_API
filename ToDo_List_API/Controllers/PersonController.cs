using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDo_List_API.Models;

namespace ToDo_List_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly DataContext _context;
        public PersonController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return Ok(await _context.Persons.ToArrayAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = _context.Persons.Where(p => p.Id == id).Include(p => p.tickets).FirstOrDefault();
            if (person == null)
                return NotFound("Person Not Found");

            List<ToDo> toDoList = person.tickets.ToList();
            var openToDo = toDoList.FindAll(td => td.Status != ToDo.ItemStatus.Done);

            var p = new PersonResponse(person, toDoList.Count, openToDo.Count);

            return Ok(p);
        }

        [HttpGet("{id}/todo")]
        public async Task<ActionResult<Person>> GetToDoForPerson(int id, bool? open)
        {
            var person = _context.Persons.Where(p => p.Id == id).Include(p => p.tickets).FirstOrDefault();
            if (person == null)
                return NotFound("Person Not Found");

            List<ToDo> toDoList = person.tickets.ToList();
            if(open.HasValue)
            {
                if (open.Value)
                    toDoList = toDoList.FindAll(t => t.Status != ToDo.ItemStatus.Done);
                else
                    toDoList = toDoList.FindAll(t => t.Status == ToDo.ItemStatus.Done);
            }

            return Ok(toDoList);
        }

        [HttpPost]
        public async Task<ActionResult<Person>> AddPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return Ok(person);
        }
    }
}
