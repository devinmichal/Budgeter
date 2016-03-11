using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class BankAccountType
    {
        // prim key 
        public int Id { get; set; }
        // props
        public string Name { get; set; }
        //nav props
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
    }
}