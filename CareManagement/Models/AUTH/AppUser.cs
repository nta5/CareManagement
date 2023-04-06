using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CareManagement.Models.AUTH
{
	public class AppUser : IdentityUser
    {
        public string Role { get; set; }

        public Guid EmployeeId { get; set; }
    }
}

