using Microsoft.AspNetCore.Http;
using smartshop.Business.Dtos.Business;
using smartshop.Business.IServices.Additional;
using smartshop.Business.IServices.Businesses;
using smartshop.Business.ViewModels.Business;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Settings;

namespace smartshop.Business.Service.Businesses
{
    public class BusinessService : IBusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public BusinessService(IBusinessRepository businessRepository, ICloudinaryService cloudinaryService)
        {
            this._businessRepository = businessRepository;
            this._cloudinaryService = cloudinaryService;
        }
        public void Active(int id)
        {
           _businessRepository.Active(id);
        }

        public int Create(BusinessViewModel entity)
        {
            string? logo = null;
            string? logoPulicId = null;

            if(entity.Logo != null)
                UploadLogo(entity.Logo, ref logo, ref logoPulicId);
            var business = _businessRepository.Create(new Entities.Businesses.Business(entity.Name, entity.ContactNo, entity.Email,
                entity.Address, entity.WebAddress, entity.Objective)
            {
                Logo = logo,
                LogoPublicId = logoPulicId
            });
            return business.Id;
        }

        public void Deactive(int id, DeactiveViewModel model)
        {
            _businessRepository.Deactive(id, model.Descriuptions);
        }

        public bool Delete(int id)
        {
            var business = _businessRepository.Get(id);
            if (business == null)
                return false;

            _businessRepository.Delete(business);
            return true;
        }

        public BusinessDetailsDto? Get(int id)
        {
            var business = _businessRepository.Get(id);
            if (business == null)
                return null;
            var deactiveList = business.BusinessDeactives.Select(bd => new DeactiveHistoryDto(bd.Id, bd.Descriptions, bd.DeactiveDate, bd.ReActivateDate)).ToList();
            return new BusinessDetailsDto(business.Id, business.Name, !business.BusinessDeactives.Any(x => x.ReActivateDate == null), business.ContactNo,
                business.Email, business.Address, business.WebAddress, business.Logo, business.Objective, deactiveList);
        }

        public BusinessConfigureDto? GetBusinessConfigure(int id)
        {
            var configure = _businessRepository.GetConfigure(id);
            if (configure == null)
                return null;
            return new BusinessConfigureDto(configure.Id, configure.CustomerIdPrefix, configure.SupplierIdPrefix, configure.PurchaseInvoicePrefix, configure.SalesInvoicePrefix, configure.DuePaymentPrefix, configure.DueCollectionPrefix);
        }

        public IEnumerable<BusinessDto> Gets(int page, int pageSize)
        {
           return _businessRepository.Gets(page, pageSize).Select(b => new BusinessDto(b.Id, b.Name, !b.BusinessDeactives.Any(d => d.ReActivateDate == null), b.ContactNo,
               b.Email, b.Address, b.WebAddress, b.Logo));
        }

        public IEnumerable<DropdownDto> Gets()
        {
            return _businessRepository.Gets().Select(x => new DropdownDto(x.Name, x.Id));
        }

        public int TotalCount()
        {
            return _businessRepository.TotalCount();
        }

        public bool Update(int id, BusinessViewModel entity)
        {
            var business = _businessRepository.Get(id);
            if (business == null)
                return false;

            string? logo = null;
            string? logoPulicId = null;
            if (entity.Logo != null)
            {
                if(business.LogoPublicId != null)
                    _cloudinaryService.DeleteResources(new List<string> { business.LogoPublicId });
                
                UploadLogo(entity.Logo, ref logo, ref logoPulicId);
            }
            else
            {
                logo = business.Logo;
                logoPulicId = business.LogoPublicId;
            }

            business.Name = entity.Name;
            business.Address = entity.Address;
            business.WebAddress = entity.WebAddress;
            business.Email = entity.Email;
            business.ContactNo = entity.ContactNo;
            business.Logo = logo;
            business.LogoPublicId = logoPulicId;

            _businessRepository.Update(business);
            return true;
        }

        public bool Update(int id, BusinessConfigureViewModel model)
        {
            var configure = _businessRepository.GetConfigure(id);
            var entity = new BusinessConfigure(id, model.CustomerIdPrefix, model.SupplierIdPrefix, model.PurchaseInvoicePrefix, model.SalesInvoicePrefix, model.DuePaymentPrefix, model.DueCollectionPrefix);
            if (configure == null)
               return _businessRepository.CreateConfigure(entity);
            
            else
                return _businessRepository.UpdateConfigure(entity); 
        }

        private void UploadLogo(IFormFile logoFile, ref string? logo, ref string? logoPulicId)
        {
            var file = _cloudinaryService.UploadImage(logoFile.OpenReadStream(), "logo", true).Result;
            logoPulicId = file.PublicId;
            logo = file.Url.AbsoluteUri;
        }
    }
}
