namespace smartshop.Data.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        public readonly ApplicationDbContext _databaseContext;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            _databaseContext = applicationDbContext;
        }
        public T Create(T entity)
        {
            _databaseContext.Add(entity);
            Save();
            return entity;
        }

        public void Delete(T entity)
        {
            _databaseContext.Set<T>().Remove(entity);
            Save();
        }

        public virtual T? Get(int id)
        {
            return _databaseContext.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> Gets(int page, int pageSize)
        {
            return _databaseContext.Set<T>()
               .Skip((page - 1) * pageSize)
               .Take(pageSize);
        }

        public int TotalCount()
        {
            return _databaseContext.Set<T>().Count();
        }

        public void Update(T entity)
        {
            _databaseContext.Entry(entity).State = EntityState.Modified;
            Save();
        }

        protected void Save()
        {
            _databaseContext.SaveChanges();
        }
    }
}
