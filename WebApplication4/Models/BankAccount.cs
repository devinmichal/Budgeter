using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class BankAccount
    {
        public BankAccount()
        {
            this.Transactions = new HashSet<Transaction>();
        }
        // primary key
        public int Id { get; set; }
        //foreign key 
        public int HouseHoldId  {get;set;}
        public int TypeId { get; set; }
        public int AccountWithId { get; set; }
        // props
        public decimal Amount { get; set; }
        public string Name { get; set; }

  
        // nav prop
        public virtual Household HouseHold { get; set; }
        public virtual BankAccountType Type { get; set; }
        public virtual AccountWith AccountWith { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set;}

    }
}