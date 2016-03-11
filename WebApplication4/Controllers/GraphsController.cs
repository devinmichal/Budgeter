using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class GraphsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Graphs
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult SurplusChart()
        {

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHousehold = db.Households.Find(currentuser.HouseholdId);

            var totalIncome = userHousehold.Budgets.Where(b => b.BudgetCate.Name == "Income").Select( b => b.Amount).Sum();
            var monthlyTotal = userHousehold.Budgets.Where(b => b.BudgetCate.Name != "Income").Select(b => b.MonthlyTotal).Sum();
            var budgetTotal = userHousehold.Budgets.Where(b => b.BudgetCate.Name != "Income").Select(b => b.Amount).Sum();
            var time = System.DateTimeOffset.Now;
            //var firstBH =  userHousehold.Budgets.First().MonthlyTotalHistories.FirstOrDefault();
          
            var line = new[]{
              
                
               new{ year = time.Year+"-" +time.Month, total =(int) budgetTotal ,current = (int) monthlyTotal }};
 
    

            return Content(JsonConvert.SerializeObject(line), "application/json");
        }




        public ActionResult SurplusChart2()
        {

            ApplicationUser currentuser = db.Users.Find(User.Identity.GetUserId());
            var userHousehold = db.Households.Find(currentuser.HouseholdId);

            var totalIncome = userHousehold.Budgets.Where(b => b.BudgetCate.Name == "Income").Select(b => b.Amount).Sum();
            var monthlyTotal = userHousehold.Budgets.Where(b => b.BudgetCate.Name != "Income").Select(b => b.MonthlyTotal).Sum();
           // var budgetTotal = userHousehold.Budgets.Where(b => b.BudgetCate.Name != "Income").Select(b => b.Amount).Sum();
            var time = System.DateTimeOffset.Now;
           
            

            var line = new List<Object>() {};

            // [surplus] is the  budget [income amount] - [monthly total]

            //[x value] is the current budget date
            // [y value] is the current surplus

            // these are values for previous months and amounts

            //[x value] is the budget monthly total history date

            //[y value] is last months surplus 

            return Content(JsonConvert.SerializeObject(line), "application/json");
        }
    }
}