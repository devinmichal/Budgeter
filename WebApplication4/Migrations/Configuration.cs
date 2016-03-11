namespace WebApplication4.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication4.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebApplication4.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                 new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            if (context.TransactionTypes.Any()) { }
            else
            {
                var TransTypes = new List<TransactionType>
            {
                new TransactionType {Name = "Deposit" },
                new TransactionType {Name = "Withdrawal" },
                new TransactionType {Name = "Transfer" },
                new TransactionType {Name = "Charge" }
            };

                TransTypes.ForEach(tt => context.TransactionTypes.Add(tt));
                context.SaveChanges();
            }
            if (context.BankAccountTypes.Any()) { }
            else
            {
                var BankAccountTypes = new List<BankAccountType>
            {
                new BankAccountType {Name = "Checking" },
                new BankAccountType {Name = "Savings" },
                new BankAccountType {Name = "Credit Card" },
                new BankAccountType {Name= "Loan" }
            };

                BankAccountTypes.ForEach(bat => context.BankAccountTypes.Add(bat));
                context.SaveChanges();
            }

            if (context.AccountWiths.Any()) { }
            else
            {
                var AccountWith = new List<AccountWith>
            {
                new AccountWith {Name = "Bank of America" },
                new AccountWith {Name = "WellsFargo" },
                new AccountWith {Name = "PNC Bank" },
                new AccountWith {Name = "Chase" },
                new AccountWith {Name = "CitiGroup" }
            };

                AccountWith.ForEach(aw => context.AccountWiths.Add(aw));
                context.SaveChanges();
            }

            if (context.BudgetCategories.Any()) { }
            else
            {
                var BudgetCategories = new List<BudgetCategory>
            {
                new BudgetCategory {Name = "Housing" },
                new BudgetCategory {Name = "Food" },
                new BudgetCategory {Name = "Entertainment" },
                new BudgetCategory {Name = "Income" },
                new BudgetCategory {Name = "Shopping" },
                new BudgetCategory {Name = "Utilities" },
                  new BudgetCategory {Name = "Misc" }

            };

                BudgetCategories.ForEach(bc => context.BudgetCategories.Add(bc));
                context.SaveChanges();
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


        }
    }
}
