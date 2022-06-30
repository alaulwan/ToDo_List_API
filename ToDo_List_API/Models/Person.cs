using System.ComponentModel.DataAnnotations;

namespace ToDo_List_API.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public HashSet<ToDo> tickets = new HashSet<ToDo>();
    }
    public class PersonResponse : Person
    {
        public int tasksCount { get; set; }
        public int openTasksCount { get; set; }
        public PersonResponse(Person person, int tasksCount, int openTasksCount)
        {
            Id = person.Id;
            Name = person.Name;
            this.tasksCount = tasksCount;
            this.openTasksCount = openTasksCount;
        }
    }
}
