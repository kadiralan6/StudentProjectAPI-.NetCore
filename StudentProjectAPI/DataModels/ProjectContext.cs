using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI.DataModels
{
    public class ProjectContext:DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options):base(options)
        {

        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Adress> Adress { get; set; }
        public DbSet<Gender> Gender { get; set; }


    }
}
