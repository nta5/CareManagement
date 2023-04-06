using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CareManagement.Models;
using CareManagement.Models.SCHDL;
using CareManagement.Models.OM;
using CareManagement.Models.CRM;

namespace CareManagement.Data
{
    public class CareManagementContext : DbContext
    {
        public CareManagementContext(DbContextOptions<CareManagementContext> options)
            : base(options)
        {
        }
        public DbSet<CareManagement.Models.SCHDL.Service>? Service { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Qualification>? Qualification { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Schedule>? Schedule { get; set; }
        public DbSet<CareManagement.Models.SCHDL.Invoice>? Invoice { get; set; }
        public DbSet<CareManagement.Models.OM.Employee>? Employee { get; set; }
        public DbSet<CareManagement.Models.CRM.Renter>? Renter { get; set; }
        public DbSet<CareManagement.Models.OM.Shift>? Shift { get; set; }
        public DbSet<CareManagement.Models.OM.EmployeeHistory>? EmployeeHistory { get; set; }
        public DbSet<CareManagement.Models.OM.Vacation>? Vacation { get; set; }
        public DbSet<CareManagement.Models.OM.Payroll>? Payroll { get; set; }
        public object Attendance { get; internal set; }
    }
}