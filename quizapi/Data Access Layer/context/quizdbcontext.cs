using quizapi.Data_Access_Layer.Entities;
using Microsoft.EntityFrameworkCore;


namespace quizapi.Data_Access_Layer.context
{
    public class quizdbcontext : DbContext
    {
        public quizdbcontext(DbContextOptions<quizdbcontext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().HasData(
        new UserRole
        {
            UserRoleId = 1, // Update with the correct primary key value
            UserRolesName = "Admin"
        },
        new UserRole
        {
            UserRoleId = 2, // Update with the correct primary key value
            UserRolesName = "Participant"
        }

        );

        }
    }
}