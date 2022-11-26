using Microsoft.AspNetCore.Http;

namespace smartshop.Business.CustomValidations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        private readonly int _maxFileSizeInMegaByte;
        public MaxFileSizeAttribute(int maxFileSizeInMegaByte)
        {
            _maxFileSize = maxFileSizeInMegaByte * 1024 * 1024;
            _maxFileSizeInMegaByte = maxFileSizeInMegaByte;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is {_maxFileSizeInMegaByte} MB.";
        }
    }
}
