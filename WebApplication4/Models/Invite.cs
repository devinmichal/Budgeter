using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Invite
    {
        // prim key
        public int Id { get; set; }


        // prop
        public string Email  { get; set; }
        public string Code { get; set; }
        public int? HId { get; set; }
    }
}