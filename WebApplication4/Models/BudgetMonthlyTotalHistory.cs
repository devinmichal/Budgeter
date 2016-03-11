using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class BudgetMonthlyTotalHistory
    {
        //prim key
      public  int Id { get; set; }

        //foreign key
         public int BudgetId { get; set; }

        //prop
         public DateTimeOffset Date { get; set; }
      
        public decimal Amount { get; set; }

        //nav prop
        public virtual Budget Budget {get; set;}
    }
}