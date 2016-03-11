using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class BankAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BankAccounts
        public PartialViewResult Index()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;
             var UserBanksAccounts = db.BanksAccounts.Where( ba => ba.HouseHoldId == userHouseholdId);
            return PartialView("_IndexBA",UserBanksAccounts.ToList());
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BanksAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;
            var UserHouseHold = db.Households.Where( hh => hh.Id == userHouseholdId);

            ViewBag.AccountWithId = new SelectList(db.AccountWiths, "Id", "Name");
            ViewBag.HouseHoldId = userHouseholdId;
            ViewBag.TypeId = new SelectList(db.BankAccountTypes, "Id", "Name");
            return View();
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseHoldId,TypeId,AccountWithId,Amount,Name")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.BanksAccounts.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.AccountWithId = new SelectList(db.AccountWiths, "Id", "Name", bankAccount.AccountWithId);
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.TypeId = new SelectList(db.BankAccountTypes, "Id", "Name", bankAccount.TypeId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BanksAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountWithId = new SelectList(db.AccountWiths, "Id", "Name", bankAccount.AccountWithId);
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.TypeId = new SelectList(db.BankAccountTypes, "Id", "Name", bankAccount.TypeId);
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseHoldId,TypeId,AccountWithId,Amount, Name")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.AccountWithId = new SelectList(db.AccountWiths, "Id", "Name", bankAccount.AccountWithId);
            ViewBag.HouseHoldId = new SelectList(db.Households, "Id", "Name", bankAccount.HouseHoldId);
            ViewBag.TypeId = new SelectList(db.BankAccountTypes, "Id", "Name", bankAccount.TypeId);
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BanksAccounts.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BanksAccounts.Find(id);
            db.BanksAccounts.Remove(bankAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
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
