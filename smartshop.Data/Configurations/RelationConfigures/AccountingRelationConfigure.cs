using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Data.Configurations.RelationConfigures
{
    internal static class AccountingRelationConfigure
    {
        internal static void AccountingRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Head>()
             .HasOne(x => x.Business)
             .WithMany(x => x.Heads)
             .HasForeignKey(x => x.BusinessId)
             .OnDelete(DeleteBehavior.Cascade)
             .IsRequired();

            modelBuilder.Entity<HeadTransaction>()
               .HasOne(x => x.Head)
               .WithMany(x => x.HeadTransactions)
               .HasForeignKey(x => x.HeadId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(x => x.Account)
                .WithMany(x => x.AccountTransactions)
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<AccountTransaction>()
                .HasOne(x => x.HeadTransaction)
                .WithMany(x => x.AccountTransactions)
                .HasForeignKey(x => x.HeadTransactionId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            //Sedding Data in Account
            modelBuilder.Entity<Account>().HasData(new List<Account>
               {
                   new Account(1, "Cash", EquationComponent.Assets),
                   new Account(2, "Account Receivable", EquationComponent.Assets),
                   new Account(3, "Product", EquationComponent.Assets),
                   new Account(4, "Account Payable", EquationComponent.Liabilities),
                   new Account(5, "Salary Payable", EquationComponent.Liabilities),
                   new Account(6, "Capital", EquationComponent.OwnersEquity),
                   new Account(7, "Drawing", EquationComponent.OwnersEquity),
                   new Account(8, "Revenues", EquationComponent.OwnersEquity),
                   new Account(9, "Expense", EquationComponent.OwnersEquity),
               });

        }
    }
}
