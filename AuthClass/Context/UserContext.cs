using AuthClass.Model;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuthClass.Context
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
