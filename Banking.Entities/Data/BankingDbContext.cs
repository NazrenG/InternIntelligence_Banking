using Banking.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Entities.Data
{
    public class BankingDbContext : IdentityDbContext<User, Role, string>
    {
        public BankingDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.SenderAccount)
                .WithMany(a => a.Senders)
                .HasForeignKey(t => t.SenderAccountId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ReceiverAccount)
                .WithMany(a => a.Receivers)
                .HasForeignKey(t => t.ReceiverAccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public virtual DbSet<UserSetting> UserSettings {  get; set; }
        public virtual DbSet<Transaction> Transactions {  get; set; }
        public virtual DbSet<Account> Accounts {  get; set; } 
    }
}
