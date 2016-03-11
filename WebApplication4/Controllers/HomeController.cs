using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Contact(string subject, string body)
        {
            var Email = new EmailService();
            await Email.SendAsync( new Microsoft.AspNet.Identity.IdentityMessage() {
                Destination = "devinfeemster@gmail.com",
                Subject = subject,
                Body = body
            });
        

            return RedirectToAction("Index");
        }
    }
}