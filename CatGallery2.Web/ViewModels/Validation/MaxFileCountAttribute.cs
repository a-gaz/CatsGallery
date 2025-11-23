using System.ComponentModel.DataAnnotations;

namespace CatGallery2.Web.ViewModels.Validation;

public class MaxFileCountAttribute : ValidationAttribute
{
    private readonly int _maxFileCount;

    public MaxFileCountAttribute(int maxFileCount)
    {
        _maxFileCount = maxFileCount;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is List<IFormFile> files)
        {
            if (files.Count > _maxFileCount)
            {
                return new ValidationResult($"Максимальное количество файлов: {_maxFileCount}");
            }
        }

        return ValidationResult.Success;
    }
}