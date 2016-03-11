using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Budget
    {
      public Budget()
        {
            this.MonthlyTotalHistories = new HashSet<BudgetMonthlyTotalHistory>();
        }
        //prim key 
        public int Id { get; set; }
        //foreign key
        public  int HouseholdId { get; set; }
        public int BudgetCateId { get; set; }
        public int BudgetStatId { get; set; }

        //props
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal? MonthlyTotal { get; set; }
        public DateTimeOffset? Date { get; set; }

        // nav props

        public virtual Household Household { get; set; }
        public virtual BudgetCategory BudgetCate { get; set; }
        public virtual BudgetStatusType BudgetStat { get; set; }
        public virtual ICollection<BudgetMonthlyTotalHistory> MonthlyTotalHistories { get; set;}
       
    }
}