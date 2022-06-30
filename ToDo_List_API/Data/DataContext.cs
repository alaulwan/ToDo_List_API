namespace ToDo_List_API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DataContext() : base() { }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Person> GetPersons() { return Persons; }

        public DbSet<ToDo> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<Person>()
                .HasMany<ToDo>(p => p.tickets)
                .WithMany(t => t.persons);
        }
    }
}
