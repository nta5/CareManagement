using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CareManagement.Models;
using CareManagement.Models.SCHDL;

namespace CareManagement.Data
{
    public class CareManagementContext : DbContext
    {
        public CareManagementContext (DbContextOptions<CareManagementContext> options)
            : base(options)
        {
        }
<<<<<<< HEAD
        public DbSet<CareManagement.Models.EmployeeHistory>? EmployeeHistory { get; set; }
        public object Qualification { get; internal set; }
        public object Service { get; internal set; }
=======
        public DbSet<CareManagement.Models.SCHDL.Service>? Service { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Qualification>? Qualification { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Schedule>? Schedule { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Invoice>? Invoice { get; set; }
>>>>>>> 5dd44b43a7c3636ad5f513d14259fda72e5c17d6
    }
}
