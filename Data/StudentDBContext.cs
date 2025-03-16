using CodeFirstEFAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirstEFAPI.Data
{
    public class StudentDBContext: DbContext
    {
        public StudentDBContext(DbContextOptions<StudentDBContext> options): base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Standard> Standards { get; set; }
    }
}
