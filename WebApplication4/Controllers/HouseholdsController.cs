using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace WebApplication4.Controllers
{
    [Authorize(Roles ="User")]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            Household House = null;
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            // grab the household the user belongs to and display that household
            var userHouseholdId = currentuser.HouseholdId;
       
                 House = db.Households.Find(userHouseholdId);
                return PartialView("_IndexHH",House);
            
            // if user doesnt belong to a household display the view with no object

         
        }

        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            id = currentuser.HouseholdId;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return PartialView("_CreateHH");
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());

            household.Users.Add(currentuser);
            currentuser.HouseholdId = household.Id;
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();
                
            }
            return RedirectToAction("Index", "Admin");
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Household household,string email)
        {
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
              
                if (!String.IsNullOrWhiteSpace(email))
                {
                    email.ToLower();
                    var CuHHId = currentUser.HouseholdId;
                    var invite = new Invite() {
                        Email = email,
                        Code = Guid.NewGuid().ToString("n"),
                        HId = CuHHId
                    };

                    db.Invites.Add(invite);
                    var Email = new EmailService();

                    var hhIndexUrl = Url.Action("Index", "Households");
                 await Email.SendAsync(new IdentityMessage()
                    {
                        Destination = email,
                        Subject = "Invited to a HouseHold",
                        Body = "Copy and Enter this Code " + invite.Code + " to join a household. <a href=\"" + hhIndexUrl + "\"> Click Link </a>"
                    });

                }
                db.Households.Add(household);
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Leave()
        {
            return PartialView("_LeaveHH");
        }
        [HttpPost]
        public ActionResult Leave(bool Leave)
        {
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
            if (Leave)
            {
                var household = db.Households.Find(currentUser.HouseholdId);
                household.Users.Remove(currentUser);
                currentUser.HouseholdId = null;
              
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Join()
        {
            return PartialView("_Join");
        }

        [HttpPost]
        public ActionResult Join(string code)
        {
            code.Trim();
            ApplicationUser currentUser = db.Users.Find(User.Identity.GetUserId());
            var currentUserInvite = db.Invites.FirstOrDefault(i => i.Code == code);
            if(currentUserInvite.Code == code)
            {
                var joinHouse = db.Households.FirstOrDefault(hh => hh.Id == currentUserInvite.HId);
                joinHouse.Users.Add(currentUser);
                currentUser.HouseholdId = null;
                currentUser.HouseholdId = joinHouse.Id;
                db.Invites.Remove(currentUserInvite);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult UserCount()
        {
            Household house = null;

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            
            var userHouseholdId = currentuser.HouseholdId;

            house = db.Households.Find(userHouseholdId);

            return PartialView("_HHUserCount", house);
        }

        public ActionResult Account()
        {
            Household house = null;

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());

            var userHouseholdId = currentuser.HouseholdId;

            house = db.Households.Find(userHouseholdId);

            return PartialView("_HHAccount", house);
        }

        public ActionResult Budget()
        {
            Household house = null;

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());

            var userHouseholdId = currentuser.HouseholdId;

            house = db.Households.Find(userHouseholdId);

            return PartialView("_HHBudget", house);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
