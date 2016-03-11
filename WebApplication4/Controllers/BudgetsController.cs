using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Budgets
        public PartialViewResult Index()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;

            var budgets = db.Budgets.Where(b => b.HouseholdId == userHouseholdId);

            // compare last month to current time
            //if they dont match save the budget monthly total amount to a history
            //wipe out the orig budget monthly total
            if (budgets.FirstOrDefault() != null)
            {
                var budget = budgets.First();
                if ((budget.Date.Value.Year != System.DateTimeOffset.Now.Year) || (budget.Date.Value.Month != System.DateTimeOffset.Now.Month))
                {
                    foreach (var bud in budgets)
                    {
                        if (bud.BudgetCate.Name != "Income")
                        {
                            var history = new BudgetMonthlyTotalHistory
                            {
                                BudgetId = bud.Id,
                                Amount = (decimal)bud.MonthlyTotal,
                                Date = System.DateTimeOffset.Now

                            };
                            bud.Date = System.DateTimeOffset.Now;
                            bud.BudgetStatId = 1;
                            bud.MonthlyTotal = 0;
                            db.MonthlyTotalHistories.Add(history);
                        }
                    }
                    db.SaveChanges();
                }
            }



            return PartialView("_IndexB",budgets.ToList());
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name");
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BudgetCateId,Amount,Name")] Budget budget)
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
    
            budget.HouseholdId = (int) currentuser.HouseholdId;

            if (ModelState.IsValid)
            {
           
                budget.BudgetStatId = 1;
                budget.MonthlyTotal = 0;
                budget.Date = System.DateTimeOffset.Now;
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name", budget.BudgetCateId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }


        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name", budget.BudgetCateId);
           
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,BudgetCateId,Amount, Name")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name", budget.BudgetCateId);
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void CreateCat(string name)
        {
            if(name != null)
            {
                var category = new BudgetCategory()
                {
                    Name = name
                };
                db.BudgetCategories.Add(category);
                db.SaveChanges();
            }
           
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
