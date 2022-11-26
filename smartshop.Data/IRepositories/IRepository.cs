namespace smartshop.Data.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Gets(int page, int pageSize);
        T? Get(int id);

        int TotalCount();

        T Create(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
