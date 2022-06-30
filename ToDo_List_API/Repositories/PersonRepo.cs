namespace ToDo_List_API.Repositories
{
    public class PersonRepo : IPersonRepo, IDisposable
    {
        private readonly DataContext _context;
        public PersonRepo(DataContext context)
        {
            this._context = context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Person[]> Get()
        {
            return _context.Persons.ToArrayAsync();
        }

        public Person? GetPerson(int id)
        {
            return _context.Persons.Where(p => p.Id == id).Include(p => p.tickets).FirstOrDefault();
        }

        public List<ToDo>? GetToDoForPerson(int id)
        {
            var person = _context.Persons.Where(p => p.Id == id).Include(p => p.tickets).FirstOrDefault();
            if(person == null)
                return null;
            return person.tickets.ToList();
        }

        public async Task<Person> Add(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }
    }
}
