using HealthTrackerSolution.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HealthTrackerSolution.DataContext
{
    public class dataContext : DbContext
    {
        public dataContext(DbContextOptions<dataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
