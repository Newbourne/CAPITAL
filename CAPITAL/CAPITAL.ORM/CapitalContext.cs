using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using CAPITAL.ORM.Objects;

namespace CAPITAL.ORM
{
    public class CapitalContext: DbContext
    {
        public CapitalContext() : base("CapitalContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Statement> Statements { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Frequency> Frequency { get; set; }
        public DbSet<Registration> Registration { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<PermissionLevel> PermissionLevel { get; set; }
        public DbSet<Share> Share { get; set; }
        public DbSet<CapitalLogError> Errors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConstructUser(modelBuilder);
            ConstructAccount(modelBuilder);
            ConstructFeedback(modelBuilder);
            ConstructShares(modelBuilder);
        }

        private void ConstructUser(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Accounts)
                .WithRequired(a => a.User)
                .HasForeignKey(k => k.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Devices)
                .WithRequired(x => x.User)
                .HasForeignKey(k => k.UserId)
                .WillCascadeOnDelete(true);
        }

        private void ConstructAccount(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(x => x.Statements)
                .WithRequired(s => s.Account)
                .HasForeignKey(k => k.AccountId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Account>()
                .HasRequired(x => x.Frequency)
                .WithMany()
                .HasForeignKey(x => x.FrequencyId);

            modelBuilder.Entity<Account>()
                .HasRequired(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountTypeId);
        }

        private void ConstructRegistration(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registration>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }

        private void ConstructFeedback(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }

        private void ConstructShares(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Share>()
                .HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Share>()
                .HasRequired(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Share>()
                .HasRequired(x => x.PermissionLevel)
                .WithMany()
                .HasForeignKey(x => x.PermissionLevelId)
                .WillCascadeOnDelete(false);
        }
    }

    public class CapitalContextInitializer : DropCreateDatabaseAlways<CapitalContext> 
    {
        public CapitalContextInitializer()
        {
            try
            {
                this.InitializeDatabase(new CapitalContext());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected override void Seed(CapitalContext context)
        {
            // Add appUser to Database and Grant necessary rights
            context.Database.ExecuteSqlCommand("CREATE USER [serviceUser] FOR LOGIN [serviceUser]");
            context.Database.ExecuteSqlCommand("GRANT DELETE TO [serviceUser]");
            context.Database.ExecuteSqlCommand("GRANT INSERT TO [serviceUser]");
            context.Database.ExecuteSqlCommand("GRANT SELECT TO [serviceUser]");
            context.Database.ExecuteSqlCommand("GRANT UPDATE TO [serviceUser]");

            // Add Frequencies
            context.Frequency.Add(new Frequency { Name = "Weekly", Days = 7, CreationDate = DateTime.Now });
            context.Frequency.Add(new Frequency { Name = "2 Weeks", Days = 14, CreationDate = DateTime.Now });
            context.Frequency.Add(new Frequency { Name = "Monthly", Days = 30, CreationDate = DateTime.Now });
            context.Frequency.Add(new Frequency { Name = "2 Months", Days = 60, CreationDate = DateTime.Now });
            context.Frequency.Add(new Frequency { Name = "3 Months", Days = 90, CreationDate = DateTime.Now });
            context.Frequency.Add(new Frequency { Name = "6 Months", Days = 180, CreationDate = DateTime.Now });

            // Add Permission Levels
            context.PermissionLevel.Add(new PermissionLevel { Name = "ReadOnly", Description = "Shared user can only view the account." });
            context.PermissionLevel.Add(new PermissionLevel { Name = "AllowPayments", Description = "Shared user can view account and process statements." });
            context.PermissionLevel.Add(new PermissionLevel { Name = "FullControl", Description = "Shared user has full control over the account." });

            // Add Account Types
            context.AccountTypes.Add(new AccountType { Name = "Balanced", Description = "For accounts that have a recurring payment that is applied towards an ending balance. Once the ending balance is paid, the recurring payments will stop. (Loans, Credit Cards, etc)", IsBalanced = true });
            context.AccountTypes.Add(new AccountType { Name = "Rolling", Description = "For accounts that have a recurring payment that is not applied towards an ending balance. Recurring payments will stop when the account is closed. (Water, Electric, Cable, Insurance, etc)", IsBalanced = false });

            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}