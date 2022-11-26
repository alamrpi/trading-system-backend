using smartshop.Business.Dtos.Users;
using smartshop.Business.IServices.Users;
using smartshop.Data.IRepositories.Users;
using smartshop.Entities.Common;
using smartshop.Entities.HumanResource;

namespace smartshop.Business.Service.Users
{
    internal class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }
        public void Active(string id)
        {
            var user = _adminRepository.Get(id);
            if (user == null)
                throw new Exception("Admin has been not found by the provided id.");

            user.IsActive = true;
            _adminRepository.Update(user);
           
        }

        public bool AnyAdminByBusiness(int storeId)
        {
            return _adminRepository.AnyAdminByBusiness(storeId);
        }

        public string Create(ApplicationUserViewModel model)
        {
            var applicationUser = new ApplicationUser(model.FirstName, model.LastName, model.Email, model.Email, model.PhoneNumber, true);
            applicationUser.Employees.Add(new Employee("",model.StoreId, 1, DateTime.UtcNow, "N/A", "N/A"));
                
            var user = _adminRepository.Create(applicationUser);

            return user.Id;
        }

     
        public void Deactive(string id)
        {
            var user = _adminRepository.Get(id);
            if (user == null)
                throw new Exception("Admin has been not found by the provided id.");

            user.IsActive = false;
            _adminRepository.Update(user);
        }

        public ApplicationUserDto? Get(string id)
        {
            var user = _adminRepository.Get(id);
            if (user == null)
                return null;

            return MapDto(user);
        }

        public IEnumerable<ApplicationUserDto> Gets(int page, int pageSize) 
            => _adminRepository.Gets(page, pageSize).Select(x => MapDto(x));

        public int TotalCount() 
            => _adminRepository.TotalCount();

        public bool Update(string id, ApplicationUserViewModel model)
        {
            var user = _adminRepository.Get(id);
            if(user == null)
                return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            _adminRepository.Update(user);

            return true;
        }

        protected ApplicationUserDto MapDto(ApplicationUser user)
        {
            Employee? employee = null;
            if (user.Employees != null)
            {
                employee = user.Employees.FirstOrDefault(x => x.ResignDate == null);
                if(employee.Store == null || employee.Store.Business == null)
                    employee = _adminRepository.GetEmployee(user.Id);
            }
            return new ApplicationUserDto(user.Id, user.FirstName, user.LastName, user.UserName, user.Email, user.PhoneNumber,
                employee.Store.Business.Name, employee.Store.Name, user.PhotoUrl, user.IsAdmin, user.IsActive)
            {
                StoreId = employee.StoreId,
                BusinessId = employee.Store.BusinessId
            };
        }
    }
}
