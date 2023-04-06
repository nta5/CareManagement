using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareManagement.Data;
using CareManagement.Models.SCHDL;
using CareManagement.Models.CRM;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CareManagement.Controllers.SCHDL
{
    public class ReportController : Controller
    {
        private readonly CareManagementContext _context;

        public ReportController(CareManagementContext context)
        {
            _context = context;
        }

        //DataContract for Serializing Data - required to serve in JSON format
        [DataContract]
        public class DataPoint
        {
            public DataPoint(string label, double y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }

        // GET: Report
        public async Task<IActionResult> Index(string searchRenter, string searchService, DateTime? startTime, DateTime? endTime, string sortOrder)
        {
            //var careManagementContext = _context.Report.Include(r => r.Renter).Include(r => r.Service);
            var reports = from m in _context.Schedule.Include(r => r.Renter).Include(r => r.Service)
                          select m;



            Console.WriteLine(startTime.ToString(), endTime.ToString());

            if (!String.IsNullOrEmpty(searchRenter))
            {
                reports = reports.Where(s => s.Renter.Name!.Contains(searchRenter));
            }
            if (!String.IsNullOrEmpty(searchService))
            {
                reports = reports.Where(s => s.Service.Type!.Contains(searchService));
            }
            if (startTime.HasValue)
            {
                reports = reports.Where(s => DateTime.Compare(s.StartTime, (DateTime)startTime) >= 0);
            }
            if (endTime.HasValue)
            {
                reports = reports.Where(s => DateTime.Compare(s.EndTime, (DateTime)endTime) <= 0);
            }

            ViewData["RenterSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StartTimeSort"] = sortOrder == "StartTime" ? "time_desc" : "StartTime";
            ViewData["EndTimeSort"] = sortOrder == "EndTime" ? "end_time_desc" : "EndTime";
            ViewData["ServiceSort"] = sortOrder == "ServiceType" ? "service_type_desc" : "ServiceType";

            switch (sortOrder)
            {
                case "name_desc":
                    reports = reports.OrderBy(i => i.Renter.Name);
                    break;
                case "StartTime":
                    reports = reports.OrderBy(i => i.StartTime);
                    break;
                case "time_desc":
                    reports = reports.OrderByDescending(i => i.StartTime);
                    break;
                case "EndTime":
                    reports = reports.OrderBy(i => i.EndTime);
                    break;
                case "end_time_desc":
                    reports = reports.OrderByDescending(i => i.EndTime);
                    break;
                case "ServiceType":
                    reports = reports.OrderBy(i => i.Service.Type); ;
                    break;
                case "service_type_desc":
                    reports = reports.OrderByDescending(i => i.Service.Type);
                    break;
                default:
                    reports = reports.OrderBy(i => i.StartTime);
                    break;
            }


            var invoices = from m in _context.Invoice.Include(r => r.Renter)
                           select m;
            var payments = await invoices.ToListAsync();

            var renters = new List<Renter> { };
            var dataPoints = new List<DataPoint>();

            foreach (var invoice in invoices)
            {
                if (!renters.Contains(invoice.Renter)) {
                    renters.Add(invoice.Renter);
                    dataPoints.Add(new DataPoint(invoice.Renter.Name, invoice.TotalCost));
                }
                else
                {
                    foreach (var dataPoint in dataPoints)
                    {
                        if (dataPoint.Label.Contains(invoice.Renter.Name))
                        {
                            dataPoint.Y += invoice.TotalCost;
                        }
                    }
                }
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View(await reports.ToListAsync());
        }
    }
}
