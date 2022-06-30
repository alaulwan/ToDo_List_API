namespace ToDo_List_API.Repositories
{
    public interface IPersonRepo
    {
        public Task<Person[]> Get();
        public Person? GetPerson(int id);
        public List<ToDo>? GetToDoForPerson(int id);
        public Task<Person> Add(Person person);

    }
}
