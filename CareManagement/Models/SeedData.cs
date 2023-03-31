using System;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.SCHDL;

namespace CareManagement.Models
{
	public static class SeedData
	{
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CareManagementContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<CareManagementContext>>()))
            {
                Guid q1 = Guid.NewGuid();
                Guid q2 = Guid.NewGuid();
                Guid q3 = Guid.NewGuid();

                if (!context.Qualification.Any())
                {
                    context.Qualification.AddRange(
                        new Qualification
                        {
                            QualificationId = q1,
                            QualificationDescription = "Certified Therapist",
                        },

                        new Qualification
                        {
                            QualificationId = q2,
                            QualificationDescription = "Certified Nurse",
                        },

                        new Qualification
                        {
                            QualificationId = q3,
                            QualificationDescription = "Certified Consultant",
                        }
                    );
                }
                if (!context.Service.Any())
                {
                    context.Service.AddRange(
                        new Service
                        {
                            Rate = 20,
                            Hours = 2,
                            Type = "Therapy Session",
                            QualificationId = q1
                        },

                        new Service
                        {
                            Rate = 40,
                            Hours = 6,
                            Type = "Nursing",
                            QualificationId = q2
                        },

                        new Service
                        {
                            Rate = 35,
                            Hours = 3,
                            Type = "Homecare",
                            QualificationId = q3
                        }
                    );
                }
                context.SaveChanges();
                return;
            }
        }
    }
}

