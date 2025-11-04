using Microsoft.EntityFrameworkCore;

namespace REST_WebAPI.Models.Context {
    public class MSSQLContext : DbContext {
        public DbSet<Person> People { get; set; }

        public MSSQLContext(DbContextOptions options) : base(options) { }
        
    }
}
