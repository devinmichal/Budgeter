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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public PartialViewResult Index()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            
            var transactions = db.Transactions.Where(t => t.BankAccount.HouseHoldId == currentuser.HouseholdId && t.IsDeleted == false && t.Date.Month == System.DateTimeOffset.Now.Month && t.Date.Year == System.DateTimeOffset.Now.Year);
            return PartialView("_IndexT",transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public PartialViewResult Create()
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;
            var UserBanksAccounts = db.BanksAccounts.Where(ba => ba.HouseHoldId == userHouseholdId);

            ViewBag.BankAccountId = new SelectList(UserBanksAccounts, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TransactionTypes, "Id", "Name");
            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name");
            return PartialView("_CreateT");
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TypeId,BankAccountId,Description,Amount,Credit, BudgetCateId")] Transaction transaction)
        {
            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHouseholdId = currentuser.HouseholdId;
            var UserBanksAccounts = db.BanksAccounts.Where(ba => ba.HouseHoldId == userHouseholdId);
            var UserBudgets = db.Budgets.Where(b => b.HouseholdId == userHouseholdId);
            if(transaction.TypeId == 5)
            {
                transaction.Credit = true;
            }
            if (ModelState.IsValid)
            {
                var bankAccountEffected = UserBanksAccounts.FirstOrDefault(ba => ba.Id == transaction.BankAccountId);
                var BudgetAmountChange = UserBudgets.FirstOrDefault(b => b.BudgetCateId == transaction.BudgetCateId && b.Name.ToLower().Trim() == transaction.Description.ToLower().Trim());

                if (transaction.Credit)
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount + transaction.Amount;
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal - transaction.Amount;
                    }
                }
                else
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount - transaction.Amount;
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal + transaction.Amount;
                    }
            
                }

                if (BudgetAmountChange != null)
                {
                    // monthly total is 33.33% or below change budget status -- below
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) <= (1 / 3)) { BudgetAmountChange.BudgetStatId = 1; }
                    // monthly total is 66.66% or greater than equal to 50% change budget status -- close 
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) > (1 / 3) && (BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) < (3 / 3)) { BudgetAmountChange.BudgetStatId = 2; }
                    // monthly total is 100% change budget status -- at budget
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) == (3 / 3)) { BudgetAmountChange.BudgetStatId = 3; }
                    // monthly total is 101% or higher change budget status -- over budget
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) > (3 / 3)) { BudgetAmountChange.BudgetStatId = 4; }
                }
                transaction.Date = System.DateTimeOffset.Now;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index","Admin");
            }

            ViewBag.BankAccountId = new SelectList(db.BanksAccounts, "Id", "Id", transaction.BankAccountId);
            ViewBag.TypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TypeId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountId = new SelectList(db.BanksAccounts, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TypeId);
            ViewBag.BudgetCateId = new SelectList(db.BudgetCategories, "Id", "Name", transaction.BudgetCateId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TypeId,BankAccountId,Description,Date,Amount,IsReconciled,IsDeleted,Credit,BudgetCateId")] Transaction transaction)
        {
            

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());

            var originalTrans = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);

            

            var userHouseholdId = currentuser.HouseholdId;
            var UserBanksAccounts = db.BanksAccounts.Where(ba => ba.HouseHoldId == userHouseholdId);
            var UserBudgets = db.Budgets.Where(b => b.HouseholdId == userHouseholdId);


            if (ModelState.IsValid)
            {
                var bankAccountEffected = UserBanksAccounts.FirstOrDefault(ba => ba.Id == transaction.BankAccountId);
                var BudgetAmountChange = UserBudgets.FirstOrDefault(b => b.BudgetCateId == transaction.BudgetCateId && b.Name.ToLower().Trim() == transaction.Description.ToLower().Trim());


                if (transaction.IsDeleted && transaction.Credit)
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount - originalTrans.Amount;
                 
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal + originalTrans.Amount;
                      
                    }
                }
                else
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount + originalTrans.Amount;
              
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal - originalTrans.Amount;
                      
                    }

                }


                if (transaction.Credit)
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount - originalTrans.Amount;
                    bankAccountEffected.Amount = bankAccountEffected.Amount + transaction.Amount;
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal + originalTrans.Amount; 
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal - transaction.Amount;
                    }
                }
                else
                {
                    bankAccountEffected.Amount = bankAccountEffected.Amount + originalTrans.Amount;
                    bankAccountEffected.Amount = bankAccountEffected.Amount - transaction.Amount;
                    if (BudgetAmountChange != null)
                    {
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal - originalTrans.Amount;
                        BudgetAmountChange.MonthlyTotal = BudgetAmountChange.MonthlyTotal + transaction.Amount;
                    }

                }

                if (BudgetAmountChange != null)
                {
                    // monthly total is 33.33% or below change budget status -- below
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) <= (1 / 3)) { BudgetAmountChange.BudgetStatId = 1; }
                    // monthly total is 66.66% or greater than equal to 50% change budget status -- close 
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) > (1 / 3) && (BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) < (3 / 3)) { BudgetAmountChange.BudgetStatId = 2; }
                    // monthly total is 100% change budget status -- at budget
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) == (3 / 3)) { BudgetAmountChange.BudgetStatId = 3; }
                    // monthly total is 101% or higher change budget status -- over budget
                    if ((BudgetAmountChange.MonthlyTotal / BudgetAmountChange.Amount) > (3 / 3)) { BudgetAmountChange.BudgetStatId = 4; }
                }

            
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.BankAccountId = new SelectList(db.BanksAccounts, "Id", "Id", transaction.BankAccountId);
            ViewBag.TypeId = new SelectList(db.TransactionTypes, "Id", "Name", transaction.TypeId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
