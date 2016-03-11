using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;
            
            if (userHouseholdId != null)
            {
                ViewBag.HasHH = true;
            }
            else
            {
                ViewBag.HasHH = false;
            }
            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details()
        {
            return View();
        }

      
        
    }
}
