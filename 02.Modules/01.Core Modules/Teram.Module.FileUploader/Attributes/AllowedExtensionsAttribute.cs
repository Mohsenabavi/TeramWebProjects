using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.FileUploader.Attributes
{

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage(file.FileName));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string fileName)
        {
            return $"پسوند فایل {fileName} مجاز نمی باشد";
        }
    }
}

