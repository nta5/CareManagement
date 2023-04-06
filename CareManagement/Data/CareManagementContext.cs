using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CareManagement.Models.OM;
using CareManagement.Models.CRM;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CareManagement.Models.SCHDL;

namespace CareManagement.Data
{
    public class CareManagementContext : IdentityDbContext
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
        public DbSet<CareManagement.Models.CRM.Appliance> Appliance { get; set; }
        public DbSet<CareManagement.Models.CRM.Asset> Asset { get; set; }
        public DbSet<CareManagement.Models.OM.Shift>? Shift { get; set; }
        public DbSet<CareManagement.Models.OM.EmployeeHistory>? EmployeeHistory { get; set; }
        public DbSet<CareManagement.Models.OM.Vacation>? Vacation { get; set; }
        public DbSet<CareManagement.Models.OM.Payroll>? Payroll { get; set; }
        public DbSet<CareManagement.Models.CRM.Applicant>? Applicant { get; set; }
        public DbSet<CareManagement.Models.CRM.AssetMaintenance>? AssetMaintenance { get; set; }
        public DbSet<CareManagement.Models.CRM.AssetRentLog>? AssetRentLog { get; set; }
        public DbSet<CareManagement.Models.CRM.RenterService>? RenterService { get; set; }
        public DbSet<CareManagement.Models.AUTH.AppUser>? AppUser { get; set; }
        public object Attendance { get; internal set; }
    }
}