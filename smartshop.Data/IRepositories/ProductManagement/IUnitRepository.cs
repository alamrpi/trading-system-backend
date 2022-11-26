namespace smartshop.Data.IRepositories
{
    public interface IUnitRepository
    {
        PaginationResponse<Unit> Get(int businessId, int currentPage, int pageSize);
        Unit? Get(int businessId, int id);
        IEnumerable<Unit> Get(int businessId);
        IEnumerable<UnitVariation> GetVariations(int businessId, int unitId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Unit entity);
        void CreateUnitVariation(List<UnitVariation> variations);
        void Update(int id, Unit entity);
        void Delete(int id);
        void DeleteUnitVariation(int variationId);
    }
}
