using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Transaction
    {
        // prim key
        public int Id { get; set; }
        // foreign key
        public int TypeId { get; set; }
        public int BankAccountId { get; set;}
        public int BudgetCateId { get; set; }
        //Properties
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsReconciled { get; set; }
        public bool IsDeleted { get; set; }
        public bool Credit { get; set;}

        // nav props

        public virtual TransactionType Type { get; set; }
        public virtual BankAccount BankAccount { get; set; }
        public virtual BudgetCategory BudgetCate { get; set; }
        
    }
}