using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IBankAccountRepsitory
    {
        PaginationResponse<BankAccount> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<BankAccount> Get(int businessId);
        BankAccount? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(BankAccount entity);
        void Update(int id, BankAccount entity);
        void Delete(int id);
        void Update(BankAccount bankAccount);
        BankAccount? GetById(int bankAccountId);
    }
}
