using Microsoft.AspNetCore.Http;

namespace smartshop.Business.CustomValidations
{
    public class AllowFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _validExtensions;
        public AllowFileExtensionsAttribute(string[] validExtensions)
        {
            _validExtensions = validExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_validExtensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed! Valid extensions are ${String.Join(", ", _validExtensions)}";
        }
    }
}
