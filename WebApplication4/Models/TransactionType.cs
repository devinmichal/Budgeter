using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class TransactionType
    {
        //prim key
        public int Id { get; set; }
        // prop
        public string Name { get; set; }

        //nav prop

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}