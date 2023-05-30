namespace Users_Server.Context
{
    public class UsersDBContext : DbContext
    {
        public UsersDBContext() { }

        public UsersDBContext(DbContextOptions<UsersDBContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        }
    }
}