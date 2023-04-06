using EmailService;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CareManagement.Controllers.SCHDL
{
    public class EmailController : Controller
    {

        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            if (TempData["Invoice"] != null)
            {
                var emailContent = "";
                var customerEmail = "";
                emailContent = (string)TempData["Invoice"];
                customerEmail = (string)TempData["CustomerEmail"];
                var message = new Message(new string[] { customerEmail },
                "Invoice CareManagement",
                emailContent);
                _emailSender.SendEmail(message);
            }

            return View();
        }
    }
}
