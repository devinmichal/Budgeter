using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace WebApplication4.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // additional props
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string DisplayName { get; set; }
        // foreign key
        public int? HouseholdId { get; set;}

        // Nav Props
        

    
        public virtual Household Household { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<AccountWith> AccountWiths { get; set; }
        public DbSet<BankAccount> BanksAccounts { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Budget> Budgets { get; set;}
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<BudgetStatusType> BudgetStatus { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<BudgetMonthlyTotalHistory> MonthlyTotalHistories { get; set; }
    }
}