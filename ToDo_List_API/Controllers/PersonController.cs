using Microsoft.AspNetCore.Mvc;
using ToDo_List_API.Repositories;

namespace ToDo_List_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        public IPersonRepo repo;

        public PersonController(DataContext dataContext)
        {
            this.repo = new PersonRepo(dataContext);
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return Ok(await repo.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = repo.GetPerson(id);
            if (person == null)
                return NotFound("Person Not Found");

            List<ToDo> toDoList = person.tickets.ToList();
            var openToDo = toDoList.FindAll(td => td.Status != ToDo.ItemStatus.Done);

            var p = new PersonResponse(person, toDoList.Count, openToDo.Count);

            return Ok(p);
        }

        [HttpGet("{id}/todo")]
        public async Task<ActionResult<List<ToDo>>> GetToDoForPerson(int id, bool? open)
        {
            var toDoList = repo.GetToDoForPerson(id);
            if (toDoList == null)
                return NotFound("Person Not Found");

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
            await repo.Add(person);
            return Ok(person);
        }
    }
}
