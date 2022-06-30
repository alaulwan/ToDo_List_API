using System.ComponentModel.DataAnnotations;

namespace ToDo_List_API.Models
{
    public class ToDo
    {
        public enum ItemStatus
        {
            New,
            InProgress,
            Test,
            Done
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Task { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDateTime { get; set; }
        public ItemStatus? Status { get; set; }

        public HashSet<Person> persons = new HashSet<Person>();

    }
}
