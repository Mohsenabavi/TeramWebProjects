using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.FileUploader.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage(file.FileName));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(string fileName)
        {
            return $"حجم فایل {fileName} باید {_maxFileSize / 1024 / 1024} مگابایت باشد";
        }
    }
}
