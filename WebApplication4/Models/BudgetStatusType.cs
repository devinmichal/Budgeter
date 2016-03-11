using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class BudgetStatusType
    {
       
        // prim key
        public int Id { get; set;}
        // prop
        public string Name { get; set; }

        //nav prop

        public virtual ICollection<Budget> Budgets { get; set; }
    }
}