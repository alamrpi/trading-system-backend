using smartshop.Business.Dtos.Business;
using smartshop.Business.IServices.Businesses;
using smartshop.Business.ViewModels.Business;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Businesses;

namespace smartshop.Business.Service.Businesses
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeService)
        {
            this._storeRepository = storeService;
        }

        public void Active(int id)
        {
            _storeRepository.Active(id);
        }

        public int Create(StoreViewModel entity)
        {
           var store = _storeRepository.Create(new Entities.Businesses.Store(entity.BusinessId.Value, entity.Name, entity.ContactNo, entity.Email, entity.Address, entity.Code));
            return store.Id;
        }

        public void Deactive(int id, DeactiveViewModel model)
        {
            _storeRepository.Deactive(id, model.Descriuptions);
        }

        public bool Delete(int id)
        {
            var store = _storeRepository.Get(id);
            if (store != null)
            {
                _storeRepository.Delete(store);
            }
            return false;
        }

        public StoreDetailDto? Get(int id)
        {
            var store = _storeRepository.Get(id);
            if (store == null)
                return null;

            bool isActive = CheckStoreActivated(store);
            var deactiveHistories = new List<DeactiveHistoryDto>();

            var business = new BusinessDto(store.BusinessId, store.Business.Name, true, store.Business.ContactNo, store.Business.Email, store.Business.Address,
                store.Business.WebAddress, store.Business.Logo);

            if (store.StoreDeactives != null)
                deactiveHistories = store.StoreDeactives.Select(deactive => new DeactiveHistoryDto(deactive.Id, deactive.Descriptions, deactive.DeactiveDate, deactive.ReActivateDate)).ToList();

            return new StoreDetailDto(store.Id, store.Name, store.Business.Name, store.ContactNo, store.Email, store.Address, store.Code, isActive,
                deactiveHistories, business);
        }

        public IEnumerable<StoreDto> Gets(int page, int pageSize, int? businessId)
        {
            return _storeRepository.Gets(page, pageSize, businessId).Select(store => new StoreDto(store.Id, store.Name, store.Business.Name, store.ContactNo, 
                store.Email, store.Address, store.ContactNo, CheckStoreActivated(store)));
        }

        public IEnumerable<DropdownDto> Gets(int? businessId) 
            => _storeRepository.Gets(businessId).Select(st => new DropdownDto(st.Name, st.Id));

        public int TotalCount() 
            => _storeRepository.TotalCount();

        public bool Update(int id, StoreViewModel entity)
        {
            var store = _storeRepository.Get(id);
            if (store == null) return false;
            store.Name = entity.Name;
            store.BusinessId = entity.BusinessId.Value;
            store.Email = entity.Email;
            store.Address = entity.Address;
            store.ContactNo = entity.ContactNo;
            store.Code = entity.Code;
            
            _storeRepository.Update(store);
            return true;
        }

        private static bool CheckStoreActivated(Store store)
        {
            var isActive = true;
            if (store.StoreDeactives != null)
                isActive = !store.StoreDeactives.Any(x => x.ReActivateDate == null);
            return isActive;
        }
    }
}
